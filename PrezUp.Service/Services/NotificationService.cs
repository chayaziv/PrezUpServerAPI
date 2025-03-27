//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using PrezUp.Core.Models;
//using PrezUp.Core.IRepositories;

//namespace PrezUp.Service.Services
//{
//    public interface INotificationService
//    {
//        Task<List<Notification>> GetUserNotificationsAsync(string userId);
//        Task<Notification> CreateNotificationAsync(Notification notification);
//        Task MarkNotificationAsReadAsync(int notificationId, string userId);
//        Task<int> GetUnreadNotificationCountAsync(string userId);
//    }

//    public class NotificationService : INotificationService
//    {
//        private readonly IRepositoryManager _repository;

//        public NotificationService(IRepositoryManager repository)
//        {
//            _repository = repository;
//        }

//        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
//        {
//            return await _repository.Notifications.GetUserNotificationsAsync(userId);
//        }

//        public async Task<Notification> CreateNotificationAsync(Notification notification)
//        {
//            return await _repository.Notifications.CreateNotificationAsync(notification);
//        }

//        public async Task MarkNotificationAsReadAsync(int notificationId, string userId)
//        {
//            var notification = await _repository.Notifications.GetNotificationByIdAsync(notificationId);
//            if (notification != null && notification.UserId == userId)
//            {
//                notification.IsRead = true;
//                await _repository.Notifications.UpdateNotificationAsync(notification);
//            }
//        }

//        public async Task<int> GetUnreadNotificationCountAsync(string userId)
//        {
//            return await _repository.Notifications.GetUnreadNotificationCountAsync(userId);
//        }
//    }
//} 