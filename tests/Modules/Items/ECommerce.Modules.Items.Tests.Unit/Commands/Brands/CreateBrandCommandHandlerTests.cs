using ECommerce.Modules.Items.Application.Commands.Brands;
using ECommerce.Modules.Items.Application.Commands.Brands.Handlers;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Services;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Messagging;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Unit.Commands.Brands
{
    public class CreateBrandCommandHandlerTests
    {
        [Fact]
        public async Task given_invalid_command_should_throw_an_exception()
        {
            var expectedException = new CreateBrandCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CreateBrandCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_empty_name_should_throw_an_exception()
        {
            var command = new CreateBrand("");
            var expectedException = new InvalidBrandNameException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBrandNameException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_too_short_name_shoul_throw_an_exception()
        {
            var name = "ab";
            var command = new CreateBrand(name);
            var expectedException = new BrandNameTooShortException(name);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BrandNameTooShortException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((BrandNameTooShortException) exception).Name.ShouldBe(name);
        }

        [Fact]
        public async Task given_too_long_name_shoul_throw_an_exception()
        {
            var name = "abcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyz123";
            var command = new CreateBrand(name);
            var expectedException = new BrandNameTooLongException(name);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BrandNameTooLongException>();
            exception.Message.ShouldBe(expectedException.Message);
            exception.Message.ShouldBe(expectedException.Message);
            ((BrandNameTooLongException) exception).Name.ShouldBe(name);
        }

        [Fact]
        public async Task given_valid_command_should_add_brand()
        {
            var name = "Brand";
            var command = new CreateBrand(name);

            await _handler.HandleAsync(command);

            await _brandRepository.Received(1).AddAsync(Arg.Any<Brand>());
        }

        private readonly CreateBrandHandler _handler;
        private readonly IBrandRepository _brandRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public CreateBrandCommandHandlerTests()
        {
            _brandRepository = Substitute.For<IBrandRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _eventMapper = Substitute.For<IEventMapper>();
            _handler = new CreateBrandHandler(_brandRepository, _messageBroker, _eventMapper);
        }
    }
}
