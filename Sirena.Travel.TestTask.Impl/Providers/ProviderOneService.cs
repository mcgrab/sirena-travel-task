using AutoMapper;
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

    public ProviderOneService(
        IHttpClientFactory httpClientFactory, 
        ProviderOneSettings settings,
        IMapper mapper)
    {
        _httpClientFactory = httpClientFactory;
        _providerOneSettings = settings;
        _mapper = mapper;
     }

    public async Task PingAsync(CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(DiExtensions.PROVIDER_ONE_CLIENT_NAME);

        var response = await client.GetAsync(_providerOneSettings.Ping, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task<Route[]> SearchRouteAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var mappedRequest = _mapper.Map<ProviderOneSearchRequest>(request);

        var client = _httpClientFactory.CreateClient(DiExtensions.PROVIDER_ONE_CLIENT_NAME);

        var response = await client.PostAsJsonAsync(_providerOneSettings.Search, mappedRequest, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>()
            ?? throw new SirenaTravelException("Не удалось получить ответ от провайдера.");

        return _mapper.Map<Route[]>(result.Routes);
    }
}
