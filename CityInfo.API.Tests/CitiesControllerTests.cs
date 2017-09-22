using CityInfo.API.Controllers;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CityInfo.API.Tests
{
    //To create new test project: Visual C#-.NET Core-Unit Test Project(.NET Core)
    //Use Moq 4.7.99 (it seems to support Core)

    [TestClass]
    public class CitiesControllerTests
    {
        private Mock<ICityInfoRepository> _mockCityInfoRepository;
        private Mock<IMongoRepository> _mockMongoRepository;
        private Mock<ILogger<CitiesController>> _mockLogger;
        private Mock<IMailService> _mockMailService;

        [TestInitialize]
        public void CreateDependencies()
        {
            _mockCityInfoRepository = new Mock<ICityInfoRepository>();
            _mockMongoRepository = new Mock<IMongoRepository>();
            _mockLogger = new Mock<ILogger<CitiesController>>();
            _mockMailService = new Mock<IMailService>();
        }

        [TestMethod]
        public void GetCity_ReturnsOk()
        {
            //set up Moq to return a city
            var cityInfoRepo = new Mock<ICityInfoRepository>();
            cityInfoRepo.Setup(r => r.GetCity(It.IsAny<int>()))
                .Returns(new City { Id=1, Name="Test City", Description = "Test Description" });

            //call GetCity
            var controller = new CitiesController(cityInfoRepo.Object, _mockMongoRepository.Object, _mockLogger.Object, _mockMailService.Object);
            var result = controller.GetCity(1);

            //cast the response to the correct type - each type has different properties
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var responseDto = ((OkObjectResult)result).Value as CityDto;
            Assert.AreEqual(1, responseDto.Id, "wrong id");
            Assert.AreEqual("Test City", responseDto.Name, "wrong name");
            Assert.AreEqual("Test Description", responseDto.Description, "wrong description");
        }

        [TestMethod]
        public void GetCity_NoEF_ReturnsNotFound()
        {
            var controller = new CitiesController(_mockCityInfoRepository.Object, _mockMongoRepository.Object, _mockLogger.Object, _mockMailService.Object);
            var result = controller.GetCity(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
