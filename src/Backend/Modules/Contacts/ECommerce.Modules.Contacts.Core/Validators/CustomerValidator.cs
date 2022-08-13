using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Exceptions.Customers;
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
    internal class CustomerValidator : IValidator<CustomerDto>
    {
        public void Validate(CustomerDto validator)
        {
            if (validator is null)
            {
                throw new CustomerCannotBeNullException();
            }

            var exceptionList = new List<ECommerceException>();
            exceptionList.AddRange(ValidateFirstName(validator));
            exceptionList.AddRange(ValidateLastName(validator));
            exceptionList.AddRange(ValidateCompany(validator));
            exceptionList.AddRange(ValidatePhoneNumber(validator));

            if (exceptionList.Any())
            {
                throw new ValidationException(exceptionList);
            }
        }

        private IEnumerable<ECommerceException> ValidateFirstName(CustomerDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (string.IsNullOrWhiteSpace(validator.FirstName))
            {
                exceptionList.Add(new FirstNameCannotBeNullException());
            }

            if (validator.FirstName is not null)
            {
                if (validator.FirstName.Length < 3)
                {
                    exceptionList.Add(new FirstNameTooShortException(validator.FirstName));
                }

                if (validator.FirstName.Length > 50)
                {
                    exceptionList.Add(new FirstNameTooLongException(validator.FirstName));
                }
            }

            return exceptionList;
        }

        private IEnumerable<ECommerceException> ValidateLastName(CustomerDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (string.IsNullOrWhiteSpace(validator.FirstName))
            {
                exceptionList.Add(new LastNameCannotBeNullException());
            }

            if (validator.FirstName is not null)
            {
                if (validator.FirstName.Length < 3)
                {
                    exceptionList.Add(new LastNameTooShortException(validator.FirstName));
                }

                if (validator.FirstName.Length > 50)
                {
                    exceptionList.Add(new LastNameTooLongException(validator.FirstName));
                }
            }

            return exceptionList;
        }

        private IEnumerable<ECommerceException> ValidateCompany(CustomerDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (validator.Company is false)
            {
                return exceptionList;
            }

            if (string.IsNullOrWhiteSpace(validator.CompanyName))
            {
                exceptionList.Add(new CompanyNameCannotBeNullException());
            }

            if (validator.CompanyName is not null)
            {
                if (validator.CompanyName.Length < 3)
                {
                    exceptionList.Add(new CompanyNameTooShortException(validator.CompanyName));
                }    

                if (validator.CompanyName.Length > 150)
                {
                    exceptionList.Add(new CompanyNameTooLongException(validator.CompanyName));
                }
            }

            if (string.IsNullOrWhiteSpace(validator.NIP))
            {
                exceptionList.Add(new NIPCannotBeNullException());
            }

            if (validator.NIP is not null)
            {
                if (validator.NIP.Length < 9)
                {
                    exceptionList.Add(new NIPTooShortException(validator.NIP));
                }

                if (validator.NIP.Length > 16)
                {
                    exceptionList.Add(new NIPTooLongException(validator.NIP));
                }
            }

            return exceptionList;
        }

        private IEnumerable<ECommerceException> ValidatePhoneNumber(CustomerDto validator)
        {
            var exceptionList = new List<ECommerceException>();

            if (string.IsNullOrWhiteSpace(validator.PhoneNumber))
            {
                exceptionList.Add(new PhoneNumberCannotBeNullException());
            }

            if (validator.PhoneNumber is not null)
            {
                // optional start with + match all digits with length between 7 and 16 in one line
                if (!Regex.Match(validator.PhoneNumber, @"^(\+?)\d([0-9]{6,15})$").Success)
                {
                    exceptionList.Add(new InvalidPhoneNumberFormatException(validator.PhoneNumber));
                }

                if (validator.PhoneNumber.Length < 7)
                {
                    exceptionList.Add(new PhoneNumberTooShortException(validator.PhoneNumber));
                }

                if (validator.PhoneNumber.Length > 16)
                {
                    exceptionList.Add(new PhoneNumberTooLongException(validator.PhoneNumber));
                }
            }

            return exceptionList;
        }
    }
}
