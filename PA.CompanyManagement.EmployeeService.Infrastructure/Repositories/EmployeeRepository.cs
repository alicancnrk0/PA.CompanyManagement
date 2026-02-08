using Microsoft.EntityFrameworkCore;
using PA.CompanyManagement.Core.Exceptions;
using PA.CompanyManagement.EmployeeService.Application.DTOs.Requests;
using PA.CompanyManagement.EmployeeService.Application.DTOs.Responses;
using PA.CompanyManagement.EmployeeService.Application.Repositories;
using PA.CompanyManagement.EmployeeService.Domain.Entities;
using PA.CompanyManagement.EmployeeService.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PA.CompanyManagement.EmployeeService.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeResponse> CreateAsync(EmployeeCreateRequest request)
        {
            try
            {
                await _context.Employees.AddAsync(new Employee
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    PhoneNumber = request.PhoneNumber,
                    EmailAddress = request.EmailAddress,
                    Address = request.Address,
                    CreatedBy = request.CreatedBy,
                });
                await _context.SaveChangesAsync();

                return await _context
                    .Employees
                    .Where(x => x.PhoneNumber == request.PhoneNumber)
                    .Select(x => new EmployeeResponse
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        BirthDate = x.BirthDate,
                        PhoneNumber = x.PhoneNumber,
                        EmailAddress = x.EmailAddress,
                        Address = x.Address,
                        Id = x.Id
                    })
                    .FirstAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextAddException(ex.Message, ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var data = await _context
                    .Employees
                    .FindAsync(id);

                if (data is null)
                    throw new PAContextQueryException("Çalışan bulunamadı!");

                data.DeletedAt = DateTimeOffset.Now;
                data.IsDeleted = true;

                _context.Employees.Update(data);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextUpdateException(ex.Message, ex);
            }
        }

        public async Task<List<EmployeeResponse>> GetAllAsync()
        {
            try
            {
                return await _context
                    .Employees
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .Select(x => new EmployeeResponse
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        BirthDate = x.BirthDate,
                        PhoneNumber = x.PhoneNumber,
                        EmailAddress = x.EmailAddress,
                        Address = x.Address,
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException(ex.Message, ex);
            }
        }

        public async Task<EmployeeResponse?> GetAsync(Guid id)
        {
            try
            {
                return await _context
                    .Employees
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new EmployeeResponse
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        BirthDate = x.BirthDate,
                        PhoneNumber = x.PhoneNumber,
                        EmailAddress = x.EmailAddress,
                        Address = x.Address,
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException(ex.Message, ex);
            }
        }

        public async Task<DetailedEmployeeResponse?> GetDetailedAsync(Guid id)
        {
            try
            {
                var employee = await _context
                    .Employees
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (employee is null)
                    return null;

                return employee as DetailedEmployeeResponse;
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException(ex.Message, ex);
            }
        }

        public async Task UpdateAsync(EmployeeUpdateRequest request)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(request.Id);

                if (employee is null)
                    throw new PAContextQueryException("Çalışan bulunamadı!");

                employee.PhoneNumber = request.PhoneNumber;
                employee.EmailAddress = request.EmailAddress;
                employee.Address = request.Address;
                employee.LastModifiedBy = request.UpdatedBy;

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextUpdateException(ex.Message, ex);
            }
        }
    }
}
