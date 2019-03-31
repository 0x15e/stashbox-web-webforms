using System;
using System.Web;

using JetBrains.Annotations;

namespace Stashbox.Web.WebForms
{
    public static class StashboxAdapter
    {
        private static readonly object Lock = new object();

        internal static IStashboxContainer RootContainer { get; private set; }

        private static void AttachEvents(HttpApplication application)
        {
            application.BeginRequest += (s, e) => StashboxScope.AddContainer();
            application.EndRequest += (s, e) => StashboxScope.RemoveContainer();
        }

        private static StashboxServiceProvider SetServiceProvider(IStashboxWebFormsConfiguration configuration)
        {
            var serviceProvider = new StashboxServiceProvider(HttpRuntime.WebObjectActivator, configuration);

            HttpRuntime.WebObjectActivator = serviceProvider;

            return serviceProvider;
        }

        [UsedImplicitly]
        public static HttpApplication AddStashbox(
            this HttpApplication application,
            IStashboxWebFormsConfiguration configuration)
        {
            lock (Lock)
            {
                AttachEvents(application);

                configuration.Validate();

                RootContainer = SetServiceProvider(configuration).RootContainer;
            }

            return application;
        }

        [UsedImplicitly]
        public static HttpApplication AddStashbox(
            this HttpApplication application,
            Action<IStashboxWebFormsConfiguration> configurationAction = null)
        {
            lock (Lock)
            {
                AttachEvents(application);

                var configuration = new StashboxWebFormsConfiguration
                {
                    TrackUnresolvableTypes = true,
                    UnresolvableTypeTrackingLimit = 100000,
                    CallNextActivator = true
                };

                configurationAction?.Invoke(configuration);

                configuration.Validate();

                RootContainer = SetServiceProvider(configuration).RootContainer;

                return application;
            }
        }
    }
}