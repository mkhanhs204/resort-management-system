namespace PhucNguyenResort.ViewModels.Payments
{
    public class VNPayRequestModel
    {
        public int OrderId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
