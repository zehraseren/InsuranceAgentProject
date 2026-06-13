using InsureYouAI.Hubs;
using InsureYouAI.Filters;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Configuration;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient
builder.Services.AddHttpClient("groq", c =>
{
    c.BaseAddress = new Uri("https://api.groq.com/openai/");
});

// Register SignalR
builder.Services.AddSignalR();

// Register DbContext
builder.Services.AddDbContext<InsureContext>();

// Identity Configuration
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<InsureContext>()
    .AddDefaultTokenProviders();

// Configure AutoMapper
builder.Services.AddMappings();

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<PageInfoFilter>();
});

var app = builder.Build();

app.UseExceptionHandler("/Error/500");
app.UseStatusCodePagesWithReExecute("/Error/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
