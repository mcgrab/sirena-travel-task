using Microsoft.Extensions.Hosting;
using Redis.OM;
using Redis.OM.Contracts;
using Sirena.Travel.TestTask.Contracts.Models;


namespace Sirena.Travel.TestTask.Impl.Cache
{
    public class RedisRouteIndexInitializer : IHostedService
    {
        private readonly IRedisConnectionProvider _redisConnectionProvider;

        public RedisRouteIndexInitializer(IRedisConnectionProvider redisConnectionProvider)
        {
            _redisConnectionProvider = redisConnectionProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _redisConnectionProvider.Connection.CreateIndexAsync(typeof(Route));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
