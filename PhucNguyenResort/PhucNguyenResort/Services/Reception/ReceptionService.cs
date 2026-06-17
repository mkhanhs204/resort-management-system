using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.ViewModels.Bookings;

namespace PhucNguyenResort.Services.Reception
{
    public class ReceptionService : IReceptionService
    {
        private readonly ResortDbContext _context;
        private readonly IBookingGuestService _bookingGuestService;

        public ReceptionService(ResortDbContext context, IBookingGuestService bookingGuestService)
        {
            _context = context;
            _bookingGuestService = bookingGuestService;
        }

        public async Task<List<Booking>> GetBookingsAsync(string? searchString, BookingStatus? statusFilter, DateTime? dateFilter = null)
        {
            var query = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .ThenInclude(r => r.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(b => 
                    b.BookingId.ToString() == searchString ||
                    (b.User != null && b.User.FullName != null && b.User.FullName.Contains(searchString)) ||
                    (b.User != null && b.User.Email != null && b.User.Email.Contains(searchString)) ||
                    (b.Room != null && b.Room.RoomNumber != null && b.Room.RoomNumber.Contains(searchString))
                );
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(b => b.BookingStatus == statusFilter.Value);
            }

            if (dateFilter.HasValue)
            {
                var targetDate = dateFilter.Value.Date;
                // Lọc những booking có CheckInDate HOẶC CheckOutDate nằm trong ngày được chọn
                query = query.Where(b => b.CheckInDate.Date == targetDate || b.CheckOutDate.Date == targetDate);
            }

            // Sắp xếp: Giảm dần theo mã Booking (Booking mới tạo lên đầu)
            return await query
                .OrderByDescending(b => b.BookingId)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<PreCheckinViewModel?> PrepareCheckInViewModelAsync(int id)
        {
            var booking = await GetBookingByIdAsync(id);
            if (booking == null) return null;

            var guests = await _bookingGuestService.GetBookingGuestsForCheckInAsync(id);
            var guestList = guests.ToList();

            int maxCapacity = (booking.NumberOfAdults ?? 0) + (booking.NumberOfChildren ?? 0);
            while (guestList.Count < maxCapacity)
            {
                guestList.Add(new BookingGuestDto
                {
                    IsPrimaryBooker = guestList.Count == 0
                });
            }

            return new PreCheckinViewModel
            {
                BookingId = booking.BookingId,
                RoomNumber = booking.Room?.RoomNumber ?? string.Empty,
                CategoryName = booking.Room?.Category?.CategoryName ?? string.Empty,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                PdpdConsentChecked = booking.PdpdConsentChecked,
                Guests = guestList
            };
        }

        public async Task<(bool Success, string ErrorMessage)> ProcessCheckInAsync(PreCheckinViewModel model)
        {
            var booking = await _context.Bookings.FindAsync(model.BookingId);
            if (booking == null)
            {
                return (false, "Không tìm thấy thông tin đặt phòng.");
            }

            if (booking.BookingStatus != BookingStatus.Reserved && booking.BookingStatus != BookingStatus.PendingPayment)
            {
                return (false, "Đơn đặt phòng này không ở trạng thái có thể Check-in.");
            }

            var validGuests = model.Guests
                .Where(g => !string.IsNullOrWhiteSpace(g.FullName))
                .ToList();

            try
            {
                await _bookingGuestService.SaveBookingGuestsAsync(model.BookingId, validGuests, true);

                booking.BookingStatus = BookingStatus.CheckedIn;
                
                // Update RoomStatus to Occupied
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                if (room != null)
                {
                    room.Status = RoomStatus.Occupied;
                }

                await _context.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<(byte[]? FileContent, string FileName, string ErrorMessage)> GetManifestDataAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return (null, string.Empty, "Không tìm thấy thông tin đặt phòng.");
            }

            var guests = await _bookingGuestService.GetBookingGuestsForCheckInAsync(id);
            var guestList = guests.ToList();

            var csv = new System.Text.StringBuilder();
            csv.AppendLine("\uFEFF"); // BOM for Excel
            csv.AppendLine("STT,Họ và Tên,Ngày Sinh,Giới Tính,Số CCCD/Hộ Chiếu,Quốc Tịch,Vai Trò,Thời Gian Tạm Trú");

            int index = 1;
            foreach (var guest in guestList)
            {
                var dobStr = guest.DateOfBirth?.ToString("dd/MM/yyyy") ?? "";
                var genderStr = guest.Gender?.ToString() ?? "";
                var role = guest.IsPrimaryBooker ? "Người đặt chính" : "Khách đi cùng";
                var durationStr = $"{booking.CheckInDate:dd/MM/yyyy} - {booking.CheckOutDate:dd/MM/yyyy}";

                var nameEscaped = $"\"{guest.FullName.Replace("\"", "\"\"")}\"";
                var nationalityEscaped = $"\"{guest.Nationality.Replace("\"", "\"\"")}\"";

                csv.AppendLine($"{index},{nameEscaped},{dobStr},{genderStr},{guest.PassportNumber},{nationalityEscaped},{role},{durationStr}");
                index++;
            }

            var fileBytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            var fileName = $"BaoCaoTamTru_Booking_{booking.BookingId}.csv";
            
            return (fileBytes, fileName, string.Empty);
        }
    }
}
