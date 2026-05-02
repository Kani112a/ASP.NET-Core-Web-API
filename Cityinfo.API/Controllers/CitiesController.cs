using Asp.Versioning;
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
    [ApiVersion(1)]
    [ApiVersion(2)]
    [Route("api/v{version:apiVersion}/cities")]
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
        //xml comments
        ///<summary>
        ///Get a city by id
        ///</summary>
        ///<response code="200">Returns the request city</response>
        [HttpGet("{cityId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public async Task<IActionResult> GetCity(int cityId, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(cityId, includePointsOfInterest);

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
