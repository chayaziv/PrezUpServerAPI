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
        public IRepository<Presentation> Presentations { get; }

        public RepositoryManager(DataContext context, IRepository<Presentation> presentations)
        {
            _context = context;
            Presentations= presentations;
            
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
