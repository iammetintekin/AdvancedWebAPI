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
// Örnek bir sýnýf üzerinden tip çözümlemesi gerçekleþtirdik.
builder.Services
    .AddControllers(config=> 
    {
        config.RespectBrowserAcceptHeader = true; // içerik pazarlýðýna açýk
        config.ReturnHttpNotAcceptable = true; // kabul etmediðimiz format gelirse 406 gönderiyor.
        config.CacheProfiles.Add("5mins", new CacheProfile()
        {
            Duration = 60 * 5 // 300 saniyelik profile
        });
    })
    .AddCustomCsvFormatter() // csv formatýnda return edebileceðiz.
    .AddXmlDataContractSerializerFormatters() // xml olarak döndürmeyi saðlar.
    .AddApplicationPart(typeof(WEBAPIFramework.AssemblyReference).Assembly) //Presentation projesine baðladý.
    .AddNewtonsoftJson();
 
builder.Services.Configure<ApiBehaviorOptions>(cfg =>
{
    cfg.SuppressModelStateInvalidFilter = true; // Modelstate invalid olduðunda varsayýlan return özelliði geçersiz kýlýyoruz.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache(); // hýz sýnýrlamada gerekli
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

// runtime ifadesi bu yüzden webapi tanýmlamasý yapýlýr
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

// servisimizi çaðýrdýk
// global hata yönetimi ile controllerda try catch gibi bloklarý kullanmýyoruz.

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
// corstan sonra çaðrýlmasý tavsiye edilir.
app.UseResponseCaching();
app.UseHttpCacheHeaders(); // cache ile ilgili etag, expires, modified gibi yeni headerler ekler
// -- önemli
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
