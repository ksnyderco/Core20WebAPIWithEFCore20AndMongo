using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                //hardcoded data
                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                //if (city == null)
                //{
                //    _logger.LogInformation($"City with id {cityId} was not found when accessing points of interest.");
                //    return NotFound();
                //}

                //return Ok(city.PointsOfInterest);

                //get entities from EF
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    //city does not exist
                    _logger.LogInformation($"City with id {cityId} was not found when accessing points of interest.");
                    return NotFound();
                }

                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);

                //convert entities to dtos
                var pointsOfInterestForCityResults = new List<PointOfInterestDto>();
                foreach (var poi in pointsOfInterestForCity)
                {
                    pointsOfInterestForCityResults.Add(new PointOfInterestDto
                    {
                        Id = poi.Id,
                        Name = poi.Name,
                        Description = poi.Description
                    });
                }

                return Ok(pointsOfInterestForCityResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.",ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name ="GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            //hardcoded data
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterest == null)
            //    return NotFound();

            //return Ok(pointOfInterest);

            if (!_cityInfoRepository.CityExists(cityId))
            {
                //city does not exist
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterest == null)
                return NotFound();

            //convert entities to dtos
            var pointOfInterestResult = new PointOfInterestDto
                {
                    Id = pointOfInterest.Id,
                    Name = pointOfInterest.Name,
                    Description = pointOfInterest.Description
                };

            return Ok(pointOfInterestResult);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            //cityId comes from the POST url, pointOfInterest is in the POST body

            if (pointOfInterest == null)
                return BadRequest();

            //add custom validation before checking data annotation rules
            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided description must be different than the name.");

            //validate data annotations on the model, returning error messages in the response
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //begin hardcoded data
            //find the city
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            ////create a new point of interest and add it to the city's collection
            //var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            //var finalPointOfInterest = new PointOfInterestDto
            //{
            //    Id = ++maxPointOfInterestId, //make a fake new id
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};

            //city.PointsOfInterest.Add(finalPointOfInterest);
            //end hardcoded data

            //validate city exists
            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            //create a new entity
            var pointOfInterestEntity = new PointOfInterest
            {
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            //add entity and save to db
            _cityInfoRepository.AddPointOfInterestForCity(cityId, pointOfInterestEntity);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            //map entity to dto (method must return a dto, not an entity)
            var poiDto = new PointOfInterestDto
            {
                Id = pointOfInterestEntity.Id, //save populated Id identity property
                Name = pointOfInterestEntity.Name,
                Description = pointOfInterestEntity.Description
            };

            //return a 201 
            //named route and parameters to get new PointOfInterest are used to build a url in the Location response header
            //the PointOfInterest just created will be in response body
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = poiDto.Id }, poiDto);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            //PUT updates all properties.  Properties not passed in will use their default values (strings will be null, etc)

            if (pointOfInterest == null)
                return BadRequest();

            //add custom validation before checking data annotation rules
            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided description must be different than the name.");

            //validate data annotations on the model, returning error messages in the response
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //begin hardcoded data
            ////find the city
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            ////find the point of interest
            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterestFromStore == null)
            //    return NotFound();

            ////update the point of interest
            //pointOfInterestFromStore.Name = pointOfInterest.Name;
            //pointOfInterestFromStore.Description = pointOfInterest.Description;
            //end hardcoded data

            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            //get entity to update
            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            //update the entity
            pointOfInterestEntity.Name = pointOfInterest.Name;
            pointOfInterestEntity.Description = pointOfInterest.Description;

            //save the updated entity
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            //return a 204
            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            //PATCH modifies properties as specified in the patchDoc.  Properties not in the patchDoc will not be changed.
            //Reuse the UpdateDto to include the same validation rules used for PUT

            //JSON PATCH SPEC https://tools.ietf.org/html/rfc6902
            /*
             * [
             *  {
             *      "op": "replace",
             *      "path": "/name",
             *      "value": "new name"
             *  }
             * ]
            */

            if (patchDoc == null)
                return BadRequest();

            //begin hardcoded data
            ////find the city
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            ////find the point of interest
            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterestFromStore == null)
            //    return NotFound();

            ////create a new object (copy the existing point of interest) 
            //var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            //{
            //    Name = pointOfInterestFromStore.Name,
            //    Description = pointOfInterestFromStore.Description
            //};

            ////apply the patch document passed in to the new object.  Pass in ModelState to capture any errors during patching.
            //patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            ////check for errors when applying the patch.  This ModelState is looking at the patchDoc, not the PointOfInterestForUpdateDto
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            ////validate the PointOfInterestForUpdateDto is valid
            //if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            //    ModelState.AddModelError("Description", "The provided description must be different than the name.");

            ////run the model validation rules
            //TryValidateModel(pointOfInterestToPatch);

            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            ////update the point of interest with the new data
            //pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
            //end hardcoded data

            //find the city
            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            //create a new dto to apply the patch to (copy the existing point of interest) 
            var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            {
                Name = pointOfInterestEntity.Name,
                Description = pointOfInterestEntity.Description
            };

            //apply the patch document passed in to the new object.  Pass in ModelState to capture any errors during patching.
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            //check for errors when applying the patch.  This ModelState is looking at the patchDoc, not the PointOfInterestForUpdateDto
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //validate the PointOfInterestForUpdateDto is valid
            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
                ModelState.AddModelError("Description", "The provided description must be different than the name.");

            //run the model validation rules
            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //update the point of interest with the new data
            pointOfInterestEntity.Name = pointOfInterestToPatch.Name;
            pointOfInterestEntity.Description = pointOfInterestToPatch.Description;

            //save the updated entity
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            //return a 204
            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            //begin hardcoded data
            ////find the city
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            ////find the point of interest
            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterestFromStore == null)
            //    return NotFound();

            ////remove the point of interest
            //city.PointsOfInterest.Remove(pointOfInterestFromStore);
            //end hardcoded data

            //find the city
            if (!_cityInfoRepository.CityExists(cityId))
                return NotFound();

            //find the point of interest
            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            //remove the point of interest
            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            //send a fake email using a custom service
            _mailService.Send("Point of interest deleted", $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            //return a 204
            return NoContent();
        }
    }
}
