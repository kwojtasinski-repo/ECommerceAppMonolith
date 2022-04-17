using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Mappings
{
    internal static class Extensions
    {
        public static Address AsEntity(this AddressDto dto)
        {
            var address = new Address
            {
                Id = dto.Id,
                BuildingNumber = dto.BuildingNumber,
                CityName = dto.CityName,
                CountryName = dto.CountryName,
                CustomerId = dto.CustomerId,
                LocaleNumber = dto.LocaleNumber,
                StreetName = dto.StreetName,
                ZipCode = dto.ZipCode,
            };
            return address;
        }

        public static AddressDto AsDto(this Address address)
        {
            var dto = new AddressDto
            {
                Id = address.Id,
                BuildingNumber = address.BuildingNumber,
                CityName = address.CityName,
                CountryName = address.CountryName,
                CustomerId = address.CustomerId,
                LocaleNumber = address.LocaleNumber,
                StreetName = address.StreetName,
                ZipCode = address.ZipCode,
            };
            return dto;
        }

        public static Customer AsEntity(this CustomerDto dto)
        {
            var customer = new Customer
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Company = dto.Company,
                CompanyName = dto.CompanyName,
                NIP = dto.NIP,
                UserId = dto.UserId
            };
            return customer;
        }

        public static CustomerDto AsDto(this Customer customer)
        {
            var dto = new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Company = customer.Company,
                CompanyName = customer.CompanyName,
                NIP = customer.NIP,
                UserId = customer.UserId
            };
            return dto;
        }

        public static CustomerDetailsDto AsDetailsDto(this Customer customer)
        {
            var dto = new CustomerDetailsDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Company = customer.Company,
                CompanyName = customer.CompanyName,
                NIP = customer.NIP,
                UserId = customer.UserId,
                Address = customer.Address?.AsDto()
            };
            return dto;
        }
    }
}
