using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Bookings;

public class BookingGuestService : IBookingGuestService
{
    private readonly ResortDbContext _context;
    private readonly IDataProtector _protector;

    public BookingGuestService(ResortDbContext context, IDataProtectionProvider provider)
    {
        _context = context;
        _protector = provider.CreateProtector("PhucNguyenResort.BookingGuestService");
    }

    public async Task<bool> SaveBookingGuestsAsync(int bookingId, IEnumerable<BookingGuestDto> guests, bool consentChecked)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Category)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
        {
            throw new ArgumentException("REG-003: Booking not found");
        }

        // Limit count check
        int maxCapacity = (booking.NumberOfAdults ?? 0) + (booking.NumberOfChildren ?? 0);
        if (guests.Count() > maxCapacity)
        {
            throw new ArgumentException("REG-002: Guest count exceeds booking limit");
        }

        // PDPD Consent check if any guest has a PassportNumber
        bool hasCccd = guests.Any(g => !string.IsNullOrWhiteSpace(g.PassportNumber));
        if (hasCccd && !consentChecked)
        {
            throw new InvalidOperationException("REG-001: PDPD consent is required");
        }

        // Update booking consent
        if (consentChecked)
        {
            booking.PdpdConsentChecked = true;
            booking.ConsentTimestamp = DateTime.Now;
        }

        // Remove old guests
        var oldGuests = await _context.BookingGuests
            .Where(bg => bg.BookingId == bookingId)
            .ToListAsync();
        _context.BookingGuests.RemoveRange(oldGuests);

        // Add new guests
        foreach (var dto in guests)
        {
            string? encryptedPassport = null;
            if (!string.IsNullOrWhiteSpace(dto.PassportNumber))
            {
                encryptedPassport = _protector.Protect(dto.PassportNumber);
            }

            var guest = new BookingGuest
            {
                BookingId = bookingId,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PassportNumber = encryptedPassport,
                Nationality = dto.Nationality,
                IsPrimaryBooker = dto.IsPrimaryBooker
            };

            _context.BookingGuests.Add(guest);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BookingGuestDto>> GetBookingGuestsForCheckInAsync(int bookingId)
    {
        var guests = await _context.BookingGuests
            .Where(bg => bg.BookingId == bookingId)
            .ToListAsync();

        var dtos = new List<BookingGuestDto>();
        foreach (var g in guests)
        {
            string decryptedPassport = string.Empty;
            if (!string.IsNullOrWhiteSpace(g.PassportNumber))
            {
                decryptedPassport = _protector.Unprotect(g.PassportNumber);
            }

            dtos.Add(new BookingGuestDto
            {
                FullName = g.FullName ?? string.Empty,
                DateOfBirth = g.DateOfBirth,
                Gender = g.Gender,
                PassportNumber = decryptedPassport,
                Nationality = g.Nationality ?? string.Empty,
                IsPrimaryBooker = g.IsPrimaryBooker
            });
        }

        return dtos;
    }

    public async Task<int> PurgeExpiredPiiDataAsync()
    {
        var thresholdDate = DateTime.Today.AddDays(-7);
        
        var expiredBookings = await _context.Bookings
            .Where(b => b.BookingStatus == BookingStatus.CheckedOut && b.CheckOutDate < thresholdDate)
            .Include(b => b.BookingGuests)
            .ToListAsync();

        int purgedCount = 0;
        foreach (var booking in expiredBookings)
        {
            bool hasPurged = false;
            foreach (var guest in booking.BookingGuests)
            {
                if (guest.PassportNumber != null || guest.DateOfBirth != null)
                {
                    guest.PassportNumber = null;
                    guest.DateOfBirth = null;
                    hasPurged = true;
                }
            }

            if (hasPurged)
            {
                purgedCount++;
            }
        }

        if (purgedCount > 0)
        {
            await _context.SaveChangesAsync();
        }

        return purgedCount;
    }
}
