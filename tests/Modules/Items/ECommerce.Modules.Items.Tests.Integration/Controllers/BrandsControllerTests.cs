using ECommerce.Modules.Items.Application.Commands.Brands;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Integration.Controllers
{
    public class BrandsControllerTests : BaseTest, IAsyncLifetime
    {
        [Fact]
        public async Task should_return_brands()
        {
            var response = (await client.Request($"{Path}").GetAsync());
            var brandFromDb = await response.GetJsonAsync<IEnumerable<BrandDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            brandFromDb.ShouldNotBeNull();
            brandFromDb.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task given_valid_id_should_return_brand()
        {
            var brand = _brands[1];
            var id = brand.Id.Value;

            var response = (await client.Request($"{Path}/{id}").GetAsync());
            var brandFromDb = await response.GetJsonAsync<BrandDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            brandFromDb.ShouldNotBeNull();
            brandFromDb.Id.ShouldBe(id);
            brandFromDb.Name.ShouldBe(brand.Name);
        }

        [Fact]
        public async Task given_valid_command_should_update()
        {
            var brand = _brands[1];
            var id = brand.Id.Value;
            var command = new UpdateBrand(id, "Brand #1234");
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}/{id}").PutJsonAsync(command));
            var brandFromDb = await dbContext.Brands.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            brandFromDb.ShouldNotBeNull();
            brandFromDb.Name.ShouldBe(command.Name);
            brandFromDb.Name.ShouldNotBe(brand.Name);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var brand = _brands[1];
            var id = brand.Id.Value;
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}/{id}").DeleteAsync());
            var brandFromDb = await dbContext.Brands.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            brandFromDb.ShouldBeNull();
        }

        [Fact]
        public async Task given_valid_command_should_add()
        {
            var command = new CreateBrand("Brand #251234");
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}").PostJsonAsync(command));
            var id = response.GetIdFromHeaders<Guid>(Path);
            var brand = dbContext.Brands.Where(c => c.Id == id).SingleOrDefault();

            brand.ShouldNotBeNull();
            brand.Name.ShouldBe(command.Name);
        }

        public async Task InitializeAsync()
        {
            _brands = await AddSampleData();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        private async Task<List<Domain.Entities.Brand>> AddSampleData()
        {
            var brands = GetSampleData();
            var brand1 = brands[0];
            var brand2 = brands[1];
            await dbContext.Brands.AddAsync(brand1);
            await dbContext.Brands.AddAsync(brand2);
            await dbContext.SaveChangesAsync();
            return brands;
        }

        private List<Domain.Entities.Brand> GetSampleData()
        {
            var brand1 = new Domain.Entities.Brand (Guid.NewGuid(), "Brand #1");
            var brand2 = new Domain.Entities.Brand (Guid.NewGuid(), "Brand #2");
            return new List<Domain.Entities.Brand> { brand1, brand2 };
        }

        private const string Path = "items-module/brands";
        private List<Brand> _brands = [];

        public BrandsControllerTests(TestApplicationFactory<Program> factory, TestItemsDbContext dbContext)
            : base(factory, dbContext)
        {
        }
    }
}