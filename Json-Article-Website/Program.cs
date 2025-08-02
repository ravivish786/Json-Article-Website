using Json_Article_Website.BGS;
using Json_Article_Website.Interface;
using Json_Article_Website.Service;
using Json_Article_Website.Service.Views;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true; // Optional: Makes URLs lowercase
    options.LowercaseQueryStrings = true; // Optional: Makes query strings lowercase
    options.AppendTrailingSlash = false; // Optional: Appends a trailing slash to URLs
});

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

builder.Services.AddScoped<IArticleService, ArticleService>();

// Register the view counter service and background service
builder.Services.AddSingleton<IViewCounterService, InMemoryViewCounterService>();
builder.Services.AddHostedService<ViewUpdateBackgroundService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
