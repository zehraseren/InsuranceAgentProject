using System.Text.Json;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers;

public class BlogController : Controller
{
    private readonly InsureContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public BlogController(InsureContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult BlogList()
    {
        return View();
    }

    public PartialViewResult SearchBlogs()
    {
        return PartialView();
    }
    [HttpPost]
    public IActionResult SearchBlogs(string keyword)
    {
        return PartialView();
    }

    public IActionResult BlogDetail(int id)
    {
        return View(id);
    }

    [HttpGet]
    public PartialViewResult AddComment()
    {
        return PartialView();
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(Comment comment)
    {
        comment.CommentDate = DateTime.Now;
        comment.AppUserId = "8dcfb6c9-9620-40d9-8060-1e702870d001";

        var client = _httpClientFactory.CreateClient();


        var apiKey = "YOUR_API_KEY_HERE";
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        try
        {
            var translateRequestBody = new
            {
                inputs = comment.CommentDetail
            };

            var translateJson = JsonSerializer.Serialize(translateRequestBody);
            var tanslateContent = new StringContent(translateJson, System.Text.Encoding.UTF8, "application/json");

            var translateResponse = await client.PostAsync("https://router.huggingface.co/hf-inference/models/Helsinki-NLP/opus-mt-tc-big-tr-en", tanslateContent);
            var translateResponseString = await translateResponse.Content.ReadAsStringAsync();

            string englishText = comment.CommentDetail;
            if (translateResponseString.TrimStart().StartsWith("["))
            {
                var translateDoc = JsonDocument.Parse(translateResponseString);
                englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString();
            }

            var toxicRequestBody = new
            {
                inputs = englishText
            };

            var toxicJson = JsonSerializer.Serialize(toxicRequestBody);
            var toxicContent = new StringContent(toxicJson, System.Text.Encoding.UTF8, "application/json");
            var toxicResponse = await client.PostAsync("https://api-inference.huggingface.co/models/unitary/toxic-bert", toxicContent);
            var toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();

            if (toxicResponseString.TrimStart().StartsWith("["))
            {
                var toxicDoc = JsonDocument.Parse(toxicResponseString);
                foreach (var item in toxicDoc.RootElement[0].EnumerateArray())
                {
                    string label = item.GetProperty("label").GetString();
                    double score = item.GetProperty("score").GetDouble();

                    if (score > 0.5)
                    {
                        comment.CommentStatus = "Toksik Yorum";
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(comment.CommentStatus))
            {
                comment.CommentStatus = "Yorum Onaylandı";
            }
        }
        catch (Exception ex)
        {
            comment.CommentStatus = "Onay Bekliyor";
        }

        _context.Comments.Add(comment);
        _context.SaveChanges();

        return RedirectToAction("BlogList");

    }
}