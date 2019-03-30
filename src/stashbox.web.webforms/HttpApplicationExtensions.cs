using System;
using System.Web;

namespace Stashbox.Web.WebForms
{
    public static class HttpApplicationExtensions
    {
        public static IStashboxContainer AddStashbox(this HttpApplication application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return StashboxAdapter.AddStashbox();
        }

        public static IStashboxContainer GetStashboxContainer(this HttpApplication application)
        {
            return StashboxAdapter.GetContainer();
        }
    }
}