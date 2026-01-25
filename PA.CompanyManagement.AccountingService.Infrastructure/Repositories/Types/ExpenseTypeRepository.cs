using Microsoft.EntityFrameworkCore;
using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Types;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Types;
using PA.CompanyManagement.AccountingService.Application.Repositories.Types;
using PA.CompanyManagement.AccountingService.Domain.Entities.Types;
using PA.CompanyManagement.AccountingService.Infrastructure.Contexts;
using PA.CompanyManagement.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Infrastructure.Repositories.Types
{
    public class ExpenseTypeRepository : IExpenseTypeRepository
    {

        private readonly AccountingDBContext _context;

        public ExpenseTypeRepository(AccountingDBContext context)
        {
            _context = context;
        }

        public async Task<ExpenseTypeResponse> CreateAsync(ExpenseTypeCreateRequest request)
        {
            try
            {
                await _context
                    .ExpenseTypes
                    .AddAsync(new ExpenseType
                    {
                        CreatedBy = request.CreatedBy,
                        Name = request.Name,
                        TaxRate = request.TaxRate,
                    });

                await _context.SaveChangesAsync();

                return await _context
                    .ExpenseTypes
                    .AsNoTracking()
                    .Where(x => x.Name == request.Name && x.TaxRate == request.TaxRate)
                    .OrderBy(x => x.CreatedAt)
                    .Select(x => new ExpenseTypeResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TaxRate = x.TaxRate
                    })
                    .LastOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextAddException("ExpenseType:Create", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                _context.ExpenseTypes.Remove(_context.ExpenseTypes.Find(id));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextRemoveException("ExpenseType:Delete", ex);
            }
        }

        public async Task<List<ExpenseTypeResponse>> GetAllAsync()
        {
            try
            {
                return await _context
                    .ExpenseTypes
                    .AsNoTracking()
                    .Select(x => new ExpenseTypeResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TaxRate = x.TaxRate
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("ExpenseTypes:GetAll", ex);
            }
        }

        public async Task<ExpenseTypeResponse?> GetAsync(Guid id)
        {
            try
            {
                return await _context
                    .ExpenseTypes
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new ExpenseTypeResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TaxRate = x.TaxRate
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("ExpenseType:Get", ex);
            }
        }

        public async Task<DetailedExpeseTypeResponse?> GetDetailedAsync(Guid id)
        {
            try
            {
                return await _context
                    .ExpenseTypes
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new DetailedExpeseTypeResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        CreatedAt = x.CreatedAt,
                        CreatedBy = x.CreatedBy,
                        LastModifiedAt = x.LastModifiedAt,
                        LastModifiedBy = x.LastModifiedBy,
                        IsDeleted = x.IsDeleted,
                        DeletedAt = x.DeletedAt,
                        DeletedBy = x.DeletedBy,
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("ExpenseType:GetDetailed", ex);
            }
        }

        public async Task UpdateAsync(ExpenseTypeUpdateRequest request)
        {
            try
            {
                var data = await _context
                    .ExpenseTypes
                    .FindAsync(request.Id);

                if (data is null)
                    throw new PAContextUpdateException("ExpenseType:Update:NotFound");

                data.Name = request.Name;
                data.TaxRate = request.TaxRate;
                data.LastModifiedBy = request.ModifiedBy;

                _context.ExpenseTypes.Update(data);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new PAContextUpdateException("ExpenseType:Update", ex);
            }
        }
    }
}
