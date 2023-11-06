using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Project.Entity.DTOs.Product;
using Project.Repositories.EfCore;
using Project.Repositories.Repository.UnitOfWork;
using Project.Services;
using Project.Services.Abstract;
using Project.Services.ServiceManager;
using WEBAPIFramework.ActionFilters;
using WEBAPIFramework.Controllers;

namespace WEBAPI.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Ioc: database connection saved.  when we need to connect DBContext it return DatabaseContext
            services.AddDbContext<DatabaseContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("databaseConnection")));
        }
        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }
        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            // her servis için ayrı scopping yapmıyoruz. bütün servislerimizi burada topladık.
            services.AddScoped<IServiceManager, ServiceManager>(); 
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            // singleton kullanılacak
            services.AddSingleton<ILoggerService,LoggerService>();
        }

        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>(); // her kullanıcıda farklı nesne üretir.
            services.AddSingleton<LogFilterAttribute>();
            services.AddScoped<ValidateMediaTypeAttribute>();
        }

        //Cross origin
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Pagination");
                });
            });
        }

        public static void ConfigureDataShaper(this IServiceCollection services)
        {
            // her entity için ayrı oluşturulacak
            services.AddScoped<IDataShaper<ProductDto>,DataShaper<ProductDto>>();
        }

        //linq of type araştır.
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config=>
            {
                // media type added
                // koleksiyondan seçim işlemi yapıldı.
                var systemTextJsonOutputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>().FirstOrDefault();
                if(systemTextJsonOutputFormatter is not null)
                {
                 //   systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/usr.metintekin.hateoas+json");
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/usr.metintekin.apiroot+json");
                }

                var xmlOutputFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>().FirstOrDefault();
                if (xmlOutputFormatter is not null)
                {
                 //   xmlOutputFormatter.SupportedMediaTypes.Add("application/usr.metintekin.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/usr.metintekin.apiroot+xml");
                }
            });
        } 
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options => {
                options.ReportApiVersions=true;
                options.AssumeDefaultVersionWhenUnspecified=true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");

                options.Conventions.Controller<ProductsController>()
               .HasApiVersion(new ApiVersion(1, 0));

                options.Conventions.Controller<ProductsV2Controller>()
                .HasDeprecatedApiVersion(new ApiVersion(2, 0));
            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            // belli metodlarda override edilebilir ve değiştirilebilir bir yapısı vardır.
            services.AddHttpCacheHeaders(expirationOptions =>
            {
                expirationOptions.MaxAge = 70;
                expirationOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Public;
            },
            validationOptions =>
            {
                validationOptions.MustRevalidate = false; // yeniden alidation zorunluluğu yok.
            }
            );
        }
    }
}
