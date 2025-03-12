using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrezUp.Core;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;
using PrezUp.Data;
using PrezUp.Data.Repositories;
using PrezUp.Service.Services;

namespace PrezUp.Extesion
{
     public static class ExtesionServiceCollection
    {
        public static void ServieDependencyInjector(this IServiceCollection s)
        {

            
            s.AddScoped<IPresentationService, PresentationService>();
            s.AddScoped<IUserService, UserService>();
            s.AddScoped<IRepositoryManager, RepositoryManager>();
            s.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            s.AddScoped<IPresentationRepository, PresentationRepository>();
            s.AddScoped<IUserRepository, UserRepository>();
            s.AddScoped<IAuthService, AuthService>();
            s.AddAutoMapper(typeof(AutoMapperProfile));
            s.AddHttpClient();
            
            s.AddDbContext<DataContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer("Data Source = DESKTOP-13C4MS2; Initial Catalog = PrezUp_DB; Integrated Security = true;TrustServerCertificate=True");

            });


        }
    }
}
