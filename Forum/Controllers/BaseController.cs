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
        protected User CurrentForumUser;
        protected UserPermissionManager PermissionManager;

        public static void InitLayoutViewModel(WebViewPage page, ForumDataContext db, LayoutViewModel lvm)
        {
            lvm.Username = "foobar";

            if (page.Context.Request.IsAuthenticated)
                lvm.CurrentUser = db.User.SingleOrDefault(u => u.Name == page.Context.User.Identity.Name);

            lvm.CurrentController = page.Context.Request.RequestContext.RouteData.GetRequiredString("controller");
            lvm.CurrentAction = page.Context.Request.RequestContext.RouteData.GetRequiredString("action");

            lvm.CurrentCategory = (string)page.ViewData["CurrentCategory"];
            if (lvm.CurrentCategory != null)
            {
                lvm.CurrentCategoryId = (int)page.ViewData["CurrentCategoryId"];
            }

            lvm.CurrentForum = (string)page.ViewData["CurrentForum"];
            if (lvm.CurrentForum != null)
            {
                lvm.CurrentForumId = (int)page.ViewData["CurrentForumId"];
            }
        }

        protected void SetCurrentLocation(Category category, Database.Forum forum)
        {
            ViewData["CurrentCategory"] = category.Name;
            ViewData["CurrentCategoryId"] = category.Id;

            if (forum != null)
            {
                ViewData["CurrentForum"] = forum.Name;
                ViewData["CurrentForumId"] = forum.Id;
            }
        }

        protected void SetCurrentLocation(Category category)
        {
            SetCurrentLocation(category, null);
        }

        protected void SetCurrentLocation(Database.Forum forum)
        {
            SetCurrentLocation(forum.Category, forum);
        }

        protected void SetCurrentLocation(Thread thread)
        {
            SetCurrentLocation(thread.Forum);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            CurrentForumUser = ForumDatabase.User.SingleOrDefault(u => u.Name == User.Identity.Name);
            PermissionManager = new UserPermissionManager(ForumDatabase, CurrentForumUser);
        }

        protected ActionResult ForumError(string message)
        {
            return View("ForumError", new ViewModels.ForumErrorViewModel()
            {
                Message = message
            });
        }
    }
}
