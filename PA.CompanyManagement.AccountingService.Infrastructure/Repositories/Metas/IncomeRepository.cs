using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Metas;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Metas;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Types;
using PA.CompanyManagement.AccountingService.Application.Repositories.Metas;
using PA.CompanyManagement.AccountingService.Domain.Entities.Metas;
using PA.CompanyManagement.AccountingService.Infrastructure.Contexts;
using PA.CompanyManagement.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.AccountingService.Infrastructure.Repositories.Metas
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly AccountingDBContext _context;

        public IncomeRepository(AccountingDBContext context)
        {
            _context = context;
        }


        public async Task<IncomeResponse> CreateAsync(IncomeCreateRequest request)
        {
            try
            {
                var income = new Income
                {
                    Id = Guid.NewGuid(),
                    Amount = request.Amount,
                    Completed = request.Completed,
                    CreatedBy = request.CreatedBy,
                    Description = request.Description,
                    IncomeDate = request.IncomeDate,
                    Title = request.Title,
                    TypeId = request.TypeId,
                };

                await _context.Incomes.AddAsync(income);
                await _context.SaveChangesAsync();

                var type = await _context
                    .IncomeTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.TypeId);

                return new IncomeResponse
                {
                    Id = income.Id,
                    Amount = income.Amount,
                    Completed = income.Completed,
                    Description = income.Description,
                    IncomeDate = income.IncomeDate,
                    Title = income.Title,
                    TaxRate = type?.TaxRate,
                    TypeName = type?.Name
                };
            }
            catch (Exception ex)
            {
                throw new PAContextAddException("Income:Create", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                _context.Incomes.Remove(await _context.Incomes.FindAsync(id));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextRemoveException("Income:Delete", ex);
            }
        }

        public async Task<List<MinimalIncomeResponse>> GetAllAsync()
        {
            try
            {
                return await _context
                    .Incomes
                    .AsNoTracking()
                    .Select(x => new MinimalIncomeResponse
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        Completed = x.Completed,
                        IncomeDate = x.IncomeDate,
                        Title = x.Title,
                        TypeName = _context.IncomeTypes.Where(y => y.Id == x.TypeId).Select(y => y.Name).FirstOrDefault()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("Income:GetAll", ex);
            }
        }

        public async Task<List<MinimalIncomeResponse>> GetAllAsync(Guid incomeTypeId)
        {
            try
            {
                return await _context
                    .Incomes
                    .AsNoTracking()
                    .Where(x => x.TypeId == incomeTypeId)
                    .Select(x => new MinimalIncomeResponse
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        Completed = x.Completed,
                        IncomeDate = x.IncomeDate,
                        Title = x.Title,
                        TypeName = _context.IncomeTypes.Where(y => y.Id == x.TypeId).Select(y => y.Name).FirstOrDefault()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("Income:GetAll", ex);
            }
        }

        public async Task<IncomeResponse?> GetAsync(Guid id)
        {
            try
            {
                return await _context
                    .Incomes
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new IncomeResponse
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        Completed = x.Completed,
                        Description = x.Description,
                        IncomeDate = x.IncomeDate,
                        Title = x.Title,
                        TaxRate = _context.IncomeTypes.Where(y=> y.Id == x.TypeId).Select(y=> y.TaxRate).FirstOrDefault(),
                        TypeName = _context.IncomeTypes.Where(y => y.Id == x.TypeId).Select(y => y.Name).FirstOrDefault(),
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("Income:Get", ex);
            }
        }

        public async Task<DetailedIncomeResponse?> GetDetailedAsync(Guid id)
        {
            try
            {
                return await _context
                    .Incomes
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new DetailedIncomeResponse
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        Completed = x.Completed,
                        Description = x.Description,
                        IncomeDate = x.IncomeDate,
                        Title = x.Title,
                        CreatedAt = x.CreatedAt,
                        CreatedBy = x.CreatedBy,
                        DeletedAt = x.DeletedAt,
                        DeletedBy = x.DeletedBy,
                        IsDeleted = x.IsDeleted,
                        LastModifiedAt = x.LastModifiedAt,
                        LastModifiedBy = x.LastModifiedBy,
                        IncomeType = _context
                        .IncomeTypes
                        .Where(y => y.Id == x.TypeId)
                        .Select(y => new IncomeTypeResponse
                        {
                            Id = y.Id,
                            Name = y.Name,
                            TaxRate = y.TaxRate
                        })
                        .FirstOrDefault()
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("Income:GetDetailed", ex);
            }
        }

        public async Task PatchAsync(IncomePatchRequest request)
        {
            try
            {
                var data = await _context
                    .Incomes
                    .FindAsync(request.Id);

                if(data is null)
                    throw new PAContextPatchException("Income:Patch:NotFound");

                data.LastModifiedBy = request.ModifiedBy;
                data.Completed = request.Completed;

                _context.Incomes.Update(data);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextPatchException("Income:Patch", ex);
            }
        }

        public async Task UpdateAsync(IncomeUpdateRequest request)
        {
            try
            {
                var data = await _context
                    .Incomes
                    .FindAsync(request.Id);

                if (data is null)
                    throw new PAContextPatchException("Income:Update:NotFound");

                data.LastModifiedBy = request.ModifiedBy;
                data.Amount = request.Amount;
                data.Description = request.Description;
                data.IncomeDate = request.IncomeDate;

                _context.Incomes.Update(data);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextPatchException("Income:Update", ex);
            }
        }
    }
}
