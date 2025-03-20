using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrezUp.Core;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;
using PrezUp.Core.Service;
using PrezUp.Data;
using PrezUp.Data.Repositories;
using PrezUp.Service.Services;

namespace PrezUp.Extesion
{
     public static class ServiceExtesion
    {
        public static void ServieDependencyInjector(this IServiceCollection s, IConfiguration configuration)
        {
            s.AddScoped<IPresentationService, PresentationService>();
            s.AddScoped<IAnalysisService, AnalysisService>();
            s.AddScoped<Is3Service, AudioUploadService>();
            s.AddScoped<IUserService, UserService>();
            s.AddScoped<IAuthService, AuthService>();
            s.AddScoped<IAdminService, AdminService>();
            s.AddScoped<ITagService, TagService>();

            s.AddScoped<IRepositoryManager, RepositoryManager>();
            s.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            s.AddScoped<IPresentationRepository, PresentationRepository>();
            s.AddScoped<IUserRepository, UserRepository>();
            s.AddScoped<IRoleRepository, RoleRepository>();


           
            s.AddAutoMapper(typeof(AutoMapperProfile));
            s.AddScoped<IValidatorService, ValidatorService>();
           
            s.AddHttpClient();    
            
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            s.AddDbContext<DataContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(connectionString);
            });


        }
    }
}
