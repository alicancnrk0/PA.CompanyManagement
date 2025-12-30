using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Metas
{
    public record IncomeCreateRequest
    {
        public required Guid CreatedBy { get; init; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset IncomeDate { get; set; }
        public bool Completed { get; set; }
        public decimal Amount { get; set; }
        public Guid TypeId { get; set; }
    }
}
