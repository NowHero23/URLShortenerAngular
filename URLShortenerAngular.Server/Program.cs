using Microsoft.EntityFrameworkCore;
using URLShortenerAngular.Server.Data;
using URLShortenerAngular.Server.Data.Domain.Repositories.Abstract;
using URLShortenerAngular.Server.Data.Domain.Repositories.EntityFramework;
using URLShortenerAngular.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApiDbContext>(
   builder.Environment.IsStaging() ?
   options => options.UseInMemoryDatabase("InMemoryDb")  //IntegrationTests
   :
   options => options.UseNpgsql(
       builder.Configuration.GetConnectionString("URLShortenerDb")
   ));


// Add repositories
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IUrlItemRepository, EFUrlItemRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
    .WithOrigins("CorsPolicy")
    .AllowAnyHeader()
    .AllowAnyMethod()
    //.AllowAnyOrigin()

    .AllowCredentials()
    );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

public partial class Program { }