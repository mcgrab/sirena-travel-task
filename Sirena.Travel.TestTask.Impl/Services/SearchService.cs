using AutoMapper;
using Sirena.Travel.TestTask.Contracts;
using Sirena.Travel.TestTask.Contracts.Exceptions;
using Sirena.Travel.TestTask.Contracts.Models;
using System.Collections.Concurrent;

namespace Sirena.Travel.TestTask.Impl.Services
{
    internal class SearchService : ISearchService
    {
        private IEnumerable<IRouteProvider> _routeProviders;
        private IMapper _mapper;
        private ICacheRepository<Route> _cachedRoutes;

        public SearchService(
            IEnumerable<IRouteProvider> routeProviders,
            IMapper mapper,
            ICacheRepository<Route> cachedRoutes)
        {
            _routeProviders = routeProviders;
            _mapper = mapper;
            _cachedRoutes = cachedRoutes;
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
            => await CheckAnyProviderAvailableAsync(cancellationToken);

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            if (request.Filters?.OnlyCached == true)
            {
                bool filter(Route route) =>
                    route.Origin == request.Origin &&
                    route.Destination == request.Destination &&
                    route.OriginDateTime >= request.OriginDateTime
                    && (!request.Filters.DestinationDateTime.HasValue || route.DestinationDateTime <= request.Filters.DestinationDateTime)
                    && (!request.Filters.MaxPrice.HasValue || route.Price <= request.Filters.MaxPrice)
                    && (!request.Filters.MinTimeLimit.HasValue || route.TimeLimit >= request.Filters.MinTimeLimit);

                var cached = _cachedRoutes.GetAll(filter);

                return _mapper.Map<SearchResponse>(cached);
            }

            var routes = new ConcurrentBag<Route>();

            bool hasAnyProviderResults = false;

            var routeTasks = _routeProviders.Select(provider => Task.Factory.StartNew(async () =>
            {
                try
                {
                    var currentRoutes = await provider.SearchRouteAsync(request, cancellationToken);
                    hasAnyProviderResults = true;
                    foreach (var route in currentRoutes)
                    {
                        routes.Add(route);
                    }
                }
                catch(Exception)
                {
                    // Log and Handle later
                }

            }));

            await Task.WhenAll(routeTasks);

            if (!hasAnyProviderResults)
                throw new SirenaTravelException("Не удалось получить информацию от провайдеров.");

            await CacheRoutesAsync(routes);

            return _mapper.Map<SearchResponse>(routes);
        }

        private async Task CacheRoutesAsync(IEnumerable<Route> routes)
        {
            foreach (var route in routes)
            {
                await _cachedRoutes.InsertAsync(route, route.TimeLimit - DateTime.UtcNow);
            }
        }


        private async Task<bool> CheckAnyProviderAvailableAsync(CancellationToken cancellationToken)
        {
            var pingTasks = _routeProviders.Select(provider => Task.Factory.StartNew(async () =>
            {
                 try
                 {
                     await provider.PingAsync(cancellationToken);
                     return true;
                 }
                 catch (Exception)
                 {
                     return false;
                 }
            })).ToList();

            while (pingTasks.Count > 0)
            {
                var completed = await Task.WhenAny(pingTasks);
                
                if (completed.Result.Result)
                {
                    return true;
                }

                pingTasks.Remove(completed);
            }

            return false;
        }
    }
}
