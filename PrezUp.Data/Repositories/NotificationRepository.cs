//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using PrezUp.Core.Models;

//namespace PrezUp.Data.Repositories
//{
//    public class NotificationRepository : Repository<Notification>, INotificationRepository
//    {
//        public NotificationRepository(DataContext context) : base(context)
//        {
//        }

//        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
//        {
//            return await Context.Set<Notification>()
//                .Where(n => n.UserId == userId)
//                .OrderByDescending(n => n.CreatedAt)
//                .ToListAsync();
//        }

//        public async Task<Notification> CreateNotificationAsync(Notification notification)
//        {
//            notification.CreatedAt = DateTime.UtcNow;
//            notification.IsRead = false;

//            await Context.Set<Notification>().AddAsync(notification);
//            await Context.SaveChangesAsync();

//            return notification;
//        }

//        public async Task<Notification> GetNotificationByIdAsync(int id)
//        {
//            return await Context.Set<Notification>()
//                .FirstOrDefaultAsync(n => n.Id == id);
//        }

//        public async Task<bool> UpdateNotificationAsync(Notification notification)
//        {
//            Context.Set<Notification>().Update(notification);
//            var result = await Context.SaveChangesAsync();
//            return result > 0;
//        }

//        public async Task<int> GetUnreadNotificationCountAsync(string userId)
//        {
//            return await Context.Set<Notification>()
//                .CountAsync(n => n.UserId == userId && !n.IsRead);
//        }

//        public async Task<bool> DeleteNotificationAsync(int id)
//        {
//            var notification = await GetNotificationByIdAsync(id);
//            if (notification == null)
//                return false;

//            Context.Set<Notification>().Remove(notification);
//            var result = await Context.SaveChangesAsync();
//            return result > 0;
//        }
//    }
//} 