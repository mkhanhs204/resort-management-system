using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhucNguyenResort.Models
{
    public class MaintenanceRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsResolved { get; set; } = false;

        public DateTime? ResolvedAt { get; set; }

        // 🟢 THÊM CỘT NÀY VÀO ĐỂ LƯU TÊN NHÂN VIÊN TIẾP NHẬN SỬA CHỮA
        [StringLength(100)]
        public string? AssignedStaffName { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }
    }
}