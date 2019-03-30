using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace Stashbox.Web.WebForms
{
    public static class StashboxAdapter
    {
        private static readonly object Lock = new object();

        public static IStashboxContainer AddStashbox()
        {
            lock (Lock)
            {
                HttpRuntime.WebObjectActivator = new ContainerServiceProvider(HttpRuntime.WebObjectActivator);

                return GetContainer();
            }
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static IStashboxContainer GetContainer()
        {
            return (HttpRuntime.WebObjectActivator as ContainerServiceProvider)?.Container;
        }
    }
}