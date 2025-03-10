using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;

namespace PrezUp.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

       
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
        
        //private readonly IPasswordHasher<User> _passwordHasher;

        //public AuthRepository(AppDbContext context, IPasswordHasher<User> passwordHasher)
        //{
        //    _context = context;
        //    _passwordHasher = passwordHasher;
        //}

        //public async Task<User?> GetUserByUsernameAsync(string username)
        //{
        //    return await _context.Users.Include(u => u.UserRoles)
        //                               .ThenInclude(ur => ur.Role)
        //                               .FirstOrDefaultAsync(u => u.Username == username);
        //}

        //public async Task<bool> UserExistsAsync(string username)
        //{
        //    return await _context.Users.AnyAsync(u => u.Username == username);
        //}

        //public async Task<User> CreateUserAsync(string username, string password, string email)
        //{
        //    var user = new User
        //    {
        //        Name = username,
        //        Email = email,
        //        PasswordHash = _passwordHasher.HashPassword(null!, password)
        //    };

        //    _dbSet.Add(user);
        //    await _context.SaveChangesAsync();
        //    return user;
        //}
    }
}
