using AutoMapper;
using rainfall_api.Dtos;
using rainfall_api.Dtos.RainfallApi;

namespace rainfall_api.Mapper
{
    public class RainFallReadingMapperProfiles : Profile
    {
        public RainFallReadingMapperProfiles()
        {
            CreateMap<GetReadingsByStationIdDto, RainfallReadingResponseModel>()
                .ForMember(s => s.Readings, c => c.MapFrom(a => a.Items));
            CreateMap<Items, RainfallReading>()

                .ForMember(s => s.DateMeasured, c => c.MapFrom(a => a.DateTime))
                .ForMember(s => s.AmountMeasured, c => c.MapFrom(a => a.Value));
        }
    }
}
