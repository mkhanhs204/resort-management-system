using System.Threading.Tasks;
using PhucNguyenResort.ViewModels.Dashboard;

namespace PhucNguyenResort.Services.Dashboard
{
    public interface IForecastService
    {
        Task<ForecastResultViewModel> GetForecastAsync();
    }
}
