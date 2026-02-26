using foERP.Application.Abstractions;
using foERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace foERP.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FoErpSqlServer")
                               ?? throw new InvalidOperationException("Connection string 'FoErpSqlServer' is missing.");

        services.AddDbContext<FoErpDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<FoErpDbContext>());

        return services;
    }
}
