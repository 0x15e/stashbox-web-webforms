using Stashbox.Exceptions;

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Hosting;

namespace Stashbox.Web.WebForms
{
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    internal class ContainerServiceProvider : IServiceProvider, IRegisteredObject
    {
        private const int MaxUnresolvableTypes = 100000;
        private readonly IServiceProvider _next;
        private readonly ConcurrentDictionary<Type, bool> _unresolvableTypes = new ConcurrentDictionary<Type, bool>();

        public IStashboxContainer Container { get; internal set; } = new StashboxContainer();

        public ContainerServiceProvider(IServiceProvider next)
        {
            _next = next;
            HostingEnvironment.RegisterObject(this);
        }

        public object GetService(Type serviceType)
        {
            if (_unresolvableTypes.ContainsKey(serviceType))
            {
                return DefaultCreateInstance(serviceType);
            }

            object result = null;

            try
            {
                result = Container.Resolve(serviceType);
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

            if ((result = DefaultCreateInstance(serviceType)) != null)
            {
                if (_unresolvableTypes.Count < MaxUnresolvableTypes)
                {
                    _unresolvableTypes.TryAdd(serviceType, true);
                }
            }

            return result;
        }

        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);

            Container?.Dispose();
        }

        protected virtual object DefaultCreateInstance(Type type)
        {
            return Activator.CreateInstance(
                type,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance,
                null, null, null);
        }
    }
}