using PhucNguyenResort.Helpers;
using PhucNguyenResort.ViewModels.Payments;

namespace PhucNguyenResort.Services.Payments
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;

        public VnPayService(IConfiguration config)
        {
            _config = config;
        }

        public string CreatePaymentUrl(HttpContext context, VNPayRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VNPay:Version"]!);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _config["VNPay:TmnCode"]!);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            
            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", context.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1");
            vnpay.AddRequestData("vnp_Locale", "vn");
            
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other");
            
            // Lấy ReturnUrl động dựa trên domain/port hiện tại của người dùng
            var returnUrl = $"{context.Request.Scheme}://{context.Request.Host}/Payment/VNPayReturn";
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            
            // Ở đây TxnRef thường dùng OrderId, nhưng VNPay yêu cầu duy nhất trong ngày, nên có thể dùng Ticks 
            // hoặc OrderId nếu không trùng lập. Để cho đúng test case thì truyền OrderId:
            vnpay.AddRequestData("vnp_TxnRef", model.OrderId.ToString());

            var paymentUrl = vnpay.CreateRequestUrl(_config["VNPay:BaseUrl"]!, _config["VNPay:HashSecret"]!);
            return paymentUrl;
        }

        public VNPayResponseModel ExecutePayment(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash.ToString(), _config["VNPay:HashSecret"]!);
            if (!checkSignature)
            {
                return new VNPayResponseModel
                {
                    Success = false,
                    VnPayResponseCode = "97" // Invalid Checksum
                };
            }

            return new VNPayResponseModel
            {
                Success = vnp_ResponseCode == "00",
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash.ToString(),
                VnPayResponseCode = vnp_ResponseCode
            };
        }
    }
}
