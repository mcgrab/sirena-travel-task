using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sirena.Travel.TestTask.Contracts
{
    public interface ICacheRepository<T> where T: notnull
    {
        Task InsertAsync(T item, TimeSpan ttl);

        Task<T?> GetAsync(string id);

        IEnumerable<T> GetAll(Func<T, bool> predicate);
    }
}
