using Systems.Web.Models;
using UnitTestExample.Interfaces;

namespace System.Web.Services
{
    public class SystemService : ISystemService
    {
        public SystemService()
        {
        }

        public bool DoSomething(bool condition)
        {
            if (condition)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public User GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
