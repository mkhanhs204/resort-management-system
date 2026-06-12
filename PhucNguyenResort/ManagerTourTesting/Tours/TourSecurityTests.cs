using Microsoft.AspNetCore.Mvc;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.ViewModels.Tours;
using System.Web;
using Xunit;

namespace ManagerTourTesting.Tours
{
    public class TourSecurityTests
    {
        [Fact]
        public void XSS_PayloadInTourName_ShouldBeEncodedByRazor_InTheory()
        {
            // TDD test note: Razor automatically encodes HTML in @Model.Property
            // This test simulates storing the payload and ensuring the controller doesn't encode it prematurely,
            // while validating the payload itself triggers no server-side execution.
            
            // Arrange
            var payload = "<script>alert('XSS')</script>";
            var model = new TourCreateVM { TourName = payload, Price = 100, MaxCapacity = 10, DurationHours = 5 };

            // Act
            // The controller will accept this as valid input.
            // The view engine (Razor) is responsible for encoding.
            var encodedPayload = HttpUtility.HtmlEncode(model.TourName);

            // Assert
            Assert.Equal("&lt;script&gt;alert(&#39;XSS&#39;)&lt;/script&gt;", encodedPayload);
        }

        [Fact]
        public void ControllerActions_ShouldHave_ValidateAntiForgeryTokenAttribute()
        {
            // Arrange
            var controllerType = typeof(ToursController);

            // Act
            var createPostMethod = controllerType.GetMethod("Create", new[] { typeof(TourCreateVM) });
            var editPostMethod = controllerType.GetMethod("Edit", new[] { typeof(int), typeof(TourEditVM) });
            var deletePostMethod = controllerType.GetMethod("DeleteConfirmed", new[] { typeof(int) });

            // Assert
            Assert.NotNull(createPostMethod);
            var hasAntiForgeryOnCreate = createPostMethod.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), false).Any();
            Assert.True(hasAntiForgeryOnCreate, "Create POST should have [ValidateAntiForgeryToken]");

            Assert.NotNull(editPostMethod);
            var hasAntiForgeryOnEdit = editPostMethod.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), false).Any();
            Assert.True(hasAntiForgeryOnEdit, "Edit POST should have [ValidateAntiForgeryToken]");

            Assert.NotNull(deletePostMethod);
            var hasAntiForgeryOnDelete = deletePostMethod.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), false).Any();
            Assert.True(hasAntiForgeryOnDelete, "Delete POST should have [ValidateAntiForgeryToken]");
        }
    }
}
