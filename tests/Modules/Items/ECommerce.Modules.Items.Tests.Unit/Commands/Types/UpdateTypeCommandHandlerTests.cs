using ECommerce.Modules.Items.Application.Commands.Types;
using ECommerce.Modules.Items.Application.Commands.Types.Handlers;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Repositories;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Unit.Commands.Types
{
    public class UpdateTypeCommandHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_update()
        {
            var command = new UpdateType(Guid.NewGuid(), "Type #1234");
            var type = CreateType(command.TypeId);
            _typeRepository.GetAsync(type.Id).Returns(type);

            await _handler.HandleAsync(command);

            await _typeRepository.Received(1).UpdateAsync(Arg.Any<Domain.Entities.Type>());
        }

        [Fact]
        public async Task given_null_command_should_throw_an_exception()
        {
            var expectedException = new UpdateTypeCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UpdateTypeCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_empty_name_should_throw_an_exception()
        {
            var command = new UpdateType(Guid.NewGuid(), "");
            var expectedException = new InvalidTypeNameException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidTypeNameException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_too_short_name_should_throw_an_exception()
        {
            var command = new UpdateType(Guid.NewGuid(), "ab");
            var expectedException = new TypeNameTooShortException(command.Name);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TypeNameTooShortException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((TypeNameTooShortException)exception).Name.ShouldBe(expectedException.Name);
        }

        [Fact]
        public async Task given_too_long_name_should_throw_an_exception()
        {
            var command = new UpdateType(Guid.NewGuid(), "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz");
            var expectedException = new TypeNameTooLongException(command.Name);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TypeNameTooLongException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((TypeNameTooLongException)exception).Name.ShouldBe(expectedException.Name);
        }

        [Fact]
        public async Task given_invalid_type_should_throw_an_exception()
        {
            var command = new UpdateType(Guid.NewGuid(), "Type");
            var expectedException = new TypeNotFoundException(command.TypeId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TypeNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((TypeNotFoundException)exception).Id.ShouldBe(expectedException.Id);
        }

        private readonly UpdateTypeHandler _handler;
        private readonly ITypeRepository _typeRepository;

        public UpdateTypeCommandHandlerTests()
        {
            _typeRepository = Substitute.For<ITypeRepository>();
            _handler = new UpdateTypeHandler(_typeRepository);
        }

        private Domain.Entities.Type CreateType(Guid id)
        {
            return new Domain.Entities.Type(id, "Type #1");
        }
    }
}
