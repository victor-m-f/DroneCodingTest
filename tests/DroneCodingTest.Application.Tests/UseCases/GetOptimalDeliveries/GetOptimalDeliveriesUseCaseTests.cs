using DroneCodingTest.Application.UseCases.GetOptimalDeliveries;
using DroneCodingTest.Shared.Requests;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text;

namespace DroneCodingTest.Application.Tests.UseCases.GetOptimalDeliveries
{
    public sealed class GetOptimalDeliveriesUseCaseTests
    {
        private readonly Mock<ILogger<GetOptimalDeliveriesUseCase>> _mockLogger;
        private readonly GetOptimalDeliveriesUseCase _useCase;

        public GetOptimalDeliveriesUseCaseTests()
        {
            _mockLogger = new Mock<ILogger<GetOptimalDeliveriesUseCase>>();
            _useCase = new GetOptimalDeliveriesUseCase(_mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenInputIsInvalid()
        {
            // Arrange
            var invalidData = "Invalid Data";
            var input = new GetOptimalDeliveriesInput(
                Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(invalidData))),
                TravelRules.NoTravelTime);

            // Act
            var result = await _useCase.Handle(input, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_ShouldReturnOK_WhenInputIsValid()
        {
            // Arrange
            var validData = "[Drone1, 10], [Drone2, 20] \r\n [LocationA], [200]\r\n[LocationB], [150]\r\n[LocationC], [50]\r\n[LocationD], [150]";
            var input = new GetOptimalDeliveriesInput(
                Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(validData))),
                TravelRules.NoTravelTime);

            // Act
            var result = await _useCase.Handle(input, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
