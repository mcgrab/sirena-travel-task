using Sirena.Travel.TestTask.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sirena.Travel.TestTask.Contracts
{
    public interface IRouteProvider : IPingProvider
    {
        Task<Route[]> SearchRouteAsync(SearchRequest request, CancellationToken cancellationToken);
    }
}
