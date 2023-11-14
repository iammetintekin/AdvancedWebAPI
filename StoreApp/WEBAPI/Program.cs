using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog; 
using Project.Repositories.EfCore;
using Project.Services.Abstract;
using WEBAPI.Extensions;
using WEBAPIFramework.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

//Nlog config
LogManager
    .Setup()
    .LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
// �rnek bir s�n�f �zerinden tip ��z�mlemesi ger�ekle�tirdik.
builder.Services
    .AddControllers(config=> 
    {
        config.RespectBrowserAcceptHeader = true; // i�erik pazarl���na a��k
        config.ReturnHttpNotAcceptable = true; // kabul etmedi�imiz format gelirse 406 g�nderiyor.
        config.CacheProfiles.Add("5mins", new CacheProfile()
        {
            Duration = 60 * 5 // 300 saniyelik profile
        });
    })
    .AddCustomCsvFormatter() // csv format�nda return edebilece�iz.
    .AddXmlDataContractSerializerFormatters() // xml olarak d�nd�rmeyi sa�lar.
    .AddApplicationPart(typeof(WEBAPIFramework.AssemblyReference).Assembly) //Presentation projesine ba�lad�.
    .AddNewtonsoftJson();
 
builder.Services.Configure<ApiBehaviorOptions>(cfg =>
{
    cfg.SuppressModelStateInvalidFilter = true; // Modelstate invalid oldu�unda varsay�lan return �zelli�i ge�ersiz k�l�yoruz.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache(); // h�z s�n�rlamada gerekli
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureUnitOfWork();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureCors();
builder.Services.ConfigureDataShaper();
builder.Services.AddCustomMediaTypes();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);

// runtime ifadesi bu y�zden webapi tan�mlamas� yap�l�r
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

// servisimizi �a��rd�k
// global hata y�netimi ile controllerda try catch gibi bloklar� kullanm�yoruz.

var logger_service = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger_service);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseHsts(); //Adds security headers
}
app.UseHttpsRedirection();
app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
// corstan sonra �a�r�lmas� tavsiye edilir.
app.UseResponseCaching();
app.UseHttpCacheHeaders(); // cache ile ilgili etag, expires, modified gibi yeni headerler ekler
// -- �nemli
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
