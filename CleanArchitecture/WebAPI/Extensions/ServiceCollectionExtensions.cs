using Core.Configuration;
using Infrastructure;
using Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Constant.Application;
using Shared.Constant.Message;
using Shared.Constant.Permission;
using Shared.Wrapper;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace WebAPI.Extensions
{
    /// <summary>
    /// Implementation of the services registered in the <see cref="Program"/> class
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the database context for the application
        /// </summary>
        internal static IServiceCollection AddWebAPIDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddDbContext<APIDbContext>(options =>
            {
                // using an SQLite provider
                options.UseSqlite(configuration.GetConnectionString(ConnectionString.SQLite), b => b.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name));
            });
        }

        /// <summary>
        /// Registers JWT authentication for the application
        /// </summary>
        internal static IServiceCollection AddWebAPIJWTAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // get the JWT key from the APP settings file
            var key = Encoding.ASCII.GetBytes(configuration[Configuration.Secret]);

            // create the parameters used to validate
            var tokenValidationParams = new TokenValidationParameters
            {
                // validates the signature of the key
                ValidateIssuerSigningKey = true,
                // specify the security key used for 
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false
            };

            // add Token Validation Parameters as singleton service
            services.AddSingleton(tokenValidationParams);

            // configures authentication using JWT
            services.AddAuthentication(options =>
            {
                // specify default schema
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // store bearer token on successful authentication
                options.SaveToken = true;
                // set the parameters used to validate
                options.TokenValidationParameters = tokenValidationParams;
                // set JWT authorization events
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        // JWT token has expired
                        if (c.Exception is SecurityTokenExpiredException)
                        {
                            c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            c.Response.ContentType = MediaTypeNames.Application.Json;
                            var result = JsonSerializer.Serialize(Result.Fail(AuthorizationError.ExpiredToken));
                            return c.Response.WriteAsync(result);
                        }
                        // unhandled server error
                        else
                        {
                            c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            c.Response.ContentType = MediaTypeNames.Application.Json;
                            var result = JsonSerializer.Serialize(Result.Fail(AuthorizationError.InternalServerError));
                            return c.Response.WriteAsync(result);
                        }
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = MediaTypeNames.Application.Json;
                            var result = JsonSerializer.Serialize(Result.Fail(AuthorizationError.Unauthorized));
                            return context.Response.WriteAsync(result);
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        var result = JsonSerializer.Serialize(Result.Fail(AuthorizationError.Forbidden));
                        return context.Response.WriteAsync(result);
                    },
                };
            });

            // add authorization to apply policy
            services.AddAuthorization(options =>
            {
                // get all permissions from static properties
                foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
                {
                    // get property value
                    var propertyValue = prop.GetValue(null);

                    if (propertyValue is not null)
                    {
                        // add new permission policy
                        options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString())
                               // add JWT Bearer authentication scheme to this policy
                               .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                    }
                }
            });

            return services;
        }

        /// <summary>
        /// Registers Microsoft Identity for the application
        /// </summary>
        internal static IServiceCollection AddWebAPIIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppIdentityUser, IdentityRole>(options =>
            {
                // require account to sign in
                options.SignIn.RequireConfirmedAccount = true;
                // password requirements
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<APIDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        /// <summary>
        /// Registers Open API / Swagger for the application
        /// </summary>
        internal static IServiceCollection AddWebAPISwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // add swagger documentation
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebAPI",
                    Description = "A .NET 6 web API demo project to showcase clean architecture",
                    Version = "v1.0",
                    Contact = new OpenApiContact()
                    {
                        Name = "Viwe Nkepu",
                        Email = "viwe.nkepu@hotmail.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // create authorization scheme to swagger UI
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                // add authorization scheme to swagger UI
                options.AddSecurityDefinition("Bearer", securitySchema);

                // create security requirements
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                // add security requirements to swagger UI
                options.AddSecurityRequirement(securityRequirement);

                // get the file path for XML documentation
                var fileName = typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml";
                var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, fileName);
                // add XML documentation to swagger UI
                options.IncludeXmlComments(xmlCommentsFilePath, true);
            });

            return services;
        }

        /// <summary>
        /// Registers Cross Origin Resource Sharing for the application
        /// </summary>
        internal static IServiceCollection AddWebAPICors(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // add CORS policy https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            // allow requests from this URL
                            .WithOrigins(configuration[Configuration.ServerUrl].TrimEnd('/'), configuration[Configuration.ApplicationUrl].TrimEnd('/'));
                    });
            });

            return services;
        }

        /// <summary>
        /// Registers AppSettings configuration section in dependency injection
        /// </summary>
        internal static IServiceCollection AddWebAPISections(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<AppConfiguration>(configuration.GetSection(nameof(AppConfiguration)));
            services.Configure<MongoDBConfiguration>(configuration.GetSection(nameof(MongoDBConfiguration)));

            return services;
        }

        /// <summary>
        /// Registers Infrastructure services in dependency injection
        /// </summary>
        internal static IServiceCollection AddWebAPIServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
