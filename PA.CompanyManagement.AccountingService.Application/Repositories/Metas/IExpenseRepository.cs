using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Metas;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Metas;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.Repositories.Metas
{
    public interface IExpensRepository
    {
        Task<List<MinimalExpenseResponse>> GetAllAsync(); 
        Task<List<MinimalExpenseResponse>> GetAllAsync(Guid expenseTypeId);

        Task<ExpenseResponse?> GetAsync(Guid id);
        Task<DetailedExpenseResponse?> GetDetailedAsync(Guid id);

        Task<ExpenseResponse> CreateAsync(ExpenseCreateRequest request);

        Task UpdateAsync(ExpenseUpdateRequest request);
        Task PatchAsync(ExpensePatchRequest request);

        Task DeleteAsync(Guid id);
    }
}
