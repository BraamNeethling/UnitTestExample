using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Systems.Web.Models;

namespace UnitTestExample.Interfaces
{
    public interface ISystemService
    {
        void DeleteUser(User user);
        bool DoSomething(bool condition);
        User GetUser(int userId);
        Task<User?> GetUserAsync(int userId);
    }
}
