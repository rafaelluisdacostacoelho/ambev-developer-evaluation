using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    options =>
                    {
                        options.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM");
                        options.UseNetTopologySuite();
                    }
                );

                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging(); // Apenas em DEV, útil para debug
                    options.EnableDetailedErrors();
                }
            });

            // O DbContextFactory é útil para cenários onde é necessário criar instâncias manuais do contexto, 
            // como em jobs, background services ou processamento paralelo, evitando problemas de concorrência.
            // É recomendado manter tanto o AddDbContext quanto o AddDbContextFactory quando necessário, 
            // pois cada um possui um propósito distinto: 
            // - AddDbContext: Injeta o DbContext como Scoped, ideal para requisições HTTP na API.
            // - AddDbContextFactory: Permite a criação de instâncias independentes do DbContext sob demanda.
            // Exemplo de configuração do DbContextFactory:
            // builder.Services.AddDbContextFactory<StoreDbContext>(options =>
            // {
            //     options.UseNpgsql(
            //         builder.Configuration.GetConnectionString("DefaultConnection"),
            //         options =>
            //         {
            //             options.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM");
            //             options.UseNetTopologySuite();
            //         }
            //     );
            // });

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddTransient<IRequest<PaginatedResponse<ListUserResponse>>, PaginationQuery<ListUsersFilter, ListUserResponse>>();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();
            app.UseMiddleware<ValidationExceptionMiddleware>();

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
    }
}
