using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Types;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.Repositories.Types
{
    public interface IIncomeTypeRepository
    {
        Task<List<IncomeTypeResponse>> GetAllAsync();

        Task<IncomeTypeResponse?> GetAsync(Guid id);
        Task<DetailedIncomeTypeResponse?> GetDetailedAsync(Guid id);

        Task<IncomeTypeResponse> CreateAsync(IncomeTypeCreateRequest request);

        Task UpdateAsync(IncomeTypeUpdateRequest request);

        Task DeleteAsync(Guid id);
    }
}
