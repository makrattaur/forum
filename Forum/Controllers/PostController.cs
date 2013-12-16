using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum.Controllers
{
    public class PostController : BaseController
    {
        //
        // GET: /Post/Delete

        public ActionResult Delete()
        {
            return View();
        }

        //
        // GET: /Post/Edit

        public ActionResult Edit()
        {
            return View();
        }
    }
}
