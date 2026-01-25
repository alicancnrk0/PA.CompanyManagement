using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Extensions
{
    public static class ContextConfigurationsExtensions
    {
        public static void Configure(this DbContextOptionsBuilder builder, 
            IConfiguration conf)
        {
            if (!builder.IsConfigured)
                builder.UseSqlServer(conf.GetConnectionString("DefaultConnection"));
        }
    }
}
