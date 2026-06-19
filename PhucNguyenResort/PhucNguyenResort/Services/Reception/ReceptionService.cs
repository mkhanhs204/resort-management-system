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
                .Include(b => b.BookingDetails)
                .ThenInclude(d => d.RoomCategory)
                .Include(b => b.BookingDetails)
                .ThenInclude(d => d.Room)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(b => 
                    b.BookingId.ToString() == searchString ||
                    (b.User != null && b.User.FullName != null && b.User.FullName.Contains(searchString)) ||
                    (b.User != null && b.User.Email != null && b.User.Email.Contains(searchString)) ||
                    b.BookingDetails.Any(d => d.Room != null && d.Room.RoomNumber != null && d.Room.RoomNumber.Contains(searchString))
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
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.RoomCategory)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
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

            var groupedRooms = booking.BookingDetails
                .GroupBy(bd => bd.RoomCategory?.CategoryName)
                .Select(g => $"{g.Count()}x {g.Key}")
                .ToList();
            string categoryNameDisplay = string.Join(", ", groupedRooms);
            var assignedRooms = booking.BookingDetails.Where(bd => bd.Room != null).Select(bd => bd.Room.RoomNumber).ToList();
            string roomNumberDisplay = assignedRooms.Any() ? string.Join(", ", assignedRooms) : "Chưa xếp phòng";

            var roomAssignments = new List<BookingDetailAssignmentViewModel>();
            foreach (var detail in booking.BookingDetails)
            {
                var categoryName = detail.RoomCategory?.CategoryName ?? "Không rõ";
                var availableRooms = await GetAvailableRoomsForCategoryAsync(
                    detail.RoomCategoryId,
                    booking.CheckInDate,
                    booking.CheckOutDate,
                    booking.BookingId
                );

                roomAssignments.Add(new BookingDetailAssignmentViewModel
                {
                    BookingDetailId = detail.BookingDetailId,
                    RoomCategoryId = detail.RoomCategoryId,
                    CategoryName = categoryName,
                    SelectedRoomId = detail.RoomId,
                    AvailableRooms = availableRooms.Select(r => new RoomDto
                    {
                        Id = r.RoomId,
                        RoomNumber = r.RoomNumber ?? string.Empty
                    }).ToList()
                });
            }

            return new PreCheckinViewModel
            {
                BookingId = booking.BookingId,
                RoomNumber = roomNumberDisplay,
                CategoryName = categoryNameDisplay,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                PdpdConsentChecked = booking.PdpdConsentChecked,
                Guests = guestList,
                RoomAssignments = roomAssignments
            };
        }

        public async Task<(bool Success, string ErrorMessage)> ProcessCheckInAsync(PreCheckinViewModel model)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingDetails)
                .ThenInclude(d => d.Room)
                .FirstOrDefaultAsync(b => b.BookingId == model.BookingId);
            if (booking == null)
            {
                return (false, "Không tìm thấy thông tin đặt phòng.");
            }

            if (booking.BookingStatus != BookingStatus.Reserved && booking.BookingStatus != BookingStatus.PendingPayment)
            {
                return (false, "Đơn đặt phòng này không ở trạng thái có thể Check-in.");
            }

            if (model.RoomAssignments == null || model.RoomAssignments.Count == 0)
            {
                return (false, "Danh sách gán phòng trống.");
            }

            if (model.RoomAssignments.Count != booking.BookingDetails.Count)
            {
                return (false, "Số lượng phòng gán không khớp với thông tin đặt phòng.");
            }

            var selectedRoomIds = new List<int>();
            foreach (var assignment in model.RoomAssignments)
            {
                if (!assignment.SelectedRoomId.HasValue)
                {
                    return (false, "Vui lòng gán phòng vật lý cho tất cả các chi tiết đặt phòng.");
                }

                int roomId = assignment.SelectedRoomId.Value;

                if (selectedRoomIds.Contains(roomId))
                {
                    return (false, "Một phòng vật lý không thể được gán cho nhiều chi tiết đặt phòng cùng lúc.");
                }
                selectedRoomIds.Add(roomId);

                var room = await _context.Rooms.FindAsync(roomId);
                if (room == null)
                {
                    return (false, $"Không tìm thấy phòng với ID {roomId}.");
                }

                if (room.CategoryId != assignment.RoomCategoryId)
                {
                    return (false, $"Phòng {room.RoomNumber} không thuộc loại phòng yêu cầu.");
                }

                if (room.Status == RoomStatus.Maintenance)
                {
                    return (false, $"Phòng {room.RoomNumber} đang trong tình trạng bảo trì.");
                }

                var isOverlapping = await _context.BookingDetails
                    .AnyAsync(bd => bd.RoomId == roomId
                                 && bd.Booking != null
                                 && bd.Booking.BookingStatus != BookingStatus.Cancelled
                                 && bd.Booking.BookingStatus != BookingStatus.CheckedOut
                                 && bd.Booking.BookingStatus != BookingStatus.NoShow
                                 && bd.Booking.CheckInDate < booking.CheckOutDate
                                 && bd.Booking.CheckOutDate > booking.CheckInDate
                                 && bd.BookingId != booking.BookingId);
                if (isOverlapping)
                {
                    return (false, $"Phòng {room.RoomNumber} đã được gán hoặc đang được sử dụng bởi một đơn đặt phòng khác trong thời gian này.");
                }
            }

            var validGuests = model.Guests
                .Where(g => !string.IsNullOrWhiteSpace(g.FullName))
                .ToList();

            try
            {
                await _bookingGuestService.SaveBookingGuestsAsync(model.BookingId, validGuests, true);

                foreach (var assignment in model.RoomAssignments)
                {
                    var detail = booking.BookingDetails.FirstOrDefault(d => d.BookingDetailId == assignment.BookingDetailId);
                    if (detail != null)
                    {
                        detail.RoomId = assignment.SelectedRoomId;
                        var room = await _context.Rooms.FindAsync(assignment.SelectedRoomId.Value);
                        if (room != null)
                        {
                            room.Status = RoomStatus.Occupied;
                        }
                    }
                }

                booking.BookingStatus = BookingStatus.CheckedIn;

                await _context.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<List<Room>> GetAvailableRoomsForCategoryAsync(int categoryId, DateTime checkIn, DateTime checkOut, int? currentBookingId = null)
        {
            var bookedRoomIds = await _context.BookingDetails
                .Where(bd => bd.RoomCategoryId == categoryId
                          && bd.RoomId.HasValue
                          && bd.Booking != null
                          && bd.Booking.BookingStatus != BookingStatus.Cancelled
                          && bd.Booking.BookingStatus != BookingStatus.CheckedOut
                          && bd.Booking.BookingStatus != BookingStatus.NoShow
                          && bd.Booking.CheckInDate < checkOut
                          && bd.Booking.CheckOutDate > checkIn
                          && (!currentBookingId.HasValue || bd.BookingId != currentBookingId.Value))
                .Select(bd => bd.RoomId!.Value)
                .Distinct()
                .ToListAsync();

            return await _context.Rooms
                .Where(r => r.CategoryId == categoryId
                         && r.Status != RoomStatus.Maintenance
                         && !bookedRoomIds.Contains(r.RoomId))
                .ToListAsync();
        }

        public async Task<(byte[]? FileContent, string FileName, string ErrorMessage)> GetManifestDataAsync(int id)
        {
            var booking = await _context.Bookings
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
