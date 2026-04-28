using AutoMapper;
using Cityinfo.API.Model;
using Cityinfo.API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cityinfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/cities")]
    //[Route("api/cities")]  //we can use but http is not supporting it is for https
    public class CitiesController : ControllerBase
    {
       // private readonly CitiesDataStore _cityDataStore;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize=20;
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new Exception(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new Exception(nameof(mapper));
        }

        [HttpGet]
        //[HttpGet("api/cities")]  //Instead of here we declared as common route above
        //public JsonResult GetCities()
        //{
        //    return new JsonResult(CitiesDataStore.Current.Cities);
        //}
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities(string? name, string? searchQuery, int pageNumber=1, int pageSize=10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }
            //return Ok(_cityDataStore.Cities);
            var (cityEntities,paginationMetadata)= await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));
            //var result = new List<CityWithoutPointOfInterestDto>();
            //foreach (var cityEntity in cityEntities)
            //{
            //    result.Add(new CityWithoutPointOfInterestDto
            //    {
            //        Id=cityEntity.Id,
            //        Name=cityEntity.Name,
            //        Description=cityEntity.Description
            //    });
            //}
            //return Ok(result);
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city)); // WITH POI
            }

            return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city)); // WITHOUT POI
        }
    }
}
