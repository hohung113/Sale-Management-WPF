using AutoMapper;
using DataAccess.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            // Config DI
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IMemberRepository), typeof(MemberRepository));
            services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddScoped(typeof(IOrderDetailRepository), typeof(OrderDetailRepository));
            services.AddAutoMapper(typeof(AutoMapping));

            services.AddSingleton<LoginWindow>();
            services.AddSingleton<HomeWindow>();
            services.AddSingleton<MemberProfileWindow>();
            services.AddSingleton<EnterNumberItemWindow>();
            services.AddSingleton<ListOrdersWindow>();
            services.AddSingleton<ViewStaticAdminWindow>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var loginWindow = _serviceProvider.GetService<LoginWindow>();
            if (loginWindow != null)
            {
                loginWindow.Show();
            }
            else
            {
                throw new ArgumentNullException(nameof(loginWindow));
            }
        }
    }

}
