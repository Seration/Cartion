using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Integrationtests.Fixtures;
using AuctionService.Integrationtests.Util;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public class AuctionControllerTests : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
    {
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;
        private const string GT_ID = "afbee524-5972-4075-8800-7d1f9d7b0a0c";

        public AuctionControllerTests(CustomWebAppFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAuctions_ShouldReturn4Auctions()
        {
            var response = await _httpClient.GetFromJsonAsync<List<AuctionDto>>("api/auctions");

            Assert.Equal(4, response.Count);
        }

        [Fact]
        public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
        {
            var response = await _httpClient.GetFromJsonAsync<AuctionDto>($"api/auctions/{GT_ID}");

            Assert.Equal("GT", response.Model);
        }

        [Fact]
        public async Task GetAuctionById_WithInvalidId_ShouldReturn404()
        {
            var response = await _httpClient.GetAsync($"api/auctions/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAuctionById_WithInvalidGuid_ShouldReturn400()
        {
            var response = await _httpClient.GetAsync("api/auctions/boyleGUIDolmazboyleyalnızkalınmaz");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateAuction_WithAuth_ShouldReturn201()
        {
            var auction = GetAuctionForCreate();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            var response = await _httpClient.PostAsJsonAsync("api/auctions", auction);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdAuction = await response.Content.ReadFromJsonAsync<AuctionDto>();
            Assert.Equal("bob", createdAuction.Seller);
        }

        [Fact]
        public async Task CreateAuction_WithInvalidDto_ShouldReturn400()
        {
            var auction = GetAuctionForUpdate();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            var response = await _httpClient.PostAsJsonAsync("api/auctions", auction);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
        {
            var auction = GetAuctionForUpdate();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            var response = await _httpClient.PutAsJsonAsync($"api/auctions/{GT_ID}", auction);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
        {
            var auction = GetAuctionForUpdate();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("fake-bob"));

            var response = await _httpClient.PutAsJsonAsync($"api/auctions/{GT_ID}", auction);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
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
