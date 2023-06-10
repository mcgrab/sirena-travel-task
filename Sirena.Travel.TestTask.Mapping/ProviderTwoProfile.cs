using AutoMapper;
using Sirena.Travel.TestTask.Contracts.Models;
using Sirena.Travel.TestTask.Contracts.Models.ProviderTwo;

namespace Sirena.Travel.TestTask.Mapping
{
    internal class ProviderTwoProfile : Profile
    {
        public ProviderTwoProfile()
        {
            CreateMap<SearchRequest, ProviderTwoSearchRequest>()
                .ForMember(dest => dest.MinTimeLimit, opts => opts.MapFrom(src => src.Filters.MinTimeLimit))
                .ForMember(dest => dest.Arrival, opts => opts.MapFrom(src => src.Destination))
                .ForMember(dest => dest.DepartureDate, opts => opts.MapFrom(src => src.OriginDateTime))
                .ForMember(dest => dest.Departure, opts => opts.MapFrom(src => src.Origin));

            CreateMap<ProviderTwoRoute, Route>()
                .ForMember(dest => dest.DestinationDateTime, opts => opts.MapFrom(src => src.Departure.Date))
                .ForMember(dest => dest.OriginDateTime, opts => opts.MapFrom(src => src.Arrival.Date))
                .ForMember(dest => dest.TimeLimit, opts => opts.MapFrom(src => src.TimeLimit))
                .ForMember(dest => dest.Price, opts => opts.MapFrom(src => src.Price))
                .ForMember(dest => dest.Destination, opts => opts.MapFrom(src => src.Arrival.Point))
                .ForMember(dest => dest.Origin, opts => opts.MapFrom(src => src.Departure.Point))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => Guid.NewGuid()));
        }
    }
}
