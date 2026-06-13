using AutoMapper;
using InsureYouAI.Models;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Attributes;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.ArticleDtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsureYouAI.Controllers;

public class ArticleController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public ArticleController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [PageInfo("Makaleler", "Makale Listesi")]
    public IActionResult ArticleList()
    {
        var articles = _context.Articles.ToList();
        var result = _mapper.Map<List<ResultArticleDto>>(articles);
        return View(result);
    }

    [HttpGet]
    [PageInfo("Makaleler", "Makale Oluştur")]
    public IActionResult CreateArticle()
    {
        var model = new CreateArticleViewModel();
        PopulateDropdowns(model);

        return View(model);
    }

    [HttpPost]
    [PageInfo("Makaleler", "Makale Oluştur")]
    public IActionResult CreateArticle(CreateArticleViewModel cavm)
    {
        ModelState.Remove(nameof(cavm.Categories));
        ModelState.Remove(nameof(cavm.Authors));

        if (!ModelState.IsValid)
        {
            PopulateDropdowns(cavm);
            return View(cavm);
        }

        cavm.Article.CreatedTime = DateTime.Now;
        var article = _mapper.Map<Article>(cavm.Article);
        _context.Articles.Add(article);
        _context.SaveChanges();

        return RedirectToAction("ArticleList");
    }

    public IActionResult DeleteArticle(int id)
    {
        var article = _context.Articles.Find(id);
        _context.Articles.Remove(article);
        _context.SaveChanges();
        return RedirectToAction("ArticleList");
    }

    [HttpGet]
    [PageInfo("Makaleler", "Makale Güncelle")]
    public IActionResult UpdateArticle(int id)
    {
        var article = _context.Articles.Find(id);
        var result = _mapper.Map<UpdateArticleDto>(article);
        return View(result);
    }

    [HttpPost]
    [PageInfo("Makaleler", "Makale Güncelle")]
    public IActionResult UpdateArticle(UpdateArticleDto uadto)
    {
        var article = _mapper.Map<Article>(uadto);
        _context.Articles.Update(article);
        _context.SaveChanges();
        return RedirectToAction("ArticleList");
    }

    [HttpGet]
    [PageInfo("Makaleler", "Open AI ile Makale Oluştur")]
    public IActionResult CreateArticleWithOpenAI()
    {
        return View();
    }

    [HttpPost]
    [PageInfo("Makaleler", "Open AI ile Makale Oluştur")]
    public async Task<IActionResult> CreateArticleWithOpenAI(string prompt)
    {
        var apiKey = "YOUR_API_KEY_HERE";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestData = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "Sen bir sigorta şirketi için çalışan, içerik yazarlığı yapan bir yapay zekasın. Kullanıcının verdiği özet ve anahtar kelimelere göre, sigortacılık sektörüyle ilgili makale üret. En az 2000 karakter olsun."
                },
                new
                {
                    role = "user",
                    content = prompt
                }
            },
            temperature = 0.7
        };

        var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
            var content = result.choices[0].message.content;
            ViewBag.article = content;
        }
        else
        {
            ViewBag.article = $"OpenAI API çağrısı başarısız oldu: {response.StatusCode}";
        }

        return View();
    }

    public class OpenAIResponse
    {
        public List<Choice> choices { get; set; }
    }

    public class Choice
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    private void PopulateDropdowns(CreateArticleViewModel cavm)
    {
        cavm.Categories = _context.Categories
            .Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

        cavm.Authors = _context.Users
            .Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.Name + " " + u.Surname
            }).ToList();
    }
}
