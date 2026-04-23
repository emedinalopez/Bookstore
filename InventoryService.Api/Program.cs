using InventoryService.Application.Books.Queries;
using InventoryService.Application.Interfaces;
using InventoryService.Infrastructure.Messaging;
using InventoryService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("InventoryConnection");
var rabbitMqConnection = builder.Configuration.GetConnectionString("RabbitMqConnection");

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IInventoryDbContext, InventoryDbContext>();

builder.Services.AddSingleton(sp =>
{
    var factory = new ConnectionFactory() { Uri = new Uri(rabbitMqConnection) };
    return factory.CreateConnectionAsync();
});
    
builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetAllBooksQuery).Assembly));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {        
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:ValidAudience"];
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
