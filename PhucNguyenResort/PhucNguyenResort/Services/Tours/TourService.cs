using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    public class TourService : ITourService
    {
        private readonly ResortDbContext _context;

        public TourService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tour>> GetAllToursAsync()
        {
            return await _context.Tours.ToListAsync();
        }

        public async Task<Tour?> GetTourByIdAsync(int tourId)
        {
            return await _context.Tours.FirstOrDefaultAsync(t => t.TourId == tourId);
        }

        public async Task<Tour> AddTourAsync(Tour tour)
        {
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
            return tour;
        }

        public async Task UpdateTourAsync(Tour tour)
        {
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTourAsync(int tourId)
        {
            var tour = await _context.Tours.FindAsync(tourId);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
            }
        }

        public bool TourExists(int tourId)
        {
            return _context.Tours.Any(e => e.TourId == tourId);
        }
    }
}
