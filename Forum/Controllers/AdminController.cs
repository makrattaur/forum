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
            return TypedView();
        }

        //
        // GET: /Admin/Users

        public ActionResult Users()
        {
            return TypedView<IEnumerable<Database.User>>(db.User);
        }

        //
        // GET: /Admin/Groups

        public ActionResult Groups()
        {
            return TypedView<IEnumerable<Database.Group>>(db.Group);
        }

        //
        // GET: /Admin/Group

        public ActionResult Group(int id)
        {
            return TypedView(db.Group.SingleOrDefault(g => g.Id == id));
        }

        //
        // GET: /Admin/EditGroup

        public ActionResult EditGroup(int id)
        {
            Models.EditGroupModel model = new Models.EditGroupModel() { GroupId = id };
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

            return TypedView(model);
        }

        //
        // POST: /Admin/EditGroupPermissions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroupPermissions(Forum.ViewModels.ViewModelWrapper<Forum.Models.EditGroupModel> model)
        {
            model.InnerModel.Group = db.Group.SingleOrDefault(g => g.Id == model.InnerModel.GroupId);

            Database.Permissions newPermissions = 0;

            foreach (var pair in model.InnerModel.Permissions)
            {
                Database.Permissions currentPermission;
                if (Enum.TryParse(pair.Key, out currentPermission))
                    if (pair.Value)
                        newPermissions |= currentPermission;
            }

            Database.Group group = model.InnerModel.Group;
            group.Permissions = (int)newPermissions;
            db.SubmitChanges();

            return RedirectToAction("Group", new { id = group.Id });
        }

        //
        // POST: /Admin/EditGroupName

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroupName(Forum.ViewModels.ViewModelWrapper<Forum.Models.EditGroupModel> model)
        {
            model.InnerModel.Group = db.Group.SingleOrDefault(g => g.Id == model.InnerModel.GroupId);

            Database.Group group = model.InnerModel.Group;

            // TODO: Invalid when system group
            // TODO: Invalid when empty name

            if (!group.IsSystem)
            {
                group.Name = model.InnerModel.NewName;
                db.SubmitChanges();
            }

            return RedirectToAction("Group", new { id = group.Id });
        }
    }
}
