using InventoryService.Application.Books.Queries;
using InventoryService.Application.Interfaces;
using InventoryService.Infrastructure.Messaging;
using InventoryService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Scalar.AspNetCore;

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
builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var schemeName = "Bearer";
                
        var securityScheme = new Microsoft.OpenApi.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        };
                
        document.Components ??= new Microsoft.OpenApi.OpenApiComponents();
                
        if (document.Components.SecuritySchemes == null)
        {
            document.Components.SecuritySchemes = new Dictionary<string, Microsoft.OpenApi.IOpenApiSecurityScheme>();
        }
        document.Components.SecuritySchemes[schemeName] = securityScheme;
                
        var requirement = new Microsoft.OpenApi.OpenApiSecurityRequirement();        
        var schemeReference = new Microsoft.OpenApi.OpenApiSecuritySchemeReference(schemeName, document);
        
        requirement[schemeReference] = new List<string>();
                
        document.Security ??= new List<Microsoft.OpenApi.OpenApiSecurityRequirement>();
        document.Security.Add(requirement);

        return Task.CompletedTask;
    });
});

const string allowSpecificOrigins = "bookstore";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{    
    app.MapOpenApi();
    app.MapScalarApiReference(); 
}

app.UseMiddleware<Bookstore.Common.Middleware.GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors(allowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
