using Kassabok.Functions.Interface;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Kassabok.Test
{
    public class TestStartup: ICollectionFixture<ServiceProviderFixture>
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            //Register dependencies
            services.AddTransient<IFunctions, Kassabok.Functions.Functions>();

            return services.BuildServiceProvider();

        }
    }


    public class ServiceProviderFixture
    {
        public ServiceProviderFixture()
        {
            ServiceProvider = TestStartup.ConfigureServices();
        }

        public IServiceProvider ServiceProvider { get; private set; }
    }
}
