using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhucNguyenResort.Models
{
    public class RoomCleaningLog
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        [StringLength(100)]
        public string AssignedStaffName { get; set; } = string.Empty;

        // Thời gian bắt đầu nhận phòng dọn (nếu có logic bấm "Bắt đầu dọn")
        public DateTime? StartedAt { get; set; }

        // Thời gian hoàn thành dọn dẹp
        [Required]
        public DateTime CompletedAt { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Notes { get; set; } // Ghi chú phát sinh khi dọn phòng (nếu có)

        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }
    }
}