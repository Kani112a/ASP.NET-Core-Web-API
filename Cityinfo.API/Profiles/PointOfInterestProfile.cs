using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Cityinfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.pointOfInterest, Model.PointOfInterestDto>();
        }
    }
}
