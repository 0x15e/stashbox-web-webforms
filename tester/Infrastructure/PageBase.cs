using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using testsite.Infrastructure.Services;

namespace testsite.Infrastructure
{
    public abstract class PageBase : Page
    {
        public IFooService Foo { get; set; }
        public IBarService Bar { get; set; }
    }
}