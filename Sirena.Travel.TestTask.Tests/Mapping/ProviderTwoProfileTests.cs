using AutoMapper;
using Bogus;
using Shouldly;
using Sirena.Travel.TestTask.Contracts.Models;
using Sirena.Travel.TestTask.Contracts.Models.ProviderTwo;
using Sirena.Travel.TestTask.Mapping;

namespace Sirena.Travel.TestTask.Tests.Mapping
{
    [TestFixture]
    internal class ProviderTwoProfileTests
    {
        private IMapper _mapper;
        private readonly Faker _faker = new();

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ProviderTwoProfile>()).CreateMapper();
        }

        [Test]
        public void SearchRequest_To_ProviderTwoSearchRequest()
        {
            var request = new SearchRequest
            {
                Destination = _faker.Address.City(),
                Origin = _faker.Address.City(),
                OriginDateTime = _faker.Date.Soon(),
                Filters = new()
                {
                    DestinationDateTime = _faker.Date.Soon(5),
                    MinTimeLimit = DateTime.UtcNow.AddDays(1),

                },
            };

            var result = _mapper.Map<ProviderTwoSearchRequest>(request);

            result.ShouldNotBeNull();
            result.Arrival.ShouldBe(request.Destination);
            result.Departure.ShouldBe(request.Origin);
            result.DepartureDate.ShouldBe(request.OriginDateTime);
            result.MinTimeLimit.ShouldBe(request.Filters.MinTimeLimit);
        }

        [Test]
        public void ProviderTwoRoute_To_Route()
        {
            var src = new ProviderTwoRoute
            {
                Arrival = new()
                {
                    Date = _faker.Date.Soon(2),
                    Point = _faker.Address.City()
                },
                Departure = new()
                {
                    Date = _faker.Date.Soon(),
                    Point = _faker.Address.City()
                },
                Price = _faker.Random.Decimal(2000, 99000),
                TimeLimit = DateTime.UtcNow.AddMinutes(15),
            };

            var result = _mapper.Map<Route>(src);

            result.ShouldNotBeNull();
            result.Price.ShouldBe(src.Price);
            result.OriginDateTime.ShouldBe(src.Departure.Date);
            result.DestinationDateTime.ShouldBe(src.Arrival.Date);
            result.TimeLimit.ShouldBe(src.TimeLimit);
            result.Origin.ShouldBe(src.Departure.Point);
            result.Destination.ShouldBe(src.Arrival.Point);
            result.Id.ShouldBe(src.GenerateGuid());
        }
    }
}
