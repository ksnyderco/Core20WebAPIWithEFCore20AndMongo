using CityInfo.API.Entities;
using System.Collections.Generic;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        //return IQueryable if you want to use LINQ on the result
        IEnumerable<City> GetCities();
        City GetCity(int cityId);

        void AddCity(City city);
        void DeleteCity(City city);
        bool Save();
    }
}
