using System;
using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.Models
{
    public class EmailOtp
    {
        public int OtpId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6)]
        public string OtpCode { get; set; } = string.Empty;

        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; } = false;

        public int Attempts { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
