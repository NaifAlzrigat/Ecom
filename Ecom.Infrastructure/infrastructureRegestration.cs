using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using Ecom.Infrastructure.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
            //services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            //services.AddSingleton<IFileProvider>(provider =>
            //{
            //    var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            //    return new PhysicalFileProvider(webRootPath);
            //});
            services.AddSingleton<IFileProvider>(provider =>
            {
                var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(webRootPath))
                {
                    Directory.CreateDirectory(webRootPath);
                }

                return new PhysicalFileProvider(webRootPath);
            });

            services.AddSingleton<IImageManagementService,ImageManagementService>();
            services.AddDbContext<AppDBContext>(
                op => { 
                    op.UseSqlServer(configuration.GetConnectionString("conn")); });
            return services;
        }
    }
}
