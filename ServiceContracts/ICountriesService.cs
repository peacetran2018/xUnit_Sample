using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating country entity
/// </summary>
public interface ICountriesService
{
    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
}
