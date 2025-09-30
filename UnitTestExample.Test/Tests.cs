using Moq;
using NUnit.Framework;
using System.Web.Controllers;
using Systems.Web.Models;
using UnitTestExample.Interfaces;
using UnitTestExample.Test.Builders;

namespace UnitTestExample.Test
{
    /// <summary>
    /// Example NUnit test suite for HomeController.
    /// Demonstrates Arrange/Act/Assert, setup/teardown, and various unit testing techniques.
    /// </summary>
    [TestFixture]
    public class Tests
    {
        // Shared fields for setup/teardown demonstration
        private Mock<ISystemService> _mockSystemService;
        private HomeController _controller;

        /// <summary>
        /// Runs before each test. Use this to set up common test resources.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Initialize the mock and controller before each test
            _mockSystemService = new Mock<ISystemService>();
            _controller = new HomeController(_mockSystemService.Object);
        }

        /// <summary>
        /// Runs after each test. Use this to clean up resources.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Example: Clean up or reset resources if needed
            _mockSystemService = null;
            _controller = null;
        }

        // Sample test to demonstrate basic assertion syntax
        [Test]
        public void SampleTest()
        {
            // Assert: Verify that basic arithmetic works as expected
            Assert.That(1 + 1, Is.EqualTo(2));
        }

        // Unit test for a simple service method call
        [Test]
        public void DoSomething_ReturnsExpectedResult()
        {
            // Arrange: Set up mock to return true when DoSomething(true) is called
            _mockSystemService.Setup(s => s.DoSomething(true)).Returns(true);

            // Act: Call the method under test
            var result = _controller.DoSomething();

            // Assert: Verify the result and that the service method was called once
            Assert.That(result, Is.True);
            _mockSystemService.Verify(s => s.DoSomething(true), Times.Once);
        }

        // Demonstrates parameterized testing for multiple input/output combinations
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void DoSomething_ReturnsExpectedResult_ForVariousInputs(bool input, bool expected)
        {
            // Arrange: Configure mock to return expected value for each input
            _mockSystemService.Setup(s => s.DoSomething(input)).Returns(expected);

            // Act: Call the method under test
            var result = _controller.DoSomething();

            // Assert: Check that the result matches the expected value
            Assert.That(result, Is.EqualTo(expected));
        }

        // Shows how to use a builder pattern for test data setup
        [Test]
        public void Build_CreatesUserWithSpecifiedProperties()
        {
            // Arrange: Use UserBuilder to create a user with specific properties
            var builder = new UserBuilder()
                .WithId(42)
                .WithName("Alice")
                .WithEmail("alice@example.com")
                .WithIsActive(false);

            // Act: Build the user object
            var user = builder.Build();

            // Assert: Verify all properties are set as expected
            Assert.That(user.Id, Is.EqualTo(42));
            Assert.That(user.Name, Is.EqualTo("Alice"));
            Assert.That(user.Email, Is.EqualTo("alice@example.com"));
            Assert.That(user.IsActive, Is.False);
        }

        // Validates that GetUser returns the correct user object
        [Test]
        public void GetUser_ReturnsExpectedUser()
        {
            // Arrange: Set up expected user and mock service to return it
            int userId = 1;
            var expectedUser = new UserBuilder()
               .WithId(userId)
               .WithName("Default User")
               .WithEmail("default@user.com")
               .WithIsActive(true)
               .Build();

            _mockSystemService.Setup(s => s.GetUser(userId)).Returns(expectedUser);

            // Act: Retrieve the user
            var result = _controller.GetUser(userId);

            // Assert: Compare all properties to expected values
            Assert.That(result.Id, Is.EqualTo(expectedUser.Id));
            Assert.That(result.Name, Is.EqualTo(expectedUser.Name));
            Assert.That(result.Email, Is.EqualTo(expectedUser.Email));
            Assert.That(result.IsActive, Is.EqualTo(expectedUser.IsActive));
        }

        // Example of testing asynchronous controller/service methods
        [Test]
        public async Task GetUserAsync_ReturnsExpectedUser()
        {
            // Arrange: Set up mock to return expected user asynchronously
            var userId = 1;
            var expectedUser = new UserBuilder().WithId(userId).Build();
            _mockSystemService.Setup(s => s.GetUserAsync(userId)).ReturnsAsync(expectedUser);

            // Act: Await the async method
            var result = await _controller.GetUserAsync(userId);

            // Assert: Verify the returned user matches expectations
            Assert.That(result!.Id, Is.EqualTo(expectedUser.Id));
        }

        // Tests a method with multiple service interactions and state changes
        [Test]
        public void DeleteUser_CallsGetUser_SetsInactive_AndCallsDeleteUser()
        {
            // Arrange: Set up mock to return a user and expect DeleteUser to be called
            var userId = 123;

            var user = new UserBuilder()
                .WithId(userId)
                .WithName("Test User")
                .WithEmail("test@example.com")
                .WithIsActive(true)
                .Build();

            _mockSystemService.Setup(s => s.GetUser(userId)).Returns(user);

            // Act: Call DeleteUser
            _controller.DeleteUser(userId);

            // Assert: Verify GetUser and DeleteUser were called, and user is inactive
            _mockSystemService.Verify(s => s.GetUser(userId), Times.Once);
            Assert.That(user.IsActive, Is.False);
            _mockSystemService.Verify(s => s.DeleteUser(user), Times.Once);
        }

        // Demonstrates testing for exception handling
        [Test]
        public void DeleteUser_ThrowsArgumentException_WhenUserNotFound()
        {
            // Arrange: Mock service returns null for any userId
            _mockSystemService.Setup(s => s.GetUser(It.IsAny<int>())).Returns((User)null);

            // Act & Assert: Expect ArgumentException when user is not found
            var ex = Assert.Throws<ArgumentException>(() => _controller.DeleteUser(999));
            Assert.That(ex.Message, Does.Contain("User not found"));
            _mockSystemService.Verify(s => s.DeleteUser(It.IsAny<User>()), Times.Never);
        }

        // Verifies state change before a method call
        [Test]
        public void DeleteUser_SetsUserInactive_BeforeCallingDeleteUser()
        {
            // Arrange: Set up user as active, expect it to be inactive after deletion
            var userId = 1;
            var user = new UserBuilder().WithId(userId).WithIsActive(true).Build();
            _mockSystemService.Setup(s => s.GetUser(userId)).Returns(user);

            // Act: Call DeleteUser
            _controller.DeleteUser(userId);

            // Assert: Verify DeleteUser called with inactive user
            _mockSystemService.Verify(s => s.DeleteUser(It.Is<User>(u => u.Id == userId && u.IsActive == false)), Times.Once);
        }

        // Shows use of Moq's It.Is for argument matching
        [Test]
        public void DeleteUser_VerifiesDeleteUserCalledWithInactiveUser()
        {
            // Arrange: Prepare user and mock
            var userId = 1;
            var user = new UserBuilder().WithId(userId).WithIsActive(true).Build();
            _mockSystemService.Setup(s => s.GetUser(userId)).Returns(user);

            // Act: Call DeleteUser
            _controller.DeleteUser(userId);

            // Assert: Ensure DeleteUser is called with the correct user state
            _mockSystemService.Verify(s => s.DeleteUser(It.Is<User>(u => u.Id == userId && u.IsActive == false)), Times.Once);
        }

        //Show test of a complex moddel structure
        [Test]
        public void GetProfile_ReturnsExpectedComplexProfile()
        {
            // Arrange
            int profileId = 1;

            var expectedProfile = new ProfileBuilder()
                .WithId(profileId)
                .WithFullName("Jane Doe")
                .WithAddress(new AddressBuilder()
                    .WithStreet("123 Main St")
                    .WithCity("Metropolis")
                    .WithCountry("Wonderland")
                    .Build())
                .WithInterests(new List<string> { "Reading", "Traveling", "Coding" })
                .WithUser(new UserBuilder()
                    .WithId(99)
                    .WithName("Jane")
                    .WithEmail("jane@example.com")
                    .WithIsActive(true)
                    .Build())
                .Build();

            // Act
            var profile = _controller.GetProfile(profileId);

            // Assert
            Assert.That(profile.Id, Is.EqualTo(expectedProfile.Id));
            Assert.That(profile.FullName, Is.EqualTo(expectedProfile.FullName));
            Assert.That(profile.Address.Street, Is.EqualTo(expectedProfile.Address.Street));
            Assert.That(profile.Address.City, Is.EqualTo(expectedProfile.Address.City));
            Assert.That(profile.Address.Country, Is.EqualTo(expectedProfile.Address.Country));
            Assert.That(profile.Interests, Is.EquivalentTo(expectedProfile.Interests));
            Assert.That(profile.User.Id, Is.EqualTo(expectedProfile.User.Id));
            Assert.That(profile.User.Name, Is.EqualTo(expectedProfile.User.Name));
            Assert.That(profile.User.Email, Is.EqualTo(expectedProfile.User.Email));
            Assert.That(profile.User.IsActive, Is.EqualTo(expectedProfile.User.IsActive));

            //Assert.That(profile, Is.EqualTo(expectedProfile));
            //Assert.That(profile.Address, Is.EqualTo(expectedProfile.Address));
            //Assert.That(profile.User, Is.EqualTo(expectedProfile.User));
        }
    }
}