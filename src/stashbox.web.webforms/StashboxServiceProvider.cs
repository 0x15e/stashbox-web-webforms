using Stashbox.Exceptions;

using System;
using System.Collections.Concurrent;
using System.Web.Hosting;

namespace Stashbox.Web.WebForms
{
    internal class StashboxServiceProvider : IServiceProvider, IRegisteredObject
    {
        private readonly IStashboxWebFormsConfiguration _configuration;

        private bool TrackUnresolvableTypes => _configuration.TrackUnresolvableTypes;
        private int MaxUnresolvableTypes => _configuration.UnresolvableTypeTrackingLimit;

        private readonly IServiceProvider _next;
        private readonly ConcurrentDictionary<Type, bool> _unresolvableTypes = new ConcurrentDictionary<Type, bool>();

        public IStashboxContainer RootContainer { get; internal set; } = new StashboxContainer();

        public StashboxServiceProvider(IServiceProvider next, IStashboxWebFormsConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _next = configuration.CallNextActivator ? null : next;

            HostingEnvironment.RegisterObject(this);
        }

        public object GetService(Type serviceType)
        {
            if (!ShouldResolveInstance(serviceType))
            {
                return SystemActivator.CreateInstance(serviceType);
            }

            object result = null;
            var container = StashboxScope.GetContainer() ?? RootContainer;

            try
            {
                result = container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                // Nothing. Eat it and continue.
            }

            if (result == null)
            {
                result = _next?.GetService(serviceType);
            }

            if (result != null)
            {
                return result;
            }

            if ((result = SystemActivator.CreateInstance(serviceType)) != null
                && TrackUnresolvableTypes
                && _unresolvableTypes.Count < MaxUnresolvableTypes)
            {
                _unresolvableTypes.TryAdd(serviceType, true);
            }

            return result;
        }

        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);

            RootContainer?.Dispose();
        }

        private bool ShouldResolveInstance(Type serviceType)
        {
            // Not simplifying this function yet because there may be more conditions

            if (TrackUnresolvableTypes && IsUnresolvable(serviceType))
            {
                return false;
            }

            return true;
        }

        private bool IsUnresolvable(Type serviceType) => _unresolvableTypes.ContainsKey(serviceType);
    }
}