using AutoMapper;
using Cityinfo.API.Model;
using Cityinfo.API.Service;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Cityinfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    //[Route("api/cities")]  //we can use but http is not supporting it is for https
    public class CitiesController : ControllerBase
    {
       // private readonly CitiesDataStore _cityDataStore;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

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
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities()
        {
            //return Ok(_cityDataStore.Cities);
            var cityEntities= await _cityInfoRepository.GetCitiesAsync();
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
        public ActionResult<CityDto> GetCity(int id)
        {
            //var citiestoreturn = _cityDataStore.Cities.FirstOrDefault(c => c.Id == id);
            //if (citiestoreturn == null)
            //{
            //    return NotFound();
            //}
            //return Ok(citiestoreturn);  // to return status code
            return Ok();
           
            //return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id)); // to return the output 
        }
    }
}
