using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Entities;
using ECommerce.Modules.Currencies.Core.Exceptions;
using ECommerce.Modules.Currencies.Core.Mappings;
using ECommerce.Modules.Currencies.Core.Repositories;
using ECommerce.Modules.Currencies.Core.Services;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Unit.Services
{
    public class CurrencyServiceTests
    {
        private readonly CurrencyService _service;
        private readonly ICurrencyRepository _repository;

        public CurrencyServiceTests()
        {
            _repository = Substitute.For<ICurrencyRepository>();
            _service = new CurrencyService(_repository);
        }

        [Fact]
        public async Task given_valid_dto_should_add_currency()
        {
            var dto = CreateCurrency();

            await _service.AddAsync(dto);

            await _repository.Received(1).AddAsync(Arg.Any<Currency>());
        }

        [Fact]
        public async Task given_not_existed_currency_when_update_should_throw_an_exception()
        {
            var dto = CreateCurrency();
            var expectedException = new CurrencyNotFoundException(dto.Id);

            var exception = await Record.ExceptionAsync(() => _service.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CurrencyNotFoundException>();
            ((CurrencyNotFoundException)exception).CurrencyId.ShouldBe(expectedException.CurrencyId);
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_not_existed_currency_when_delete_should_throw_an_exception()
        {
            var id = Guid.NewGuid();
            var expectedException = new CurrencyNotFoundException(id);

            var exception = await Record.ExceptionAsync(() => _service.DeleteAsync(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CurrencyNotFoundException>();
            ((CurrencyNotFoundException)exception).CurrencyId.ShouldBe(expectedException.CurrencyId);
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_currency_with_rates_when_delete_should_throw_an_exception()
        {
            var dto = CreateCurrencyDetails();
            var expectedException = new CannotDeleteCurrencyException(dto.Id);
            _repository.GetDetailsAsync(dto.Id).Returns(dto.AsEntity());

            var exception = await Record.ExceptionAsync(() => _service.DeleteAsync(dto.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CannotDeleteCurrencyException>();
            ((CannotDeleteCurrencyException)exception).CurrencyId.ShouldBe(expectedException.CurrencyId);
            exception.Message.ShouldBe(expectedException.Message);
        }

        private CurrencyDto CreateCurrency()
        {
            return new CurrencyDto()
            {
                Id = Guid.NewGuid(),
                Code = "EUR",
                Description = "Euro"
            };
        }
        
        private CurrencyDetailsDto CreateCurrencyDetails()
        {
            var currencyId = Guid.NewGuid();
            return new CurrencyDetailsDto()
            {
                Id = currencyId,
                Code = "EUR",
                Description = "Euro",
                CurrencyRates = new List<CurrencyRateDto> { new CurrencyRateDto { Id = Guid.NewGuid(), CurrencyDate = DateOnly.FromDateTime(DateTime.Now), CurrencyId = currencyId, Rate = new decimal(4.5544) } }
            };
        }
    }
}