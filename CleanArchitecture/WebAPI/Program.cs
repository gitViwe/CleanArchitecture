using Core;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// include database context for application
builder.Services.AddDbContext<APIDbContext>(options =>
{
    // using an SQLite provider
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"));
});

// register JWTConfig configuration section in dependency injection
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// get the JWT key from the APP settings file
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);

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
builder.Services.AddSingleton(tokenValidationParams);

// configures authentication using JWT
builder.Services.AddAuthentication(options =>
{
    // specify default schema
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // store bearer token on successful authentication
    options.SaveToken = true;
    // set the parameters used to validate
    options.TokenValidationParameters = tokenValidationParams;
});

// configures authentication to use identity cookies
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // require account to sign in
    options.SignIn.RequireConfirmedAccount = true;
    // password requirements
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<APIDbContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // add swagger documentation
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebAPI",
        Description = "A .NET 6 web API demo project to showcase clean architecture",
        Version = "v0.5",
        Contact = new OpenApiContact()
        {
            Name = "Viwe Nkepu",
            Email = "viwe.nkepu@hotmail.com"
        }
    });

    // create authorization scheme to swagger UI
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
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

// add CORS policy
builder.Services.AddCors(opions =>
{
    // TODO: open policy for testing only
    opions.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // specify the swagger endpoint
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v0.5");
    });
}

app.UseHttpsRedirection();

// use routing
app.UseRouting();

// use the CORS policy as defined above
app.UseCors("Open");

// add authentication middle-ware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
