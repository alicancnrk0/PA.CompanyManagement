using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Types;
using PA.CompanyManagement.AccountingService.Domain.Entities.Metas;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Metas
{
    public class IncomeResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? ExpenseDate { get; set; }
        public bool Completed { get; set; }
        public decimal? Amount { get; set; }
        public string? TypeName { get; set; }
        public decimal? TaxRate { get; set; }
    }

    public class DetailedIncomeResponse : Expense
    {
        public IncomeTypeResponse? IncomeType { get; set; } 

    }

    public class MinimalIncomeResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset? ExpenseDate { get; set; }
        public bool Completed { get; set; }
        public decimal? Amount { get; set; }
        public string? TypeName { get; set; }
    }
}
