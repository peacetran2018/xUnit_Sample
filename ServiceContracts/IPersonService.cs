using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating person entity
    /// </summary>
    public interface IPersonService
    {
        PersonResponse AddPerson(PersonAddRequest personAddRequest);
        List<PersonResponse> GetAllPersons();
        PersonResponse? GetPersonById(Guid? personID);
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
        List<PersonResponse> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrders);
        PersonResponse UpdatePerson(PersonUpdateRequest? personAddRequest);

        /// <summary>
        /// Deletes a person based on the given persion ID
        /// </summary>
        /// <param name="PersonID"></param>
        bool DeletePerson(Guid? personID);
    }
}