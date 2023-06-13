using AutoMapper;
using Sirena.Travel.TestTask.Contracts;
using Sirena.Travel.TestTask.Contracts.Exceptions;
using Sirena.Travel.TestTask.Contracts.Models.ProviderOne;
using Sirena.Travel.TestTask.Contracts.Models;
using Sirena.Travel.TestTask.Impl.Settings;
using System.Net.Http.Json;
using Sirena.Travel.TestTask.Contracts.Models.ProviderTwo;
using Microsoft.Extensions.Logging;

namespace Sirena.Travel.TestTask.Impl.Providers;

internal class ProviderTwoService : IRouteProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ProviderTwoSettings _providerTwoSettings;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public ProviderTwoService(
        IHttpClientFactory httpClientFactory,
        ProviderTwoSettings settings,
        IMapper mapper,
        ILogger<ProviderTwoService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _providerTwoSettings = settings;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task PingAsync(CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(DiExtensions.PROVIDER_TWO_CLIENT_NAME);

        _logger.LogInformation("Проверка доступности провайдера 2.");

        var response = await client.GetAsync(_providerTwoSettings.Ping, cancellationToken);

        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Провайдер 2 доступен.");
    }

    public async Task<Route[]> SearchRouteAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var mappedRequest = _mapper.Map<ProviderTwoSearchRequest>(request);

        var client = _httpClientFactory.CreateClient(DiExtensions.PROVIDER_TWO_CLIENT_NAME);

        var response = await client.PostAsJsonAsync(_providerTwoSettings.Search, mappedRequest, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<ProviderTwoSearchResponse>()
            ?? throw new SirenaTravelException("Не удалось получить ответ от провайдера.");

        return _mapper.Map<Route[]>(result.Routes);
    }
}
