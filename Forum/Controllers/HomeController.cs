using System;
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
            var model = new TestModel();
            model.foo = 42;
            model.bar = "foobar";

            return TypedView(model);
        }

    }
}
