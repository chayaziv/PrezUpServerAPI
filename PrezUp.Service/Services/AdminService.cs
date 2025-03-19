using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using PrezUp.Core.IRepositories;

namespace PrezUp.Service.Services
{
    public class AdminService
    {
        private readonly IRepositoryManager _repository;

        public AdminService(IRepositoryManager manager)
        {
            _repository = manager;
        }

        public async Task<object> GetUsersActivity()
        {
            var userActivity = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    PresentationCount = u.Presentations.Count()
                })
                .ToListAsync();

            return new
            {
                TotalUsers = userActivity.Count,
                ActiveUsers = userActivity.Count(u => u.PresentationCount > 0),
                InactiveUsers = userActivity.Count(u => u.PresentationCount == 0),
                UserActivity = userActivity
            };
        }

        public async Task<object> GetRolesDistribution()
        {
            var roleDistribution = await _context.Roles
                .Select(r => new
                {
                    RoleName = r.RoleName,
                    UserCount = r.Users.Count()
                })
                .ToListAsync();

            return roleDistribution;
        }

        public async Task<object> GetPresentationStats()
        {
            var totalPresentations = await _context.Presentations.CountAsync();
            var publicPresentations = await _context.Presentations.CountAsync(p => p.IsPublic);
            var privatePresentations = totalPresentations - publicPresentations;

            var topUsers = await _context.Users
                .OrderByDescending(u => u.Presentations.Count)
                .Take(5)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    PresentationsCount = u.Presentations.Count()
                })
                .ToListAsync();

            return new
            {
                TotalPresentations = totalPresentations,
                PublicPresentations = publicPresentations,
                PrivatePresentations = privatePresentations,
                TopUsers = topUsers
            };
        }

        public async Task<object> GetMonthlyPresentations()
        {
            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);

            var monthlyData = await _context.Presentations
                .Where(p => p.Id > 0) // להחליף לתאריך יצירה אם קיים
                .GroupBy(p => new { Year = DateTime.UtcNow.Year, Month = DateTime.UtcNow.Month }) // להחליף לפי תאריך יצירה
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToListAsync();

            return monthlyData;
        }

        public async Task<object> GetUnusualActivity()
        {
            var threshold = 10; // מספר מינימלי של מצגות בפרק זמן קצר שנחשב לחריג

            var unusualUsers = await _context.Users
                .Where(u => u.Presentations.Count > threshold)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    PresentationsCount = u.Presentations.Count()
                })
                .ToListAsync();

            return unusualUsers;
        }
    }
}
