using JetBrains.Annotations;

using System.Web;

namespace Stashbox.Web.WebForms
{
    [UsedImplicitly]
    public static class HttpContextExtensions
    {
        [UsedImplicitly]
        public static IStashboxContainer GetStashboxContainer(this HttpContext httpContext) =>
            StashboxScope.GetContainer();
    }
}