using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.ArticleDtos;

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

    public IActionResult ArticleList()
    {
        var articles = _context.Articles.ToList();
        var result = _mapper.Map<List<ResultArticleDto>>(articles);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateArticle()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateArticle(CreateArticleDto cadto)
    {
        cadto.CreatedTime = DateTime.Now;
        var article = _mapper.Map<Article>(cadto);
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
    public IActionResult UpdateArticle(int id)
    {
        var article = _context.Articles.Find(id);
        var result = _mapper.Map<UpdateArticleDto>(article);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateArticle(UpdateArticleDto uadto)
    {
        var article = _mapper.Map<Article>(uadto);
        _context.Articles.Update(article);
        _context.SaveChanges();
        return RedirectToAction("ArticleList");
    }

    [HttpGet]
    public IActionResult CreateArticleWithOpenAI()
    {
        return View();
    }

    [HttpPost]
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
}
