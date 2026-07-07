using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.Notifications;

namespace PhucNguyenResort.Controllers;

[Route("api/notifications")]
[ApiController]
[Authorize]
public class NotificationsApiController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsApiController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// GET /api/notifications — Get recent notifications for the logged-in user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] int take = 10)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var notifications = await _notificationService.GetByUserAsync(userId.Value, take);
        var unreadCount = await _notificationService.GetUnreadCountAsync(userId.Value);

        var result = notifications.Select(n => new
        {
            n.NotificationId,
            n.Title,
            n.Message,
            n.IsRead,
            SentAt = n.SentAt.ToString("dd/MM/yyyy HH:mm"),
            SentAtRelative = GetRelativeTime(n.SentAt)
        });

        return Ok(new { notifications = result, unreadCount });
    }

    /// <summary>
    /// GET /api/notifications/unread-count — Get the count of unread notifications.
    /// </summary>
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var count = await _notificationService.GetUnreadCountAsync(userId.Value);
        return Ok(new { unreadCount = count });
    }

    /// <summary>
    /// POST /api/notifications/{id}/read — Mark a single notification as read.
    /// </summary>
    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        await _notificationService.MarkAsReadAsync(id, userId.Value);
        var unreadCount = await _notificationService.GetUnreadCountAsync(userId.Value);
        return Ok(new { unreadCount });
    }

    /// <summary>
    /// POST /api/notifications/read-all — Mark all notifications as read.
    /// </summary>
    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        await _notificationService.MarkAllAsReadAsync(userId.Value);
        return Ok(new { unreadCount = 0 });
    }

    private int? GetCurrentUserId()
    {
        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ? userId : null;
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
