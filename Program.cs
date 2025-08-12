namespace users_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Конфигурация сервисов
            ServiceConfigurator.ConfigureServices(builder);

            var app = builder.Build();

            // Конфигурация приложения
            AppConfigurator.ConfigureApplication(app);

            app.Run();
        }
    }
}