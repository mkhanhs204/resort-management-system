using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.Auth
{
    public class VerifyOtpViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mã xác thực.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã xác thực phải gồm 6 chữ số.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Mã xác thực chỉ chứa chữ số.")]
        [Display(Name = "Mã xác thực (OTP)")]
        public string OtpCode { get; set; } = string.Empty;
    }

    public class ResendOtpRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}
