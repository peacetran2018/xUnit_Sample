using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email {get; set; }
        public DateTime? DateOfBirth {get; set; }
        public string? Gender {get; set; }
        public Guid? CountryId { get; set; }
        public string? Address {get; set; }
        public bool ReceiveNewsLetters {get; set; }
        public double? Age {get; set; }

        public override bool Equals(object? obj){
            if(obj == null) return false;
            if(obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;
            return this.PersonId == person.PersonId && this.PersonName == person.PersonName && this.Email == person.Email && this.DateOfBirth == person.DateOfBirth;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class PersonExtensions{
        public static PersonResponse ToPersonResponse(this Person person){
            return new PersonResponse(){
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25): null,
            };
        }
    }
}