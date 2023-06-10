using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sirena.Travel.TestTask.Mapping
{
    public static class DiExtensions
    {
        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
            => services.AddAutoMapper(typeof(SearchResponseProfile).Assembly);
    }
}
