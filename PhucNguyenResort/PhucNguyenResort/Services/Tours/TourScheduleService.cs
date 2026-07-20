using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    public class TourScheduleService : ITourScheduleService
    {
        private readonly ResortDbContext _context;

        public TourScheduleService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourSchedule>> GetAllSchedulesAsync()
        {
            return await _context.TourSchedules
                .Include(ts => ts.Tour)
                .Include(ts => ts.Guide)
                .OrderByDescending(ts => ts.TourDate)
                .ToListAsync();
        }

        public async Task<TourSchedule?> GetScheduleByIdAsync(int scheduleId)
        {
            return await _context.TourSchedules
                .Include(ts => ts.Tour)
                .Include(ts => ts.Guide)
                .FirstOrDefaultAsync(ts => ts.ScheduleId == scheduleId);
        }

        public async Task<TourSchedule> AddScheduleAsync(TourSchedule schedule)
        {
            _context.TourSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task UpdateScheduleAsync(TourSchedule schedule)
        {
            _context.TourSchedules.Update(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteScheduleAsync(int scheduleId)
        {
            var schedule = await _context.TourSchedules.FindAsync(scheduleId);
            if (schedule != null)
            {
                _context.TourSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
        }

        public bool ScheduleExists(int scheduleId)
        {
            return _context.TourSchedules.Any(e => e.ScheduleId == scheduleId);
        }

        public async Task<bool> HasScheduleOnDateAsync(int guideId, System.DateTime date, int? excludeScheduleId = null)
        {
            var query = _context.TourSchedules
                .Where(ts => ts.GuideId == guideId && ts.TourDate.HasValue && ts.TourDate.Value.Date == date.Date);

            if (excludeScheduleId.HasValue)
            {
                query = query.Where(ts => ts.ScheduleId != excludeScheduleId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
