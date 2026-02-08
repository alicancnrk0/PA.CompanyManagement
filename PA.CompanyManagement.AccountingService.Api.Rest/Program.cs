using PA.CompanyManagement.AccountingService.Application.Repositories.Metas;
using PA.CompanyManagement.AccountingService.Application.Repositories.Types;
using PA.CompanyManagement.AccountingService.Infrastructure.Contexts;
using PA.CompanyManagement.AccountingService.Infrastructure.Repositories.Metas;
using PA.CompanyManagement.AccountingService.Infrastructure.Repositories.Types;
using static PA.CompanyManagement.AccountingService.Infrastructure.DependencyInjection;

namespace PA.CompanyManagement.AccountingService.Api.Rest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddAccountingContext(builder.Configuration);

            builder.Services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
            builder.Services.AddScoped<IIncomeTypeRepository, IncomeTypeRepository>();

            builder.Services.AddScoped<IExpensRepository, ExpenseRepository>();
            builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
