using Cityinfo.API.Model;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Cityinfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    //[Route("api/cities")]  //we can use but http is not supporting it is for https
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        //[HttpGet("api/cities")]  //Instead of here we declared as common route above
        //public JsonResult GetCities()
        //{
        //    return new JsonResult(CitiesDataStore.Current.Cities);
        //}
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }
        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var citiestoreturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (citiestoreturn == null)
            {
                return NotFound();
            }
            return Ok(citiestoreturn);  // to return status code
           
            //return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id)); // to return the output 
        }
    }
}
