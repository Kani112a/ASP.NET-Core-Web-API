using Cityinfo.API.DbContexts;
using Cityinfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cityinfo.API.Service
{
    public class CityInfoRepository : ICityInfoRepository
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
        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            //if (string.IsNullOrEmpty(name) && string.IsNullOrWhiteSpace(searchQuery))
            //{
            //    return await GetCitiesAsync();
            //}
            var collection = _context.Cities as IQueryable<City>;
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery) || (a.Description != null && a.Description.Contains(searchQuery)));
            }
            var totalItemCount= await collection.CountAsync();
            var paginataionMetdata = new PaginationMetadata(totalItemCount,pageSize, pageNumber);
            var collectionToReturn= await collection.OrderBy(c => c.Name).Skip(pageSize*(pageNumber-1)).Take(pageSize)
                .ToListAsync();
            return (collectionToReturn, paginataionMetdata);
            //throw new NotImplementedException();
            //return await _context.Cities.Where(c=>c.Name==name).OrderBy(c => c.Name).ToListAsync();
        }
        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            //throw new NotImplementedException();
            if (includePointsOfInterest)
            {
                return await _context.Cities.Include(c=>c.PointsOfInterest).Where(c=>c.Id == cityId).FirstOrDefaultAsync();
            }
            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }
        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }
        public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        }
        public async Task<IEnumerable<pointOfInterest>> GetPointOfInterestAsync(int cityId)
        {
            return await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();    
            //throw new NotImplementedException();
        }
        public async Task<pointOfInterest?> GetpointsOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            //throw new NotImplementedException();
            return await _context.PointsOfInterest.Where(p => p.CityId==cityId && p.Id== pointOfInterestId).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<pointOfInterest>> GetpointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }
        public async Task AddPointOfInterestForCityAsync(int cityId, pointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
           {
                if (city != null)
                {
                    city.PointsOfInterest.Add(pointOfInterest);
                }
            }
        }

        public void DeletePointOfInterest(pointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()>=0);
        }
    }
}
