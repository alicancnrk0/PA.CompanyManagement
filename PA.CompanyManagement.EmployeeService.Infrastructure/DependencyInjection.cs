using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PA.CompanyManagement.EmployeeService.Application.Repositories;
using PA.CompanyManagement.EmployeeService.Infrastructure.Contexts;
using PA.CompanyManagement.EmployeeService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.EmployeeService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmployeeContext(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<EmployeeDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    conf =>
                    {
                        conf.MigrationsAssembly(typeof(EmployeeDbContext).Assembly.FullName);
                    });
            });

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}
