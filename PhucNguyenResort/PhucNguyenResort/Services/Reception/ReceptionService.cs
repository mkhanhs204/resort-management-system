using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.ViewModels.Bookings;
using PhucNguyenResort.Helpers;
using PhucNguyenResort.Services.Email;
using PhucNguyenResort.Services.Notifications;
using Microsoft.AspNetCore.Hosting;

namespace PhucNguyenResort.Services.Reception
{
    public class ReceptionService : IReceptionService
    {
        private readonly ResortDbContext _context;
        private readonly IBookingGuestService _bookingGuestService;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly INotificationService _notificationService;

        public ReceptionService(ResortDbContext context, IBookingGuestService bookingGuestService, IEmailSender emailSender, IWebHostEnvironment env, INotificationService notificationService)
        {
            _context = context;
            _bookingGuestService = bookingGuestService;
            _emailSender = emailSender;
            _env = env;
            _notificationService = notificationService;
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
                query = query.Where(b => b.CheckInDate.Date == targetDate || b.CheckOutDate.Date == targetDate);
            }

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

                var availableRoomDtos = availableRooms.Select(r => new RoomDto
                {
                    Id = r.RoomId,
                    RoomNumber = r.RoomNumber ?? string.Empty
                }).ToList();

                if (detail.RoomId.HasValue && !availableRoomDtos.Any(r => r.Id == detail.RoomId.Value))
                {
                    var currentRoom = await _context.Rooms.FindAsync(detail.RoomId.Value);
                    if (currentRoom != null)
                    {
                        availableRoomDtos.Add(new RoomDto
                        {
                            Id = currentRoom.RoomId,
                            RoomNumber = currentRoom.RoomNumber ?? string.Empty
                        });
                    }
                }

                roomAssignments.Add(new BookingDetailAssignmentViewModel
                {
                    BookingDetailId = detail.BookingDetailId,
                    RoomCategoryId = detail.RoomCategoryId,
                    CategoryName = categoryName,
                    SelectedRoomId = detail.RoomId,
                    AvailableRooms = availableRoomDtos
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
                CurrentBookingStatus = booking.BookingStatus,
                Guests = guestList,
                RoomAssignments = roomAssignments
            };
        }

        public async Task<(bool Success, string ErrorMessage)> ProcessCheckInAsync(PreCheckinViewModel model)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.BookingDetails)
                .ThenInclude(d => d.Room)
                .FirstOrDefaultAsync(b => b.BookingId == model.BookingId);
            if (booking == null)
            {
                return (false, "Không tìm thấy thông tin đặt phòng.");
            }

            if (booking.BookingStatus != BookingStatus.Reserved && booking.BookingStatus != BookingStatus.PendingPayment && booking.BookingStatus != BookingStatus.Arrived)
            {
                return (false, "Đơn đặt phòng này không ở trạng thái có thể Check-in.");
            }

            if (!model.IsArrivalOnly)
            {
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

                if (room.Status == RoomStatus.Occupied)
                {
                    return (false, $"Phòng {room.RoomNumber} đang có khách lưu trú (chưa Check-out).");
                }

                if (room.Status == RoomStatus.NeedsCleaning || room.Status == RoomStatus.CleaningInProgress)
                {
                    return (false, $"Phòng {room.RoomNumber} đang cần dọn dẹp hoặc đang được dọn dẹp.");
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
            }

            var validGuests = model.Guests
                .Where(g => !string.IsNullOrWhiteSpace(g.FullName))
                .ToList();

            try
            {
                await _bookingGuestService.SaveBookingGuestsAsync(model.BookingId, validGuests, true);

                if (model.IsArrivalOnly)
                {
                    if (booking.ActualCheckInTime == null)
                    {
                        booking.ActualCheckInTime = DateTime.Now;
                    }
                    booking.BookingStatus = BookingStatus.Arrived;

                    var folio = await _context.GuestFolios.FirstOrDefaultAsync(f => f.BookingId == booking.BookingId);
                    if (folio == null)
                    {
                        folio = new GuestFolio
                        {
                            BookingId = booking.BookingId,
                            TotalRoomCharges = 0,
                            TotalFoodCharges = 0,
                            TotalTourCharges = 0,
                            TotalAmount = 0,
                            Status = FolioStatus.Unpaid
                        };
                        _context.GuestFolios.Add(folio);
                    }
                }
                else
                {
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

                    if (booking.ActualCheckInTime == null)
                    {
                        booking.ActualCheckInTime = DateTime.Now;
                    }
                    if (booking.RoomHandoverTime == null)
                    {
                        booking.RoomHandoverTime = DateTime.Now;
                    }
                    booking.BookingStatus = BookingStatus.CheckedIn;

                    var baseFolio = await _context.GuestFolios.FirstOrDefaultAsync(f => f.BookingId == booking.BookingId);
                    if (baseFolio == null)
                    {
                        baseFolio = new GuestFolio
                        {
                            BookingId = booking.BookingId,
                            TotalRoomCharges = 0,
                            TotalFoodCharges = 0,
                            TotalTourCharges = 0,
                            TotalAmount = 0,
                            Status = FolioStatus.Unpaid
                        };
                        _context.GuestFolios.Add(baseFolio);
                        await _context.SaveChangesAsync();
                    }

                    if (booking.RoomHandoverTime.Value.Date == booking.CheckInDate.Date)
                    {
                        var handoverTime = booking.RoomHandoverTime.Value.TimeOfDay;
                        if (handoverTime < TimeSpan.FromHours(14))
                        {
                            decimal roomPrice = booking.BookingDetails.Sum(bd => bd.Price ?? 0);
                            decimal surcharge = 0;

                            if (handoverTime < TimeSpan.FromHours(6))
                            {
                                surcharge = roomPrice * 1.0m; // Trước 6h: 100%
                            }
                            else if (handoverTime < TimeSpan.FromHours(9))
                            {
                                surcharge = roomPrice * 0.5m; // 6h - 9h: 50%
                            }
                            else if (handoverTime < TimeSpan.FromHours(11))
                            {
                                surcharge = roomPrice * 0.3m; // 9h - 11h: 30%
                            }
                            else
                            {
                                surcharge = 0; // 11h - 14h: Miễn phí
                            }

                            if (surcharge > 0)
                            {
                                var folio = await _context.GuestFolios.FirstOrDefaultAsync(f => f.BookingId == booking.BookingId);
                                if (folio == null)
                                {
                                    folio = new GuestFolio
                                    {
                                        BookingId = booking.BookingId,
                                        TotalRoomCharges = 0,
                                        TotalFoodCharges = 0,
                                        TotalTourCharges = 0,
                                        TotalAmount = 0,
                                        Status = FolioStatus.Unpaid
                                    };
                                    _context.GuestFolios.Add(folio);
                                    await _context.SaveChangesAsync();
                                }

                                folio.TotalRoomCharges += surcharge;
                                folio.TotalAmount += surcharge;

                                var transaction = new FolioTransaction
                                {
                                    FolioId = folio.FolioId,
                                    TransactionType = FolioTransactionType.RoomCharge,
                                    Amount = surcharge,
                                    CreatedAt = DateTime.Now
                                };
                                _context.FolioTransactions.Add(transaction);
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();

                if (!model.IsArrivalOnly)
                {
                    SendCheckInEmail(booking);
                }

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

            var query = _context.Rooms
                .Where(r => r.CategoryId == categoryId
                         && r.Status != RoomStatus.Maintenance
                         && !bookedRoomIds.Contains(r.RoomId));

            if (checkIn.Date <= DateTime.Today)
            {
                query = query.Where(r => r.Status == RoomStatus.Available);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Room>> GetAvailableRoomsForDatesAsync(DateTime checkIn, DateTime checkOut)
        {
            var bookedRoomIds = await _context.BookingDetails
                .Where(bd => bd.RoomId.HasValue
                          && bd.Booking != null
                          && bd.Booking.BookingStatus != BookingStatus.Cancelled
                          && bd.Booking.BookingStatus != BookingStatus.CheckedOut
                          && bd.Booking.BookingStatus != BookingStatus.NoShow
                          && bd.Booking.CheckInDate < checkOut
                          && bd.Booking.CheckOutDate > checkIn)
                .Select(bd => bd.RoomId!.Value)
                .Distinct()
                .ToListAsync();

            var query = _context.Rooms
                .Include(r => r.Category)
                .Where(r => r.Status != RoomStatus.Maintenance
                         && !bookedRoomIds.Contains(r.RoomId));

            if (checkIn.Date <= DateTime.Today)
            {
                query = query.Where(r => r.Status == RoomStatus.Available);
            }

            return await query.ToListAsync();
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
            csv.AppendLine("\uFEFF");
            csv.AppendLine("STT,Họ và Tên,Ngày Sinh,Giới Tính,Số CCCD/Hộ Chiếu,Quốc Tịch,Vai Trò,Thời Gian Tạm Trú");

            int index = 1;
            foreach (var guest in guestList)
            {
                var dobStr = guest.DateOfBirth?.ToString("dd/MM/yyyy") ?? "";
                var genderStr = guest.Gender?.ToString() ?? "";
                var role = guest.IsPrimaryBooker ? "Người đặt chính" : "Khách đi cùng";
                var durationStr = $"{booking.CheckInDate:dd/MM/yyyy} - {booking.CheckOutDate:dd/MM/yyyy}";

                var nameEscaped = $"\"{guest.FullName?.Replace("\"", "\"\"")}\"";
                var nationalityEscaped = $"\"{guest.Nationality?.Replace("\"", "\"\"")}\"";

                csv.AppendLine($"{index},{nameEscaped},{dobStr},{genderStr},{guest.PassportNumber},{nationalityEscaped},{role},{durationStr}");
                index++;
            }

            var fileBytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            var fileName = $"BaoCaoTamTru_Booking_{booking.BookingId}.csv";

            return (fileBytes, fileName, string.Empty);
        }

        public async Task<WalkInResult> ProcessWalkInBookingAsync(WalkInBookingViewModel model)
        {
            // 0.1. Kiểm tra ngày nhận phòng không được ở quá khứ
            if (model.CheckInDate.Date < DateTime.Today)
            {
                return new WalkInResult
                {
                    Success = false,
                    ErrorCode = "WKB-007",
                    ErrorMessage = "Ngày nhận phòng không được ở quá khứ."
                };
            }

            // 0.2. Kiểm tra ngày trả phòng phải sau ngày nhận phòng
            if (model.CheckOutDate.Date <= model.CheckInDate.Date)
            {
                return new WalkInResult
                {
                    Success = false,
                    ErrorCode = "WKB-008",
                    ErrorMessage = "Ngày trả phòng phải sau ngày nhận phòng."
                };
            }

            // 0. Kiểm tra chọn ít nhất một phòng vật lý
            if (model.SelectedRoomIds == null || !model.SelectedRoomIds.Any())
            {
                return new WalkInResult
                {
                    Success = false,
                    ErrorCode = "WKB-006",
                    ErrorMessage = "Vui lòng chọn ít nhất một phòng vật lý."
                };
            }

            // 1. Kiểm tra số điện thoại hợp lệ
            if (string.IsNullOrWhiteSpace(model.PhoneNumber) || !model.PhoneNumber.All(char.IsDigit) || model.PhoneNumber.Length < 9)
            {
                return new WalkInResult
                {
                    Success = false,
                    ErrorCode = "WKB-001",
                    ErrorMessage = "Số điện thoại không hợp lệ."
                };
            }

            // 2. Kiểm tra trùng phòng vật lý (Double booking)
            foreach (var roomId in model.SelectedRoomIds)
            {
                var isOverlapping = await _context.BookingDetails
                    .AnyAsync(bd => bd.RoomId == roomId
                                 && bd.Booking != null
                                 && bd.Booking.BookingStatus != BookingStatus.Cancelled
                                 && bd.Booking.BookingStatus != BookingStatus.CheckedOut
                                 && bd.Booking.BookingStatus != BookingStatus.NoShow
                                 && bd.Booking.CheckInDate < model.CheckOutDate
                                 && bd.Booking.CheckOutDate > model.CheckInDate);
                if (isOverlapping)
                {
                    return new WalkInResult
                    {
                        Success = false,
                        ErrorCode = "WKB-002",
                        ErrorMessage = "Phòng vật lý đã được đặt hoặc bận trong khoảng thời gian này."
                    };
                }
            }

            // 3. Kiểm tra thông tin định danh khi nhận phòng ngay
            if (model.InstantCheckIn)
            {
                if (model.Guests == null || !model.Guests.Any())
                {
                    return new WalkInResult
                    {
                        Success = false,
                        ErrorCode = "WKB-003",
                        ErrorMessage = "Vui lòng điền thông tin khai báo tạm trú khi nhận phòng ngay."
                    };
                }

                for (int i = 0; i < model.Guests.Count; i++)
                {
                    var guest = model.Guests[i];
                    if (guest.IsPrimaryBooker)
                    {
                        if (string.IsNullOrWhiteSpace(guest.FullName) ||
                            guest.DateOfBirth == null ||
                            guest.Gender == null ||
                            string.IsNullOrWhiteSpace(guest.PassportNumber) ||
                            string.IsNullOrWhiteSpace(guest.Nationality))
                        {
                            return new WalkInResult
                            {
                                Success = false,
                                ErrorCode = "WKB-003",
                                ErrorMessage = "Vui lòng nhập đầy đủ thông tin (Họ tên, Ngày sinh, Giới tính, CCCD/Passport, Quốc tịch) của người đặt chính khi nhận phòng ngay."
                            };
                        }
                    }
                    else
                    {
                        bool hasAnyField = !string.IsNullOrWhiteSpace(guest.FullName) 
                                           || !string.IsNullOrWhiteSpace(guest.PassportNumber) 
                                           || guest.DateOfBirth != null;
                        if (hasAnyField)
                        {
                            if (string.IsNullOrWhiteSpace(guest.FullName) ||
                                guest.DateOfBirth == null ||
                                guest.Gender == null ||
                                string.IsNullOrWhiteSpace(guest.Nationality))
                            {
                                return new WalkInResult
                                {
                                    Success = false,
                                    ErrorCode = "WKB-003",
                                    ErrorMessage = $"Vui lòng nhập đầy đủ thông tin (Họ tên, Ngày sinh, Giới tính, Quốc tịch) cho khách đi cùng thứ {i}."
                                };
                            }
                        }
                    }
                }
            }

            // 4. Tìm kiếm hoặc tự động tạo tài khoản Customer mới
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
            if (user == null)
            {
                var email = model.Email;
                if (string.IsNullOrWhiteSpace(email))
                {
                    email = $"{model.PhoneNumber}@phucnguyenresort.temp";
                }

                var baseEmail = email;
                int counter = 1;
                while (await _context.Users.AnyAsync(u => u.Email == email))
                {
                    var parts = baseEmail.Split('@');
                    if (parts.Length == 2)
                    {
                        email = $"{parts[0]}_{counter}@{parts[1]}";
                    }
                    else
                    {
                        email = $"{baseEmail}_{counter}";
                    }
                    counter++;
                }

                user = new User
                {
                    FullName = model.CustomerName,
                    PhoneNumber = model.PhoneNumber,
                    Email = email,
                    RoleId = 3, // Customer
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                user.PasswordHash = PasswordHelper.HashPassword(user, "123456");

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // 5. Lấy danh sách phòng vật lý được chọn
            var rooms = await _context.Rooms
                .Include(r => r.Category)
                .Where(r => model.SelectedRoomIds.Contains(r.RoomId))
                .ToListAsync();

            if (rooms.Count != model.SelectedRoomIds.Count)
            {
                return new WalkInResult
                {
                    Success = false,
                    ErrorCode = "WKB-004",
                    ErrorMessage = "Một hoặc nhiều phòng vật lý được chọn không tồn tại."
                };
            }

            if (model.InstantCheckIn)
            {
                foreach (var room in rooms)
                {
                    if (room.Status != RoomStatus.Available)
                    {
                        return new WalkInResult
                        {
                            Success = false,
                            ErrorCode = "WKB-009",
                            ErrorMessage = $"Phòng {room.RoomNumber} không ở trạng thái sẵn sàng (Available) để Check-in ngay."
                        };
                    }
                }
            }

            // 6. Tính toán giá trị tiền đặt phòng
            var nights = (model.CheckOutDate - model.CheckInDate).Days;
            if (nights <= 0) nights = 1;

            decimal totalAmount = 0;
            foreach (var room in rooms)
            {
                var pricePerNight = room.Category?.PricePerNight ?? 0;
                totalAmount += pricePerNight * nights;
            }

            var minDeposit = totalAmount * 0.4m;
            if (model.PaidAmount < minDeposit)
            {
                return new WalkInResult
                {
                    Success = false,
                    ErrorCode = "WKB-005",
                    ErrorMessage = $"Số tiền cọc thực nhận phải đạt tối thiểu 40% tổng tiền phòng ({minDeposit:N0} VNĐ)."
                };
            }

            // 7. Tạo bản ghi Booking
            var booking = new Booking
            {
                UserId = user.UserId,
                User = user,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                NumberOfAdults = model.NumberOfAdults,
                NumberOfChildren = model.NumberOfChildren,
                BookingStatus = model.InstantCheckIn ? BookingStatus.CheckedIn : BookingStatus.Reserved,
                ActualCheckInTime = model.InstantCheckIn ? DateTime.Now : (DateTime?)null,
                TotalAmount = totalAmount,
                DepositAmount = model.PaidAmount,
                CreatedAt = DateTime.Now,
                PdpdConsentChecked = model.PdpdConsentChecked
            };

            foreach (var room in rooms)
            {
                var detail = new BookingDetail
                {
                    RoomCategoryId = room.CategoryId,
                    RoomId = room.RoomId,
                    Room = room,
                    Price = room.Category?.PricePerNight
                };
                booking.BookingDetails.Add(detail);

                if (model.InstantCheckIn)
                {
                    room.Status = RoomStatus.Occupied;
                }
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            if (model.InstantCheckIn)
            {
                var folio = new GuestFolio
                {
                    BookingId = booking.BookingId,
                    TotalRoomCharges = 0,
                    TotalFoodCharges = 0,
                    TotalTourCharges = 0,
                    TotalAmount = 0,
                    Status = FolioStatus.Unpaid
                };
                _context.GuestFolios.Add(folio);
                await _context.SaveChangesAsync();
            }

            // 8. Lưu thông tin khách hàng đi cùng
            if (model.InstantCheckIn && model.Guests != null && model.Guests.Any())
            {
                await _bookingGuestService.SaveBookingGuestsAsync(booking.BookingId, model.Guests, true);
            }

            if (model.InstantCheckIn)
            {
                SendCheckInEmail(booking);
            }

            return new WalkInResult
            {
                Success = true,
                BookingId = booking.BookingId
            };
        }
        protected virtual DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public async Task<CheckOutViewModel?> PrepareCheckOutViewModelAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.BookingDetails)
                .ThenInclude(bd => bd.Room)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null || booking.BookingStatus != BookingStatus.CheckedIn)
            {
                return null;
            }

            var folio = await _context.GuestFolios
                .Include(f => f.FolioTransactions)
                .FirstOrDefaultAsync(f => f.BookingId == bookingId);

            if (folio == null)
            {
                folio = new GuestFolio
                {
                    BookingId = bookingId,
                    TotalRoomCharges = 0,
                    TotalFoodCharges = 0,
                    TotalTourCharges = 0,
                    TotalAmount = 0,
                    Status = FolioStatus.Unpaid
                };
                _context.GuestFolios.Add(folio);
                await _context.SaveChangesAsync();
                
                folio.FolioTransactions = new List<FolioTransaction>();
            }

            // Compensate for existing folios that missed the base room charge during check-in
            decimal baseRoomCharge = booking.TotalAmount ?? 0;
            bool hasBaseCharge = folio.FolioTransactions.Any(t => t.TransactionType == FolioTransactionType.RoomCharge && t.Amount == baseRoomCharge) || folio.TotalRoomCharges >= baseRoomCharge;
            
            decimal displayTotalRoomCharges = folio.TotalRoomCharges;
            decimal displayTotalAmount = folio.TotalAmount;
            
            if (!hasBaseCharge && baseRoomCharge > 0)
            {
                displayTotalRoomCharges += baseRoomCharge;
                displayTotalAmount += baseRoomCharge;
            }

            var model = new CheckOutViewModel
            {
                BookingId = booking.BookingId,
                GuestName = booking.User?.FullName ?? "N/A",
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                ActualCheckInTime = booking.ActualCheckInTime,
                FolioId = folio.FolioId,
                DepositAmount = booking.DepositAmount ?? 0,
                TotalRoomCharges = displayTotalRoomCharges,
                TotalFoodCharges = folio.TotalFoodCharges,
                TotalTourCharges = folio.TotalTourCharges,
                TotalAmount = displayTotalAmount
            };

            var rooms = booking.BookingDetails
                .Where(bd => bd.Room != null)
                .Select(bd => bd.Room!.RoomNumber)
                .ToList();
            model.RoomNumbers = string.Join(", ", rooms);

            // Calculate Late Check-out Surcharge
            var now = GetCurrentDateTime();
            var checkoutTime = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0); // 12:00 PM standard
            if (now.Date == booking.CheckOutDate.Date && now > checkoutTime)
            {
                decimal surchargeRatio = 0;
                var hour = now.Hour;
                
                if (hour >= 12 && hour < 15) surchargeRatio = 0.3m;
                else if (hour >= 15 && hour < 18) surchargeRatio = 0.5m;
                else if (hour >= 18) surchargeRatio = 1.0m;

                if (surchargeRatio > 0)
                {
                    decimal oneNightPrice = 0;
                    foreach (var bd in booking.BookingDetails.Where(bd => bd.Room != null))
                    {
                        var roomPrice = await _context.Rooms
                            .Include(r => r.Category)
                            .Where(r => r.RoomId == bd.RoomId)
                            .Select(r => r.Category.PricePerNight)
                            .FirstOrDefaultAsync();
                        oneNightPrice += roomPrice ?? 0;
                    }
                    
                    model.LateCheckOutSurcharge = oneNightPrice * surchargeRatio;
                    model.TotalAmount += model.LateCheckOutSurcharge;
                }
            }

            model.BalanceDue = Math.Max(0, model.TotalAmount - model.DepositAmount);

            var foodOrderTypesByReferenceId = await GetFoodOrderTypesByReferenceIdAsync(folio.FolioTransactions);

            // Mapping transactions
            var transactions = folio.FolioTransactions
                .OrderBy(t => t.CreatedAt)
                .Select(t => new FolioTransactionDto
            {
                TransactionId = t.TransactionId,
                TransactionTypeName = ResolveTransactionTypeName(t, booking, foodOrderTypesByReferenceId),
                Amount = t.Amount,
                CreatedAt = t.CreatedAt
            }).ToList();

            if (!hasBaseCharge && baseRoomCharge > 0)
            {
                transactions.Insert(0, new FolioTransactionDto
                {
                    TransactionId = 0,
                    TransactionTypeName = "Tiền phòng đặt trước",
                    Amount = baseRoomCharge,
                    CreatedAt = DateTime.Now
                });
            }
            model.Transactions = transactions;

            return model;
        }

        public async Task<(bool Success, string ErrorMessage)> ProcessCheckOutAsync(CheckOutViewModel model)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.BookingDetails)
                .ThenInclude(bd => bd.Room)
                .FirstOrDefaultAsync(b => b.BookingId == model.BookingId);

            if (booking == null)
            {
                return (false, "Không tìm thấy thông tin đặt phòng.");
            }

            if (booking.BookingStatus != BookingStatus.CheckedIn)
            {
                return (false, "Đơn đặt phòng này không ở trạng thái Check-in.");
            }

            var folio = await _context.GuestFolios
                .Include(f => f.FolioTransactions)
                .FirstOrDefaultAsync(f => f.FolioId == model.FolioId);

            if (folio == null)
            {
                return (false, "Không tìm thấy Guest Folio.");
            }

            if (model.BalanceDue > 0 && model.SelectedPaymentMethod == null)
            {
                return (false, "Vui lòng chọn phương thức thanh toán.");
            }

            var now = GetCurrentDateTime();

            // Add late surcharge transaction if applicable
            if (model.LateCheckOutSurcharge > 0)
            {
                var txn = new FolioTransaction
                {
                    FolioId = folio.FolioId,
                    TransactionType = FolioTransactionType.RoomCharge,
                    Amount = model.LateCheckOutSurcharge,
                    CreatedAt = now
                };
                _context.FolioTransactions.Add(txn);
                folio.TotalRoomCharges += model.LateCheckOutSurcharge;
                folio.TotalAmount += model.LateCheckOutSurcharge;
            }

            // Compensate for existing folios that missed the base room charge
            decimal baseRoomCharge = booking.TotalAmount ?? 0;
            bool hasBaseCharge = folio.FolioTransactions.Any(t => t.TransactionType == FolioTransactionType.RoomCharge && t.Amount == baseRoomCharge) || folio.TotalRoomCharges >= baseRoomCharge;
            if (!hasBaseCharge && baseRoomCharge > 0)
            {
                folio.TotalRoomCharges += baseRoomCharge;
                folio.TotalAmount += baseRoomCharge;
                _context.FolioTransactions.Add(new FolioTransaction
                {
                    FolioId = folio.FolioId,
                    TransactionType = FolioTransactionType.RoomCharge,
                    Amount = baseRoomCharge,
                    CreatedAt = now
                });
            }

            // Update Folio
            folio.Status = FolioStatus.Paid;

            // Create Invoice
            var invoice = new Invoice
            {
                FolioId = folio.FolioId,
                InvoiceNumber = $"INV-{now:yyyyMMdd}-{folio.FolioId}",
                TotalAmount = model.TotalAmount,
                FinalAmount = model.TotalAmount,
                InvoiceStatus = InvoiceStatus.Paid,
                IssuedAt = now
            };
            _context.Invoices.Add(invoice);

            // Create Payment if there is balance due
            if (model.BalanceDue > 0)
            {
                var payment = new Payment
                {
                    InvoiceId = invoice.InvoiceId,
                    Invoice = invoice,
                    Amount = model.BalanceDue,
                    PaidAt = now,
                    PaymentMethod = model.SelectedPaymentMethod.Value,
                    PaymentStatus = PaymentStatus.Success
                };
                _context.Payments.Add(payment);
            }

            // Update Booking Status and Actual Check-out time
            booking.BookingStatus = BookingStatus.CheckedOut;
            booking.ActualCheckOutTime = now;

            // Update Room Status
            foreach (var detail in booking.BookingDetails.Where(bd => bd.RoomId.HasValue))
            {
                var room = await _context.Rooms.FindAsync(detail.RoomId.Value);
                if (room != null)
                {
                    room.Status = RoomStatus.NeedsCleaning;
                }
            }

            await _context.SaveChangesAsync();

            // Gửi email thông báo Check-out
            SendCheckOutEmail(booking, invoice);

            return (true, string.Empty);
        }

        private void SendCheckInEmail(Booking booking)
        {
            if (_emailSender == null || _env == null || booking.User == null || string.IsNullOrEmpty(booking.User.Email))
            {
                return;
            }

            try
            {
                var templatePath = System.IO.Path.Combine(_env.ContentRootPath, "EmailTemplates", "CheckInSuccess.html");
                if (System.IO.File.Exists(templatePath))
                {
                    var htmlBody = System.IO.File.ReadAllText(templatePath);
                    var roomName = booking.BookingDetails != null && booking.BookingDetails.Any(bd => bd.Room != null) 
                        ? string.Join(", ", booking.BookingDetails.Where(bd => bd.Room != null).Select(bd => bd.Room!.RoomNumber)) 
                        : "Chưa xếp phòng";
                    var guestCount = (booking.NumberOfAdults ?? 0) + (booking.NumberOfChildren ?? 0);

                    htmlBody = htmlBody.Replace("{{CustomerName}}", booking.User.FullName ?? "Quý khách")
                                       .Replace("{{BookingId}}", booking.BookingId.ToString())
                                       .Replace("{{RoomName}}", roomName)
                                       .Replace("{{ActualCheckInTime}}", (booking.ActualCheckInTime ?? DateTime.Now).ToString("dd/MM/yyyy HH:mm"))
                                       .Replace("{{CheckOutDate}}", booking.CheckOutDate.ToString("dd/MM/yyyy"))
                                       .Replace("{{GuestCount}}", guestCount.ToString());

                    // Fire and forget
                    _ = _emailSender.SendEmailAsync(booking.User.Email, "Thông báo Nhận phòng (Check-in) Thành công - PhucNguyen Resort", htmlBody);

                    // Send In-App Notification for Check-in
                    _ = _notificationService.CreateAsync(
                        booking.UserId,
                        "Check-in thành công 🔑",
                        $"Bạn đã nhận phòng thành công cho đơn đặt phòng #{booking.BookingId} (Phòng: {roomName}). Chúc bạn có một kỳ nghỉ thật tuyệt vời tại PhucNguyen Resort!"
                    );
                }
            }
            catch
            {
                // Silence exceptions to avoid failing check-in due to mail errors
            }
        }

        public async Task<InvoiceViewModel?> PrepareInvoiceViewModelAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.BookingDetails)
                .ThenInclude(bd => bd.Room)
                .Include(b => b.GuestFolio)
                .ThenInclude(f => f!.FolioTransactions)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null || booking.GuestFolio == null) return null;

            var folio = booking.GuestFolio;
            var invoice = await _context.Invoices
                .Include(i => i.Payments)
                .OrderByDescending(i => i.IssuedAt)
                .FirstOrDefaultAsync(i => i.FolioId == folio.FolioId);

            if (invoice == null) return null;

            var rooms = booking.BookingDetails
                .Where(bd => bd.Room != null)
                .Select(bd => bd.Room!.RoomNumber)
                .ToList();
            
            var roomNumbers = string.Join(", ", rooms);
            
            var paymentMethod = invoice.Payments.Any() 
                ? invoice.Payments.First().PaymentMethod.ToString() 
                : "Tiền mặt"; // Default fallback

            var model = new InvoiceViewModel
            {
                BookingId = booking.BookingId,
                InvoiceNumber = invoice.InvoiceNumber ?? $"INV-{booking.BookingId}",
                IssuedAt = invoice.IssuedAt,
                GuestName = booking.User?.FullName ?? "Quý khách",
                GuestPhone = booking.User?.PhoneNumber ?? "",
                RoomNumbers = roomNumbers,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                ActualCheckInTime = booking.ActualCheckInTime,
                ActualCheckOutTime = booking.ActualCheckOutTime,
                TotalRoomCharges = folio.TotalRoomCharges,
                TotalFoodCharges = folio.TotalFoodCharges,
                TotalTourCharges = folio.TotalTourCharges,
                TotalAmount = invoice.TotalAmount ?? 0,
                DepositAmount = booking.DepositAmount ?? 0,
                AmountPaid = invoice.Payments.Sum(p => p.Amount ?? 0),
                PaymentMethod = paymentMethod
            };

            // Calculate Late Surcharge if it exists in transactions
            var lateTxn = folio.FolioTransactions.FirstOrDefault(t => t.TransactionType == FolioTransactionType.RoomCharge && t.Amount > 0 && t.CreatedAt >= booking.CheckOutDate.AddHours(12));
            if (lateTxn != null)
            {
                // Simple heuristic for late check-out surcharge in folio
                // Actual system might have a specific type, but this works
            }
            // Actually, we can just use the transactions list for details.
            
            var foodOrderTypesByReferenceId = await GetFoodOrderTypesByReferenceIdAsync(folio.FolioTransactions);

            model.Transactions = folio.FolioTransactions
                .OrderBy(t => t.CreatedAt)
                .Select(t => new FolioTransactionDto
            {
                TransactionId = t.TransactionId,
                TransactionTypeName = ResolveTransactionTypeName(t, booking, foodOrderTypesByReferenceId),
                Amount = t.Amount,
                CreatedAt = t.CreatedAt
            }).ToList();
            
            decimal baseRoomCharge = booking.TotalAmount ?? 0;
            bool hasBaseCharge = folio.FolioTransactions.Any(t => t.TransactionType == FolioTransactionType.RoomCharge && t.Amount == baseRoomCharge);
            if (!hasBaseCharge && baseRoomCharge > 0)
            {
                model.Transactions.Insert(0, new FolioTransactionDto
                {
                    TransactionId = 0,
                    TransactionTypeName = "Tiền phòng đặt trước",
                    Amount = baseRoomCharge,
                    CreatedAt = DateTime.Now
                });
            }

            return model;
        }

        private async Task<Dictionary<int, FoodOrderType>> GetFoodOrderTypesByReferenceIdAsync(
            IEnumerable<FolioTransaction> transactions)
        {
            var foodOrderIds = transactions
                .Where(t => t.TransactionType == FolioTransactionType.FoodCharge && t.ReferenceId.HasValue)
                .Select(t => t.ReferenceId!.Value)
                .Distinct()
                .ToList();

            if (foodOrderIds.Count == 0)
            {
                return new Dictionary<int, FoodOrderType>();
            }

            return await _context.FoodOrders
                .AsNoTracking()
                .Where(order => foodOrderIds.Contains(order.FoodOrderId))
                .ToDictionaryAsync(order => order.FoodOrderId, order => order.OrderType);
        }

        private static string? ResolveTransactionTypeName(
            FolioTransaction transaction,
            Booking booking,
            IReadOnlyDictionary<int, FoodOrderType> foodOrderTypesByReferenceId)
        {
            if (transaction.TransactionType == FolioTransactionType.FoodCharge &&
                transaction.ReferenceId.HasValue &&
                foodOrderTypesByReferenceId.TryGetValue(transaction.ReferenceId.Value, out var orderType))
            {
                return orderType == FoodOrderType.TableService
                    ? "Tiền ăn tại bàn"
                    : "Tiền gọi món lên phòng";
            }

            if (transaction.TransactionType == FolioTransactionType.RoomCharge)
            {
                if (transaction.Amount == booking.TotalAmount)
                    return "Tiền phòng đặt trước";
                if (transaction.CreatedAt.Date == booking.CheckInDate.Date)
                    return "Phụ thu nhận phòng sớm";
                if (transaction.CreatedAt.Date >= booking.CheckOutDate.Date)
                    return "Phụ thu trả phòng muộn";
                return "Phụ thu tiền phòng";
            }

            return transaction.TransactionType switch
            {
                FolioTransactionType.FoodCharge => "Tiền ăn uống",
                FolioTransactionType.TourCharge => "Tiền tour",
                FolioTransactionType.Adjustment => "Điều chỉnh",
                FolioTransactionType.Payment => "Thanh toán",
                FolioTransactionType.Refund => "Hoàn tiền",
                _ => transaction.TransactionType.ToString()
            };
        }

        private void SendCheckOutEmail(Booking booking, Invoice invoice)
        {
            if (_emailSender == null || _env == null || booking.User == null || string.IsNullOrEmpty(booking.User.Email))
            {
                return;
            }

            try
            {
                var templatePath = System.IO.Path.Combine(_env.ContentRootPath, "EmailTemplates", "CheckOutSuccess.html");
                if (System.IO.File.Exists(templatePath))
                {
                    var htmlBody = System.IO.File.ReadAllText(templatePath);
                    var roomName = booking.BookingDetails != null && booking.BookingDetails.Any(bd => bd.Room != null) 
                        ? string.Join(", ", booking.BookingDetails.Where(bd => bd.Room != null).Select(bd => bd.Room!.RoomNumber)) 
                        : "Chưa xếp phòng";

                    htmlBody = htmlBody.Replace("{{CustomerName}}", booking.User.FullName ?? "Quý khách")
                                       .Replace("{{BookingId}}", booking.BookingId.ToString())
                                       .Replace("{{RoomName}}", roomName)
                                       .Replace("{{ActualCheckOutTime}}", (booking.ActualCheckOutTime ?? DateTime.Now).ToString("dd/MM/yyyy HH:mm"))
                                       .Replace("{{InvoiceNumber}}", invoice.InvoiceNumber ?? "")
                                       .Replace("{{TotalAmount}}", (invoice.TotalAmount ?? 0).ToString("N0"))
                                       .Replace("{{FinalAmount}}", (invoice.FinalAmount ?? 0).ToString("N0"));

                    _ = _emailSender.SendEmailAsync(booking.User.Email, "Cảm ơn Quý khách - Thông báo Check-out thành công từ PhucNguyen Resort", htmlBody);

                    // Send In-App Notification for Check-out
                    _ = _notificationService.CreateAsync(
                        booking.UserId,
                        "Check-out thành công 🏨",
                        $"Bạn đã trả phòng thành công cho đơn đặt phòng #{booking.BookingId} (Phòng: {roomName}). Cảm ơn bạn đã lựa chọn PhucNguyen Resort, hy vọng sớm được đón tiếp bạn trở lại!"
                    );
                }
            }
            catch
            {
                // Silence exceptions
            }
        }
    }
}