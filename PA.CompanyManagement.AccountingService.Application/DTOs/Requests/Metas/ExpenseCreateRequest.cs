using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Metas
{
    public record ExpenseCreateRequest
    {
        public required Guid CreatedBy { get; init; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset ExpenseDate { get; set; }
        public bool Completed { get; set; }
        public decimal Amount { get; set; }
        public Guid TypeId { get; set; }
    }
}
