using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace xUnit_Test
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest(){
            _countriesService = new CountriesService();
        }

        [Fact]
        public async Task AddCountryAsync(){
            //Arrange
            CountryAddRequest countryAddRequest= new CountryAddRequest();
            
        }
    }
}