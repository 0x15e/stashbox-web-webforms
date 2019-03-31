using JetBrains.Annotations;

using System.Web;

namespace Stashbox.Web.WebForms
{
    [UsedImplicitly]
    public class StashboxScope
    {
        private const string ChildContainerKey = "StashboxChildContainer";
        private static HttpContext HttpContext => HttpContext.Current;

        public static IStashboxContainer GetContainer()
        {
            var items = HttpContext?.Items;

            if (items != null && items[ChildContainerKey] is IStashboxContainer container)
            {
                return container;
            }

            return null;
        }

        public static void AddContainer()
        {
            var items = HttpContext?.Items;

            if (items == null || items[ChildContainerKey] is IStashboxContainer)
            {
                return;
            }

            items[ChildContainerKey] = StashboxAdapter.RootContainer?.CreateChildContainer();
        }

        public static void RemoveContainer()
        {
            var items = HttpContext?.Items;

            if (items == null || !(items[ChildContainerKey] is IStashboxContainer container))
            {
                return;
            }

            container.Dispose();

            items.Remove(ChildContainerKey);
        }
    }
}