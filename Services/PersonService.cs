using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons;
        public PersonService(){
            _persons = new List<Person>();
        }
        
        public PersonResponse AddPerson(PersonAddRequest personAddRequest)
        {
            if(personAddRequest == null){
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            
            // if(string.IsNullOrEmpty(personAddRequest.PersonName)){
            //     throw new ArgumentException(nameof(personAddRequest.PersonName));
            // }
            ValidationHelper.ModelValidation(personAddRequest);

            var person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();
            _persons.Add(person);

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(x => x.ToPersonResponse()).ToList();
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)){
                return matchingPersons;
            }

            matchingPersons = searchBy switch{
                //PersonName
                nameof(Person.PersonName) => allPersons.Where(x => 
                (!string.IsNullOrEmpty(x.PersonName) ? 
                 x.PersonName!.Contains(searchString, StringComparison.OrdinalIgnoreCase) :
                 true
                 )).ToList(),
                //Email
                nameof(Person.Email) => allPersons.Where(x => 
                (!string.IsNullOrEmpty(x.Email) ?
                x.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) :
                true
                )).ToList(),
                //DOB
                nameof(Person.DateOfBirth) => allPersons.Where(x => 
                (x.DateOfBirth != null) ?
                x.DateOfBirth.Value.ToString("dd-MMM-yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) :
                true
                ).ToList(),
                //Email
                nameof(Person.Gender) => allPersons.Where(x => 
                (!string.IsNullOrEmpty(x.Gender) ?
                x.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) :
                true
                )).ToList(),
                //Country
                nameof(Person.CountryId) => allPersons.Where(x => 
                (!string.IsNullOrEmpty(x.Country) ?
                x.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) :
                true
                )).ToList(),
                //Address
                nameof(Person.Address) => allPersons.Where(x => 
                (!string.IsNullOrEmpty(x.Address) ?
                x.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) :
                true
                )).ToList(),
                _ => allPersons
            };
            return matchingPersons;
        }

        public PersonResponse? GetPersonById(Guid? personID)
        {
            if(personID == null){
                return null;
            }

            Person? person = _persons.FirstOrDefault(x => x.PersonId == personID);
            if(person == null){
                return null;
            }

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrders)
        {
            if(string.IsNullOrEmpty(sortBy)){
                return allPersons;
            }
           List<PersonResponse> sortedPerson = (sortBy, sortOrders) switch{
                //PersonName
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                //Email
                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                //Address
                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                //DOB
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.DateOfBirth).ToList(),
                //Age
                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Age).ToList(),
                //Gender
                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                //Country
                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                //ReceiveNewLetter
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(x => x.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(x => x.ReceiveNewsLetters).ToList(),
                _ => allPersons
           };

            return sortedPerson;
        }
    }
}