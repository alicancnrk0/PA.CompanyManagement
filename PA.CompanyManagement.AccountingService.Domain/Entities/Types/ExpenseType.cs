using PA.CompanyManagement.Core.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Domain.Entities.Types
{
    [Table("ExpenseTypes", Schema = "type")]
    public class ExpenseType : BaseEntity
    {
        public string? Name { get; set; }
        public decimal? TaxRate { get; set; }
    }
}
