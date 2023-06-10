using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sirena.Travel.TestTask.Contracts
{
    public interface IPingProvider
    {
        Task PingAsync(CancellationToken cancellationToken);
    }
}
