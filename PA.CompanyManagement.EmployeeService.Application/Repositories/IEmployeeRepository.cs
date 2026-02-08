using PA.CompanyManagement.EmployeeService.Application.DTOs.Requests;
using PA.CompanyManagement.EmployeeService.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.EmployeeService.Application.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeResponse>> GetAllAsync();

        Task<EmployeeResponse?> GetAsync(Guid id);
        Task<DetailedEmployeeResponse?> GetDetailedAsync(Guid id);

        Task<EmployeeResponse> CreateAsync(EmployeeCreateRequest request);

        Task UpdateAsync(EmployeeUpdateRequest request);

        Task DeleteAsync(Guid id);
    }
}
