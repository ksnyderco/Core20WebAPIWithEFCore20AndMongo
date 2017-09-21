using CityInfo.API.Entities;
using System.Collections.Generic;

namespace CityInfo.API.Services
{
    public interface IMongoRepository
    {
        IEnumerable<MongoCity> GetCities();
        MongoCity GetCity(int cityId);
    }
}
