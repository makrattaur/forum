using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum.Controllers
{
    [CustomAuthorize(Roles = "Administrators", NoRedirect = true, SilentError = true)]
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Admin/Users

        public ActionResult Users()
        {
            return View((IEnumerable<Database.User>)db.User);
        }

        //
        // GET: /Admin/Groups

        public ActionResult Groups()
        {
            return View((IEnumerable<Database.Group>)db.Group);
        }

        //
        // GET: /Admin/Group

        public ActionResult Group(int id)
        {
            return View(db.Group.SingleOrDefault(g => g.Id == id));
        }

        //
        // GET: /Admin/EditGroupPermissions

        public ActionResult EditGroupPermissions(int id)
        {
            Models.EditGroupPermissionsModel model = new Models.EditGroupPermissionsModel() { GroupId = id };
            model.Group = db.Group.SingleOrDefault(g => g.Id == id);

            foreach (var flagName in Enum.GetNames(typeof(Database.Permissions)))
            {
                var flagValue = (Database.Permissions)Enum.Parse(typeof(Database.Permissions), flagName);

                if ((flagValue & flagValue - 1) != 0)
                {
                    continue;
                }

                model.Permissions.Add(flagName, ((Database.Permissions)model.Group.Permissions & flagValue) != 0);
            }

            return View(model);
        }

        //
        // POST: /Admin/EditGroupPermissions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroupPermissions(Forum.Models.EditGroupPermissionsModel model)
        {
            model.Group = db.Group.SingleOrDefault(g => g.Id == model.GroupId);

            Database.Permissions newPermissions = 0;

            foreach (var pair in model.Permissions)
            {
                Database.Permissions currentPermission;
                if (Enum.TryParse(pair.Key, out currentPermission))
                    if (pair.Value)
                        newPermissions |= currentPermission;
            }

            Database.Group group = model.Group;
            group.Permissions = (int)newPermissions;
            db.SubmitChanges();

            return RedirectToAction("Group", new { id = group.Id });
        }

        //
        // GET: /Admin/EditGroupName

        public ActionResult EditGroupName(int id)
        {
            Models.EditGroupNameModel model = new Models.EditGroupNameModel() { GroupId = id };
            model.Group = db.Group.SingleOrDefault(g => g.Id == id);

            return View(model);
        }

        //
        // POST: /Admin/EditGroupName

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroupName(Forum.Models.EditGroupNameModel model)
        {
            model.Group = db.Group.SingleOrDefault(g => g.Id == model.GroupId);

            Database.Group group = model.Group;

            // TODO: Invalid when system group
            // TODO: Invalid when empty name

            if (!group.IsSystem)
            {
                group.Name = model.NewName;
                db.SubmitChanges();
            }

            return RedirectToAction("Group", new { id = group.Id });
        }
    }
}
