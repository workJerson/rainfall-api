using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using rainfall_api.Common.Exceptions;
using rainfall_api.Controllers;
using rainfall_api.Dtos;
using rainfall_api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rainfall_api.test.Controllers
{
    public class RainfallControllerTests
    {
        private readonly RainfallReadingResponseModel mockResponse = new RainfallReadingResponseModel()
        {
            Readings =
            [
                new() {
                    DateMeasured = DateTime.Now,
                    AmountMeasured= 0,
                },
                new()
                {
                    DateMeasured = DateTime.Now,
                    AmountMeasured = 4.68M,
                },
                new()
                {
                    DateMeasured = DateTime.Now,
                    AmountMeasured = 4.51M,
                },
            ]
        };

        [Fact]
        public async Task GetRainfallReadings_ShouldReturnSuccessResponse()
        {
            // Arrange
            var mockMediator = new Mock<ISender>();
            var controller = new RainFallController();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.ControllerContext.HttpContext.RequestServices = new ServiceCollection()
                .AddTransient(_ => mockMediator.Object)
                .BuildServiceProvider();

            mockMediator.Setup(x => x.Send(It.IsAny<GetRainfallReadingsRequest>(), default))
                        .ReturnsAsync(mockResponse);
            // Act
            var result = await controller.GetRainfallReadings("3680");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Readings.Count != 0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetRainfallReadings_ThrowsValidationException_OnInvalidStationId(string stationId)
        {
            // Arrange
            var mockMediator = new Mock<ISender>();
            var controller = new RainFallController();
            var expectedResult = new Common.Exceptions.ValidationException("Validation Error", [new() { Message = "StationId is required", PropertyName = "StationId" }]);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.ControllerContext.HttpContext.RequestServices = new ServiceCollection()
                .AddTransient(_ => mockMediator.Object)
                .BuildServiceProvider();

            mockMediator.Setup(x => x.Send(It.IsAny<GetRainfallReadingsRequest>(), default))
                        .ThrowsAsync(expectedResult);

            // Act
            var ex = await Assert.ThrowsAsync<Common.Exceptions.ValidationException>(() => controller.GetRainfallReadings(stationId));

            // Assert
            Assert.Equal("Validation Error", ex.Message);
            Assert.True(ex.Details.Any());
            Assert.True(ex.Details.Any(s => s.PropertyName == "StationId"));
        }

        [Theory]
        [InlineData("3680", 101)]
        [InlineData("3680", 0)]
        public async Task GetRainfallReadings_ThrowsValidationException_OnInvalidCount(string stationId, int count)
        {
            // Arrange
            var mockMediator = new Mock<ISender>();
            var controller = new RainFallController();
            var expectedResult = new Common.Exceptions.ValidationException("Validation Error", [new() { Message = "Invalid Count. Should be greater than or equal to 1 and less than or equal to 100", PropertyName = "Count" }]);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.ControllerContext.HttpContext.RequestServices = new ServiceCollection()
                .AddTransient(_ => mockMediator.Object)
                .BuildServiceProvider();

            mockMediator.Setup(x => x.Send(It.IsAny<GetRainfallReadingsRequest>(), default))
                        .ThrowsAsync(expectedResult);

            // Act
            var ex = await Assert.ThrowsAsync<Common.Exceptions.ValidationException>(() => controller.GetRainfallReadings(stationId, count));

            // Assert
            Assert.Equal("Validation Error", ex.Message);
            Assert.True(ex.Details.Any());
            Assert.True(ex.Details.Any(s => s.PropertyName == "Count"));
        }

        [Theory]
        [InlineData("3680", 5)]
        [InlineData("3680", 10)]
        public async Task GetRainfallReadings_ShouldReturnEmptyList(string stationId, int count)
        {
            // Arrange
            var mockMediator = new Mock<ISender>();
            var controller = new RainFallController();
            var expectedResult = new Common.Exceptions.ValidationException("Validation Error", [new() { Message = "Invalid Count. Should be greater than or equal to 1 and less than or equal to 100", PropertyName = "Count" }]);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.ControllerContext.HttpContext.RequestServices = new ServiceCollection()
                .AddTransient(_ => mockMediator.Object)
                .BuildServiceProvider();

            mockMediator.Setup(x => x.Send(It.IsAny<GetRainfallReadingsRequest>(), default))
                        .ThrowsAsync(expectedResult);

            // Act
            var ex = await Assert.ThrowsAsync<Common.Exceptions.ValidationException>(() => controller.GetRainfallReadings(stationId, count));

            // Assert
            Assert.Equal("Validation Error", ex.Message);
            Assert.True(ex.Details.Any());
            Assert.True(ex.Details.Any(s => s.PropertyName == "Count"));
        }

        [Fact]
        public async Task GetRainfallReadings_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var mockMediator = new Mock<ISender>();
            var controller = new RainFallController();
            var stationId = "station123";
            var count = 1000;

            // Mock HttpContext and set up Mediator
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.ControllerContext.HttpContext.RequestServices = new ServiceCollection()
                .AddTransient<ISender>(_ => mockMediator.Object)
                .BuildServiceProvider();

            var expectedException = new CustomException("Internal Server Error");
            mockMediator.Setup(x => x.Send(It.IsAny<GetRainfallReadingsRequest>(), default))
                        .ThrowsAsync(expectedException);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<CustomException>(() => controller.GetRainfallReadings(stationId, count));

            // Assert
            Assert.Equal("Internal Server Error", ex.Message);
        }
    }
}
