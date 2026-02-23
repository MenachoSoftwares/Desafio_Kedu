using Kedu.API.Converters;
using Kedu.API.GraphQL.Mutations;
using Kedu.API.GraphQL.Queries;
using Kedu.API.GraphQL.Types;
using Kedu.Application.Interfaces;
using Kedu.Application.Services;
using Kedu.Domain.Interfaces;
using Kedu.Infrastructure.Data;
using Kedu.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kedu.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddApplication();
        services.AddPresentation();

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null)));

        services.AddScoped<IResponsavelRepository, ResponsavelRepository>();
        services.AddScoped<ICentroDeCustoRepository, CentroDeCustoRepository>();
        services.AddScoped<IPlanoRepository, PlanoRepository>();
        services.AddScoped<ICobrancaRepository, CobrancaRepository>();

        return services;
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IResponsavelService, ResponsavelService>();
        services.AddScoped<ICentroDeCustoService, CentroDeCustoService>();
        services.AddScoped<IPlanoService, PlanoService>();
        services.AddScoped<ICobrancaService, CobrancaService>();

        return services;
    }

    private static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new FlexibleDateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Kedu Payment Plans API", Version = "v1" });
        });

        services
            .AddGraphQLServer()
            .AddQueryType<RootQuery>()
            .AddMutationType<RootMutation>()
            .AddType<PlanoType>()
            .AddType<CobrancaType>()
            .AddType<ResponsavelType>()
            .AddType<CobrancaFiltroInputType>();

        return services;
    }
}
