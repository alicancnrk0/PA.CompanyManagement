using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Metas;
using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Types;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.Repositories.Types
{
    public interface IExpenseTypeRepository
    {
        Task<List<ExpenseTypeResponse>> GetAllAsync();

        Task<ExpenseTypeResponse?> GetAsync(Guid id);
        Task<DetailedExpeseTypeResponse?> GetDetailedAsync(Guid id);

        Task<ExpenseTypeResponse> CreateAsync(ExpenseTypeCreateRequest request);

        Task UpdateAsync(ExpenseTypeUpdateRequest request);

        Task DeleteAsync(Guid id);

    }
}
