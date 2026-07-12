using PhucNguyenResort.Models;

namespace PhucNguyenResort.Services.Notifications;

public interface INotificationService
{
    /// <summary>
    /// Create a notification and push it via SignalR to the target user.
    /// </summary>
    Task<Notification> CreateAsync(int userId, string title, string message);

    /// <summary>
    /// Get most recent notifications for a user (paged).
    /// </summary>
    Task<List<Notification>> GetByUserAsync(int userId, int take = 10);

    /// <summary>
    /// Get count of unread notifications.
    /// </summary>
    Task<int> GetUnreadCountAsync(int userId);

    /// <summary>
    /// Mark a single notification as read.
    /// </summary>
    Task MarkAsReadAsync(int notificationId, int userId);

    /// <summary>
    /// Mark all notifications as read for a user.
    /// </summary>
    Task MarkAllAsReadAsync(int userId);
}
