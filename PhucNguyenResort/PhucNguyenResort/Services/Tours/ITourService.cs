using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    public interface ITourService
    {
        Task<IEnumerable<Tour>> GetAllToursAsync();
        Task<Tour?> GetTourByIdAsync(int tourId);
        Task<Tour> AddTourAsync(Tour tour);
        Task UpdateTourAsync(Tour tour);
        Task DeleteTourAsync(int tourId);
        bool TourExists(int tourId);
    }
}
