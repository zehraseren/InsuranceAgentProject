using Microsoft.ML;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Transforms.TimeSeries;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHSalesForecastComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHSalesForecastComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        // Modelin bakacağı geçmiş ay sayısı
        int monthsBack = 12;

        // Bugün ve geriye dönük başlangıç tarihi hesaplama
        var endDate = DateTime.UtcNow;
        var startDate = endDate.AddMonths(-monthsBack);

        // 1) VERİ HAZIRLIĞI (DATA PREPROCESSING)
        // DB'den son 12 aylık poliçelerim çekilmesi
        // PolicyType bazında gruplanması
        // Her grup için aylık satış sayıları çıkarılması
        var rawData = _context.Policies
            .Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate)
            .AsEnumerable() // EF tarafında değil memory'de işlem yapılır (grouping için gerekli)
            .GroupBy(x => x.PolicyType)
            .Select(policyGroup => new
            {
                PolicyType = policyGroup.Key,

                // Eksik aylar 0 ile doldurulması (time series continuity)
                MonthlyCounts = FillMissingMonths(
                    policyGroup,
                    startDate,
                    monthsBack
                )
            })
            .ToList();

        // 2) ML.NET FORECASTING (SSA MODEL)
        var ml = new MLContext();

        var result = new List<DSHSalesForecastViewModel>();

        foreach (var item in rawData)
        {
            // ML modeline verilecek input formatına çevirilmesi
            var mlData = item.MonthlyCounts.Select(m => new PolicyMonthlyData
            {
                MonthIndex = m.MonthIndex,  // zaman sırası
                Value = m.Value             // satış sayısı
            });

            // ML.NET DataView oluşturulması
            var dataView = ml.Data.LoadFromEnumerable(mlData);

            // SSA Forecasting pipeline
            // windowSize: pattern öğrenme penceresi
            // seriesLength: toplam zaman serisi uzunluğu
            // trainSize: eğitim için kullanılacak veri uzunluğu
            var pipeline = ml.Forecasting.ForecastBySsa(
                outputColumnName: "Forecast",
                inputColumnName: "Value",
                windowSize: 2,
                seriesLength: monthsBack,
                trainSize: monthsBack,
                horizon: 1 // sadece 1 ay ileri tahmin
            );

            // Modelin eğitilmesi
            var model = pipeline.Fit(dataView);

            // Prediction engine oluşturulması
            var engine = model.CreateTimeSeriesEngine<PolicyMonthlyData, PolicyForecastOutput>(ml);

            // Gelecek ay tahmini alınması
            var prediction = engine.Predict();

            // Float sonucu int'e çevirilmesi (satış sayısı olduğu için)
            var forecast = (int)Math.Round(prediction.Forecast[0]);

            // Sonuç listesine eklenmesi
            result.Add(new DSHSalesForecastViewModel
            {
                PolicyType = item.PolicyType,
                ForecastCount = forecast
            });
        }

        // 3) YÜZDELİK DAĞILIM HESABI
        // Toplam tahmini satış
        int total = result.Sum(x => x.ForecastCount);

        // Her policy type'ın toplam içindeki payı hesaplanması
        foreach (var item in result)
        {
            item.Percentage = total > 0 ? (decimal)item.ForecastCount * 100m / total : 0;

            item.Percentage = Math.Round(item.Percentage, 0);
        }

        return View(result);
    }

    // TIME SERIES DATA NORMALIZATION (CRITICAL PART)
    private List<PolicyMonthlyData> FillMissingMonths(
        IEnumerable<Policy> policies,
        DateTime startDate,
        int monthsBack)
    {
        // Aylık kırılımda gruplanma
        var grouped = policies
            .GroupBy(x => new { x.CreatedDate.Year, x.CreatedDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Count = g.Count()
            })
            .ToList();

        var result = new List<PolicyMonthlyData>();

        // Eksik aylar 0 olarak doldurulması
        for (int i = 0; i < monthsBack; i++)
        {
            var date = startDate.AddMonths(i);

            var match = grouped.FirstOrDefault(x => x.Year == date.Year && x.Month == date.Month);

            result.Add(new PolicyMonthlyData
            {
                MonthIndex = i + 1,         // 1..N sıralı zaman indexi
                Value = match?.Count ?? 0   // veri yoksa 0
            });
        }

        return result;
    }

    // ML INPUT MODEL
    public class PolicyMonthlyData
    {
        public float MonthIndex { get; set; }   // zaman ekseni
        public float Value { get; set; }        // satış değeri
    }

    // ML OUTPUT MODEL
    public class PolicyForecastOutput
    {
        public float[] Forecast { get; set; }   // SSA çıktısı (1 aylık tahmin)
    }

}
