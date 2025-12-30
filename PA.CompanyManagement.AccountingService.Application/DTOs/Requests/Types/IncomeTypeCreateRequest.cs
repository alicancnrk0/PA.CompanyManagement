using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Types
{
    public record IncomeTypeCreateRequest
    {
        public required Guid CreatedBy { get; set; }
        public string Name { get; set; } = null!;
        public decimal? TaxRate { get; set; }
    }
}
