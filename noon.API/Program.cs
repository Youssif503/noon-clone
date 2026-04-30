using Microsoft.EntityFrameworkCore;
using noon.Application;
using noon.Application.Repository.Contract;
using noon.Application.Service.Contract;
using noon.Application.Services.Concrete;
using noon.Infrastructure;
using noon.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
