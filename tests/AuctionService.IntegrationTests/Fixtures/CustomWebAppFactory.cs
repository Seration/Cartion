using AuctionService.Data;
using AuctionService.Integrationtests.Util;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using WebMotions.Fake.Authentication.JwtBearer;

namespace AuctionService.Integrationtests.Fixtures;

public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<AuctionDbContext>();

            services.AddDbContext<AuctionDbContext>(opt =>
            {
                opt.UseNpgsql(_postgreSqlContainer.GetConnectionString());
            });

            services.AddMassTransitTestHarness();

            services.EnsureCreated<AuctionDbContext>();

            services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer(opt =>
            {
                opt.BearerValueType = FakeJwtBearerBearerValueType.Jwt;
            });
        });
    }

    Task IAsyncLifetime.DisposeAsync() => _postgreSqlContainer?.DisposeAsync().AsTask();



}

//internal class PostgreSqlContainer
//{

//}


