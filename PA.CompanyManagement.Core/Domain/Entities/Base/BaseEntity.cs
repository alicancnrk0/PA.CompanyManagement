using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PA.CompanyManagement.Core.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        [Key] public Guid Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }

        public Guid? CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set;}
        public bool IsDeleted { get; set; }
    }
}
