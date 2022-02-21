using ECommerce.Modules.Items.Application.Commands.Types;
using ECommerce.Modules.Items.Application.Commands.Types.Handlers;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Unit.Commands.Types
{
    public class DeleteTypeCommandHandlerTests
    {
        [Fact]
        public async Task given_invalid_type_id_should_throw_an_exception()
        {
            var typeId = Guid.NewGuid();
            var expectedException = new TypeNotFoundException(typeId);
            var command = new DeleteType(typeId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TypeNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((TypeNotFoundException)exception).Id.ShouldBe(expectedException.Id);
        }

        [Fact]
        public async Task given_valid_command_should_throw_an_exception_when_brand_has_items()
        {
            var name = "Type";
            var type = CreateType(name: name);
            type.Items.Add(Item.Create(Guid.NewGuid(), "test", Domain.Entities.Brand.Create(Guid.NewGuid(), "abc"), type, "description"));
            var command = new DeleteType(type.Id);
            _typeRepository.GetDetailsAsync(type.Id).Returns(type);
            var expectedException = new TypeCannotBeDeletedException(type.Id, name);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TypeCannotBeDeletedException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((TypeCannotBeDeletedException)exception).Id.ShouldBe(expectedException.Id);
            ((TypeCannotBeDeletedException)exception).Name.ShouldBe(expectedException.Name);
        }

        [Fact]
        public async Task given_valid_command_should_delete()
        {
            var name = "Brand";
            var type = CreateType(name: name);
            var command = new DeleteType(type.Id);
            _typeRepository.GetDetailsAsync(type.Id).Returns(type);

            await _handler.HandleAsync(command);

            await _typeRepository.Received(1).DeleteAsync(Arg.Any<Domain.Entities.Type>());
        }

        private readonly DeleteTypeHandler _handler;
        private readonly ITypeRepository _typeRepository;

        public DeleteTypeCommandHandlerTests()
        {
            _typeRepository = Substitute.For<ITypeRepository>();
            _handler = new DeleteTypeHandler(_typeRepository);
        }

        private Domain.Entities.Type CreateType(Guid? id = null, string name = "")
            => new Domain.Entities.Type(id != null ? id : Guid.NewGuid(), name, items: new List<Item>());
    }
}
