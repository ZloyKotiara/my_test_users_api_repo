using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace users_api
{
    public static class AppConfigurator
    {
        public static void ConfigureApplication(WebApplication app)
        {
            // Инициализация базы данных
            DatabaseInitializer.Initialize(app);

            // Конфигурация Swagger
            ConfigureSwagger(app);

            // Конфигурация middleware
            ConfigureMiddleware(app);
        }

        private static void ConfigureSwagger(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
            }
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}