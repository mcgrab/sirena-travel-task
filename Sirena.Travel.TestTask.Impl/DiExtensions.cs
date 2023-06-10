using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sirena.Travel.TestTask.Contracts;
using Sirena.Travel.TestTask.Impl.Services;
using Sirena.Travel.TestTask.Impl.Settings;


namespace Sirena.Travel.TestTask.Impl
{
    public static class DiExtensions
    {
        public const string PROVIDER_ONE_CLIENT_NAME = "ProviderOneClient";
        public const string PROVIDER_TWO_CLIENT_NAME = "ProviderTwoClient";

        public static IServiceCollection AddSearchService(this IServiceCollection services)
            => services.AddScoped<ISearchService, SearchService>();

        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var firstProviderSettings = configuration.GetSection("ProviderOneSettings").Get<ProviderOneSettings>() 
                ?? throw new ArgumentException($"Не найдена конфигурация для {nameof(ProviderOneSettings)}");
            
            var secondProviderSettings = configuration.GetSection("ProviderOneSettings").Get<ProviderTwoSettings>()
                ?? throw new ArgumentException($"Не найдена конфигурация для {nameof(ProviderTwoSettings)}");

            services.AddSingleton(firstProviderSettings);
            services.AddSingleton(secondProviderSettings);

            services.AddHttpClient(PROVIDER_ONE_CLIENT_NAME, client =>
            {
                client.BaseAddress = firstProviderSettings.Host;
            });

            services.AddHttpClient(PROVIDER_TWO_CLIENT_NAME, client =>
            {
                client.BaseAddress = secondProviderSettings.Host;
            });

            return services;
        }
    }
}
