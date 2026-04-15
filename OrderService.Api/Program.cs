using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Persistence;
using OrderService.Application.Orders.Commands;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OrderConnection");

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IOrderDbContext, OrderDbContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
