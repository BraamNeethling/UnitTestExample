using Systems.Web.Models;

namespace UnitTestExample.Test.Builders
{
    public class ProfileBuilder
    {
        private readonly Profile _profile = new Profile();

        public ProfileBuilder WithId(int id)
        {
            _profile.Id = id;
            return this;
        }

        public ProfileBuilder WithFullName(string fullName)
        {
            _profile.FullName = fullName;
            return this;
        }

        public ProfileBuilder WithAddress(Address address)
        {
            _profile.Address = address;
            return this;
        }

        public ProfileBuilder WithInterests(List<string> interests)
        {
            _profile.Interests = interests;
            return this;
        }

        public ProfileBuilder WithUser(User user)
        {
            _profile.User = user;
            return this;
        }

        public Profile Build()
        {
            return _profile;
        }
    }
}