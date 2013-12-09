using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Forum.ViewModels;
using Database;

namespace Forum.Controllers
{
    public class BaseController : Controller
    {
        protected ForumDataContext ForumDatabase = new ForumDataContext();

        public static void InitLayoutViewModel(WebViewPage page, ForumDataContext db, LayoutViewModel lvm)
        {
            lvm.Username = "foobar";

            if (page.Context.Request.IsAuthenticated)
                lvm.CurrentUser = db.User.SingleOrDefault(u => u.Name == page.Context.User.Identity.Name);

            lvm.CurrentController = page.Context.Request.RequestContext.RouteData.GetRequiredString("controller");
            lvm.CurrentAction = page.Context.Request.RequestContext.RouteData.GetRequiredString("action");
        }
    }
}
