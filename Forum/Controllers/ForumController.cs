using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Database;
using Forum.ViewModels;

namespace Forum.Controllers
{
    public class ForumController : BaseController
    {
        //
        // GET: /Forum/

        public ActionResult Index(int? id, int? page)
        {
            if (id == null)
                return ForumError("No forum specified.");

            var forum = ForumDatabase.Forum.SingleOrDefault(f => f.Id == id);
            if (forum == null)
                return ForumError("Invalid forum specified.");

            SetCurrentLocation(forum);

            return View(new ForumViewModel()
            {
                Forum = forum,
                Page = page.HasValue ? page.Value : 1,
                PermissionManager = PermissionManager
            });
        }

        //
        // GET: /Forum/Category

        public ActionResult Category(int? id)
        {
            if (id == null)
                return ForumError("No category specified.");

            var category = ForumDatabase.Category.Single(c => c.Id == id);
            if (id == null)
                return ForumError("Invalid category specified.");

            SetCurrentLocation(category);

            return View(new ViewModels.CategoryViewModel()
            {
                Category = category,
                PermissionManager = PermissionManager,
                IsSingle = true
            });
        }

    }
}
