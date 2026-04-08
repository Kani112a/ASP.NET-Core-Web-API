using Cityinfo.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Cityinfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city=CitiesDataStore.Current.Cities.FirstOrDefault(c=>c.Id==cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city.PointsOfInterest);
        }
        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointofinterestid)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var maxpointofinterestid = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c=>c.Id== cityId);
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
            var city=CitiesDataStore.Current.Cities.FirstOrDefault(c=>c.Id==cityId);
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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
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
            return NoContent();
        }
    }
}
