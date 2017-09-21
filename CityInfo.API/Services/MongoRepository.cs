using System.Collections.Generic;
using CityInfo.API.Entities;
using MongoDB.Driver;

namespace CityInfo.API.Services
{
    public class MongoRepository : IMongoRepository
    {
        //had a lot of serialization issues with the MongoCity model class
        //names need to match exactly, are case-sensitive
        //insert _id as an int to prevent Mongo from creating ObjectId object by default
        //I used the following seed data (entered manually in command line, different than EF data)
        //use CityInfo
        //db.CityInfo.insert( {_id:1,Name:'Denver',Description:'The one with the mountains.'})
        //db.CityInfo.insert( {_id:2,Name:'Tokyo',Description:'The one with Godzilla.'})
        //db.CityInfo.insert( {_id:3,Name:'London',Description:'The one with the clock.'})

        //OTHER MONGO INFO (see Syncfusion Succinctly book)
        //from command prompt run server by going to C:\temp\mongodb\bin and run mongod
        //
        //in a separate command window
        //cd c:\temp\mongodb\bin
        //mongo shows window is connected to server
        //"show dbs" shows databases
        //"show catalogs" shows tables
        //"use CityInfo" creates a CityInfo database
        //"db.CityInfo.find()" shows data in CityInfo table
        //"db.CityInfo.drop()" drops CityInfo table


        //use constructor to create database context
        private MongoContext _context = null;
        public MongoRepository()
        {
            _context = new MongoContext();
        }

        public IEnumerable<MongoCity> GetCities()
        {
            return _context.Cities.AsQueryable();
        }

        public MongoCity GetCity(int cityId)
        {
            //filter is case-sensitive; Id must match property name in MongoCity ("id" does not return any results)
            var filter = Builders<MongoCity>.Filter.Eq("Id", cityId);
            return _context.Cities.Find(filter).FirstOrDefault();
        }
    }
}
