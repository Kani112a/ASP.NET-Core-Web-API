using Cityinfo.API.Entities;

namespace Cityinfo.API.Service
{
    public interface ICityInfoRepository
    {
        Task <IEnumerable<City>> GetCitiesAsync();
        Task <(IEnumerable<City>,PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<IEnumerable<pointOfInterest>> GetpointsOfInterestForCityAsync(int cityId);
        Task<bool> CityExistsAsync(int cityId);
        Task<pointOfInterest?> GetpointsOfInterestForCityAsync(int cityId, int pointOfInterestId);
        Task AddPointOfInterestForCityAsync(int cityId, pointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
        void DeletePointOfInterest(pointOfInterest pointOfInterest);
    }
}
