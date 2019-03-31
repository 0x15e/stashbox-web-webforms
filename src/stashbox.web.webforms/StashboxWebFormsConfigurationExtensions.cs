using System;

namespace Stashbox.Web.WebForms
{
    public static class StashboxWebFormsConfigurationExtensions
    {
        internal static void Validate(this IStashboxWebFormsConfiguration config)
        {
            if (config.UnresolvableTypeTrackingLimit < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(StashboxWebFormsConfiguration.UnresolvableTypeTrackingLimit));
            }
        }

        public static TConfiguration WithUnresolvableTypeTrackingLimit<TConfiguration>(
            this TConfiguration config,
            int limit) where TConfiguration : IStashboxWebFormsConfiguration
        {
            if (config != null)
            {
                config.UnresolvableTypeTrackingLimit = limit;
            }

            return config;
        }

        public static TConfiguration WithUnresolvableTypeTrackingDisabled<TConfiguration>(
            this TConfiguration config) where TConfiguration : IStashboxWebFormsConfiguration
        {
            if (config != null)
            {
                config.TrackUnresolvableTypes = false;
            }

            return config;
        }

        public static TConfiguration WithNextActivatorDisabled<TConfiguration>(
            this TConfiguration config) where TConfiguration : IStashboxWebFormsConfiguration
        {
            if (config != null)
            {
                config.CallNextActivator = false;
            }

            return config;
        }
    }
}