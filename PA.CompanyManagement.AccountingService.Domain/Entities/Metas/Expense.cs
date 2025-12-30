using PA.CompanyManagement.Core.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Domain.Entities.Metas
{
    [Table("Expenses", Schema = "meta")]
    public class Expense : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? ExpenseDate { get; set; }
        public bool Completed { get; set; }
        public decimal? Amount { get; set; }

        public Guid? TypeId { get; set; }
    }
}
