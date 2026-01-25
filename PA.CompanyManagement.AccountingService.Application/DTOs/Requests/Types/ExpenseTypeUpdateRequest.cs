using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Types
{
    public record ExpenseTypeUpdateRequest
    {
        public required Guid ModifiedBy { get; set; }
        public required Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal? TaxRate { get; set; }
    }
}
