using MimeKit;
using AutoMapper;
using System.Text;
using MailKit.Net.Smtp;
using System.Text.Json;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using System.Text.Json.Nodes;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.MessageDtos;

namespace InsureYouAI.Controllers;

public class DefaultController : Controller
{
    private readonly IMapper _mapper;
    private readonly InsureContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public DefaultController(IMapper mapper, InsureContext context, IHttpClientFactory httpClientFactory)
    {
        _mapper = mapper;
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public PartialViewResult SendMessage()
    {
        return PartialView();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(CreateMessageDto cmdto)
    {
        // cmdto → Message → Database
        var message = _mapper.Map<Message>(cmdto);
        message.SendDate = DateTime.Now;
        message.IsRead = false;
        _context.Messages.Add(message);
        _context.SaveChanges();

        #region GroqAI_Analysis

        var apiKey = "YOUR_API_KEY_HERE";
        var prompt = $"Sen bir sigorta firmasının müşteri iletişim asistanısın.\r\n\r\nKurumsal ama samimi, net ve anlaşılır bir dille yaz.\r\n\r\nYanıtlarını 2–3 paragrafla sınırla.\r\n\r\nEksik bilgi (poliçe numarası, kimlik vb.) varsa kibarca talep et.\r\n\r\nFiyat, ödeme, teminat gibi kritik konularda kesin rakam verme, müşteri temsilcisine yönlendir.\r\n\r\nHasar ve sağlık gibi hassas durumlarda empati kur.\r\n\r\nCevaplarını teşekkür ve iyi dilekle bitir.\r\n\r\n Kullanıcının sana gönderdiği mesaj şu şekilde:' {message.MessageDetail}.'";

        var client = _httpClientFactory.CreateClient("groq");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model = "llama-3.3-70b-versatile",
            max_tokens = 1000,
            temperature = 0.5,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("v1/chat/completions", jsonContent);
        var responseString = await response.Content.ReadAsStringAsync();

        var json = JsonNode.Parse(responseString);
        string? textContent = json?["choices"]?[0]?["message"]?["content"]?.ToString();

        #endregion

        #region SendEmail
        MimeMessage mimeMessage = new();
        mimeMessage.From.Add(new MailboxAddress("InsureYouAI Admin", "fatmazehraseren@gmail.com"));
        mimeMessage.To.Add(new MailboxAddress("User", cmdto.Email));

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = textContent;
        mimeMessage.Body = bodyBuilder.ToMessageBody();
        mimeMessage.Subject = "InsureYouAI Email Yanıtı";

        SmtpClient smtpClient = new();
        smtpClient.Connect("smtp.gmail.com", 587, false);
        smtpClient.Authenticate("fatmazehraseren@gmail.com", "GOOGLE_APP_PASSWORD");
        smtpClient.Send(mimeMessage);
        smtpClient.Disconnect(true);

        #endregion

        #region GroqAIMessage_SaveDB

        var aiMessage = new GroqAIMessage
        {
            MessageDetail = textContent,
            ReceiveEmail = cmdto.Email,
            ReceiveNameSurname = cmdto.NameSurname,
            SendDate = DateTime.Now
        };

        _context.GroqAIMessages.Add(aiMessage);
        _context.SaveChanges();

        #endregion

        return RedirectToAction("Index");
    }

    public PartialViewResult SubscribeEmail()
    {
        return PartialView();
    }

    [HttpPost]
    public IActionResult SubscribeEmail(string email)
    {
        return View();
    }

}
