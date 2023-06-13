using AutoMapper;
using Bogus;
using Shouldly;
using Sirena.Travel.TestTask.Contracts.Models;
using Sirena.Travel.TestTask.Contracts.Models.ProviderOne;
using Sirena.Travel.TestTask.Mapping;

namespace Sirena.Travel.TestTask.Tests.Mapping
{
    [TestFixture]
    internal class ProviderOneProfileTests
    {
        private IMapper _mapper;
        private readonly Faker _faker = new();

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ProviderOneProfile>()).CreateMapper();
        }

        [Test]
        public void SearchRequest_To_ProviderOneSearchRequest()
        {
            var request = new SearchRequest
            {
                Destination = _faker.Address.City(),
                Origin = _faker.Address.City(),
                OriginDateTime = _faker.Date.Soon(),
                Filters = new()
                {
                    DestinationDateTime = _faker.Date.Soon(5),
                    MaxPrice = _faker.Random.Decimal(15000, 90000),
                },
            };

            var result = _mapper.Map<ProviderOneSearchRequest>(request);

            result.ShouldNotBeNull();
            result.MaxPrice.ShouldBe(request.Filters.MaxPrice);
            result.DateFrom.ShouldBe(request.OriginDateTime);
            result.DateTo.ShouldBe(request.Filters.DestinationDateTime);
            result.From.ShouldBe(request.Origin);
            result.To.ShouldBe(request.Destination);
        }

        [Test]
        public void ProviderOneRoute_To_Route()
        {
            var src = new ProviderOneRoute
            {
                DateFrom = _faker.Date.Soon(),
                DateTo = _faker.Date.Soon(2),
                From = _faker.Address.City(),
                Price = _faker.Random.Decimal(2000, 99000),
                TimeLimit = DateTime.UtcNow.AddMinutes(15),
                To = _faker.Address.City(),
            };

            var result = _mapper.Map<Route>(src);

            result.ShouldNotBeNull();
            result.Price.ShouldBe(src.Price);
            result.OriginDateTime.ShouldBe(src.DateFrom);
            result.DestinationDateTime.ShouldBe(src.DateTo);
            result.TimeLimit.ShouldBe(src.TimeLimit);
            result.Origin.ShouldBe(src.From);
            result.Destination.ShouldBe(src.To);
            result.Id.ShouldBe(src.GenerateGuid());
        }
    }
}
