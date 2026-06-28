using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Services.Reviews;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Controllers.Reviews;

public class ReviewControllerTests
{
    // Note: True Authorization tests (REVIEW-TC-SEC-001, REVIEW-TC-SEC-002) 
    // are better tested via Integration tests since [Authorize] is a middleware.
    // Here we can test that the actions require certain parameters or use services properly.

    [Fact]
    public async Task Manage_ReturnsViewResult_WithReviews()
    {
        var mockService = new Mock<IReviewService>();
        mockService.Setup(s => s.GetAllReviewsAsync()).ReturnsAsync(new List<PhucNguyenResort.ViewModels.Reviews.ReviewListVM>());

        var controller = new ReviewController(mockService.Object);

        var result = await controller.Manage();

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<IEnumerable<PhucNguyenResort.ViewModels.Reviews.ReviewListVM>>(viewResult.Model);
    }
}
