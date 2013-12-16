﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Forum.Models;
using Forum.ViewModels;

namespace Forum.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(new ViewModels.BoardIndexViewModel()
            {
                Categories = ForumDatabase.Category,
                PermissionManager = new UserPermissionManager(ForumDatabase, CurrentForumUser)
            });
        }


    }
}
