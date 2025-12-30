using PA.CompanyManagement.AccountingService.Domain.Entities.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Types
{
    public class ExpenseTypeResponses
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal TaxRate { get; set; }
    }

    public class DetailedExpenseTypeResponse : ExpenseType
    {

    } 

}
