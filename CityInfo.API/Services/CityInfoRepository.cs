using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId)
        {
            return _context.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        public void AddCity(City city)
        {
            _context.Cities.Add(city);
        }

        public void DeleteCity(City city)
        {
            _context.Cities.Remove(city);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
