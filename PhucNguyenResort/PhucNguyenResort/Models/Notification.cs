using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class Notification
{
    public int NotificationId { get; set; }
    public int? UserId { get; set; }

    [StringLength(255)]
    public string? Title { get; set; }

    public string? Message { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime SentAt { get; set; } = DateTime.Now;

    public User? User { get; set; }
}
