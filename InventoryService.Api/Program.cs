using Microsoft.EntityFrameworkCore;
using InventoryService.Application.Interfaces;
using InventoryService.Infrastructure.Persistence;
using InventoryService.Application.Books.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IInventoryDbContext, InventoryDbContext>();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetAllBooksQuery).Assembly));



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
