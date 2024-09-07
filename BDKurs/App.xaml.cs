using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BDKurs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Статическое свойство для доступа к провайдеру сервисов
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();

            // Добавление контекста базы данных
            serviceCollection.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(ConfigurationManager.ConnectionStrings["LibraryDbConnection"].ConnectionString));

            // Добавление других сервисов
            // ...

            // Создание провайдера сервисов из конфигурации сервисов
            ServiceProvider = serviceCollection.BuildServiceProvider();

            base.OnStartup(e);
        }
    }
}
