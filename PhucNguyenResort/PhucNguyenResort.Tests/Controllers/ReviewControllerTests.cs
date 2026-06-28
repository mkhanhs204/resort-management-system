using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Services.Reviews;
using PhucNguyenResort.ViewModels.Reviews;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Controllers
{
    public class ReviewControllerTests
    {
        private ReviewController CreateControllerWithUser(string role, int userId, IReviewService service)
        {
            var controller = new ReviewController(service);

            var claims = new List<Claim>();
            if (role != "Guest")
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var tempData = new TempDataDictionary(controller.HttpContext, Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            return controller;
        }

        [Fact]
        public async Task REVIEW_TC_SEC_001_Manage_GuestAccess_IsHandledByAuthorizeAttribute()
        {
            // Note: In ASP.NET Core, [Authorize] attribute enforcement is handled by the framework pipeline.
            // Unit testing the attribute directly requires an integration test with TestServer.
            // Here we verify the attribute exists on the Manage action.
            var type = typeof(ReviewController);
            var methodInfo = type.GetMethod("Manage");
            var attributes = methodInfo!.GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute), false);
            
            Assert.NotEmpty(attributes);
            var authorizeAttr = (Microsoft.AspNetCore.Authorization.AuthorizeAttribute)attributes[0];
            Assert.Equal("Business Manager", authorizeAttr.Roles);
        }

        [Fact]
        public async Task REVIEW_TC_SEC_002_ToggleVisibility_CustomerAccess_IsHandledByAuthorizeAttribute()
        {
            // Verify Authorize attribute exists and requires BusinessManager role
            var type = typeof(ReviewController);
            var methodInfo = type.GetMethod("ToggleVisibility");
            var attributes = methodInfo!.GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute), false);
            
            Assert.NotEmpty(attributes);
            var authorizeAttr = (Microsoft.AspNetCore.Authorization.AuthorizeAttribute)attributes[0];
            Assert.Equal("Business Manager", authorizeAttr.Roles);
        }
    }
}
