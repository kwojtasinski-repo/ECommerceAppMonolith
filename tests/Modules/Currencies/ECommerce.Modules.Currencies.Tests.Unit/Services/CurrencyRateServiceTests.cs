using ECommerce.Modules.Currencies.Core.Clients.External;
using ECommerce.Modules.Currencies.Core.Entities;
using ECommerce.Modules.Currencies.Core.Exceptions;
using ECommerce.Modules.Currencies.Core.Repositories;
using ECommerce.Modules.Currencies.Core.Services;
using ECommerce.Shared.Abstractions.Messagging;
using ECommerce.Shared.Abstractions.Time;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Unit.Services
{
    public class CurrencyRateServiceTests
    {
        private readonly CurrencyRateService _service;
        private readonly ICurrencyRateRepository _repository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly INbpClient _nbpClient;
        private readonly IClock _clock;
        private readonly IMessageBroker _messageBroker;

        public CurrencyRateServiceTests()
        {
            _repository = Substitute.For<ICurrencyRateRepository>();
            _currencyRepository = Substitute.For<ICurrencyRepository>();
            _nbpClient = Substitute.For<INbpClient>();
            _clock = Substitute.For<IClock>();
            _clock.CurrentDate().Returns(DateTime.UtcNow);
            _messageBroker = Substitute.For<IMessageBroker>();
            _service = new CurrencyRateService(_repository, _currencyRepository, _nbpClient, _clock, _messageBroker);
        }

        [Fact]
        public async Task given_valid_id_and_date_should_return_currency_rate()
        {
            var currencyId = Guid.NewGuid();
            var date = new DateOnly(2022, 2, 11);
            var rate = new decimal(4.25);
            var currency = CreateSampleCurrency(currencyId);
            _currencyRepository.GetAsync(currencyId).Returns(currency);
            var currencyRateId = Guid.NewGuid();
            var currencyRate = CreateSampleCurrencyRate(currencyRateId, currencyId, date, rate);
            _repository.GetCurrencyRateForDateAsync(currencyId, date).Returns(currencyRate);

            var currencyRateFromDb = await _service.GetCurrencyForDateAsync(currencyId, date);

            currencyRateFromDb.ShouldNotBeNull();
            currencyRateFromDb.Id.ShouldBe(currencyRateId);
            currencyRateFromDb.CurrencyDate.ShouldBe(date);
            currencyRateFromDb.Rate.ShouldBe(rate);
        }

        [Fact]
        public async Task given_valid_id_and_rate_not_exist_on_this_date_should_return_null()
        {
            var currencyId = Guid.NewGuid();
            var date = new DateOnly(2022, 2, 11);
            var currency = CreateSampleCurrency(currencyId);
            _currencyRepository.GetAsync(currencyId).Returns(currency);

            var currencyRateFromDb = await _service.GetCurrencyForDateAsync(currencyId, date);

            currencyRateFromDb.ShouldBeNull();
        }

        [Fact]
        public async Task given_invalid_currency_id_should_throw_an_exception()
        {
            var currencyId = Guid.NewGuid();
            var date = new DateOnly(2022, 2, 11);
            var expectedException = new CurrencyNotFoundException(currencyId);

            var exception = await Record.ExceptionAsync(() => _service.GetCurrencyForDateAsync(currencyId, date));

            exception.ShouldBeOfType<CurrencyNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((CurrencyNotFoundException) exception).CurrencyId.ShouldBe(expectedException.CurrencyId);
        }

        [Fact]
        public async Task given_currency_pln_should_return_currency_rate_and_add_to_db()
        {
            var currencyId = Guid.NewGuid();
            var date = DateOnly.FromDateTime(DateTime.Now);
            var rate = decimal.One;
            var currency = new Currency { Id = currencyId, Code = "PLN", CurrencyRates = new List<CurrencyRate>(), Description = "description" };
            _currencyRepository.GetAsync(currencyId).Returns(currency);

            var currencyRate = await _service.GetLatestRateAsync(currencyId);

            await _repository.Received(1).AddAsync(Arg.Any<CurrencyRate>());
            currencyRate.ShouldNotBeNull();
            currencyRate.CurrencyId.ShouldBe(currencyId);
            currencyRate.CurrencyDate.ShouldBe(date);
            currencyRate.Rate.ShouldBe(rate);
        }

        [Fact]
        public async Task given_id_should_return_latest_currency_rate_from_db()
        {
            var currencyId = Guid.NewGuid();
            var date = DateOnly.FromDateTime(DateTime.Now);
            var rate = new decimal(4.25);
            var currency = CreateSampleCurrency(currencyId);
            _currencyRepository.GetAsync(currencyId).Returns(currency);
            var currencyRateId = Guid.NewGuid();
            var currencyRate = CreateSampleCurrencyRate(currencyRateId, currencyId, date, rate);
            _repository.GetCurrencyRateForDateAsync(currencyId, date).Returns(currencyRate);

            var currencyRateFromDb = await _service.GetLatestRateAsync(currencyId);

            await _repository.Received(0).AddAsync(Arg.Any<CurrencyRate>());
            await _nbpClient.Received(0).GetCurrencyRateOnDateAsync(Arg.Any<string>(), Arg.Any<DateOnly>());
            currencyRateFromDb.ShouldNotBeNull();
            currencyRateFromDb.CurrencyId.ShouldBe(currencyId);
            currencyRateFromDb.CurrencyDate.ShouldBe(date);
            currencyRateFromDb.Rate.ShouldBe(rate);
        }

        [Fact]
        public async Task given_id_should_return_latest_currency_rate_from_api_and_save_to_db()
        {
            var currencyId = Guid.NewGuid();
            var date = DateOnly.FromDateTime(DateTime.Now);
            var rate = new decimal(4.25);
            var currency = CreateSampleCurrency(currencyId); 
            _currencyRepository.GetAsync(currencyId).Returns(currency);
            var exchangeRate = new ExchangeRate { Code = currency.Code, Currency = currency.Description, Table = "A", Rates = new List<Rate> { new Rate { EffectiveDate = date, Mid = rate, No = "NO" } } };
            _nbpClient.GetCurrencyRateOnDateAsync(currency.Code, Arg.Any<DateOnly>()).Returns(exchangeRate);

            var currencyRateFromDb = await _service.GetLatestRateAsync(currencyId);

            await _repository.Received(1).AddAsync(Arg.Any<CurrencyRate>());
            currencyRateFromDb.ShouldNotBeNull();
            currencyRateFromDb.CurrencyId.ShouldBe(currencyId);
            currencyRateFromDb.CurrencyDate.ShouldBe(date);
            currencyRateFromDb.Rate.ShouldBe(rate);
        }

        [Fact]
        public async Task given_invalid_currency_code_should_throw_an_exception()
        {
            var currencyId = Guid.NewGuid();
            var currency = CreateSampleCurrency(currencyId);
            _currencyRepository.GetAsync(currencyId).Returns(currency);
            var expectedException = new CannotFindCurrencyRateException(currencyId, currency.Code);

            var exception = await Record.ExceptionAsync(() => _service.GetLatestRateAsync(currencyId));

            exception.ShouldBeOfType<CannotFindCurrencyRateException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((CannotFindCurrencyRateException) exception).CurrencyId.ShouldBe(expectedException.CurrencyId);
            ((CannotFindCurrencyRateException) exception).CurrencyCode.ShouldBe(expectedException.CurrencyCode);
        }

        [Fact]
        public async Task given_valid_currency_and_null_rate_should_throw_an_exception()
        {
            var currencyId = Guid.NewGuid();
            var date = DateOnly.FromDateTime(DateTime.Now);
            var rate = new decimal(4.25);
            var currency = CreateSampleCurrency(currencyId);
            _currencyRepository.GetAsync(currencyId).Returns(currency);
            var exchangeRate = new ExchangeRate { Code = currency.Code, Currency = currency.Description, Table = "A", Rates = new List<Rate>() };
            _nbpClient.GetCurrencyRateOnDateAsync(currency.Code, Arg.Any<DateOnly>()).Returns(exchangeRate);
            var expectedException = new RateNotFoundException(currency.Code, date);

            var exception = await Record.ExceptionAsync(() => _service.GetLatestRateAsync(currencyId));

            exception.ShouldBeOfType<RateNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((RateNotFoundException) exception).Date.ShouldBe(expectedException.Date);
            ((RateNotFoundException) exception).CurrencyCode.ShouldBe(expectedException.CurrencyCode);
        }

        [Fact]
        public async Task given_invalid_currency_id_when_getting_latest_rate_should_throw_an_exception()
        {
            var currencyId = Guid.NewGuid();
            var expectedException = new CurrencyNotFoundException(currencyId);

            var exception = await Record.ExceptionAsync(() => _service.GetLatestRateAsync(currencyId));

            exception.ShouldBeOfType<CurrencyNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((CurrencyNotFoundException) exception).CurrencyId.ShouldBe(expectedException.CurrencyId);
        }

        private Currency CreateSampleCurrency(Guid id)
        {
            return new Currency { Id = id, Code = "TST", CurrencyRates = new List<CurrencyRate>(), Description = "description" };
        }

        private CurrencyRate CreateSampleCurrencyRate(Guid id, Guid currencyId, DateOnly date, decimal rate)
        {
            return new CurrencyRate { Id = id, CurrencyId = currencyId, CurrencyDate = date, Rate = rate };
        }
    }
}
