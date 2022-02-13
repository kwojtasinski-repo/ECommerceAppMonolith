using ECommerce.Modules.Items.Application.Commands.Brands;
using ECommerce.Modules.Items.Application.Commands.Brands.Handlers;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
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
    public class UpdateBrandCommandHandlerTests
    {
        [Fact]
        public async Task given_invalid_command_should_throw_an_exception()
        {
            var expectedException = new UpdateBrandCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UpdateBrandCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_empty_name_should_throw_an_exception()
        {
            var command = new UpdateBrand(Guid.NewGuid(), "");
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
            var command = new UpdateBrand(Guid.NewGuid(), name);
            var expectedException = new BrandNameTooShortException(name);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BrandNameTooShortException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((BrandNameTooShortException)exception).Name.ShouldBe(name);
        }

        [Fact]
        public async Task given_too_long_name_shoul_throw_an_exception()
        {
            var name = "abcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyz123";
            var command = new UpdateBrand(Guid.NewGuid(), name);
            var expectedException = new BrandNameTooLongException(name);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BrandNameTooLongException>();
            exception.Message.ShouldBe(expectedException.Message);
            exception.Message.ShouldBe(expectedException.Message);
            ((BrandNameTooLongException)exception).Name.ShouldBe(name);
        }

        [Fact]
        public async Task given_invalid_id_should_throw_an_exception()
        {
            var name = "Brand";
            var command = new UpdateBrand(Guid.NewGuid(), name);
            var expectedException = new BrandNotFoundException(command.BrandId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BrandNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            exception.Message.ShouldBe(expectedException.Message);
            ((BrandNotFoundException)exception).Id.ShouldBe(command.BrandId);
        }

        [Fact]
        public async Task given_valid_command_should_update_brand()
        {
            var name = "Brand";
            var command = new UpdateBrand(Guid.NewGuid(), name);
            var brand = CreateBrand(command.BrandId, "test");
            _brandRepository.GetAsync(command.BrandId).Returns(brand);

            await _handler.HandleAsync(command);

            await _brandRepository.Received(1).UpdateAsync(Arg.Any<Brand>());
        }

        private readonly UpdateBrandHandler _handler;
        private readonly IBrandRepository _brandRepository;

        public UpdateBrandCommandHandlerTests()
        {
            _brandRepository = Substitute.For<IBrandRepository>();
            _handler = new UpdateBrandHandler(_brandRepository);
        }

        private Brand CreateBrand(Guid? id = null, string name = "")
            => new Brand(id != null ? id : Guid.NewGuid(), name, items: new List<Item>());
    }
}
