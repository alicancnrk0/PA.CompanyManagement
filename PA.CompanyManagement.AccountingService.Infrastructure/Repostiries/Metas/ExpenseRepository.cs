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

namespace PA.CompanyManagement.AccountingService.Infrastructure.Repostiries.Metas
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AccountingDbContext _context;

        public ExpenseRepository(AccountingDbContext context) => _context = context;

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
                catch(Exception ex)
                {
                    throw new PAContextAddException("AccountingService:ExpenseRepository:CreateAsync:AddAsync", ex);
                }


                int response = await _context.SaveChangesAsync();
                if(response <= 0)
                    throw new PAContextSaveException("AccountingService:ExpenseRepository:CreateAsync:SaveChangesAsync");
                // Kayıt Sırasında hata oluşma ihtimali 
                // Kayıdın Başarısız olması ihtimali


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
            catch (Exception ex) when
            (
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
                }catch(Exception ex)
                {
                    throw new PAContextRemoveException("AccountingService:ExpenseRepository:DeleteAsync:Remove", ex);
                }

                try
                {
                    int response = await _context.SaveChangesAsync();
                    if(response <= 0)
                        throw new PAContextSaveException("AccountingService:ExpenseRepository:DeleteAsync:SaveChangesAsync");
                }
                catch(Exception ex)
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

        public Task<List<MinimalExpenseResponse>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<MinimalExpenseResponse>> GetAllAsync(Guid expenseTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<ExpenseResponse> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DetailedExpenseResponse> GetDetailedAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task PatchAsync(ExpensePatchRequest request)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ExpenseUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
