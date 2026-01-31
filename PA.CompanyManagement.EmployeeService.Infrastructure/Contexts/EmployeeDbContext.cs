using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PA.CompanyManagement.Core.Domain.Settings;
using PA.CompanyManagement.EmployeeService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static PA.CompanyManagement.Core.Extensions.SaveSchangesExtensions;
using static PA.CompanyManagement.Core.Extensions.ContextConfigurationsExtensions;


namespace PA.CompanyManagement.EmployeeService.Infrastructure.Contexts
{
    public class EmployeeDbContext : DbContext
    {
        private readonly IConfiguration? _conf;
        private readonly ICurrentUser? _currentUser;

        public EmployeeDbContext() { }
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public EmployeeDbContext(IConfiguration conf) => _conf = conf;
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options, IConfiguration conf)
            : base(options) => _conf = conf;

        public EmployeeDbContext(ICurrentUser currentUser) => _currentUser = currentUser;
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options, ICurrentUser currentUser)
            : base(options) => _currentUser = currentUser;

        public EmployeeDbContext(IConfiguration conf, ICurrentUser currentUser)
        {
            _conf = conf;
            _currentUser = currentUser;
        }
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options,
            IConfiguration conf,
            ICurrentUser currentUser) : base(options)
        {
            _conf = conf;
            _currentUser = currentUser;
        }

        public DbSet<Employee> Emploees { get; set; }


        public override int SaveChanges()
        {
            if (_currentUser != null)
                this.OnBeforeSaving(_currentUser);
            else
                this.OnBeforeSaving();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_currentUser != null)
                this.OnBeforeSaving(_currentUser);
            else
                this.OnBeforeSaving();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_conf != null)
                optionsBuilder.Configure(_conf);

#if DEBUG
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=127.0.0.1;User Id=sa;Password=11;Encrypt=False;Database=EmployeeDb;");
#endif

            base.OnConfiguring(optionsBuilder);
        }


    }
}
