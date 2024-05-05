using FlaschenpostAPI.Data.Repos;
using FlaschenpostAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IProductService, ProductService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();