using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

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

            if(string.IsNullOrEmpty(personAddRequest.PersonName)){
                throw new ArgumentException(nameof(personAddRequest.PersonName));
            }

            var person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();
            _persons.Add(person);

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(x => x.ToPersonResponse()).ToList();
        }
    }
}