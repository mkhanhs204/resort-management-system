using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    public interface ITourScheduleService
    {
        Task<IEnumerable<TourSchedule>> GetAllSchedulesAsync();
        Task<TourSchedule?> GetScheduleByIdAsync(int scheduleId);
        Task<TourSchedule> AddScheduleAsync(TourSchedule schedule);
        Task UpdateScheduleAsync(TourSchedule schedule);
        Task DeleteScheduleAsync(int scheduleId);
        bool ScheduleExists(int scheduleId);
        Task<bool> HasScheduleOnDateAsync(int guideId, System.DateTime date, int? excludeScheduleId = null);
    }
}
