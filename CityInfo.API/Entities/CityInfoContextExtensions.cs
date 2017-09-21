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
                    Description="The one with that big park.",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest
                        {
                            Name="Central Park",
                            Description="The most visited urban park in the United States."
                        },
                        new PointOfInterest
                        {
                            Name="Empire State Building",
                            Description="A 102-story skyscraper located in Midtown Manhatten."
                        }
                    }
                },
                new City
                {
                    Name="Antwerp",
                    Description="The one with the cathedral that was never really finished.",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest
                        {
                            Name="Point A",
                            Description="A point."
                        },
                        new PointOfInterest
                        {
                            Name="Point B",
                            Description="B Point."
                        }
                    }
                },
                new City
                {
                    Name="Paris",
                    Description="The one with that big tower.",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest
                        {
                            Name="Eiffel Tower",
                            Description="A tower."
                        },
                        new PointOfInterest
                        {
                            Name="Louvre",
                            Description="A museum."
                        }
                    }
                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
