using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace xUnit_Test
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        public PersonServiceTest(){
            _personService = new PersonService();
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
    }
}