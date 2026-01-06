namespace Orc
{
    using Catel.Services;
    using Catel.ThirdPartyNotices;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Orc.Memento;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcMementoModule
    {
        public static IServiceCollection AddOrcMemento(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IMementoService, MementoService>();

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Memento", "Orc.Memento.Properties", "Resources"));

            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.Memento", "https://github.com/wildgums/orc.memento"));

            return serviceCollection;
        }
    }
}
