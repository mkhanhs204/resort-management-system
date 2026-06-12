using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Services.Tours
{
    /// <summary>
    /// TourService — CRUD operations for Tour Catalog.
    /// Implements FK dependency check before hard-delete (ADR-002, BR-TOUR-005).
    /// </summary>
    public class TourService : ITourService
    {
        private readonly ResortDbContext _context;

        public TourService(ResortDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Tour>> GetAllToursAsync()
        {
            return await _context.Tours
                .OrderBy(t => t.TourId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Tour?> GetTourByIdAsync(int tourId)
        {
            return await _context.Tours
                .Include(t => t.TourSchedules)
                .FirstOrDefaultAsync(t => t.TourId == tourId);
        }

        /// <inheritdoc/>
        public async Task<Tour> AddTourAsync(Tour tour)
        {
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
            return tour;
        }

        /// <inheritdoc/>
        public async Task UpdateTourAsync(Tour tour)
        {
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Xóa tour theo ID.
        /// Throws <see cref="InvalidOperationException"/> nếu tour có TourSchedule liên kết (BR-TOUR-005 / ADR-002).
        /// </summary>
        /// <exception cref="InvalidOperationException">Tour has active TourSchedule dependencies.</exception>
        public async Task DeleteTourAsync(int tourId)
        {
            // BR-TOUR-005: Không được xóa nếu có TourSchedule đang hoạt động
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

        /// <inheritdoc/>
        public bool TourExists(int tourId)
        {
            return _context.Tours.Any(e => e.TourId == tourId);
        }
    }
}
