using Microsoft.Extensions.DependencyInjection;

namespace foERP.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // 注册 CQRS、校验器、领域事件处理器等
        return services;
    }
}
