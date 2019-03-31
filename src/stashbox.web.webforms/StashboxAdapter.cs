using System;
using System.Web;

using JetBrains.Annotations;

namespace Stashbox.Web.WebForms
{
    public static class StashboxAdapter
    {
        private static readonly object Lock = new object();

        internal static IStashboxContainer RootContainer { get; private set; }

        #region [ Helpers ]
        private static void AttachEvents(HttpApplication application)
        {
            application.BeginRequest += (s, e) => StashboxScope.AddContainer();
            application.EndRequest += (s, e) => StashboxScope.RemoveContainer();
        }

        private static IStashboxWebFormsConfiguration GetDefaultConfiguration() => new StashboxWebFormsConfiguration
        {
            TrackUnresolvableTypes = true,
            UnresolvableTypeTrackingLimit = 100000,
            CallNextActivator = true
        };

        private static StashboxServiceProvider SetServiceProvider(IStashboxWebFormsConfiguration configuration)
        {
            var serviceProvider = new StashboxServiceProvider(HttpRuntime.WebObjectActivator, configuration);

            HttpRuntime.WebObjectActivator = serviceProvider;

            return serviceProvider;
        }
        #endregion

        [UsedImplicitly]
        public static HttpApplication AddStashbox(
            this HttpApplication application)
        {
            lock (Lock)
            {
                AttachEvents(application);

                var configuration = GetDefaultConfiguration();

                RootContainer = SetServiceProvider(configuration).RootContainer;

                return application;
            }
        }

        [UsedImplicitly]
        public static HttpApplication AddStashbox(
            this HttpApplication application,
            [NotNull] IStashboxWebFormsConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            lock (Lock)
            {
                AttachEvents(application);

                configuration.Validate();

                RootContainer = SetServiceProvider(configuration).RootContainer;
                
                return application;
            }
        }

        [UsedImplicitly]
        public static HttpApplication AddStashbox(
            this HttpApplication application,
            [NotNull] Action<IStashboxWebFormsConfiguration> configurationAction)
        {
            if (configurationAction == null)
            {
                throw new ArgumentNullException(nameof(configurationAction));
            }

            lock (Lock)
            {
                AttachEvents(application);

                var configuration = GetDefaultConfiguration();

                configurationAction?.Invoke(configuration);

                configuration.Validate();

                RootContainer = SetServiceProvider(configuration).RootContainer;

                return application;
            }
        }
    }
}