using Sirena.Travel.TestTask.Contracts.Models;

namespace Sirena.Travel.TestTask.Contracts;

public interface ISearchService
{
    Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
    Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
}