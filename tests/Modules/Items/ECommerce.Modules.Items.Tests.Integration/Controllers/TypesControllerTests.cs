using ECommerce.Modules.Items.Application.Commands.Types;
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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Integration.Controllers
{
    [Collection("integrationTypes")]
    public class TypesControllerTests
    : IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestItemsDbContext>
    {
        [Fact]
        public async Task should_return_types()
        {
            await AddSampleData();

            var response = (await _client.Request($"{Path}").GetAsync());
            var typeFromDb = await response.GetJsonAsync<IEnumerable<TypeDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            typeFromDb.ShouldNotBeNull();
            typeFromDb.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task given_valid_id_should_return_type()
        {
            var types = await AddSampleData();
            var type = types[1];
            var id = type.Id.Value;

            var response = (await _client.Request($"{Path}/{id}").GetAsync());
            var typeFromDb = await response.GetJsonAsync<TypeDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            typeFromDb.ShouldNotBeNull();
            typeFromDb.Id.ShouldBe(id);
            typeFromDb.Name.ShouldBe(type.Name);
        }

        [Fact]
        public async Task given_valid_command_should_update()
        {
            var types = await AddSampleData();
            var type = types[1];
            var id = type.Id.Value;
            var command = new UpdateType(id, "Type #1234");
            Authenticate(Guid.NewGuid());

            var response = (await _client.Request($"{Path}").PutJsonAsync(command));
            var typeFromDb = await _dbContext.Types.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            typeFromDb.ShouldNotBeNull();
            typeFromDb.Name.ShouldBe(command.Name);
            typeFromDb.Name.ShouldNotBe(type.Name);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var types = await AddSampleData();
            var type = types[1];
            var id = type.Id.Value;
            Authenticate(Guid.NewGuid());

            var response = (await _client.Request($"{Path}/{id}").DeleteAsync());
            var typeFromDb = await _dbContext.Types.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            typeFromDb.ShouldBeNull();
        }

        [Fact]
        public async Task given_valid_command_should_add()
        {
            var command = new CreateType("Type #251234");
            Authenticate(Guid.NewGuid());

            var response = (await _client.Request($"{Path}").PostJsonAsync(command));
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            var splitted = responseHeaderValue.Split(Path + '/');
            Guid.TryParse(splitted[1], out var id);
            var type = _dbContext.Types.Where(c => c.Id == id).SingleOrDefault();

            type.ShouldNotBeNull();
            type.Name.ShouldBe(command.Name);
        }

        private void Authenticate(Guid userId)
        {
            var claims = new Dictionary<string, IEnumerable<string>>();
            claims.Add("permissions", new[] { "items", "item-sale" });
            var jwt = AuthHelper.GenerateJwt(userId.ToString(), "admin", claims: claims);
            _client.WithOAuthBearerToken(jwt);
        }

        private async Task<List<Domain.Entities.Type>> AddSampleData()
        {
            var types = GetSampleData();
            var type1 = types[0];
            var type2 = types[1];
            await _dbContext.Types.AddAsync(type1);
            await _dbContext.Types.AddAsync(type2);
            await _dbContext.SaveChangesAsync();
            return types;
        }

        private List<Domain.Entities.Type> GetSampleData()
        {
            var type1 = new Domain.Entities.Type(Guid.NewGuid(), "Type #1");
            var type2 = new Domain.Entities.Type(Guid.NewGuid(), "Type #2");
            return new List<Domain.Entities.Type> { type1, type2 };
        }

        private const string Path = "items-module/types";
        private readonly IFlurlClient _client;
        private readonly ItemsDbContext _dbContext;

        public TypesControllerTests(TestApplicationFactory<Program> factory, TestItemsDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
        }
    }
}
