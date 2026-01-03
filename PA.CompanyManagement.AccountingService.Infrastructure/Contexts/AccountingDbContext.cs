using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PA.CompanyManagement.AccountingService.Domain.Entities.Metas;
using PA.CompanyManagement.AccountingService.Domain.Entities.Types;
using PA.CompanyManagement.Core.Domain.Settings;
using PA.CompanyManagement.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Infrastructure.Contexts
{
    public class AccountingDbContext : DbContext
    {
        private readonly IConfiguration? _conf;
        private readonly ICurrentUser? _currentUser;

        public AccountingDbContext() { }

        public AccountingDbContext(DbContextOptions<AccountingDbContext> options): base(options) { }

        public AccountingDbContext(IConfiguration conf) => _conf = conf;

        public AccountingDbContext(DbContextOptions<AccountingDbContext> options, IConfiguration conf) : base(options) => _conf = conf;

        public AccountingDbContext(ICurrentUser currentUser) => _currentUser = currentUser;

        public AccountingDbContext(DbContextOptions<AccountingDbContext> options, ICurrentUser currentUser) : base(options) => _currentUser = currentUser;

        public AccountingDbContext(IConfiguration conf, ICurrentUser currentUser) 
        {
            _conf = conf;
            _currentUser = currentUser;
        }

        public AccountingDbContext(DbContextOptions<AccountingDbContext> options, 
            IConfiguration conf,
            ICurrentUser currentUser) : base(options) 
        {
            _conf = conf;
            _currentUser = currentUser;
        }

        public DbSet<IncomeType> IncomeTypes { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public override int SaveChanges()
        {
            if(_currentUser != null)
                this.OnBeforeSaving(_currentUser);
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_currentUser != null)
                this.OnBeforeSaving(_currentUser);
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_conf != null)
                optionsBuilder.Configure(_conf);

#if DEBUG
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=AccountingDb;User Id=sa;Password=11;Encrypt=False");
#endif

            base.OnConfiguring(optionsBuilder);
        }

    }
}
