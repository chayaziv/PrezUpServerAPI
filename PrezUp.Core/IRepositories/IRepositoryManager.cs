using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;
//using PrezUp.Data.Repositories;

namespace PrezUp.Core.IRepositories
{
   public interface IRepositoryManager
    {
        public IPresentationRepository Presentations { get; }
        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; }
        public IRepository<Tag> Tags { get; }
        //public INotificationRepository Notifications { get; }
        Task<int> SaveAsync();
    }
}
