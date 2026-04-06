using Cityinfo.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace Cityinfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Route("api/cities")]  //we can use but http is not supporting it is for https
    public class CitiesController :ControllerBase
    {
        [HttpGet]
        //[HttpGet("api/cities")]  //Instead of here we declared as common route above
        public JsonResult GetCities()
        {
            //return new JsonResult(new List<object>
            //{
            //      new {id="1",name ="Tamilnadu" },
            //      new {id="2", name="Kerala" }
            //});
            return new JsonResult(CitiesDataStore.Current.Cities);
        }
    }
}
