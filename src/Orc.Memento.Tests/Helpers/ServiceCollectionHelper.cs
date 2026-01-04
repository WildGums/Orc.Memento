namespace Orc.Memento.Tests
{
    using Catel;
    using Microsoft.Extensions.DependencyInjection;
    using Orc.Memento;

    internal static class ServiceCollectionHelper
    {
        public static IServiceCollection CreateServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging();
            serviceCollection.AddCatelCore();
            serviceCollection.AddOrcMemento();

            return serviceCollection;
        }
    }
}
