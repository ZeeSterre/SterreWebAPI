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
    public sealed class EnvironmentsControllerTests
    {
        [TestMethod]
        public async Task Add_AddEnvironmentToUserWithNoEnvironments_ReturnsCreatedEnvironment()
        {
            // Arrange
            var newEnvironment = GenerateRandomEnvironment("new environment");

            var environmentRepository = new Mock<IEnvironment2DRepository>();

            // Zorg ervoor dat GetAllAsync een lege lijst teruggeeft
            environmentRepository.Setup(x => x.GetAllAsync())
                                 .ReturnsAsync(new List<Environment2D>());

            // Zorg ervoor dat AddAsync het Environment2D object teruggeeft, met een toegekend Id
            environmentRepository.Setup(x => x.AddAsync(It.IsAny<Environment2D>()))
                                 .ReturnsAsync((Environment2D env) =>
                                 {
                                     env.Id = new Random().Next(1001, 2000); // Simuleer database ID generatie
                                     return env;
                                 });

            var environmentController = new Environment2DController(environmentRepository.Object);

            // Act
            var response = await environmentController.Create(newEnvironment);

            // Assert
            Assert.IsInstanceOfType(response, typeof(ActionResult<Environment2D>));

            var actionResult = (ActionResult<Environment2D>)response;
            var createdAtActionResult = actionResult.Result as CreatedAtActionResult;

            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(nameof(Environment2DController.GetById), createdAtActionResult.ActionName);
            Assert.IsInstanceOfType(createdAtActionResult.Value, typeof(Environment2D));

            var actualEnvironment = (Environment2D)createdAtActionResult.Value;
            Assert.AreEqual(newEnvironment.Name, actualEnvironment.Name);
            Assert.AreEqual(newEnvironment.MaxHeight, actualEnvironment.MaxHeight);
            Assert.AreEqual(newEnvironment.MaxLength, actualEnvironment.MaxLength);
            Assert.IsTrue(actualEnvironment.Id >= 1001 && actualEnvironment.Id <= 2000, "ID should be assigned by repository.");
        }

        // Helper method to generate a random environment
        private static Environment2D GenerateRandomEnvironment(string name)
        {
            var random = new Random();
            return new Environment2D
            {
                Id = 0, // Laat de repository het Id instellen
                Name = name,
                MaxLength = random.Next(1, 100),
                MaxHeight = random.Next(1, 100)
            };
        }
    }
}
