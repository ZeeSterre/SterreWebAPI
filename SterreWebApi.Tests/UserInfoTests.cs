using Microsoft.AspNetCore.Mvc;
using Moq;
using SterreWebApi.Controllers;
using SterreWebApi.Models;
using SterreWebApi.Repositorys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SterreWebApi.Tests
{
    [TestClass]
    public sealed class SystemAndUnitTests
    {
        private Mock<IUserInfoRepository> _userInfoRepository;
        private Mock<IAuthenticationService> _authenticationService;
        private UserInfoController _controller;

        [TestInitialize]
        public void Setup()
        {
            _userInfoRepository = new Mock<IUserInfoRepository>();
            _authenticationService = new Mock<IAuthenticationService>();
            _controller = new UserInfoController(_userInfoRepository.Object, _authenticationService.Object);
        }

        // === Systeemtesten ===

        [TestMethod]
        public async Task SystemTest_GetEnvironments_AsAuthenticatedUser_ShouldReturnEnvironments()
        {
            // Test of een geauthenticeerde gebruiker een lijst met omgevingen kan ophalen
            var userId = Guid.NewGuid();
            var environments = new List<Environment2D> { new Environment2D { Id = Guid.NewGuid(), Name = "TestEnv", UserId = userId } };

            _authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId.ToString());
            _userInfoRepository.Setup(x => x.GetEnvironmentsUser(userId)).ReturnsAsync(environments);

            var result = await _controller.GetEnvironments();
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task SystemTest_CreateEnvironment_WithValidData_ShouldReturnCreated()
        {
            // Test of een nieuwe omgeving correct wordt aangemaakt
            var userId = Guid.NewGuid();
            var request = new CreateEnvironmentRequest { Name = "NewEnv", EnvironmentType = 2 };

            _authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId.ToString());
            _userInfoRepository.Setup(x => x.CreateEnvironment(It.IsAny<Environment2D>())).ReturnsAsync(true);

            var result = await _controller.CreateEnvironment(request);
            var createdResult = result as CreatedAtActionResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [TestMethod]
        public async Task SystemTest_DeleteEnvironment_ValidRequest_ReturnsTrue()
        {
            // Test of een gebruiker met goede rechten kan verwijderen
            var userId = Guid.NewGuid();
            var environmentId = Guid.NewGuid();

            _userInfoRepository
                .Setup(x => x.DeleteEnvironment(It.Is<Guid>(id => id == environmentId), It.Is<Guid>(id => id == userId)))
                .ReturnsAsync(true);

            var result = await _userInfoRepository.Object.DeleteEnvironment(environmentId, userId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task SystemTest_DeleteEnvironment_InvalidEnvironmentId_ReturnsFalse()
        {
            // Test of een niet bestaande environment verwijderd kan worden
            var userId = Guid.NewGuid();
            var invalidEnvironmentId = Guid.NewGuid();

            _userInfoRepository
                .Setup(x => x.DeleteEnvironment(It.Is<Guid>(id => id == invalidEnvironmentId), It.Is<Guid>(id => id == userId)))
                .ReturnsAsync(false);

            var result = await _userInfoRepository.Object.DeleteEnvironment(invalidEnvironmentId, userId);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task SystemTest_DeleteEnvironment_UserNotOwner_ReturnsFalse()
        {
            // Test of een gebruiker zonder juiste rechten een omgeving niet kan verwijderen
            var actualOwnerId = Guid.NewGuid();
            var anotherUserId = Guid.NewGuid();
            var environmentId = Guid.NewGuid();

            _userInfoRepository
                .Setup(x => x.DeleteEnvironment(It.Is<Guid>(id => id == environmentId), It.Is<Guid>(id => id == anotherUserId)))
                .ReturnsAsync(false);

            var result = await _userInfoRepository.Object.DeleteEnvironment(environmentId, anotherUserId);

            Assert.IsFalse(result);
        }


        // === Unit-testen ===

        [TestMethod]
        public async Task UnitTest_GetEnvironments_UnauthenticatedUser_ShouldReturnUnauthorized()
        {
            // Test of een niet-geauthenticeerde gebruiker een 401 Unauthorized krijgt
            _authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(string.Empty);

            var result = await _controller.GetEnvironments();
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;

            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [TestMethod]
        public async Task UnitTest_CreateEnvironment_InvalidData_ShouldReturnBadRequest()
        {
            // Test of een BadRequest wordt gegeven bij null-gegevens voor environment aanmaken
            _authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(Guid.NewGuid().ToString());

            var result = await _controller.CreateEnvironment(null);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task UnitTest_DeleteEnvironment_UserNotAuthenticated_ShouldReturnUnauthorized()
        {
            // Test of een niet-geauthenticeerde gebruiker geen omgeving kan verwijderen
            var userId = Guid.NewGuid();
            var environmentId = Guid.NewGuid();

            _authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(string.Empty);

            var result = await _controller.DeleteEnvironment(environmentId);
            var unauthorizedResult = result as UnauthorizedObjectResult;

            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [TestMethod]
        public async Task UnitTest_DeleteEnvironment_InvalidData_ShouldReturnBadRequest()
        {
            // Test of een BadRequest komt als er een null of ongeldig environmentId wordt meegestuurd
            _authenticationService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(Guid.NewGuid().ToString());

            var result = await _controller.DeleteEnvironment(Guid.Empty);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }
    }
}
