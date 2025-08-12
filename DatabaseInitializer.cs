using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace users_api
{
    public static class DatabaseInitializer
    {
        public static void Initialize(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("DatabaseInitializer");

            try
            {
                var context = services.GetRequiredService<UserContext>();
                ApplyMigrations(context, logger);
                TestConnection(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database initialization failed");
                throw;
            }
        }

        private static void ApplyMigrations(UserContext context, ILogger logger)
        {
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();

            if (pendingMigrations.Any())
            {
                logger.LogInformation("Applying {Count} migrations...", pendingMigrations.Count);
                logger.LogDebug("Pending migrations: {Migrations}", string.Join(", ", pendingMigrations));

                context.Database.Migrate();
                logger.LogInformation("Migrations applied successfully");
            }
            else
            {
                logger.LogInformation("Database is up-to-date, no migrations to apply");
            }
        }

        private static void TestConnection(UserContext context, ILogger logger)
        {
            logger.LogInformation("Testing database connection...");

            if (context.Database.CanConnect())
            {
                logger.LogInformation("Database connection successful");
            }
            else
            {
                logger.LogCritical("Database connection failed");
                throw new InvalidOperationException("Could not connect to database");
            }
        }
    }
}
