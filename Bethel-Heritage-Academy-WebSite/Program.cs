using Bethel_Heritage_Academy_WebSite.Services;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess.Data;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// Services Registration
// ==========================

// Razor Pages
builder.Services.AddRazorPages();

// API Controllers (IMPORTANT – required for your v1Controller)
builder.Services.AddControllers();

// CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAdmin",
        policy =>
        {
            policy.WithOrigins("https://admin.bethelheritageacademy.com.ng",
                "https://localhost:7101", "https://localhost:44379"
                )
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Database
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connection, ServerVersion.AutoDetect(connection));
});

// Dependency Injection
builder.Services.AddHttpClient();
builder.Services.AddScoped<IRecaptchaService, RecaptchaService>();
builder.Services.AddScoped<IFeedBackRepository, FeedBackRepository>();
builder.Services.AddScoped<IImageManager, ImageManager>();

var app = builder.Build();

// ==========================
// Middleware Pipeline
// ==========================

// CORS (must come before routing endpoints)
app.UseCors("AllowAdmin");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ==========================
// Endpoint Mapping
// ==========================

app.MapRazorPages();      // Razor Pages
app.MapControllers();     // API Controllers (VERY IMPORTANT)

app.Run();