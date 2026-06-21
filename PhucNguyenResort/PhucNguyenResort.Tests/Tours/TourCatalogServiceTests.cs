using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Tests.Tours
{
    // DTOs & Inputs
    public class TourFilterInput { public bool? IsActive { get; set; } }
    public class PagedResult<T> { public List<T> Items { get; set; } = new(); public int TotalCount { get; set; } }
    public class TourDetailDto { public int TourId { get; set; } public string TourName { get; set; } = string.Empty; public decimal BasePrice { get; set; } public object? Weather { get; set; } }
    public class CreateTourInput { public string TourName { get; set; } = string.Empty; public decimal Price { get; set; } public int MaxCapacity { get; set; } }
    
    public interface ITourCatalogRepository
    {
        Task<PagedResult<Tour>> FindActiveToursWithFilter(TourFilterInput filter);
        Task<Tour?> FindById(int tourId);
        Task<Tour> Save(Tour entity);
    }
    
    public interface IWeatherService
    {
        Task<object?> GetWeatherForecast(string location);
    }

    public class TourCatalogService
    {
        private readonly ITourCatalogRepository _repo;
        private readonly IWeatherService _weatherService;

        public TourCatalogService(ITourCatalogRepository repo, IWeatherService weatherService)
        {
            _repo = repo;
            _weatherService = weatherService;
        }

        public async Task<PagedResult<Tour>> GetActiveTours(TourFilterInput? filter)
        {
            var actualFilter = filter ?? new TourFilterInput { IsActive = true };
            return await _repo.FindActiveToursWithFilter(actualFilter);
        }

        public async Task<TourDetailDto> GetTourDetail(int tourId)
        {
            var tour = await _repo.FindById(tourId);
            if (tour == null) throw new Exception("Tour not found");

            object? weather = null;
            try
            {
                weather = await _weatherService.GetWeatherForecast("Quang Ninh"); // Assuming location
            }
            catch
            {
                // Graceful degradation
            }

            return new TourDetailDto
            {
                TourId = tour.TourId,
                TourName = tour.TourName ?? string.Empty,
                BasePrice = tour.Price ?? 0,
                Weather = weather
            };
        }

        public async Task<Tour> CreateTour(CreateTourInput input)
        {
            if (string.IsNullOrWhiteSpace(input.TourName)) throw new ArgumentException("Tour name required");
            if (input.Price <= 0) throw new ArgumentException("Price must be > 0");
            if (input.MaxCapacity <= 0) throw new ArgumentException("Capacity must be > 0");

            var tour = new Tour
            {
                TourName = input.TourName,
                Price = input.Price,
                MaxCapacity = input.MaxCapacity
            };

            return await _repo.Save(tour);
        }
    }

    public class TourCatalogServiceTests
    {
        private readonly Mock<ITourCatalogRepository> _mockRepo;
        private readonly Mock<IWeatherService> _mockWeather;
        private readonly TourCatalogService _service;

        public TourCatalogServiceTests()
        {
            _mockRepo = new Mock<ITourCatalogRepository>();
            _mockWeather = new Mock<IWeatherService>();
            _service = new TourCatalogService(_mockRepo.Object, _mockWeather.Object);
        }

        [Fact]
        public async Task TOUR_TC_001_GetActiveTours_ReturnsList()
        {
            // Arrange
            var activeTour = new Tour { TourId = 1, TourName = "Ha Long Bay Tour", Price = 1500000, MaxCapacity = 30 };
            _mockRepo.Setup(r => r.FindActiveToursWithFilter(It.IsAny<TourFilterInput>()))
                .ReturnsAsync(new PagedResult<Tour> { Items = new List<Tour> { activeTour } });

            // Act
            var result = await _service.GetActiveTours(null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(1, result.Items.First().TourId);
        }

        [Fact]
        public async Task TOUR_TC_002_GetTourDetail_IncludesWeather()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "Ha Long Bay Tour", Price = 1500000 };
            _mockRepo.Setup(r => r.FindById(1)).ReturnsAsync(tour);
            _mockWeather.Setup(w => w.GetWeatherForecast("Quang Ninh")).ReturnsAsync(new { Temp = 30 });

            // Act
            var result = await _service.GetTourDetail(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Ha Long Bay Tour", result.TourName);
            Assert.NotNull(result.Weather);
        }

        [Fact]
        public async Task TOUR_TC_009_CreateTour_Admin()
        {
            // Arrange
            var input = new CreateTourInput { TourName = "New Tour", Price = 1000, MaxCapacity = 20 };
            _mockRepo.Setup(r => r.Save(It.IsAny<Tour>())).ReturnsAsync((Tour t) => { t.TourId = 1; return t; });

            // Act
            var result = await _service.CreateTour(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Tour", result.TourName);
            Assert.Equal(1000, result.Price);
        }
    }
}
