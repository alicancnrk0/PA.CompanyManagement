using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PA.CompanyManagement.AccountingService.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountingContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<AccountingDBContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    conf =>
                    {
                        conf.MigrationsAssembly(typeof(AccountingDBContext).Assembly.FullName);
                    }));

            return services;
        }
    }
}
