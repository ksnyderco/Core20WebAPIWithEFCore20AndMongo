using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;
        private IMongoRepository _mongoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMongoRepository mongoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
            _mongoRepository = mongoRepository;
        }

        #region ENTITY FRAMEWORK

        [HttpGet()] //uses api/cities route
        public IActionResult GetCities()
        {
            //hardcoded data
            //return Ok(CitiesDataStore.Current.Cities);

            //get entities from EF
            var cityEntities = _cityInfoRepository.GetCities();

            //convert entities to dtos
            var results = new List<CityWithoutPointsOfInterestDto>();
            foreach(var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDto
                {
                    Id = cityEntity.Id,
                    Name = cityEntity.Name,
                    Description = cityEntity.Description
                });
            }

            return Ok(results);
        }

        [HttpGet("{id}")] //uses api/cities/{id} route
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            //hardcoded data
            //var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            //if (cityToReturn == null)
            //    return NotFound();

            //return Ok(cityToReturn);

            //get entities from EF
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);
            if (city == null)
                return NotFound();

            //convert entities to dtos
            var cityResult = new CityDto
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            };

            if (includePointsOfInterest)
            {
                foreach (var poi in city.PointsOfInterest)
                {
                    cityResult.PointsOfInterest.Add(
                        new PointOfInterestDto
                        {
                            Id = poi.Id,
                            Name = poi.Name,
                            Description = poi.Description
                        });
                }
            }

            return Ok(cityResult);
        }

        #endregion


        #region MONGO

        [HttpGet("mongo")] //uses api/cities/mongo route
        public IActionResult GetCitiesMongo()
        {
            //get entities from Mongo
            var cityEntities = _mongoRepository.GetCities();

            //convert entities to dtos
            var results = new List<CityWithoutPointsOfInterestDto>();
            foreach (var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDto
                {
                    Id = cityEntity.Id,
                    Name = cityEntity.Name,
                    Description = cityEntity.Description
                });
            }

            return Ok(results);
        }

        [HttpGet("mongo/{id}")] //uses api/cities/mongo/{id} route
        public IActionResult GetCityMongo(int id)
        {
            //get entities from Mongo
            var city = _mongoRepository.GetCity(id);
            if (city == null)
                return NotFound();

            //convert entities to dtos
            var cityResult = new CityWithoutPointsOfInterestDto
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            };

            return Ok(cityResult);
        }

        #endregion
    }
}
