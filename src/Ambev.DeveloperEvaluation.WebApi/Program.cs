using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Services.Interfces;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog (Logging global desde o início)
builder.AddDefaultLogging();

// Configuração de Controllers e Swagger
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Add swagger configurations
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// Health Checks
builder.AddBasicHealthChecks();

// DbContext Configuration
builder.Services.AddDbContext<StoreDbContext>(options =>
{
    if (builder.Environment.IsEnvironment("Testing"))
    {
        options.UseInMemoryDatabase("TestDb");
    }
    else
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM");
                npgsqlOptions.UseNetTopologySuite();
            }
        );

        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    }

    if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Testing"))
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Add DbContextFactory (Useful for parallel processing, background jobs, etc.)
builder.Services.AddSingleton<IDbContextFactory<StoreDbContext>>(provider =>
{
    var options = provider.GetRequiredService<DbContextOptions<StoreDbContext>>();
    return new PooledDbContextFactory<StoreDbContext>(options);
});

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Dependency injection via IoC
await builder.RegisterDependencies();


// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

builder.Services.AddScoped<IProductPriceService, ProductPriceService>();

// Add event publisher pipeline
builder.Services.AddScoped(typeof(IDomainEventDispatcher<>), typeof(DomainEventDispatcher<>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DomainEventPublisherBehavior<,>));


// MediatR Configuration
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(ApplicationLayer).Assembly)
);

// Adding a validation pipeline, but it did not prove to be a flexible and robust option, using attributes proved to be more efficient
// builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Building the application
var app = builder.Build();

// Logging Middleware
app.UseMiddleware<LoggingMiddleware>();

// Validation and Exception middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

// Main middleware configuration
app.UseRouting();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing"))
{
    app.UseDeveloperExceptionPage(); // Exibe detalhes do erro durante os testes
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseBasicHealthChecks();

app.MapControllers();

// Initialize the application and catch exceptions
try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { } // Used specifically for integration testing