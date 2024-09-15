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

        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry(){
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsNull(){
            //Arrange
            CountryAddRequest? request = new CountryAddRequest(){
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName(){
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest(){
                CountryName = "Vietnam"
            };
            CountryAddRequest? request2 = new CountryAddRequest(){
                CountryName = "Vietnam"
            };

            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public void AddCountry_ProperCountry(){
            //Arrange
            CountryAddRequest? request = new CountryAddRequest(){
                CountryName = "Vietnam"
            };

            //Act
            CountryResponse response = _countriesService.AddCountry(request);

            //Assert
            Assert.True(response.CountryId != Guid.Empty);
        }
    }
}