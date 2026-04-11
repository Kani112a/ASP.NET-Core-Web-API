using Cityinfo.API.Entities;

namespace Cityinfo.API.Service
{
    public interface ICityInfoRepository
    {
        Task <IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCity(int cityId);
        Task<IEnumerable<pointOfInterest>> GetPointOfInterestAsync(int cityId);
        Task<pointOfInterest?> GetPointsOfInterestAsync(int cityId, int pointOfInterestId);
    }
}
