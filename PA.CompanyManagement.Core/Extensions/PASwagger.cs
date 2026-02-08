using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.OpenApi;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Extensions
{
    public class PASwaggerOptions
    {
        public string Title { get; set; } = "PA API";
        public string Version { get; set; } = "v1";

        public bool EnableSwaggerGen { get; set; } = true;
        public bool EnableMicrosoftOpenApi { get; set; } = true;

        public string SwaggerDocName { get; set; } = "v1";
        public string SwaggerUiName { get; set; } = "PA API";
    }

    public static class PASwagger
    {
        public static IServiceCollection AddPASwagger(this IServiceCollection services, Action<PASwaggerOptions>? configure = null)
        {
            var options = new PASwaggerOptions();
            configure?.Invoke(options);

            if (options.EnableMicrosoftOpenApi)
            {
                services.AddOpenApi(opt =>
                {
                    opt.AddDocumentTransformer((document, context, _) =>
                    {
                        document.Info = new Microsoft.OpenApi.OpenApiInfo
                        {
                            Title = options.Title,
                            Version = options.Version
                        };
                        return Task.CompletedTask;
                    });
                });
            }

            if (options.EnableSwaggerGen)
            {
                services.AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc(options.SwaggerDocName, new()
                    {
                        Title = options.Title,
                        Version = options.Version
                    });
                });
            }

            return services;
        }

        public static WebApplication UsePASwagger(this WebApplication app, PASwaggerOptions options)
        {
            if (options.EnableMicrosoftOpenApi)
            {
                app.MapOpenApi();
            }

            if (options.EnableSwaggerGen)
            {
                app.UseSwagger();
                app.UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint($"/swagger/{options.SwaggerDocName}/swagger.json", options.SwaggerUiName);

                    if (options.EnableMicrosoftOpenApi)
                    {
                        opt.SwaggerEndpoint($"/openapi/v1.json", "Microsoft OpenAPI");
                    }
                });
            }

            return app;
        }
    }
}
