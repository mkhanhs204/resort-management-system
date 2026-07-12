using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PhucNguyenResort.Data;

namespace PhucNguyenResort.Services.Gemini
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly ResortDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiService> _logger;

        public GeminiService(
            HttpClient httpClient, 
            ResortDbContext dbContext, 
            IConfiguration configuration,
            ILogger<GeminiService> logger)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> ChatAsync(string userMessage, List<ChatMessageDto> history)
        {
            try
            {
                var apiKey = _configuration["Gemini:ApiKey"];
                var model = _configuration["Gemini:Model"] ?? "gemini-1.5-flash";
                var rawUrl = _configuration["Gemini:ApiUrl"] ?? "https://generativelanguage.googleapis.com/v1beta/models/{0}:generateContent?key={1}";
                var apiUrl = string.Format(rawUrl, model, apiKey);

                // 1. Lấy dữ liệu động từ database làm ngữ cảnh (Context) - ĐÃ TỐI ƯU THEO INTENT
                string dynamicContext = await BuildOptimizedContextAsync(userMessage);

                // 2. Định nghĩa System Instruction (Đã thêm Guardrails, Đa ngôn ngữ, và Deep Links)
                string systemInstruction = "Bạn là trợ lý ảo AI của resort cao cấp 'Phúc Nguyễn Resort'.\n" +
                    "Nhiệm vụ của bạn là tư vấn các dịch vụ của resort và hướng dẫn cách đặt phòng, đặt tour.\n\n" +
                    "--- QUY TẮC CỐT LÕI (GUARDRAILS) ---\n" +
                    "1. ĐA NGÔN NGỮ: Hãy phát hiện ngôn ngữ khách hàng đang sử dụng và trả lời bằng chính ngôn ngữ đó (ưu tiên Tiếng Việt và Tiếng Anh).\n" +
                    "2. BẢO VỆ THƯƠNG HIỆU: Tuyệt đối KHÔNG cung cấp thông tin, so sánh hoặc nhắc đến các Resort đối thủ cạnh tranh.\n" +
                    "3. PHẠM VI TRẢ LỜI: KHÔNG trả lời các chủ đề về chính trị, tôn giáo, y tế, hoặc các câu hỏi ngoài lề không liên quan đến du lịch/nghỉ dưỡng.\n" +
                    "4. TÍNH CHÍNH XÁC: Chỉ sử dụng thông tin được cung cấp trong phần NGỮ CẢNH bên dưới. Nếu thông tin không có sẵn, hãy khuyên khách liên hệ Hotline: +84 901 234 567 hoặc Email: info@phucnguyenresort.com.\n" +
                    "5. TĂNG CHUYỂN ĐỔI (CALL-TO-ACTION): Khi giới thiệu phòng hoặc tour, HÃY LUÔN kèm theo một đường dẫn đặt chỗ (Deep Link) dưới dạng Markdown. Ví dụ: `[👉 Đặt phòng ngay](/Home/SearchRooms)` hoặc `[👉 Xem chi tiết Tour](/Tours)`.\n\n" +
                    "--- NGỮ CẢNH DỮ LIỆU THỰC TẾ ---\n" +
                    dynamicContext + "\n" +
                    "--- QUY ĐỊNH & CHÍNH SÁCH ---\n" +
                    "- Nhận phòng: 14:00 | Trả phòng: 12:00. Phụ thu nhận sớm/trả muộn theo quy định thực tế.\n" +
                    "- Hỗ trợ thanh toán an toàn và nhanh chóng qua cổng VNPay trực tuyến.";

                // 3. Cấu trúc Payload cho Gemini API (hỗ trợ System Instruction & Chat History)
                var contents = new List<object>();

                // Thêm lịch sử trò chuyện
                if (history != null)
                {
                    foreach (var chat in history)
                    {
                        contents.Add(new
                        {
                            role = chat.Role == "model" ? "model" : "user",
                            parts = new[] { new { text = chat.Text } }
                        });
                    }
                }

                // Thêm tin nhắn hiện tại
                contents.Add(new
                {
                    role = "user",
                    parts = new[] { new { text = userMessage } }
                });

                var requestPayload = new
                {
                    contents = contents,
                    systemInstruction = new
                    {
                        parts = new[] { new { text = systemInstruction } }
                    },
                    generationConfig = new
                    {
                        temperature = 0.4, // Độ sáng tạo vừa phải để đảm bảo tính chính xác
                        maxOutputTokens = 8000 // Tăng max token vì gemini-2.5-flash cần nhiều token để thinking
                    }
                };

                var jsonPayload = JsonSerializer.Serialize(requestPayload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Gemini API Error: Status {Status}, Body: {Body}", response.StatusCode, errBody);
                    return "Xin lỗi, hệ thống AI đang bận xử lý. Quý khách vui lòng thử lại sau giây lát hoặc liên hệ hotline để được phục vụ trực tiếp.";
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseBody);
              
                // Trích xuất text phản hồi từ cấu trúc JSON của Gemini
                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? "Tôi không nhận diện được câu trả lời. Quý khách cần hỗ trợ thêm thông tin nào ạ?";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra trong quá trình kết nối với Gemini AI");
                return "Đã xảy ra sự cố kết nối. Quý khách có thể liên hệ trực tiếp qua số Hotline của chúng tôi để được tư vấn nhanh nhất.";
            }
        }

        private async Task<string> BuildOptimizedContextAsync(string userMessage)
        {
            var sb = new StringBuilder();
            var msg = userMessage.ToLower();

            // Phân loại ý định cơ bản bằng Keyword (Tối ưu hóa token)
            bool isAskingRoom = msg.Contains("phòng") || msg.Contains("ngủ") || msg.Contains("giá") || msg.Contains("room") || msg.Contains("book");
            bool isAskingTour = msg.Contains("tour") || msg.Contains("tham quan") || msg.Contains("đi chơi") || msg.Contains("trải nghiệm") || msg.Contains("guide");
            bool isAskingFood = msg.Contains("ăn") || msg.Contains("uống") || msg.Contains("nhà hàng") || msg.Contains("thực đơn") || msg.Contains("menu") || msg.Contains("food");

            // Nếu không xác định rõ, load thông tin tiêu biểu cơ bản
            if (!isAskingRoom && !isAskingTour && !isAskingFood) {
                isAskingRoom = true; // Default
            }

            if (isAskingRoom)
            {
                sb.AppendLine("[DANH SÁCH LOẠI PHÒNG NGỦ]");
                var categories = await _dbContext.RoomCategories.AsNoTracking().ToListAsync();
                foreach (var cat in categories)
                {
                    sb.AppendLine($"- {cat.CategoryName} | Giá mỗi đêm: {cat.PricePerNight:N0} VNĐ | Tối đa: {cat.MaxAdults} Người lớn, {cat.MaxChildren} Trẻ em. {(string.IsNullOrEmpty(cat.Description) ? "" : "Đặc điểm: " + cat.Description)}");
                }
                sb.AppendLine();
            }

            if (isAskingTour)
            {
                sb.AppendLine("[DANH SÁCH TOUR DU LỊCH]");
                var tours = await _dbContext.Tours.AsNoTracking().ToListAsync();
                foreach (var tour in tours)
                {
                    sb.AppendLine($"- {tour.TourName} | Giá tour: {tour.Price:N0} VNĐ/khách | Vị trí: {tour.Location} | Thời gian: {tour.DurationHours} tiếng.");
                }
                sb.AppendLine();
            }

            if (isAskingFood)
            {
                sb.AppendLine("[THỰC ĐƠN NHÀ HÀNG - CÁC MÓN TIÊU BIỂU]");
                var menuItems = await _dbContext.MenuItems.Where(m => m.IsAvailable).Take(10).AsNoTracking().ToListAsync();
                foreach (var item in menuItems)
                {
                    sb.AppendLine($"- {item.ItemName} | Giá: {item.Price:N0} VNĐ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
