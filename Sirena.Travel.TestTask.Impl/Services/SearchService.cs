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

        public SearchService(
            IEnumerable<IRouteProvider> routeProviders,
            IMapper mapper)
        {
            _routeProviders = routeProviders;
            _mapper = mapper;
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
            => await CheckAnyProviderAvailableAsync(cancellationToken);

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var routes = new ConcurrentBag<Route>();

            var routeTasks = _routeProviders.Select(provider => Task.Factory.StartNew(async () =>
            {
                try
                {
                    var currentRoutes = await provider.SearchRouteAsync(request, cancellationToken);
                    foreach (var route in currentRoutes)
                    {
                        routes.Add(route);
                    }
                }
                catch(Exception ex)
                {
                    //
                }

            }));

            await Task.WhenAll(routeTasks);

            return _mapper.Map<SearchResponse>(routes);
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
