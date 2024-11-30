using ECommerce.Modules.Items.Application.Commands.Types;
using ECommerce.Modules.Items.Application.DTO;
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
    public class TypesControllerTests : BaseTest, IAsyncLifetime
    {
        [Fact]
        public async Task should_return_types()
        {
            var response = (await client.Request($"{Path}").GetAsync());
            var typeFromDb = await response.GetJsonAsync<IEnumerable<TypeDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            typeFromDb.ShouldNotBeNull();
            typeFromDb.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task given_valid_id_should_return_type()
        {
            var type = _types[1];
            var id = type.Id.Value;

            var response = (await client.Request($"{Path}/{id}").GetAsync());
            var typeFromDb = await response.GetJsonAsync<TypeDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            typeFromDb.ShouldNotBeNull();
            typeFromDb.Id.ShouldBe(id);
            typeFromDb.Name.ShouldBe(type.Name);
        }

        [Fact]
        public async Task given_valid_command_should_update()
        {
            var type = _types[1];
            var id = type.Id.Value;
            var command = new UpdateType(id, "Type #1234");
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}/{id}").PutJsonAsync(command));
            var typeFromDb = await dbContext.Types.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            typeFromDb.ShouldNotBeNull();
            typeFromDb.Name.ShouldBe(command.Name);
            typeFromDb.Name.ShouldNotBe(type.Name);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var type = _types[1];
            var id = type.Id.Value;
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}/{id}").DeleteAsync());
            var typeFromDb = await dbContext.Types.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            typeFromDb.ShouldBeNull();
        }

        [Fact]
        public async Task given_valid_command_should_add()
        {
            var command = new CreateType("Type #251234");
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}").PostJsonAsync(command));
            var id = response.GetIdFromHeaders<Guid>(Path);
            var type = dbContext.Types.Where(c => c.Id == id).SingleOrDefault();

            type.ShouldNotBeNull();
            type.Name.ShouldBe(command.Name);
        }

        public async Task InitializeAsync()
        {
            _types = await AddSampleData();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        private async Task<List<Domain.Entities.Type>> AddSampleData()
        {
            var types = GetSampleData();
            var type1 = types[0];
            var type2 = types[1];
            await dbContext.Types.AddAsync(type1);
            await dbContext.Types.AddAsync(type2);
            await dbContext.SaveChangesAsync();
            return types;
        }

        private List<Domain.Entities.Type> GetSampleData()
        {
            var type1 = new Domain.Entities.Type(Guid.NewGuid(), "Type #1");
            var type2 = new Domain.Entities.Type(Guid.NewGuid(), "Type #2");
            return new List<Domain.Entities.Type> { type1, type2 };
        }

        private const string Path = "items-module/types";
        private List<Domain.Entities.Type> _types;

        public TypesControllerTests(TestApplicationFactory<Program> factory, TestItemsDbContext dbContext)
            : base(factory, dbContext)
        {
        }
    }
}
