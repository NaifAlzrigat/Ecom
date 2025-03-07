using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure
{
    public static class infrastructureRegestration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
            //services.AddScoped(typeof(IPhotoRepository), typeof(PhotoRepository));
            //services.AddScoped(typeof(IProfuctRepository), typeof(ProfuctRepository));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AppDBContext>(op => { op.UseSqlServer(configuration.GetConnectionString("conn")); });
            return services;
        }
    }
}
