﻿using CityInfo.API.Models;
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
                    Description="The one with that big park."
                },
                new CityDto
                {
                    Id=2,
                    Name="Antwerp",
                    Description="The one with the cathedral that was never really finished."
                },
                new CityDto
                {
                    Id=3,
                    Name="Paris",
                    Description="The one with that big tower."
                }
            };
        }
    }
}
