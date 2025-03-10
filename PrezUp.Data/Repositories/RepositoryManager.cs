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
   public class RepositoryManager:IRepositoryManager
    {
        private readonly DataContext _context;
        public IPresentationRepository Presentations { get; }

        public IUserRepository Users { get; }

        public RepositoryManager(DataContext context, IPresentationRepository presentations, IUserRepository users)
        {
            _context = context;
            Presentations = presentations;
            Users = users;
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
