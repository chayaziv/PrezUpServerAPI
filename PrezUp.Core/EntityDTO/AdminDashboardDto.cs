using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.EntityDTO
{
    public class AdminDashboardDto
    {
        public UserStatisticsDto UserStatistics { get; set; }
        public List<UserActivityDto> UserActivities { get; set; }
        public List<RoleDistributionDto> RolesDistribution { get; set; }
        public PresentationStatisticsDto PresentationStatistics { get; set; }
        public List<TopUserDto> TopUsers { get; set; }
        public List<MonthlyPresentationsDto> MonthlyPresentations { get; set; }
        public List<UnusualActivityDto> UnusualActivities { get; set; }
    }

    public class UserStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
    }

    public class UserActivityDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PresentationCount { get; set; }
    }

    public class RoleDistributionDto
    {
        public string RoleName { get; set; }
        public int UserCount { get; set; }
    }

    public class PresentationStatisticsDto
    {
        public int TotalPresentations { get; set; }
        public int PublicPresentations { get; set; }
    }

    public class TopUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PresentationsCount { get; set; }
    }

    public class MonthlyPresentationsDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int PresentationCount { get; set; }
    }

    public class UnusualActivityDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PresentationsCount { get; set; }
    }
}
