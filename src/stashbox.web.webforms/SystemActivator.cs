using System;
using System.Reflection;

namespace Stashbox.Web.WebForms
{
    internal static class SystemActivator
    {
        private const BindingFlags SystemActivatorBindingFlags =
            BindingFlags.Instance
            | BindingFlags.NonPublic
            | BindingFlags.Public
            | BindingFlags.CreateInstance;

        public static object CreateInstance(Type type) =>
            Activator.CreateInstance(
                type,
                SystemActivatorBindingFlags,
                null,
                null,
                null);
    }
}