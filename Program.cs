namespace users_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ������������ ��������
            ServiceConfigurator.ConfigureServices(builder);

            var app = builder.Build();

            // ������������ ����������
            AppConfigurator.ConfigureApplication(app);

            app.Run();
        }
    }
}