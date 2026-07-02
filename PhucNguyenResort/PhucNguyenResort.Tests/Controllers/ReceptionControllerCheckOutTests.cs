using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Controllers;
using System.Reflection;

namespace PhucNguyenResort.Tests.Controllers
{
    public class ReceptionControllerCheckOutTests
    {
        [Fact]
        public void ReceptionController_HasAuthorizeAttribute_WithReceptionistAndAdminRoles()
        {
            // Arrange
            var controllerType = typeof(ReceptionController);

            // Act
            var authorizeAttribute = controllerType.GetCustomAttribute<AuthorizeAttribute>();

            // Assert
            Assert.NotNull(authorizeAttribute);
            Assert.Contains("Receptionist", authorizeAttribute.Roles);
            Assert.Contains("Admin", authorizeAttribute.Roles);
        }
    }
}
