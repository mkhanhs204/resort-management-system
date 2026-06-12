using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using Xunit;

namespace ManagerTourTesting.Tours
{
    public class TourIntegrationTests
    {
        private ResortDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            return new ResortDbContext(options);
        }

        [Fact]
        public async Task FullCrudFlow_WorksCorrectly()
        {
            // Setup DB and Service
            var dbContext = GetDbContext();
            var tourService = new TourService(dbContext);

            // 1. Create
            var newTour = new Tour
            {
                TourName = "Integration Test Tour",
                Price = 500000,
                MaxCapacity = 20,
                DurationHours = 8
            };
            var createdTour = await tourService.AddTourAsync(newTour);
            
            Assert.True(createdTour.TourId > 0);

            // 2. Read Detail
            var tourFromDb = await tourService.GetTourByIdAsync(createdTour.TourId);
            Assert.NotNull(tourFromDb);
            Assert.Equal("Integration Test Tour", tourFromDb.TourName);

            // 3. Read List
            var allTours = await tourService.GetAllToursAsync();
            Assert.Single(allTours);

            // 4. Update
            tourFromDb.TourName = "Updated Tour Name";
            await tourService.UpdateTourAsync(tourFromDb);

            var updatedTour = await tourService.GetTourByIdAsync(createdTour.TourId);
            Assert.Equal("Updated Tour Name", updatedTour.TourName);

            // 5. Delete
            await tourService.DeleteTourAsync(createdTour.TourId);
            var deletedTour = await tourService.GetTourByIdAsync(createdTour.TourId);
            
            Assert.Null(deletedTour);
            
            var toursAfterDelete = await tourService.GetAllToursAsync();
            Assert.Empty(toursAfterDelete);
        }

        [Fact]
        public async Task AddTourAsync_ConcurrentCreates_ShouldBothSucceed()
        {
            var dbContext = GetDbContext();
            var tourService = new TourService(dbContext);

            var tour1 = new Tour { TourName = "Tour 1", Price = 100, MaxCapacity = 10, DurationHours = 1 };
            var tour2 = new Tour { TourName = "Tour 2", Price = 200, MaxCapacity = 20, DurationHours = 2 };

            var task1 = tourService.AddTourAsync(tour1);
            var task2 = tourService.AddTourAsync(tour2);

            await Task.WhenAll(task1, task2);

            var allTours = await tourService.GetAllToursAsync();
            Assert.Equal(2, allTours.Count());
            Assert.Contains(allTours, t => t.TourName == "Tour 1");
            Assert.Contains(allTours, t => t.TourName == "Tour 2");
        }
    }
}
