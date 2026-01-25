using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Metas
{
    public record IncomeUpdateRequest
    {
        public required Guid ModifiedBy { get; set; }
        public required Guid Id { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? IncomeDate { get; set; }
        public decimal? Amount { get; set; }
    }

    public record IncomePatchRequest
    {
        public required Guid ModifiedBy { get; set; }
        public required Guid Id { get; set; }
        public bool Completed { get; set; }
    }
}
