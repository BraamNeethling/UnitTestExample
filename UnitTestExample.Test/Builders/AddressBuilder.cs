using Systems.Web.Models;

namespace UnitTestExample.Test.Builders
{
    public class AddressBuilder
    {
        private readonly Address _address = new Address();

        public AddressBuilder WithStreet(string street)
        {
            _address.Street = street;
            return this;
        }

        public AddressBuilder WithCity(string city)
        {
            _address.City = city;
            return this;
        }

        public AddressBuilder WithCountry(string country)
        {
            _address.Country = country;
            return this;
        }

        public Address Build()
        {
            return _address;
        }
    }
}