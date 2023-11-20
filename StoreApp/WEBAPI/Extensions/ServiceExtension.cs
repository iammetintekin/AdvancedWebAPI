using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Project.Entity.DTOs.Product;
using Project.Entity.Models;
using Project.Repositories.EfCore;
using Project.Repositories.Repository.UnitOfWork;
using Project.Services;
using Project.Services.Abstract;
using Project.Services.ServiceManager;
using System.Text;
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

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
                // 1 dakikada max 60 istek atmaya izin veriyor
                new RateLimitRule
                {
                    Endpoint="",
                    Limit = 60,
                    Period = "1m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DatabaseContext>()
              .AddDefaultTokenProviders();
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSeciton = configuration.GetSection("JwtSettings");

            var secretKey = jwtSeciton["secretKey"];
            var validIssuer = jwtSeciton["validIssuer"];
            var validAudience = jwtSeciton["validAudience"];

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,
                    IssuerSigningKey = new  SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>{
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Product API V1", Version="v1" });
                s.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Product API V2", Version = "v2" });

                s.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Lütfen JWT bearer kullanınız",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Name = "Bearer"
                        },
                        new List<string>(){"Bearer"}
                    }
                });
            });
        }

    }
}
