using Redis.OM;
using Redis.OM.Contracts;
using Sirena.Travel.TestTask.Contracts;
using Sirena.Travel.TestTask.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sirena.Travel.TestTask.Impl.Cache
{
    internal class RedisCacheRepository<T> : ICacheRepository<T> where T: notnull
    {
        private readonly IRedisConnectionProvider _provider;

        public RedisCacheRepository(IRedisConnectionProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return _provider.RedisCollection<T>().Where(predicate);
        }

        public async Task<T?> GetAsync(string id)
        {
            return await _provider.RedisCollection<T>().FindByIdAsync(id);
        }

        public async Task InsertAsync(T item, TimeSpan ttl)
        {
            await _provider.RedisCollection<T>().InsertAsync(item, ttl);
        }
    }
}
