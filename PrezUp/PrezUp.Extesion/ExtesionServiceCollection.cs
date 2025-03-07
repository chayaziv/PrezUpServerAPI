using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PrezUp.Extesion
{
     public class ExtesionServiceCollection
    {
        public static void ServieDependencyInjector(this IServiceCollection s)
        {

            s.AddScoped<IAgreementService, AgreementService>();
            s.AddScoped<ICompanyService, CompanyService>();
            s.AddScoped<IDeliveryManService, DeliveryManService>();
            s.AddScoped<ISendingService, SendingService>();
            s.AddScoped<IRepositoryManager, RepositoryManager>();
            s.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            s.AddDbContext<DataContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer("Data Source = DESKTOP-13C4MS2; Initial Catalog = Deliveries_DB; Integrated Security = true; ");

            });


        }
    }
}
