using System.Threading.Tasks;
using PhucNguyenResort.ViewModels.Dashboard;

namespace PhucNguyenResort.Services.Dashboard
{
    public interface IDashboardReportService
    {
        Task<OccupancyData> GetOccupancyDataAsync(int? month, int? year);
        Task<RevenueBreakdownData> GetRevenueBreakdownAsync(int? month, int? year);
        Task<FinancialReportViewModel> GetFinancialReportAsync(int? month, int? year);
        Task<byte[]> ExportToExcelAsync(int? month, int? year);
        Task<byte[]> ExportToPdfAsync(int? month, int? year);
    }
}
