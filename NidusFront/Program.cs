using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// =========================================================================
// 1. MARRETADA GLOBAL: Força o C# inteiro a nascer falando PT-BR
// =========================================================================
var cultureInfo = new CultureInfo("pt-BR");
cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
cultureInfo.NumberFormat.NumberGroupSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 2. SERVIÇOS
//    AddControllersWithViews já habilita roteamento por atributo ([Route(...)])
//    além do roteamento MVC convencional.
// =========================================================================
builder.Services.AddControllersWithViews();

// Configurações de Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// =========================================================================
// 3. APLICA A REGRA NAS REQUISIÇÕES WEB
// =========================================================================
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = new[] { cultureInfo },
    SupportedUICultures = new[] { cultureInfo }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

// =========================================================================
// 4. ROTAS
//    MapControllers  → habilita os [Route] dos controllers de API
//    MapControllerRoute → mantém o MVC convencional (Home, Incubadora, etc.)
// =========================================================================
app.MapControllers();   // ← LINHA NOVA: resolve /api/fiware/ligar e /api/fiware/desligar

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();