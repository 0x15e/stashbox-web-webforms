using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Stashbox.Web.WebForms;

namespace testsite
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            this.AddStashbox();

            var container = this.GetStashboxRootContainer();

            container.Configure(
                config => config
                    .WithCircularDependencyTracking()
                    .WithUnknownTypeResolution()
                    .WithMemberInjectionWithoutAnnotation());
        }
    }
}