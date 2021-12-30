using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddWebAPIServices();
builder.Services.AddWebAPIDatabase(builder.Configuration);
builder.Services.AddWebAPISections(builder.Configuration);
builder.Services.AddWebAPIJWTAuthentication(builder.Configuration);
builder.Services.AddWebAPIIdentity();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddWebAPISwagger(builder.Configuration);
builder.Services.AddWebAPICors(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseWebAPISwagger();

app.UseHttpsRedirection();

// use routing
app.UseRouting();

// use the CORS policy as defined above 'AddWebAPICors'
app.UseCors();

// add authentication middle-ware
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
