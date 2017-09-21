using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        //use a static variable to access hardcoded data without creating instance of this class
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>
            {
                new CityDto
                {
                    Id=1,
                    Name="New York City",
                    Description="The one with that big park.",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id=1,
                            Name="Central Park",
                            Description="The most visited urban park in the United States."
                        },
                        new PointOfInterestDto
                        {
                            Id=2,
                            Name="Empire State Building",
                            Description="A 102-story skyscraper located in Midtown Manhatten."
                        }
                    }
                },
                new CityDto
                {
                    Id=2,
                    Name="Antwerp",
                    Description="The one with the cathedral that was never really finished.",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id=1,
                            Name="Point A",
                            Description="A point."
                        },
                        new PointOfInterestDto
                        {
                            Id=2,
                            Name="Point B",
                            Description="B Point."
                        }
                    }
                },
                new CityDto
                {
                    Id=3,
                    Name="Paris",
                    Description="The one with that big tower.",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id=1,
                            Name="Eiffel Tower",
                            Description="A tower."
                        },
                        new PointOfInterestDto
                        {
                            Id=2,
                            Name="Louvre",
                            Description="A museum."
                        }
                    }
                }
            };
        }
    }
}
