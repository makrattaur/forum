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

        public static void InitLayoutViewModel(HttpContextBase context, ForumDataContext db, LayoutViewModel lvm)
        {
            lvm.Username = "foobar";

            if (context.Request.IsAuthenticated)
                lvm.CurrentUser = db.User.SingleOrDefault(u => u.Name == context.User.Identity.Name);

            lvm.CurrentController = context.Request.RequestContext.RouteData.GetRequiredString("controller");
            lvm.CurrentAction = context.Request.RequestContext.RouteData.GetRequiredString("action");
        }
    }
}
