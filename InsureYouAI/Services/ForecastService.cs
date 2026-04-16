using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace InsureYouAI.Services;

public class PolicySalesData
{
    public DateTime Date { get; set; }
    public float SaleCount { get; set; }
}

public class PolicySalesForecast
{
    public float[] ForecastedValues { get; set; }
    public float[] LowerBoundValues { get; set; }
    public float[] UpperBoundValues { get; set; }
}

public class ForecastService
{
    private readonly MLContext _mlContext;

    public ForecastService()
    {
        _mlContext = new MLContext();
    }

    public PolicySalesForecast GetForecast(List<PolicySalesData> salesData, int horizon = 3)
    {
        // Veri setindeki toplam kayıt sayısını alma
        int count = salesData.Count;

        // salesData listesini ML.NET'in anlayacağı IDataView formatına çevirme
        var dataView = _mlContext.Data.LoadFromEnumerable(salesData);

        // SSA (Singular Spectrum Analysis) tabanlı zaman serisi tahmin pipeline'ı oluşturma
        var forecastingPipeLine = _mlContext.Forecasting.ForecastBySsa(

            // Tahmin edilen değerlerin yazılacağı kolon adı
            outputColumnName: "ForecastedValues",

            // Giriş verisindeki tahmin yapılacak kolon (satış sayısı)
            inputColumnName: "SaleCount",

            // Pencere boyutu (modelin geçmişten kaç veri noktasına bakacağını belirler)
            // Burada minimum 2 olacak şekilde toplam verinin 1/4'ü alınır
            windowSize: Math.Max(2, count / 4),

            // Modelin öğrenme için kullanacağı toplam seri uzunluğu
            // Minimum 4 olacak şekilde toplam verinin yarısı alınır
            seriesLength: Math.Max(4, count / 2),

            // Eğitim için kullanılacak veri sayısı - horizon kadar veri test/tahmin için ayrılır
            trainSize: count - horizon,

            // Kaç adım ileri tahmin yapılacağı (örneğin 3 ay ileri gibi)
            horizon: horizon,

            // Güven aralığı (confidence interval) seviyesi (%95)
            confidenceLevel: 0.95f,

            // Tahminlerin alt sınırının yazılacağı kolon
            confidenceLowerBoundColumn: "LowerBoundValues",

            // Tahminlerin üst sınırının yazılacağı kolon
            confidenceUpperBoundColumn: "UpperBoundValues"
            );

        // Pipeline eğitilir (model oluşturulur)
        var model = forecastingPipeLine.Fit(dataView);

        // Eğitilen modelden bir tahmin motoru (forecasting engine) oluşturulur
        var forecastingEngine = model.CreateTimeSeriesEngine<PolicySalesData, PolicySalesForecast>(_mlContext);

        // Tahmin işlemini çalıştırılıp sonucu döndürülür
        return forecastingEngine.Predict();
    }
}
