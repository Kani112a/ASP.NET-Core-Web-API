using Cityinfo.API.Model;
using Cityinfo.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Cityinfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailservice;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailservice, CitiesDataStore citiesDataStore)
        {
            _logger=logger??throw new ArgumentNullException(nameof(logger));
            _mailservice=mailservice??throw new ArgumentNullException(nameof(mailservice));
            _citiesDataStore=citiesDataStore??throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            //throw new Exception("Exception Sample");
            try
            {
                
                var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasnt found with the id");
                    return NotFound();
                }
                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while passing the id {cityId}", ex);
                return StatusCode(500, "Exception while passing the point of interest");
            }
        }
        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointofinterestid)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointofinterest = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointofinterestid);
            if (pointofinterest == null)
            {
                return NotFound();
            }
            return Ok(pointofinterest);
        }
        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, pointOfInterestCreationDto pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var maxpointofinterestid = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            var finalpointofinterest = new PointOfInterestDto()
            {
                Id = ++maxpointofinterestid,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            city.PointsOfInterest.Add(finalpointofinterest);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterest = finalpointofinterest.Id
                },
                finalpointofinterest);
        }
        [HttpPut("{pointofinterestid}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointofInterestId, pointOfInterestForUpdateCtocs pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c=>c.Id== cityId);
            if (city==null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointofInterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }
            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;
            return NoContent();
        }
        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId,int pointofInterestId, JsonPatchDocument<pointOfInterestForUpdateCtocs> patchDocument)
        {
            var city=_citiesDataStore.Cities.FirstOrDefault(c=>c.Id==cityId);
            if (city==null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointofInterestId);
            if(pointOfInterestFromStore==null)
            {
                return NotFound();
            }
            var pointOfInterestToPatch = new pointOfInterestForUpdateCtocs()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            pointOfInterestFromStore.Name=pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description=pointOfInterestToPatch.Description;
            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }
            city.PointsOfInterest.Remove(pointOfInterestFromStore);
            _mailservice.Send("Point of interest deleted.", $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} is deleted.");
            return NoContent();
        }
    }
}
