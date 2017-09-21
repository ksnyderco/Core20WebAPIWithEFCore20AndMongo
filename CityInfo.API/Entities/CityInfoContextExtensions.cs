using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Entities
{
    public static class CityInfoContextExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                //data exists - do not seed
                return;
            }

            //create seed data
            var cities = new List<City>
            {
                new City
                {
                    Name="New York City",
                    Description="The one with that big park."
                },
                new City
                {
                    Name="Antwerp",
                    Description="The one with the cathedral that was never really finished."
                },
                new City
                {
                    Name="Paris",
                    Description="The one with that big tower."
                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
