using Microsoft.AspNetCore.Mvc;
using Sirena.Travel.TestTask.Contracts;
using Sirena.Travel.TestTask.Contracts.Models;

namespace Sirena.Travel.TestTask.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpPost]
        public async Task<SearchResponse> SearchAsync(
            [FromBody] SearchRequest request,
            [FromServices] ISearchService searchService,
             CancellationToken cancellationToken)
        {
            return await searchService.SearchAsync(request, cancellationToken);
        }
    }
}
