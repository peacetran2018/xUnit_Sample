using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace xUnit_Test
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public CountriesServiceTest(ITestOutputHelper testOutputHelper)
        {
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }
        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            //Act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();
            _testOutputHelper.WriteLine(response.CountryId.ToString());
            //Assert
            Assert.True(response.CountryId != Guid.Empty);
            //Contains method will call to Equals method to compare but default Equals method only compare reference instead actual values
            //so need to override Equals Method
            Assert.Contains(response, countries_from_GetAllCountries);
        }
        #endregion
        #region GetAllCountries
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            // Act
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            // Assert
            Assert.Empty(actual_country_response_list);
        }
        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            // Given
            List<CountryAddRequest> country_add_request_list = new List<CountryAddRequest>(){
                new CountryAddRequest(){ CountryName = "VN" },
                new CountryAddRequest(){ CountryName = "SG" }
            };
            // When
            List<CountryResponse> country_response_list_from_add_country = new List<CountryResponse>();
            foreach (var country_request in country_add_request_list)
            {
                country_response_list_from_add_country.Add(_countriesService.AddCountry(country_request));
            }
            // Then
            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            foreach (var expected_country in country_response_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }
        #endregion
        #region GetCountryByID
        [Fact]
        public void GetCountryByID_NullCountryID()
        {
            // Arrange
            Guid? countryID = null;
            // Act
            CountryResponse? country_response_from_get_method = _countriesService.GetCountryByID(countryID);
            // Assert
            Assert.Null(country_response_from_get_method);
        }

        [Fact]
        public void GetCountryByID_ValidCountryID()
        {
            // Arrange
            CountryAddRequest? country_add_request = new CountryAddRequest()
            {
                CountryName = "Vietnam",
            };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);
            // Act
            CountryResponse? country_response_from_get = _countriesService.GetCountryByID(country_response_from_add.CountryId);
            // Assert
            Assert.Equal(country_response_from_add, country_response_from_get);
        }
        #endregion
    }
}