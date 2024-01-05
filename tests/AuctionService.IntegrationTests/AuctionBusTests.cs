using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Integrationtests.Fixtures;
using AuctionService.Integrationtests.Util;
using Contracts;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AuctionService.Integrationtests
{
    public class AuctionBusTests : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
    {
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;
        private readonly ITestHarness _harness;

        public AuctionBusTests(CustomWebAppFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _harness = factory.Services.GetTestHarness();
        }

        [Fact]
        public async Task CreateAuction_WithValidObject_ShouldPublishAuctionCreated()
        {
            var auction = GetAuctionForCreate();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            var response = await _httpClient.PostAsJsonAsync("api/auctions", auction);

            response.EnsureSuccessStatusCode();
            Assert.True(await _harness.Published.Any<AuctionCreated>());
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
            DbHelper.ReinitDbForTests(db);
            return Task.CompletedTask;
        }

        private CreateAuctionDto GetAuctionForCreate()
        {
            return new CreateAuctionDto
            {
                Make = "test",
                Model = "testModel",
                ImageUrl = "test",
                Color = "test",
                Mileage = 10,
                Year = 2010,
                ReservePrice = 100,
            };
        }

        private UpdateAuctionDtos GetAuctionForUpdate()
        {
            return new UpdateAuctionDtos
            {
                Make = "test",
                Model = "testModel",
                Color = "test",
                Mileage = 10,
                Year = 2010,
            };
        }
    }
}
