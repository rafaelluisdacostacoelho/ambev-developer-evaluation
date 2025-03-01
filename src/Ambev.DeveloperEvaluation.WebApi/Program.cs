using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do Serilog (Logging global desde o in�cio)
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

// Configura��o de Controllers e Swagger
builder.Services.AddControllers();
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

// Configura��o do DbContext
builder.Services.AddDbContext<StoreDbContext>(options =>
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
});

// Adicionar DbContextFactory (�til para processamento paralelo, background jobs, etc.)
builder.Services.AddSingleton<IDbContextFactory<StoreDbContext>>(provider =>
{
    var options = provider.GetRequiredService<DbContextOptions<StoreDbContext>>();
    return new PooledDbContextFactory<StoreDbContext>(options);
});

// Autentica��o JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Inje��o de depend�ncias via IoC
builder.RegisterDependencies();

// Configura��o do AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

// Configura��o do MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(ApplicationLayer).Assembly)
);

// Adicionando Pipeline de valida��o
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Constru��o da aplica��o
var app = builder.Build();

// Middleware de valida��o
app.UseMiddleware<ValidationExceptionMiddleware>();

// Configura��o de middlewares principais
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseBasicHealthChecks();

app.MapControllers();

// Inicializa a aplica��o e captura exce��es
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
