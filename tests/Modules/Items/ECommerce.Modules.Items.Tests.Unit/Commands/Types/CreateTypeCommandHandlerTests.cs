﻿using ECommerce.Modules.Items.Application.Commands.Types;
using ECommerce.Modules.Items.Application.Commands.Types.Handlers;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Services;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Kernel;
using ECommerce.Shared.Abstractions.Messagging;
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
    public class CreateTypeCommandHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_add()
        {
            var command = new CreateType("Type #1");

            await _handler.HandleAsync(command);

            await _typeRepository.Received(1).AddAsync(Arg.Any<Domain.Entities.Type>());
            _eventMapper.MapAll(Arg.Any<IEnumerable<IDomainEvent>>()).Received(1);
            await _messageBroker.Received(1).PublishAsync(Arg.Any<IMessage[]>());
        }

        [Fact]
        public async Task given_null_command_should_throw_an_exception()
        {
            var expectedException = new CreateTypeCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CreateTypeCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_empty_name_should_throw_an_exception()
        {
            var command = new CreateType("");
            var expectedException = new InvalidTypeNameException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidTypeNameException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_too_short_name_should_throw_an_exception()
        {
            var command = new CreateType("ab");
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
            var command = new CreateType("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz");
            var expectedException = new TypeNameTooLongException(command.Name);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TypeNameTooLongException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((TypeNameTooLongException)exception).Name.ShouldBe(expectedException.Name);
        }
        
        private readonly CreateTypeHandler _handler;
        private readonly ITypeRepository _typeRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public CreateTypeCommandHandlerTests()
        {
            _typeRepository = Substitute.For<ITypeRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _eventMapper = Substitute.For<IEventMapper>();
            _handler = new CreateTypeHandler(_typeRepository, _messageBroker, _eventMapper);
        }
    }
}
