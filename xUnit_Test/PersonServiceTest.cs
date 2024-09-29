using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace xUnit_Test
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonServiceTest(ITestOutputHelper testOutputHelper){
            _personService = new PersonService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region Add Person
        [Fact]
        public void AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequest? personAddRequest= null;
            // Assert
            Assert.Throws<ArgumentNullException>(() =>{
                //Act
                _personService.AddPerson(personAddRequest!);
            });
        }

        [Fact]
        public void AddPerson_PersonNameNull()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest(){
                PersonName = null
            };
            // Assert
            Assert.Throws<ArgumentException>(() =>{
                //Act
                _personService.AddPerson(personAddRequest!);
            });
        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest(){
                PersonName = "Peace",
                Email = "peace@example.com",
                Address = "Singapre",
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true
            };
        
            // Act
            PersonResponse? person_response_from_add = _personService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personService.GetAllPersons();
        
            // Assert
            Assert.True(person_response_from_add.CountryId != Guid.Empty);
            Assert.Contains(person_response_from_add, persons_list);
        }
        #endregion

        #region GetPersonById
        [Fact]
        public void GetPersonById_PersonIdNull()
        {
            // Arrage
            Guid? personId = null;
            //Act
            PersonResponse? person_response_from_get = _personService.GetPersonById(personId);
            // Assert
            Assert.Null(person_response_from_get);
        }
        [Fact]
        public void GetPersonById_WithPersonID()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest(){
                CountryName = "Vietnam"
            };
            CountryResponse country_response_from_add = _countriesService.AddCountry(countryAddRequest);
            // Act
            PersonAddRequest personAddRequest = new PersonAddRequest{
                PersonName = "Peace",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Sample Address",
                CountryId = country_response_from_add.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true
            };
            PersonResponse person_response_from_add = _personService.AddPerson(personAddRequest);
            
            PersonResponse? person_response_from_get = _personService.GetPersonById(person_response_from_add.PersonId);
        
            // Assert
            Assert.Equal(person_response_from_add, person_response_from_get);
        }
        #endregion
        #region GetAllPerson
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            // Arrange

            // Act
            List<PersonResponse> persons_from_get = _personService.GetAllPersons();
            // Assert
            Assert.Empty(persons_from_get);
        }

        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            // Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest(){
                CountryName = "Vietnam"
            };
            CountryAddRequest country_request_2 = new CountryAddRequest(){
                CountryName = "Singapore"
            };

            CountryResponse add_country_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse add_country_2 = _countriesService.AddCountry(country_request_2);
            
            PersonAddRequest person_add_request_1 = new PersonAddRequest(){
                PersonName = "Peace",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_1.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            PersonAddRequest person_add_request_2 = new PersonAddRequest(){
                PersonName = "Peace",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_2.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>(){
                person_add_request_1, person_add_request_2
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(var person_request in person_requests){
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }
            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach(PersonResponse person_response_from_add in person_response_list_from_add){
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            // Act  
            List<PersonResponse> get_all_persons = _personService.GetAllPersons();
            _testOutputHelper.WriteLine("Actual:");
            foreach(PersonResponse person_response_from_get in get_all_persons){
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            // Assert
            foreach(PersonResponse person_response_from_add in person_response_list_from_add){
                Assert.Contains(person_response_from_add, get_all_persons);
            }
        }
        #endregion
        #region GetFilteredPersons
        //If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            // Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest(){
                CountryName = "Vietnam"
            };
            CountryAddRequest country_request_2 = new CountryAddRequest(){
                CountryName = "Singapore"
            };

            CountryResponse add_country_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse add_country_2 = _countriesService.AddCountry(country_request_2);
            
            PersonAddRequest person_add_request_1 = new PersonAddRequest(){
                PersonName = "Peace",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_1.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            PersonAddRequest person_add_request_2 = new PersonAddRequest(){
                PersonName = "Peace",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_2.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>(){
                person_add_request_1, person_add_request_2
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(var person_request in person_requests){
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }
            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach(PersonResponse person_response_from_add in person_response_list_from_add){
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            // Act  
            List<PersonResponse> person_response_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual:");
            foreach(PersonResponse person_response_from_get in person_response_from_search){
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            // Assert
            foreach(PersonResponse person_response_from_add in person_response_list_from_add){
                Assert.Contains(person_response_from_add, person_response_from_search);
            }
        }
        
        //First we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
        [Fact]
        public void GetFilteredPersons_SearchByPersonName(){
            // Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest(){
                CountryName = "Vietnam"
            };
            CountryAddRequest country_request_2 = new CountryAddRequest(){
                CountryName = "Singapore"
            };

            CountryResponse add_country_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse add_country_2 = _countriesService.AddCountry(country_request_2);
            
            PersonAddRequest person_add_request_1 = new PersonAddRequest(){
                PersonName = "An",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_1.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            PersonAddRequest person_add_request_2 = new PersonAddRequest(){
                PersonName = "Giang",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_2.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            PersonAddRequest person_add_request_3 = new PersonAddRequest(){
                PersonName = "Mít",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_2.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>(){
                person_add_request_1, person_add_request_2, person_add_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(var person_request in person_requests){
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }
            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach(PersonResponse person_response_from_add in person_response_list_from_add){
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            // Act  
            List<PersonResponse> person_response_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "an");

            _testOutputHelper.WriteLine("Actual:");
            foreach(PersonResponse person_response_from_get in person_response_from_search){
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            // Assert
            foreach(PersonResponse person_response_from_add in person_response_list_from_add){
                if(!string.IsNullOrEmpty(person_response_from_add.PersonName)){
                    if(person_response_from_add.PersonName.Contains("an", StringComparison.OrdinalIgnoreCase)){
                        Assert.Contains(person_response_from_add, person_response_from_search);
                    }
                }
            }
        }
        #endregion

        #region GetSortedPersons
        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]
        public void GetSortedPersons()
        {
            // Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest(){
                CountryName = "Vietnam"
            };
            CountryAddRequest country_request_2 = new CountryAddRequest(){
                CountryName = "Singapore"
            };

            CountryResponse add_country_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse add_country_2 = _countriesService.AddCountry(country_request_2);
            
            PersonAddRequest person_add_request_1 = new PersonAddRequest(){
                PersonName = "Giang",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_1.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            PersonAddRequest person_add_request_2 = new PersonAddRequest(){
                PersonName = "An",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_2.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            PersonAddRequest person_add_request_3 = new PersonAddRequest(){
                PersonName = "Mít",
                Address = "Singapore",
                Email = "peace@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = add_country_2.CountryId,
                DateOfBirth = DateTime.Parse("1992-06-18"),
                ReceiveNewsLetters = true  
            };
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>(){
                person_add_request_1, person_add_request_2, person_add_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(var person_request in person_requests){
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            List<PersonResponse> allPersons = _personService.GetAllPersons();
            // Act  
            List<PersonResponse> person_list_from_sort = _personService.GetSortedPerson( allPersons, nameof(PersonResponse.PersonName), SortOrderOptions.DESC);
            //print person_response_list_from_sort

            _testOutputHelper.WriteLine("Expected:");
            foreach(PersonResponse person_response_from_get in person_list_from_sort){
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            person_response_list_from_add = person_response_list_from_add.OrderByDescending(x => x.PersonName).ToList();

            _testOutputHelper.WriteLine("Actual:");
            foreach(PersonResponse person_response_from_add in person_response_list_from_add){
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }
            // Assert
            for (int i = 0; i < person_response_list_from_add.Count; i++){
                Assert.Equal(person_response_list_from_add[i], person_list_from_sort[i]);
            }
        }
        #endregion
    }
}