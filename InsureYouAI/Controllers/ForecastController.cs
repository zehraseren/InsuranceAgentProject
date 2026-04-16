using InsureYouAI.Models;
using InsureYouAI.Context;
using InsureYouAI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers;

public class ForecastController : Controller
{
    private readonly InsureContext _context;
    private readonly ForecastService _forecastService;

    public ForecastController(InsureContext context)
    {
        _context = context;
        _forecastService = new ForecastService();
    }

    public IActionResult Index()
    {
        // Veritabanından poliçeleri çekip aylık bazda gruplayarak satış verisi oluştur
        var salesData = _context.Policies

            // StartDate alanına göre yıl + ay bazında gruplama
            .GroupBy(p => new { p.StartDate.Year, p.StartDate.Month })

            // Her grup için yıl, ay ve kayıt sayısını alma
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count()
            })

            // EF Core SQL tarafında çalışamayan kısımları (DateTime oluşturma vs.) client-side'a çekme
            .AsEnumerable()

            // UI/servis katmanında kullanılacak domain modele map etme
            .Select(g => new PolicySalesData
            {
                // Ayın ilk günü olacak şekilde tarih oluşturma
                Date = new DateTime(g.Year, g.Month, 1),
                SaleCount = g.Count
            })
            .OrderBy(x => x.Date)
            .ToList();

        // Oluşturulan geçmiş satış verisini kullanarak tahmin (forecast) üretme
        var forecast = _forecastService.GetForecast(salesData, horizon: 3);

        // UI tarafında gösterilecek ViewModel'i hazırlama
        var model = new ForecastViewModel
        {
            Items = Enumerable.Range(0, forecast.ForecastedValues.Length)

                // Tahmin sonuçlarını ekrana uygun formata çevirme
                .Select(i => new ForecastItemViewModel
                {
                    // Burada gerçek tarih yerine basit bir "Ay index'i" kullanımı
                    Month = $"Ay: {i + 1}",

                    // Tahmin edilen değer
                    Forecasted = forecast.ForecastedValues[i],

                    // Alt güven aralığı (lower bound)
                    LowerBound = forecast.LowerBoundValues[i],

                    // Üst güven aralığı (upper bound)
                    UpperBound = forecast.UpperBoundValues[i]
                }).ToList()
        };

        return View(model);
    }
}
