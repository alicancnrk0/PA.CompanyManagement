using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Metas;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Metas;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Application.Repositories.Metas
{
    public interface IIncomeRepository
    {
        Task<List<MinimalIncomeResponse>> GetAllAsync();
        Task<List<MinimalIncomeResponse>> GetAllAsync(Guid IncomeTypeId);

        Task<IncomeResponse> GetAsync(Guid id);
        Task<DetailedIncomeResponse> GetDetailedAsync(Guid id);

        Task<IncomeResponse> CreateAsync(IncomeCreateRequest request);

        Task UpdateAsync(IncomeUpdateRequest request);
        Task PatchAsync(IncomePatchRequest request);
        Task DeleteAsync(Guid id);
    }
}
