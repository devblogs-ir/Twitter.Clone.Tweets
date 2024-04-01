using Mapster;
using MapsterMapper;
using System.Reflection;

namespace Twitter.Clone.Tweets.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigurMapster(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }
}
