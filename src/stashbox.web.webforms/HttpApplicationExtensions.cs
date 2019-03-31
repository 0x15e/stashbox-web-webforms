using System.Web;

using JetBrains.Annotations;

namespace Stashbox.Web.WebForms
{
    [UsedImplicitly]
    public static class HttpApplicationExtensions
    {
        [UsedImplicitly]
        public static IStashboxContainer GetStashboxRootContainer(this HttpApplication application) =>
            StashboxAdapter.RootContainer;
    }
}