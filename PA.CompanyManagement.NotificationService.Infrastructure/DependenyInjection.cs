using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PA.CompanyManagement.NotificationService.Application.Repositories;
using PA.CompanyManagement.NotificationService.Infrastructure.Contexts;
using PA.CompanyManagement.NotificationService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.NotificationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotificationContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<NotificationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    conf =>
                    {
                        conf.MigrationsAssembly(typeof(NotificationDbContext).Assembly.FullName);
                    });
            });

            services.AddScoped<IMessageRepository, MessageRepository>();

            return services;
        }
    }
}
