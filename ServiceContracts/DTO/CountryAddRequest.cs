using System;
using System.Collections.Generic;
using Entities;

namespace ServiceContracts.DTO
{
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }
        public Country ToCountr(){
            return new Country(){
                CountryName = CountryName,
            };
        }
    }
}