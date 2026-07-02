using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Tours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    /// <summary>
    /// Customer-facing tour booking service.
    /// Uses Pessimistic Locking (UPDLOCK, ROWLOCK) to prevent overbooking (ADR-002).
    /// </summary>
    public class CustomerTourBookingService : ICustomerTourBookingService
    {
        private readonly ResortDbContext _context;

        public CustomerTourBookingService(ResortDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<TourListVM> GetTourListAsync(string? searchKeyword, string? sortBy, int page = 1, int pageSize = 9)
        {
            var query = _context.Tours
                .Include(t => t.TourSchedules)
                .Include(t => t.Reviews)
                .AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                var keyword = searchKeyword.Trim().ToLower();
                query = query.Where(t =>
                    (t.TourName != null && t.TourName.ToLower().Contains(keyword)) ||
                    (t.Location != null && t.Location.ToLower().Contains(keyword)) ||
                    (t.Description != null && t.Description.ToLower().Contains(keyword)));
            }

            var tours = await query.ToListAsync();

            var tourCards = tours.Select(t => new TourCardVM
            {
                TourId = t.TourId,
                TourName = t.TourName ?? "Tour",
                Description = t.Description,
                Price = t.Price ?? 0,
                MaxCapacity = t.MaxCapacity ?? 0,
                TotalAvailableSlots = t.TourSchedules
                    .Where(s => s.TourDate != null && s.TourDate >= DateTime.Today && (s.AvailableSlots ?? 0) > 0)
                    .Sum(s => s.AvailableSlots ?? 0),
                DurationHours = t.DurationHours ?? 0,
                Location = t.Location,
                ImageUrl = t.ImageUrl,
                UpcomingScheduleCount = t.TourSchedules
                    .Count(s => s.TourDate != null && s.TourDate >= DateTime.Today && (s.AvailableSlots ?? 0) > 0),
                AverageRating = t.Reviews.Any() ? t.Reviews.Average(r => r.Rating ?? 0) : 0,
                ReviewCount = t.Reviews.Count
            })
            .Where(t => t.UpcomingScheduleCount > 0)
            .ToList();

            // Sorting
            tourCards = sortBy switch
            {
                "rating_desc" => tourCards.OrderByDescending(t => t.AverageRating).ToList(),
                "rating_asc" => tourCards.OrderBy(t => t.AverageRating).ToList(),
                "schedules_desc" => tourCards.OrderByDescending(t => t.UpcomingScheduleCount).ToList(),
                "price_asc" => tourCards.OrderBy(t => t.Price).ToList(),
                "price_desc" => tourCards.OrderByDescending(t => t.Price).ToList(),
                "name" => tourCards.OrderBy(t => t.TourName).ToList(),
                "duration" => tourCards.OrderBy(t => t.DurationHours).ToList(),
                _ => tourCards.OrderBy(t => t.TourName).ToList()
            };

            int totalCount = tourCards.Count;
            var pagedTours = tourCards.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new TourListVM
            {
                Tours = pagedTours,
                SearchKeyword = searchKeyword,
                SortBy = sortBy,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        /// <inheritdoc/>
        public async Task<TourDetailVM?> GetTourDetailAsync(int tourId)
        {
            var tour = await _context.Tours
                .Include(t => t.TourSchedules)
                    .ThenInclude(s => s.Guide)
                .Include(t => t.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(t => t.TourId == tourId);

            if (tour == null) return null;

            return new TourDetailVM
            {
                TourId = tour.TourId,
                TourName = tour.TourName ?? "Tour",
                Description = tour.Description,
                Price = tour.Price ?? 0,
                MaxCapacity = tour.MaxCapacity ?? 0,
                DurationHours = tour.DurationHours ?? 0,
                Location = tour.Location,
                ImageUrl = tour.ImageUrl,
                AverageRating = tour.Reviews.Any() ? tour.Reviews.Average(r => r.Rating ?? 0) : 0,
                ReviewCount = tour.Reviews.Count,
                Reviews = tour.Reviews.Where(r => r.IsVisible).Select(r => new PhucNguyenResort.ViewModels.Reviews.ReviewListVM
                {
                    ReviewId = r.ReviewId,
                    UserName = string.IsNullOrEmpty(r.User?.FullName) ? "Khách hàng" : r.User.FullName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    IsVisible = r.IsVisible,
                    CreatedAt = r.CreatedAt,
                    ServiceType = "Tour",
                    ServiceName = tour.TourName
                }).OrderByDescending(r => r.CreatedAt).ToList(),
                UpcomingSchedules = tour.TourSchedules
                    .Where(s => s.TourDate != null && s.TourDate >= DateTime.Today)
                    .OrderBy(s => s.TourDate)
                    .ThenBy(s => s.StartTime)
                    .Select(s => new ScheduleDisplayVM
                    {
                        ScheduleId = s.ScheduleId,
                        TourDate = s.TourDate!.Value,
                        StartTime = s.StartTime,
                        AvailableSlots = s.AvailableSlots ?? 0,
                        GuideName = s.Guide?.FullName
                    })
                    .ToList()
            };
        }

        /// <inheritdoc/>
        public async Task<BookTourVM?> PrepareBookingFormAsync(int scheduleId, int userId)
        {
            var schedule = await _context.TourSchedules
                .Include(s => s.Tour)
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

            if (schedule == null || schedule.Tour == null) return null;
            if (schedule.TourDate == null) return null;

            // Get user's CheckedIn room bookings for Charge to Room option
            var checkedInBookings = await _context.Bookings
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Room)
                .Where(b => b.UserId == userId && b.BookingStatus == BookingStatus.CheckedIn)
                .ToListAsync();

            var roomOptions = checkedInBookings.Select(b =>
            {
                var rooms = b.BookingDetails
                    .Where(bd => bd.Room != null)
                    .Select(bd => bd.Room!.RoomNumber)
                    .ToList();
                string roomDisplay = rooms.Any() ? string.Join(", ", rooms) : "N/A";
                return new RoomBookingOptionVM
                {
                    BookingId = b.BookingId,
                    DisplayText = $"Booking #{b.BookingId} - Phòng {roomDisplay}"
                };
            }).ToList();

            return new BookTourVM
            {
                ScheduleId = schedule.ScheduleId,
                TourName = schedule.Tour.TourName ?? "Tour",
                TourImage = schedule.Tour.ImageUrl,
                Location = schedule.Tour.Location,
                DurationHours = schedule.Tour.DurationHours ?? 0,
                PricePerPerson = schedule.Tour.Price ?? 0,
                TourDate = schedule.TourDate.Value,
                StartTime = schedule.StartTime,
                AvailableSlots = schedule.AvailableSlots ?? 0,
                AvailableRoomBookings = roomOptions
            };
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Uses Pessimistic Locking with UPDLOCK + ROWLOCK to prevent concurrent overbooking (BR-TOUR-01).
        /// </remarks>
        public async Task<TourBookingResultVM> BookTourAsync(BookTourVM model, int userId)
        {
            // Start a SERIALIZABLE transaction for pessimistic locking
            using var transaction = await _context.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.Serializable);
            try
            {
                // PESSIMISTIC LOCK: Lock the TourSchedule row to prevent concurrent reads
                var schedule = await _context.TourSchedules
                    .FromSqlRaw(
                        "SELECT * FROM TourSchedules WITH (UPDLOCK, ROWLOCK) WHERE ScheduleId = {0}",
                        model.ScheduleId)
                    .Include(s => s.Tour)
                    .FirstOrDefaultAsync();

                if (schedule == null)
                {
                    await transaction.RollbackAsync();
                    return new TourBookingResultVM
                    {
                        Success = false,
                        ErrorCode = "TB-003",
                        ErrorMessage = "Không tìm thấy lịch trình tour."
                    };
                }

                if (schedule.Tour == null)
                {
                    await transaction.RollbackAsync();
                    return new TourBookingResultVM
                    {
                        Success = false,
                        ErrorCode = "TB-003",
                        ErrorMessage = "Tour không tồn tại."
                    };
                }

                // Check available slots (BR-TOUR-01)
                if ((schedule.AvailableSlots ?? 0) < model.Participants)
                {
                    await transaction.RollbackAsync();
                    return new TourBookingResultVM
                    {
                        Success = false,
                        ErrorCode = "TB-001",
                        ErrorMessage = $"Lịch trình không đủ chỗ trống. Chỉ còn {schedule.AvailableSlots ?? 0} chỗ."
                    };
                }

                decimal totalAmount = (schedule.Tour.Price ?? 0) * model.Participants;

                // Validate Charge to Room (BR-TOUR-02)
                int? linkedBookingId = null;
                GuestFolio? guestFolio = null;
                if (model.PaymentMethod == "ChargeToRoom")
                {
                    if (!model.RoomBookingId.HasValue)
                    {
                        await transaction.RollbackAsync();
                        return new TourBookingResultVM
                        {
                            Success = false,
                            ErrorCode = "TB-002",
                            ErrorMessage = "Vui lòng chọn phòng để ghi nợ."
                        };
                    }

                    var roomBooking = await _context.Bookings
                        .Include(b => b.GuestFolio)
                        .FirstOrDefaultAsync(b =>
                            b.BookingId == model.RoomBookingId.Value &&
                            b.UserId == userId &&
                            b.BookingStatus == BookingStatus.CheckedIn);

                    if (roomBooking == null)
                    {
                        await transaction.RollbackAsync();
                        return new TourBookingResultVM
                        {
                            Success = false,
                            ErrorCode = "TB-004",
                            ErrorMessage = "Mã phòng không hợp lệ hoặc khách chưa Check-in."
                        };
                    }

                    linkedBookingId = roomBooking.BookingId;
                    guestFolio = roomBooking.GuestFolio;

                    // Add charge to GuestFolio
                    if (guestFolio == null)
                    {
                        // Auto-create Folio if it's missing for some reason
                        guestFolio = new GuestFolio
                        {
                            BookingId = roomBooking.BookingId,
                            Status = FolioStatus.Unpaid
                        };
                        _context.GuestFolios.Add(guestFolio);
                    }
                    
                    guestFolio.TotalTourCharges += totalAmount;
                    guestFolio.TotalAmount += totalAmount;
                }

                // Deduct available slots
                schedule.AvailableSlots -= model.Participants;
                _context.TourSchedules.Update(schedule);

                // Create tour booking
                var tourBooking = new TourBooking
                {
                    UserId = userId,
                    ScheduleId = model.ScheduleId,
                    BookingId = linkedBookingId,
                    Participants = model.Participants,
                    TotalAmount = totalAmount,
                    BookingStatus = model.PaymentMethod == "ChargeToRoom"
                        ? TourBookingStatus.Confirmed
                        : TourBookingStatus.Pending, // Pending until VNPay confirms
                    PrimaryGuestName = model.PrimaryGuestName,
                    PrimaryGuestDateOfBirth = model.PrimaryGuestDateOfBirth,
                    PrimaryGuestGender = model.PrimaryGuestGender,
                    PrimaryGuestPassportNumber = model.PrimaryGuestPassportNumber,
                    PrimaryGuestNationality = model.PrimaryGuestNationality
                };

                _context.TourBookings.Add(tourBooking);
                await _context.SaveChangesAsync();

                if (guestFolio != null && model.PaymentMethod == "ChargeToRoom")
                {
                    var folioTransaction = new FolioTransaction
                    {
                        Folio = guestFolio, // Use navigation property so EF handles ID if it's new
                        TransactionType = FolioTransactionType.TourCharge,
                        ReferenceId = tourBooking.TourBookingId,
                        Amount = totalAmount,
                        CreatedAt = DateTime.Now
                    };
                    _context.FolioTransactions.Add(folioTransaction);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return new TourBookingResultVM
                {
                    Success = true,
                    TourBookingId = tourBooking.TourBookingId,
                    TotalAmount = totalAmount
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<MyTourBookingItemVM>> GetUserTourBookingsAsync(int userId)
        {
            // Auto-cancel pending bookings older than 1 minute
            var expiredBookings = await _context.TourBookings
                .Include(tb => tb.Schedule)
                .Where(tb => tb.UserId == userId && tb.BookingStatus == TourBookingStatus.Pending)
                .ToListAsync();

            bool hasChanges = false;
            foreach (var tb in expiredBookings)
            {
                if ((DateTime.Now - tb.CreatedAt).TotalMinutes >= 1)
                {
                    tb.BookingStatus = TourBookingStatus.Cancelled;
                    tb.CancellationReason = "Hết hạn thanh toán VNPay";
                    if (tb.Schedule != null && tb.Schedule.AvailableSlots.HasValue)
                    {
                        tb.Schedule.AvailableSlots += tb.Participants ?? 0;
                    }
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                await _context.SaveChangesAsync();
            }

            var bookings = await _context.TourBookings
                .Include(tb => tb.Schedule)
                    .ThenInclude(s => s.Tour)
                .Include(tb => tb.Payments)
                .Where(tb => tb.UserId == userId)
                .OrderByDescending(tb => tb.TourBookingId)
                .ToListAsync();

            var userReviewedTourBookingIds = await _context.Reviews
                .Where(r => r.UserId == userId && r.TourBookingId != null)
                .Select(r => r.TourBookingId!.Value)
                .ToListAsync();

            return bookings.Select(tb =>
            {
                // Determine payment method
                string? paymentMethod = null;
                if (tb.BookingId.HasValue)
                {
                    paymentMethod = "Ghi nợ phòng";
                }
                else if (tb.Payments.Any())
                {
                    paymentMethod = "VNPay";
                }

                // Can cancel if Confirmed (and > 48h) or Pending (anytime)
                bool canCancel = (tb.BookingStatus == TourBookingStatus.Confirmed &&
                                  tb.Schedule?.TourDate != null &&
                                  (tb.Schedule.TourDate.Value - DateTime.Now).TotalHours > 48) ||
                                 (tb.BookingStatus == TourBookingStatus.Pending);

                return new MyTourBookingItemVM
                {
                    TourBookingId = tb.TourBookingId,
                    TourId = tb.Schedule?.TourId ?? 0,
                    TourName = tb.Schedule?.Tour?.TourName ?? "N/A",
                    TourImage = tb.Schedule?.Tour?.ImageUrl,
                    Location = tb.Schedule?.Tour?.Location,
                    TourDate = tb.Schedule?.TourDate ?? DateTime.MinValue,
                    StartTime = tb.Schedule?.StartTime,
                    Participants = tb.Participants ?? 0,
                    TotalAmount = tb.TotalAmount ?? 0,
                    BookingStatus = tb.BookingStatus.ToString(),
                    CanCancel = canCancel,
                    PaymentMethod = paymentMethod,
                    CreatedAt = tb.CreatedAt,
                    CancellationReason = tb.CancellationReason,
                    HasReviewed = userReviewedTourBookingIds.Contains(tb.TourBookingId)
                };
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<TourBookingResultVM> CancelTourBookingAsync(int tourBookingId, int userId, string? reason = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tourBooking = await _context.TourBookings
                    .Include(tb => tb.Schedule)
                    .Include(tb => tb.Booking)
                        .ThenInclude(b => b!.GuestFolio)
                    .FirstOrDefaultAsync(tb => tb.TourBookingId == tourBookingId && tb.UserId == userId);

                if (tourBooking == null)
                {
                    return new TourBookingResultVM
                    {
                        Success = false,
                        ErrorMessage = "Không tìm thấy đặt tour."
                    };
                }

                if (tourBooking.BookingStatus == TourBookingStatus.Cancelled)
                {
                    return new TourBookingResultVM
                    {
                        Success = false,
                        ErrorMessage = "Đặt tour đã được hủy trước đó."
                    };
                }

                if (tourBooking.BookingStatus == TourBookingStatus.Completed)
                {
                    return new TourBookingResultVM
                    {
                        Success = false,
                        ErrorMessage = "Không thể hủy tour đã hoàn thành."
                    };
                }

                // BR-TOUR-04: Check 48h cancellation policy (only for Confirmed bookings)
                if (tourBooking.BookingStatus == TourBookingStatus.Confirmed &&
                    tourBooking.Schedule?.TourDate != null &&
                    (tourBooking.Schedule.TourDate.Value - DateTime.Now).TotalHours <= 48)
                {
                    return new TourBookingResultVM
                    {
                        Success = false,
                        ErrorMessage = "Không thể hủy tour trong vòng 48 giờ trước giờ khởi hành."
                    };
                }

                // Restore available slots
                if (tourBooking.Schedule != null && tourBooking.Schedule.AvailableSlots.HasValue)
                {
                    tourBooking.Schedule.AvailableSlots += tourBooking.Participants ?? 0;
                }

                // Remove charge from GuestFolio if Charge to Room
                if (tourBooking.Booking?.GuestFolio != null)
                {
                    var folio = tourBooking.Booking.GuestFolio;
                    var chargeAmount = tourBooking.TotalAmount ?? 0;
                    folio.TotalTourCharges = Math.Max(0, folio.TotalTourCharges - chargeAmount);
                    folio.TotalAmount = Math.Max(0, folio.TotalAmount - chargeAmount);

                    var folioTransaction = new FolioTransaction
                    {
                        FolioId = folio.FolioId,
                        TransactionType = FolioTransactionType.Refund,
                        ReferenceId = tourBooking.TourBookingId,
                        Amount = chargeAmount,
                        CreatedAt = DateTime.Now
                    };
                    _context.FolioTransactions.Add(folioTransaction);
                }

                tourBooking.BookingStatus = TourBookingStatus.Cancelled;
                tourBooking.CancellationReason = reason;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new TourBookingResultVM
                {
                    Success = true,
                    TourBookingId = tourBookingId
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ConfirmPaymentAsync(int tourBookingId, string transactionCode)
        {
            var tourBooking = await _context.TourBookings
                .FirstOrDefaultAsync(tb => tb.TourBookingId == tourBookingId);

            if (tourBooking == null || tourBooking.BookingStatus != TourBookingStatus.Pending)
                return false;

            tourBooking.BookingStatus = TourBookingStatus.Confirmed;

            var payment = new Payment
            {
                TourBookingId = tourBookingId,
                PaymentMethod = PaymentMethod.VNPay,
                Amount = tourBooking.TotalAmount,
                PaymentStatus = PaymentStatus.Success,
                TransactionCode = transactionCode,
                PaidAt = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<TourBookingSuccessVM?> GetBookingSuccessInfoAsync(int tourBookingId, int userId)
        {
            var tb = await _context.TourBookings
                .Include(t => t.Schedule)
                    .ThenInclude(s => s.Tour)
                .FirstOrDefaultAsync(t => t.TourBookingId == tourBookingId && t.UserId == userId);

            if (tb == null) return null;

            string qrData = $"TourBooking:{tb.TourBookingId}";
            string qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={System.Net.WebUtility.UrlEncode(qrData)}";

            return new TourBookingSuccessVM
            {
                TourBookingId = tb.TourBookingId,
                TourName = tb.Schedule?.Tour?.TourName ?? "N/A",
                TourDate = tb.Schedule?.TourDate ?? DateTime.MinValue,
                StartTime = tb.Schedule?.StartTime,
                Participants = tb.Participants ?? 0,
                TotalAmount = tb.TotalAmount ?? 0,
                PaymentMethod = tb.BookingId.HasValue ? "Ghi nợ phòng" : "VNPay",
                QrCodeUrl = qrUrl
            };
        }
    }
}
