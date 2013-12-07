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

        public ActionResult Index()
        {
            return View(new ViewModels.ForumViewModel() {
                Categories = db.Category,
                PermissionManager = new UserPermissionManager(db, db.User.SingleOrDefault(u => u.Name == User.Identity.Name))
            });
        }

    }
}
