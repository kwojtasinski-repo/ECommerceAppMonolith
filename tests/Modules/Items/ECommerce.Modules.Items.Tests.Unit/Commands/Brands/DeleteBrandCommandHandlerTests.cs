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
    public class DeleteBrandCommandHandlerTests
    {
        [Fact]
        public async Task given_invalid_brand_id_should_throw_an_exception()
        {
            var brandId = Guid.NewGuid();
            var expectedException = new BrandNotFoundException(brandId);
            var command = new DeleteBrand(brandId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BrandNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((BrandNotFoundException) exception).Id.ShouldBe(expectedException.Id);
        }

        [Fact]
        public async Task given_valid_command_should_throw_an_exception_when_brand_has_items()
        {
            var name = "Brand";
            var brand = CreateBrand(name: name);
            brand.Items.Add(Item.Create(Guid.NewGuid(), "test", brand, Domain.Entities.Type.Create(Guid.NewGuid(), "abc"), "description"));
            var command = new DeleteBrand(brand.Id);
            _brandRepository.GetDetailsAsync(brand.Id).Returns(brand);
            var expectedException = new BrandCannotBeDeletedException(brand.Id, name);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BrandCannotBeDeletedException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((BrandCannotBeDeletedException) exception).Id.ShouldBe(expectedException.Id);
            ((BrandCannotBeDeletedException) exception).Name.ShouldBe(expectedException.Name);
        }

        [Fact]
        public async Task given_valid_command_should_delete()
        {
            var name = "Brand";
            var brand = CreateBrand(name: name);
            var command = new DeleteBrand(brand.Id);
            _brandRepository.GetDetailsAsync(brand.Id).Returns(brand);

            await _handler.HandleAsync(command);

            await _brandRepository.Received(1).DeleteAsync(Arg.Any<Brand>());
        }

        private readonly DeleteBrandHandler _handler;
        private readonly IBrandRepository _brandRepository;

        public DeleteBrandCommandHandlerTests()
        {
            _brandRepository = Substitute.For<IBrandRepository>();
            _handler = new DeleteBrandHandler(_brandRepository);
        }

        private Brand CreateBrand(Guid? id = null, string name = "")
            => new Brand(id != null ? id : Guid.NewGuid(), name, items: new List<Item>());
    }
}
