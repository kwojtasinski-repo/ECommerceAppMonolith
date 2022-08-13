using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Exceptions.Addresses;
using ECommerce.Shared.Abstractions.Exceptions;
using ECommerce.Shared.Abstractions.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Validators
{
    internal class AddressValidator : IValidator<AddressDto>
    {
        public void Validate(AddressDto validator)
        {
            if (validator is null)
            {
                throw new AddressCannotBeNullException();
            }

            var exceptionList = new List<ECommerceException>();
            exceptionList.AddRange(ValidateCityName(validator));
            exceptionList.AddRange(ValidateCountryName(validator));
            exceptionList.AddRange(ValidateStreetName(validator));
            exceptionList.AddRange(ValidateBuildingNumber(validator));
            exceptionList.AddRange(ValidateLocaleNumber(validator));
            exceptionList.AddRange(ValidateZipCode(validator));

            if (exceptionList.Any())
            {
                throw new ValidationException(exceptionList);
            }
        }

        private IEnumerable<ECommerceException> ValidateCityName(AddressDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (string.IsNullOrWhiteSpace(validator.CityName))
            {
                exceptionList.Add(new CityNameCannotBeEmptyException());
            }

            if (validator.CityName is not null)
            {
                if (validator.CityName.Length < 3)
                {
                    exceptionList.Add(new CityNameTooSmallException(validator.CityName));
                }

                if (validator.CityName.Length > 100)
                {
                    exceptionList.Add(new CityNameTooLongException(validator.CityName));
                }
            }

            return exceptionList;
        }
        
        private IEnumerable<ECommerceException> ValidateCountryName(AddressDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (string.IsNullOrWhiteSpace(validator.CountryName))
            {
                exceptionList.Add(new CountryNameCannotBeEmptyException());
            }

            if (validator.CountryName is not null)
            {

                if (validator.CountryName.Length < 3)
                {
                    exceptionList.Add(new CountryNameTooSmallException(validator.CityName));
                }

                if (validator.CountryName.Length > 100)
                {
                    exceptionList.Add(new CountryNameTooLongException(validator.CityName));
                }
            }

            return exceptionList;
        }
        
        private IEnumerable<ECommerceException> ValidateStreetName(AddressDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (string.IsNullOrWhiteSpace(validator.StreetName))
            {
                exceptionList.Add(new StreetNameCannotBeEmptyException());
            }

            if (validator.StreetName is not null)
            {

                if (validator.StreetName.Length < 3)
                {
                    exceptionList.Add(new StreetNameTooSmallException(validator.StreetName));
                }

                if (validator.StreetName.Length > 100)
                {
                    exceptionList.Add(new StreetNameTooLongException(validator.StreetName));
                }
            }

            return exceptionList;
        }
        
        private IEnumerable<ECommerceException> ValidateBuildingNumber(AddressDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (string.IsNullOrWhiteSpace(validator.BuildingNumber))
            {
                exceptionList.Add(new BuildingNumberCannotBeEmptyException());
            }

            if (validator.BuildingNumber is not null)
            {

                if (validator.BuildingNumber?.Length > 16)
                {
                    exceptionList.Add(new BuildingNumberTooLongException(validator.BuildingNumber));
                }
            }

            return exceptionList;
        }
        
        private IEnumerable<ECommerceException> ValidateLocaleNumber(AddressDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (validator.LocaleNumber != null)
            {
                if (string.IsNullOrWhiteSpace(validator.LocaleNumber))
                {
                    exceptionList.Add(new LocaleNumberCannotBeEmptyException());
                }

                if (validator.LocaleNumber.Length > 16)
                {
                    exceptionList.Add(new LocaleNumberTooLongException(validator.ZipCode));
                }
            }

            return exceptionList;
        }
        
        private IEnumerable<ECommerceException> ValidateZipCode(AddressDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (string.IsNullOrWhiteSpace(validator.ZipCode))
            {
                exceptionList.Add(new ZipCodeCannotBeEmptyException());
            }

            if (validator.ZipCode is not null)
            {

                if (validator.ZipCode.Length > 16)
                {
                    exceptionList.Add(new ZipCodeTooLongException(validator.ZipCode));
                }

                if (!Regex.Match(validator.ZipCode, @"([0-9]{2})-([0-9]{2})\w+").Success)
                {
                    exceptionList.Add(new InvalidZipCodeFormatException(validator.ZipCode));
                }
            }

            return exceptionList;
        }
    }
}
