using AutoMapper;

namespace Cityinfo.API.NewFolder
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Entities.City, Model.CityWithoutPointOfInterestDto>();
        }
    }
}
