using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PA.CompanyManagement.Core.Domain.Settings;
using PA.CompanyManagement.Core.Extensions;
using PA.CompanyManagement.NotificationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.NotificationService.Infrastructure.Contexts
{
    public class NotificationDbContext : DbContext
    {
        private readonly IConfiguration? _conf;
        private readonly ICurrentUser? _currentUser;

        public NotificationDbContext()
        {

        }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {

        }

        public NotificationDbContext(IConfiguration conf)
        {
            _conf = conf;
        }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options, IConfiguration conf) : base(options)
        {
            _conf = conf;
        }

        public NotificationDbContext(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options, ICurrentUser currentUser) : base(options)
        {
            _currentUser = currentUser;
        }

        public NotificationDbContext(IConfiguration conf, ICurrentUser currentUser)
        {
            _conf = conf;
            _currentUser = currentUser;
        }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options, IConfiguration conf, ICurrentUser currentUser) : base(options)
        {
            _conf = conf;
            _currentUser = currentUser;
        }

        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_conf is not null)
                ContextConfigurationsExtensions.Configure(optionsBuilder, _conf);

#if DEBUG
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=127.0.0.1;User Id=TheRoslyn;Password=1q2w3e4r5.T!;Encrypt=False;Database=NotificationDb;");
#endif

            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            if (_currentUser is not null)
                SaveSchangesExtensions.OnBeforeSaving(this, _currentUser);


            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (_currentUser is not null)
                SaveSchangesExtensions.OnBeforeSaving(this, _currentUser);

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}
