﻿using System;
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
        // GET: /Forum/Category

        public ActionResult Category(int id)
        {
            return View(new ViewModels.CategoryViewModel()
            {
                Category = db.Category.Single(c => c.Id == id),
                PermissionManager = new UserPermissionManager(db, db.User.SingleOrDefault(u => u.Name == User.Identity.Name))
            });
        }

    }
}
