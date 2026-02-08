
using PA.CompanyManagement.Core.Extensions;
using PA.CompanyManagement.EmployeeService.Infrastructure;

namespace PA.CompanyManagement.EmployeeService.Api.Rest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        

            builder.Services.AddEmployeeContext(builder.Configuration);

            builder.Services.AddPASwagger(options =>
            {
                options.Title = "Employee API";
            });

            var app = builder.Build();

            var swopt = new PASwaggerOptions();
            app.UsePASwagger(swopt);

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
