using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    public class TourCatalogService : ITourCatalogService
    {
        private readonly ResortDbContext _context;

        public TourCatalogService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tour>> GetAllToursAsync()
        {
            return await _context.Tours
                .OrderBy(t => t.TourId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tour>> GetActiveToursForHomeAsync()
        {
            return await _context.Tours
                .Include(t => t.TourSchedules)
                .Where(t => t.TourSchedules.Any(ts => ts.TourDate >= DateTime.Today && ts.AvailableSlots > 0))
                .OrderBy(t => t.TourId)
                .ToListAsync();
        }

        public async Task<Tour?> GetTourByIdAsync(int tourId)
        {
            return await _context.Tours
                .Include(t => t.TourSchedules)
                .Include(t => t.Images)
                .FirstOrDefaultAsync(t => t.TourId == tourId);
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
            bool hasSchedules = await _context.TourSchedules
                .AnyAsync(ts => ts.TourId == tourId);

            if (hasSchedules)
            {
                throw new InvalidOperationException("TOUR-003: Tour has active dependencies (TourSchedules). Cannot delete.");
            }

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

        public async Task<List<User>> GetAvailableManagersAsync()
        {
            // Return all active users who are Tour Guides (RoleId 7)
            return await _context.Users
                .Where(u => u.IsActive && u.RoleId == 7)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task<TourImage?> GetGalleryImageByIdAsync(int imageId)
        {
            return await _context.TourImages.FindAsync(imageId);
        }

        public async Task DeleteGalleryImageAsync(int imageId)
        {
            var image = await _context.TourImages.FindAsync(imageId);
            if (image != null)
            {
                _context.TourImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }
    }
}
