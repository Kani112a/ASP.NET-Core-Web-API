using Cityinfo.API.DbContexts;
using Cityinfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cityinfo.API.Service
{
    public class CityInfoRepository:ICityInfoRepository
    {
        private readonly cityInfoContext _context;
        public CityInfoRepository(cityInfoContext context)
        {
            _context=context?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            //throw new NotImplementedException();
            return await _context.Cities.OrderBy(c=>c.Name ).ToListAsync();
        }
        public Task<City?> GetCity(int cityId)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<pointOfInterest>> GetPointOfInterestAsync(int cityId)
        {
            throw new NotImplementedException();
        }
        public Task<pointOfInterest?> GetPointsOfInterestAsync(int cityId, int pointOfInterestId)
        {
            throw new NotImplementedException();
        }
    }
}
