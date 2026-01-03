using Microsoft.EntityFrameworkCore;
using PA.CompanyManagement.Core.Domain.Entities.Base;
using PA.CompanyManagement.Core.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Extensions
{
    public static class SaveChangesExtensions
    {
        public static void OnBeforeSaving(this DbContext context, ICurrentUser currentUser)
        {
            var modifiedEntires = context.ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && x.State is EntityState.Added or EntityState.Modified);

            foreach (var entire in modifiedEntires)
            {
                var entity = entire.Entity as BaseEntity;

                switch (entire.State)
                {
                    case EntityState.Modified:
                        entity.LastModifiedAt = DateTimeOffset.UtcNow;
                        if(entity.IsDeleted == true)
                        {
                            entity.DeletedBy = currentUser.Id;
                            entity.DeletedAt = DateTimeOffset.UtcNow;
                        }
                        break;
                    case EntityState.Added:
                        entity.IsDeleted = false;
                        entity.CreatedAt = DateTimeOffset.UtcNow;
                        entity.CreatedBy = currentUser.Id;
                        entity.LastModifiedAt = DateTimeOffset.UtcNow;
                        entity.LastModifiedBy = currentUser.Id;
                        entity.Id = Guid.NewGuid();
                        break;
                }
            }
        }
    }
}
