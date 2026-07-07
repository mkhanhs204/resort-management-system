using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Hubs;
using PhucNguyenResort.Models;

namespace PhucNguyenResort.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly ResortDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(ResortDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<Notification> CreateAsync(int userId, string title, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            IsRead = false,
            SentAt = DateTime.Now
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // Push real-time via SignalR to the specific user
        var unreadCount = await GetUnreadCountAsync(userId);
        await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", new
        {
            notification.NotificationId,
            notification.Title,
            notification.Message,
            notification.IsRead,
            SentAt = notification.SentAt.ToString("dd/MM/yyyy HH:mm"),
            SentAtRelative = GetRelativeTime(notification.SentAt),
            UnreadCount = unreadCount
        });

        return notification;
    }

    public async Task<List<Notification>> GetByUserAsync(int userId, int take = 10)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.SentAt)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId && !n.IsRead)
            .CountAsync();
    }

    public async Task MarkAsReadAsync(int notificationId, int userId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.NotificationId == notificationId && n.UserId == userId);

        if (notification != null && !notification.IsRead)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(int userId)
    {
        await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));
    }

    private static string GetRelativeTime(DateTime dateTime)
    {
        var span = DateTime.Now - dateTime;

        if (span.TotalMinutes < 1) return "Vừa xong";
        if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} phút trước";
        if (span.TotalHours < 24) return $"{(int)span.TotalHours} giờ trước";
        if (span.TotalDays < 7) return $"{(int)span.TotalDays} ngày trước";
        return dateTime.ToString("dd/MM/yyyy");
    }
}
