using System.Threading.Tasks;
using System.Collections.Generic;

namespace PhucNguyenResort.Services.Gemini
{
    public interface IGeminiService
    {
        Task<string> ChatAsync(string userMessage, List<ChatMessageDto> history);
    }

    public class ChatMessageDto
    {
        public string Role { get; set; } = ""; // "user" hoặc "model"
        public string Text { get; set; } = "";
    }
}
