namespace PhucNguyenResort.Web.Models.ViewModels
{
    public class CleaningHistoryViewModel
    {
        public int LogId { get; set; }
        public int RoomId { get; set; }

        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;

        public string AssignedStaffName { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public string? Notes { get; set; }

        public string Duration
        {
            get
            {
                if (StartedAt.HasValue)
                {
                    var timeSpan = CompletedAt - StartedAt.Value;
                    if (timeSpan.TotalMinutes < 60)
                    {
                        return $"{(int)timeSpan.TotalMinutes} phút";
                    }
                    return $"{(int)timeSpan.TotalHours} giờ {timeSpan.Minutes} phút";
                }
                return "Không ghi nhận";
            }
        }
    }
}