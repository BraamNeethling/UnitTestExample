using Systems.Web.Models;

namespace UnitTestExample.Test.Builders
{
    public class UserBuilder
    {
        private readonly User _user = new User();

        public UserBuilder WithId(int id)
        {
            _user.Id = id;
            return this;
        }

        public UserBuilder WithName(string name)
        {
            _user.Name = name;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _user.Email = email;
            return this;
        }

        public UserBuilder WithIsActive(bool isActive)
        {
            _user.IsActive = isActive;
            return this;
        }

        public User Build()
        {
            return _user;
        }
    }
}