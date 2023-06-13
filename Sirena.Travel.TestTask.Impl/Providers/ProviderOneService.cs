using AutoMapper;
using Microsoft.Extensions.Logging;
using Sirena.Travel.TestTask.Contracts;
using Sirena.Travel.TestTask.Contracts.Exceptions;
using Sirena.Travel.TestTask.Contracts.Models;
using Sirena.Travel.TestTask.Contracts.Models.ProviderOne;
using Sirena.Travel.TestTask.Impl.Settings;
using System.Net.Http.Json;

namespace Sirena.Travel.TestTask.Impl.Providers;

internal class ProviderOneService : IRouteProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ProviderOneSettings _providerOneSettings;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public ProviderOneService(
        IHttpClientFactory httpClientFactory, 
        ProviderOneSettings settings,
        IMapper mapper,
        ILogger<ProviderOneService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _providerOneSettings = settings;
        _mapper = mapper;
        _logger = logger;
     }

    public async Task PingAsync(CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(DiExtensions.PROVIDER_ONE_CLIENT_NAME);

        _logger.LogInformation("Проверка доступности провайдера 1.");

        var response = await client.GetAsync(_providerOneSettings.Ping, cancellationToken);

        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Провайдер 1 доступен.");
    }

    public async Task<Route[]> SearchRouteAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var mappedRequest = _mapper.Map<ProviderOneSearchRequest>(request);

        var client = _httpClientFactory.CreateClient(DiExtensions.PROVIDER_ONE_CLIENT_NAME);

        _logger.LogInformation("Поиск маршрутов в провайдере 1.");

        var response = await client.PostAsJsonAsync(_providerOneSettings.Search, mappedRequest, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>()
            ?? throw new SirenaTravelException("Не удалось получить ответ от провайдера.");

        return _mapper.Map<Route[]>(result.Routes);
    }
}
