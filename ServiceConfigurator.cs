using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using users_api.DBRepository;

namespace users_api
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Загрузка переменных окружения
            Env.Load();

            // Конфигурация базы данных
            ConfigureDatabase(builder);

            // Конфигурация контроллеров
            ConfigureControllers(builder);

            // Регистрация репозиториев
            ConfigureRepositories(builder);

            // Конфигурация версионирования API
            ConfigureApiVersioning(builder);

            // Конфигурация Swagger
            ConfigureSwagger(builder);
        }

        private static void ConfigureDatabase(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<UserContext>(options =>
                options.UseNpgsql(
                    Env.GetString("POSTGRES_CONNECTION_STRING")));
        }

        private static void ConfigureControllers(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
        }

        private static void ConfigureRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<INoteRepository, NoteRepository>();
        }

        private static void ConfigureApiVersioning(WebApplicationBuilder builder)
        {
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}
