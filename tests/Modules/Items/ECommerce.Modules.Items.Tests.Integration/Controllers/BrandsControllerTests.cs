using ECommerce.Modules.Items.Application.Commands.Brands;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
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
    [Collection("integrationBrands")]
    public class BrandsControllerTests : IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestItemsDbContext>
    {
        [Fact]
        public async Task should_return_brands()
        {
            await AddSampleData();

            var response = (await _client.Request($"{Path}").GetAsync());
            var brandFromDb = await response.GetJsonAsync<IEnumerable<BrandDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            brandFromDb.ShouldNotBeNull();
            brandFromDb.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task given_valid_id_should_return_brand()
        {
            var brands = await AddSampleData();
            var brand = brands[1];
            var id = brand.Id.Value;

            var response = (await _client.Request($"{Path}/{id}").GetAsync());
            var brandFromDb = await response.GetJsonAsync<BrandDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            brandFromDb.ShouldNotBeNull();
            brandFromDb.Id.ShouldBe(id);
            brandFromDb.Name.ShouldBe(brand.Name);
        }

        [Fact]
        public async Task given_valid_dto_should_update()
        {
            var brands = await AddSampleData();
            var brand = brands[1];
            var id = brand.Id.Value;
            var dto = new UpdateBrand(id, "Brand #1234");
            Authenticate(Guid.NewGuid());

            var response = (await _client.Request($"{Path}").PutJsonAsync(dto));
            var brandFromDb = await _dbContext.Brands.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            brandFromDb.ShouldNotBeNull();
            brandFromDb.Name.ShouldBe(dto.Name);
            brandFromDb.Name.ShouldNotBe(brand.Name);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var brands = await AddSampleData();
            var brand = brands[1];
            var id = brand.Id.Value;
            Authenticate(Guid.NewGuid());

            var response = (await _client.Request($"{Path}/{id}").DeleteAsync());
            var brandFromDb = await _dbContext.Brands.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            brandFromDb.ShouldBeNull();
        }

        [Fact]
        public async Task given_valid_dto_should_add()
        {
            var dto = new CreateBrand("Brand #251234");
            Authenticate(Guid.NewGuid());

            var response = (await _client.Request($"{Path}").PostJsonAsync(dto));
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            var splitted = responseHeaderValue.Split(Path + '/');
            Guid.TryParse(splitted[1], out var id);
            var brand = _dbContext.Brands.Where(c => c.Id == id).SingleOrDefault();

            brand.ShouldNotBeNull();
            brand.Name.ShouldBe(dto.Name);
        }

        private void Authenticate(Guid userId)
        {
            var claims = new Dictionary<string, IEnumerable<string>>();
            claims.Add("permissions", new[] { "items", "item-sale" });
            var jwt = AuthHelper.GenerateJwt(userId.ToString(), "admin", claims: claims);
            _client.WithOAuthBearerToken(jwt);
        }

        private async Task<List<Domain.Entities.Brand>> AddSampleData()
        {
            var brands = GetSampleData();
            var brand1 = brands[0];
            var brand2 = brands[1];
            await _dbContext.Brands.AddAsync(brand1);
            await _dbContext.Brands.AddAsync(brand2);
            await _dbContext.SaveChangesAsync();
            return brands;
        }

        private List<Domain.Entities.Brand> GetSampleData()
        {
            var brand1 = new Domain.Entities.Brand (Guid.NewGuid(), "Brand #1");
            var brand2 = new Domain.Entities.Brand (Guid.NewGuid(), "Brand #2");
            return new List<Domain.Entities.Brand> { brand1, brand2 };
        }

        private const string Path = "items-module/brands";
        private readonly IFlurlClient _client;
        private readonly ItemsDbContext _dbContext;

        public BrandsControllerTests(TestApplicationFactory<Program> factory, TestItemsDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
        }
    }
}