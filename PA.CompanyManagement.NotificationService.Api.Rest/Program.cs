
using PA.CompanyManagement.Core.Extensions;
using PA.CompanyManagement.NotificationService.Infrastructure;

namespace PA.CompanyManagement.NotificationService.Api.Rest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddNotificationContext(builder.Configuration);
            
            builder.Services.AddPASwagger(options =>
            {
                options.Title = "Notification Service";
            });

            var app = builder.Build();

            app.UsePASwagger(new PASwaggerOptions());

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
