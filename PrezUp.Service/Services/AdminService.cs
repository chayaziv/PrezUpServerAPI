using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;
using PrezUp.Core.Utils;

namespace PrezUp.Service.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepositoryManager _repository;

        public AdminService(IRepositoryManager manager)
        {
            _repository = manager;
        }

        public async Task<Result<AdminDashboardDto>> GetDashboardDataAsync()
        {
           var dashboard= new AdminDashboardDto
            {
                UserStatistics = new UserStatisticsDto
                {
                    TotalUsers = await _repository.Users.GetTotalUsersAsync(),
                    ActiveUsers = await _repository.Users.GetActiveUsersAsync(),
                    InactiveUsers = await _repository.Users.GetInactiveUsersAsync()
                },
                UserActivities = await _repository.Users.GetUserActivityAsync(),
                RolesDistribution = await _repository.Roles.GetRolesDistributionAsync(),
                PresentationStatistics = new PresentationStatisticsDto
                {
                    TotalPresentations = await _repository.Presentations.GetTotalPresentationsAsync(),
                    PublicPresentations = await _repository.Presentations.GetPublicPresentationsCountAsync()
                },
                TopUsers = await _repository.Users.GetTopUsersAsync(),
                //MonthlyPresentations = await _adminRepository.GetMonthlyPresentationsAsync(),
                MonthlyPresentations = new List<MonthlyPresentationsDto>(),
                UnusualActivities = await _repository.Users.GetUnusualActivityAsync()
            };
            if (dashboard == null)
            {
                return Result<AdminDashboardDto>.Failure("error in loading data");
            }
            return Result<AdminDashboardDto>.Success(dashboard);
        }

    }
}
