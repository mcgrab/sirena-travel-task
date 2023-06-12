using AutoMapper;
using Sirena.Travel.TestTask.Contracts.Models;
using Sirena.Travel.TestTask.Contracts.Models.ProviderOne;

namespace Sirena.Travel.TestTask.Mapping;

internal class ProviderOneProfile : Profile
{
    public ProviderOneProfile()
    {
        CreateMap<SearchRequest, ProviderOneSearchRequest>()
            .ForMember(dest => dest.From, opts => opts.MapFrom(src => src.Origin))
            .ForMember(dest => dest.DateFrom, opts => opts.MapFrom(src => src.OriginDateTime))
            .ForMember(dest => dest.MaxPrice, opts => opts.MapFrom(src => src.Filters.MaxPrice))
            .ForMember(dest => dest.DateTo, opts => opts.MapFrom(src => src.Filters.DestinationDateTime))
            .ForMember(dest => dest.To, opts => opts.MapFrom(src => src.Destination));

        CreateMap<ProviderOneRoute, Route>()
            .ForMember(dest => dest.DestinationDateTime, opts => opts.MapFrom(src => src.DateTo))
            .ForMember(dest => dest.OriginDateTime, opts => opts.MapFrom(src => src.DateFrom))
            .ForMember(dest => dest.TimeLimit, opts => opts.MapFrom(src => src.TimeLimit))
            .ForMember(dest => dest.Price, opts => opts.MapFrom(src => src.Price))
            .ForMember(dest => dest.Destination, opts => opts.MapFrom(src => src.To))
            .ForMember(dest => dest.Origin, opts => opts.MapFrom(src => src.From))
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.GenerateGuid()));
    }
}
