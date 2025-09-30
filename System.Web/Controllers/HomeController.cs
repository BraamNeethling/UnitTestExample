using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web.Models;
using Systems.Web.Models;
using UnitTestExample.Interfaces;

namespace System.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISystemService _systemService;
        public HomeController(ISystemService systemService)
        {
            _systemService = systemService ?? throw new ArgumentNullException(nameof(systemService));
        }

        public bool DoSomething()
        {
            return _systemService.DoSomething(true);
        }

        public User GetUser(int userId)
        {
            return _systemService.GetUser(userId);
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            return await _systemService.GetUserAsync(userId);
        }

        public void DeleteUser(int userId)
        {
            var user = _systemService.GetUser(userId);
            if(user == null)
            {
                throw new ArgumentException("User not found", nameof(userId));
            }
            user.IsActive = false;
            _systemService.DeleteUser(user);
        }

        public Profile GetProfile(int profileId)
        {
            return new Profile
            {
                Id = profileId,
                FullName = "Jane Doe",
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Metropolis",
                    Country = "Wonderland"
                },
                Interests = new List<string> { "Reading", "Traveling", "Coding" },
                User = new User
                {
                    Id = 99,
                    Name = "Jane",
                    Email = "jane@example.com",
                    IsActive = true
                }
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
