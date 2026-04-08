using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Services;

var builder = WebApplication.CreateBuilder(args);

var configBasePath = builder.Environment.ContentRootPath;
if (!File.Exists(Path.Combine(configBasePath, "appsettings.json")))
{
    var srcConfigPath = Path.Combine(configBasePath, "src");
    if (File.Exists(Path.Combine(srcConfigPath, "appsettings.json")))
    {
        configBasePath = srcConfigPath;
    }
}

builder.Configuration
    .SetBasePath(configBasePath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("DefaultConnection is not configured.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
// 1. サービスの登録
builder.Services.AddControllersWithViews(); // APIとView両方対応

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSpaStaticFiles(configuration =>
    {
        configuration.RootPath = "../vue/dist";
    });
}
else
{
    builder.Services.AddSpaStaticFiles(configuration =>
    {
        configuration.RootPath = "wwwroot";
    });
}

builder.Services.AddScoped<ITestService,TestService>();

var app = builder.Build();

// --- ミドルウェアの順序が重要 ---

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

// if (!app.Environment.IsDevelopment())
// {
//     app.UseSpaStaticFiles();
// }

app.UseStaticFiles();
app.UseSpaStaticFiles();
if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.UseRouting();

// 認証・認可はルーティングの後、エンドポイントの前
app.UseAuthorization();

app.UseEndpoints(endpoint => { });

// 2. エンドポイントの登録（APIやMVCを先に判定させる）
app.MapControllers();

// 3. SPAミドルウェア（最後に配置！）
// これにより、API等のルートに不一致だったリクエストがVue側に流れます
app.UseSpa(spa =>
{
    if (app.Environment.IsDevelopment())
    {
        // Docker環境などで localhost が使えない場合は、サービス名（例: http://vue:8888）を確認してください
        spa.UseProxyToSpaDevelopmentServer("http://localhost:8888"); 
    }
});

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();