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
        public async Task<City?> GetCity(int cityId, bool includePointsOfInterest)
        {
            //throw new NotImplementedException();
            if (includePointsOfInterest)
            {
                return await _context.Cities.Include(c=>c.pointOfInterests).Where(c=>c.Id == cityId).FirstOrDefaultAsync();
            }
            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<pointOfInterest>> GetPointOfInterestAsync(int cityId)
        {
            return await _context.pointOfInterests.Where(p => p.CityId == cityId).ToListAsync();    
            //throw new NotImplementedException();
        }
        public async Task<pointOfInterest?> GetPointsOfInterestAsync(int cityId, int pointOfInterestId)
        {
            //throw new NotImplementedException();
            return await _context.pointOfInterests.Where(p => p.CityId==cityId && p.Id== pointOfInterestId).FirstOrDefaultAsync();
        }
    }
}
