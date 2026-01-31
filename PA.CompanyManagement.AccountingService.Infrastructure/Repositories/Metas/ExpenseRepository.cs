using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PA.CompanyManagement.AccountingService.Application.DTOs.Requests.Metas;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Metas;
using PA.CompanyManagement.AccountingService.Application.Repositories.Metas;
using PA.CompanyManagement.AccountingService.Domain.Entities.Metas;
using PA.CompanyManagement.AccountingService.Infrastructure.Contexts;
using PA.CompanyManagement.Core.Exceptions;
using static PA.CompanyManagement.Core.Utils.ValidationHelper;
using System;
using System.Collections.Generic;
using System.Text;
using PA.CompanyManagement.AccountingService.Domain.Entities.Types;
using PA.CompanyManagement.AccountingService.Application.DTOs.Responses.Types;

namespace PA.CompanyManagement.AccountingService.Infrastructure.Repositories.Metas
{
    public class ExpenseRepository : IExpensRepository
    {
        private readonly AccountingDBContext _context;

        public ExpenseRepository(AccountingDBContext context)
            => _context = context;

        public async Task<ExpenseResponse> CreateAsync(ExpenseCreateRequest request)
        {
            try
            {
                try
                {
                    await _context.Expenses.AddAsync(new Expense
                    {
                        Amount = request.Amount,
                        Completed = request.Completed,
                        TypeId = request.TypeId,
                        ExpenseDate = request.ExpenseDate,
                        Description = request.Description,
                        Title = request.Title,
                        CreatedBy = request.CreatedBy,

                    });
                }
                catch (Exception ex)
                {
                    throw new PAContextAddException("AccountingService:ExpenseRepository:CreateAsync:AddAsync", ex);
                }

                int response = await _context.SaveChangesAsync();
                if (response <= 0)
                    throw new PAContextSaveException("AccountingService:ExpenseRepository:CreateAsync:SaveChangesAsync");

                return new ExpenseResponse
                {
                    Amount = request.Amount,
                    Completed = request.Completed,
                    Description = request.Description,
                    ExpenseDate = request.ExpenseDate,
                    TaxRate = _context.ExpenseTypes.Find(request.TypeId)?.TaxRate ?? 0,
                    Title = request.Title,
                    TypeName = _context.ExpenseTypes.Find(request.TypeId)?.Name ?? string.Empty,
                    Id = _context.Expenses.Where(x =>
                            x.Amount == request.Amount
                            && x.ExpenseDate == request.ExpenseDate
                            && x.TypeId == request.TypeId)
                        .OrderBy(x => x.CreatedAt)
                        .LastOrDefault()?.Id ?? Guid.Empty
                };
            }
            catch (DbUpdateException dbEx) when (IsUniqueViolation(dbEx))
            {
                throw new PAContextSaveException("AccountingService:ExpenseRepository:CreateAsync:SaveChangesAsync", dbEx);
            }
            catch (Exception ex) when (
                ex.GetType() != typeof(PAContextAddException) &&
                ex.GetType() != typeof(PAContextSaveException))
            {
                throw new PAContextUncatchedException("AccountingService:ExpenseRepository:CreateAsync", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                try
                {
                    _context.Expenses.Remove(_context.Expenses.Find(id));
                }
                catch (Exception ex)
                {
                    throw new PAContextRemoveException("AccountingService:ExpenseRepository:DeleteAsync:Remove", ex);
                }

                try
                {
                    int response = await _context.SaveChangesAsync();
                    if (response <= 0)
                        throw new PAContextSaveException("AccountingService:ExpenseRepository:DeleteAsync:SaveChangesAsync");
                }
                catch (Exception ex)
                {
                    throw new PAContextSaveException("AccountingService:ExpenseRepository:DeleteAsync:SaveChangesAsync", ex);
                }
            }
            catch (Exception ex) when (
                ex is not PAContextRemoveException &&
                ex is not PAContextSaveException
            )
            {
                throw new PAContextUncatchedException("AccountingService:ExpenseRepository:DeleteAsync", ex);
            }
        }

        public async Task<List<MinimalExpenseResponse>> GetAllAsync()
        {
            try
            {
                var response = await _context
                    .Expenses
                    .Select(x => new MinimalExpenseResponse
                    {
                        Amount = x.Amount,
                        Completed = x.Completed,
                        ExpenseDate = x.ExpenseDate,
                        Id = x.Id,
                        Title = x.Title,
                        TypeName = x.TypeId.ToString()
                        //TypeName = _context.ExpenseTypes.Find(x.TypeId).Name ?? string.Empty
                        //TypeName = GetTypeName(x.TypeId)
                    })
                    .ToListAsync();

                return GetTypeName(response);
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("AccountingService:ExpenseRepository:GetAllAsync", ex);
            }
        }

        private List<MinimalExpenseResponse> GetTypeName(List<MinimalExpenseResponse> model)
        {
            //if (id == Guid.Empty)
            //    return string.Empty;
            //var item = _context.ExpenseTypes.Find(id);
            //if (item != null)
            //    return item.Name ?? string.Empty;
            //return string.Empty;

            foreach(var item in model)
            {
                item.TypeName = _context.ExpenseTypes.Find(Guid.Parse(item.TypeName))?.Name ?? "";
                
            }
            return model;
        }

        public async Task<List<MinimalExpenseResponse>> GetAllAsync(Guid expenseTypeId)
        {
            try
            {
                string typeName = _context.ExpenseTypes.Find(expenseTypeId).Name ?? string.Empty;
                return await _context
                    .Expenses
                    .Where(x => x.TypeId == expenseTypeId)
                    .Select(x => new MinimalExpenseResponse
                    {
                        Amount = x.Amount,
                        Completed = x.Completed,
                        ExpenseDate = x.ExpenseDate,
                        Id = x.Id,
                        Title = x.Title,
                        TypeName = typeName
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("AccountingService:ExpenseRepository:GetAllAsync", ex);
            }
        }

        public async Task<ExpenseResponse?> GetAsync(Guid id)
        {
            try
            {
                var response = await _context
                    .Expenses
                    .Where(x => x.Id == id)
                    .Select(x => new ExpenseResponse
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        Completed = x.Completed,
                        Description = x.Description,
                        ExpenseDate = x.ExpenseDate,
                        Title = x.Title,
                        TypeName = x.TypeId.ToString()
                    })
                    .FirstOrDefaultAsync();
                var type = _context.ExpenseTypes.Find(Guid.Parse(response.TypeName));
                response.TypeName = type?.Name ?? "";
                response.TaxRate = type?.TaxRate ?? null;

                return response;
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("AccountingService:ExpenseRepository:GetAsync", ex);
            }
        }

        public async Task<DetailedExpenseResponse?> GetDetailedAsync(Guid id)
        {
            try
            {
                return await _context
                    .Expenses
                    .Where(x => x.Id == id)
                    .Select(x => new DetailedExpenseResponse
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        Completed = x.Completed,
                        Description = x.Description,
                        ExpenseDate = x.ExpenseDate,
                        Title = x.Title,
                        CreatedAt = x.CreatedAt,
                        CreatedBy = x.CreatedBy,
                        DeletedAt = x.DeletedAt,
                        DeletedBy = x.DeletedBy,
                        IsDeleted = x.IsDeleted,
                        LastModifiedAt = x.LastModifiedAt,
                        LastModifiedBy = x.LastModifiedBy,
                        TypeId = x.TypeId,
                        ExpenseType = _context
                            .ExpenseTypes
                            .Where(y => y.Id == x.TypeId)
                            .Select(y => new ExpenseTypeResponse
                            {
                                Id = y.Id,
                                Name = y.Name ?? string.Empty,
                                TaxRate = y.TaxRate ?? 20
                            })
                            .FirstOrDefault()
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException("AccountingService:ExpenseRepository:GetAsync", ex);
            }
        }

        public async Task PatchAsync(ExpensePatchRequest request)
        {
            try
            {
                Expense? item = await _context.Expenses.FindAsync(request.Id);

                if (item is null)
                    throw new PAContextPatchException("AccountingService:ExpenseRepository:PatchAsync:NotFound");

                item.LastModifiedBy = request.ModifiedBy;
                item.Completed = request.Completed;

                try
                {
                    _context.Expenses.Update(item);
                }
                catch (Exception ex)
                {
                    throw new PAContextPatchException("AccountingService:ExpenseRepository:PasthAsync:Update", ex);
                }

                int response = await _context.SaveChangesAsync();
                if (response <= 0)
                    throw new PAContextSaveException("AccountingService:ExpenseRepository:SaveChangesAsync");



            }
            catch (DbUpdateException dbEx) when (IsUniqueViolation(dbEx))
            {
                throw new PAContextSaveException("AccountingService:ExpenseRepository:PatchAsync:SaveChangesAsync", dbEx);
            }
            catch (Exception ex) when (
                ex.GetType() != typeof(PAContextPatchException) &&
                ex.GetType() != typeof(PAContextSaveException))
            {
                throw new PAContextUncatchedException("AccountingService:ExpenseRepository:PatchAsync", ex);
            }
        }

        public async Task UpdateAsync(ExpenseUpdateRequest request)
        {
            try
            {
                Expense? item = await _context.Expenses.FindAsync(request.Id);

                if (item is null)
                    throw new PAContextUpdateException("AccountingService:ExpenseRepository:UpdateAsync:NotFound");

                item.LastModifiedBy = request.ModifiedBy;
                item.Amount = request.Amount;
                item.Description = request.Description;
                item.ExpenseDate = request.ExpenseDate;

                try
                {
                    _context.Expenses.Update(item);
                }
                catch (Exception ex)
                {
                    throw new PAContextUpdateException("AccountingService:ExpenseRepository:UpdateAsync:Update", ex);
                }

                int response = await _context.SaveChangesAsync();
                if (response <= 0)
                    throw new PAContextSaveException("AccountingService:ExpenseRepository:UpdateAsync:SaveChangesAsync");



            }
            catch (DbUpdateException dbEx) when (IsUniqueViolation(dbEx))
            {
                throw new PAContextSaveException("AccountingService:ExpenseRepository:PatchAsync:SaveChangesAsync", dbEx);
            }
            catch (Exception ex) when (
                ex.GetType() != typeof(PAContextPatchException) &&
                ex.GetType() != typeof(PAContextSaveException))
            {
                throw new PAContextUncatchedException("AccountingService:ExpenseRepository:PatchAsync", ex);
            }
        }
    }
}
