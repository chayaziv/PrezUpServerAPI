//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using PrezUp.Core.Models;
//using PrezUp.Service.Services;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace PrezUp.API.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/[controller]")]
//    public class NotificationsController : ControllerBase
//    {
//        private readonly INotificationService _notificationService;

//        public NotificationsController(INotificationService notificationService)
//        {
//            _notificationService = notificationService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetNotifications()
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
//            return Ok(notifications);
//        }

//        [HttpGet("unread-count")]
//        public async Task<IActionResult> GetUnreadCount()
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            var count = await _notificationService.GetUnreadNotificationCountAsync(userId);
//            return Ok(new { count });
//        }

//        [HttpPost]
//        [Authorize(Roles = "Admin")] // רק מנהלים יכולים ליצור התראות
//        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var createdNotification = await _notificationService.CreateNotificationAsync(notification);
//            return CreatedAtAction(nameof(GetNotifications), new { id = createdNotification.Id }, createdNotification);
//        }

//        [HttpPut("{id}/mark-as-read")]
//        public async Task<IActionResult> MarkAsRead(int id)
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            await _notificationService.MarkNotificationAsReadAsync(id, userId);
//            return NoContent();
//        }
//    }
//} 