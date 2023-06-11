using Microsoft.AspNetCore.Mvc;
using Sirena.Travel.TestTask.Contracts;
using System.Web.Http;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Sirena.Travel.TestTask.Host.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IResult> PingAsync([FromServices] ISearchService searchService, CancellationToken cancellationToken)
        {
            var result = await searchService.IsAvailableAsync(cancellationToken);
            return result ? Results.Ok() : Results.StatusCode(500);
        }
    }
}
