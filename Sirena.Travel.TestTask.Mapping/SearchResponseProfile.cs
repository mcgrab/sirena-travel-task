using AutoMapper;
using Sirena.Travel.TestTask.Contracts.Models;
using System.Collections.Concurrent;

namespace Sirena.Travel.TestTask.Mapping;

internal class SearchResponseProfile : Profile
{
    public SearchResponseProfile()
    {
        CreateMap<IEnumerable<Route>, SearchResponse>()
            .ForMember(dest => dest.Routes, opts => opts.MapFrom(src => src))
            .ForMember(dest => dest.MinMinutesRoute, opts => opts.Ignore())
            .ForMember(dest => dest.MaxMinutesRoute, opts => opts.Ignore())
            .ForMember(dest => dest.MinPrice, opts => opts.Ignore())
            .ForMember(dest => dest.MaxPrice, opts => opts.Ignore())
            .AfterMap((src, dest) =>
            {
                if (dest.Routes == null || !dest.Routes.Any())
                    return;
                        
                var maxPrice = decimal.Zero;
                var minPrice = decimal.MaxValue;
                var minMinutes = int.MaxValue;
                var maxMinutes = 0;

                foreach (var route in dest.Routes)
                {
                    var duration = (int)(route.DestinationDateTime - route.OriginDateTime).TotalMinutes;
                    var price = route.Price;
                    minMinutes = Math.Min(minMinutes, duration);
                    maxMinutes = Math.Max(maxMinutes, duration);
                    minPrice = Math.Min(minPrice, price);
                    maxPrice = Math.Max(maxPrice, price);    
                }

                dest.MaxPrice = maxPrice;
                dest.MinPrice = minPrice;
                dest.MinMinutesRoute = minMinutes;
                dest.MaxMinutesRoute = maxMinutes;
            });

    }
}
