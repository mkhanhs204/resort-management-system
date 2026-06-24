using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    public interface ITourCatalogService
    {
        Task<IEnumerable<Tour>> GetAllToursAsync();
        Task<Tour?> GetTourByIdAsync(int tourId);
        Task<Tour> AddTourAsync(Tour tour);
        Task UpdateTourAsync(Tour tour);
        Task DeleteTourAsync(int tourId);
        Task<IEnumerable<Tour>> GetActiveToursForHomeAsync();
        bool TourExists(int tourId);
        Task<List<User>> GetAvailableManagersAsync();
    }
}
