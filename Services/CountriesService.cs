using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(){
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null){
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if(string.IsNullOrEmpty(countryAddRequest.CountryName)){
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            var existingCountry = _countries.Where(x =>x.CountryName!.Equals(countryAddRequest.CountryName)).Count();
            if(existingCountry > 0){
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country
            Country country = countryAddRequest.ToCountry();
            //Generate new Guid
            country.CountryId = Guid.NewGuid();
            //Add Country to list
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries(){
            return _countries.Select(x => x.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByID(Guid? countryID){
            if(countryID == null){
                return null;
            }
            var country = _countries.FirstOrDefault(x => x.CountryId == countryID);
            if(country == null){
                return null;
            }
            return country.ToCountryResponse();
        }
    }
}

