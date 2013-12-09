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

        public ActionResult Index(int id)
        {
            var forum = ForumDatabase.Forum.SingleOrDefault(f => f.Id == id);
            return View(new ThreadViewModel(forum, forum.Thread.ToList()));
        }

        //
        // GET: /Forum/Category

        public ActionResult Category(int id)
        {
            return View(new ViewModels.CategoryViewModel()
            {
                Category = ForumDatabase.Category.Single(c => c.Id == id),
                PermissionManager = new UserPermissionManager(ForumDatabase, ForumDatabase.User.SingleOrDefault(u => u.Name == User.Identity.Name)),
                IsSingle = true
            });
        }

    }
}
