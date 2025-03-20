using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace PrezUp.Data.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DataContext _context;
        public IPresentationRepository Presentations { get; }

        public IUserRepository Users { get; }

        public IRoleRepository Roles { get; }

        public IRepository<Tag> Tags { get; }
        public RepositoryManager(DataContext context, IPresentationRepository presentations, IUserRepository users, IRoleRepository roles, IRepository<Tag> tags)
        {
            _context = context;
            Presentations = presentations;
            Users = users;
            Roles = roles;
            Tags = tags;
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
