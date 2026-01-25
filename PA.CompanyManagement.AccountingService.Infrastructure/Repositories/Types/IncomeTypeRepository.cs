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
    public class IncomeTypeRepository : IIncomeTypeRepository
    {

        private readonly AccountingDBContext _context;

        public IncomeTypeRepository(AccountingDBContext context)
        {
            _context = context;
        }

        public async Task<IncomeTypeResponse> CreateAsync(IncomeTypeCreateRequest request)
        {
            try
            {
                await _context
                    .IncomeTypes
                    .AddAsync(new IncomeType
                    {
                        CreatedBy = request.CreatedBy,
                        Name = request.Name,
                        TaxRate = request.TaxRate,
                    });

                await _context.SaveChangesAsync();

                return await _context
                    .IncomeTypes
                    .AsNoTracking()
                    .Where(x => x.Name == request.Name && x.TaxRate == request.TaxRate)
                    .OrderBy(x => x.CreatedAt)
                    .Select(x => new IncomeTypeResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TaxRate = x.TaxRate
                    })
                    .LastOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextAddException("IncomeType:Create", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                _context.IncomeTypes.Remove(_context.IncomeTypes.Find(id));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextRemoveException("IncomeType:Delete", ex);
            }
        }

        public async Task<List<IncomeTypeResponse>> GetAllAsync()
        {
            try
            {
                return await _context
                    .IncomeTypes
                    .AsNoTracking()
                    .Select(x => new IncomeTypeResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TaxRate = x.TaxRate
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("IncomeTypes:GetAll", ex);
            }
        }

        public async Task<IncomeTypeResponse?> GetAsync(Guid id)
        {
            try
            {
                return await _context
                    .IncomeTypes
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new IncomeTypeResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TaxRate = x.TaxRate
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("IncomeType:Get", ex);
            }
        }

        public async Task<DetailedIncomeTypeResponse?> GetDetailedAsync(Guid id)
        {
            try
            {
                return await _context
                    .IncomeTypes
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new DetailedIncomeTypeResponse
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
                throw new PAContextQueryException("IncomeType:GetDetailed", ex);
            }
        }

        public async Task UpdateAsync(IncomeTypeUpdateRequest request)
        {
            try
            {
                var data = await _context
                    .IncomeTypes
                    .FindAsync(request.Id);

                if (data is null)
                    throw new PAContextUpdateException("IncomeType:Update:NotFound");

                data.Name = request.Name;
                data.TaxRate = request.TaxRate;
                data.LastModifiedBy = request.ModifiedBy;

                _context.IncomeTypes.Update(data);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new PAContextUpdateException("IncomeType:Update", ex);
            }
        }

    }
}
