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
            return View((IEnumerable<Database.User>)ForumDatabase.User);
        }

        //
        // GET: /Admin/Groups

        public ActionResult Groups()
        {
            return View((IEnumerable<Database.Group>)ForumDatabase.Group);
        }

        //
        // GET: /Admin/Group

        public ActionResult Group(int id)
        {
            return View(ForumDatabase.Group.SingleOrDefault(g => g.Id == id));
        }

        //
        // GET: /Admin/EditGroupPermissions

        public ActionResult EditGroupPermissions(int id)
        {
            Models.EditGroupPermissionsModel model = new Models.EditGroupPermissionsModel() { GroupId = id };
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == id);

            WritePermissionsToDictionary((Database.Permissions)model.Group.Permissions, model.Permissions);

            return View(model);
        }

        //
        // POST: /Admin/EditGroupPermissions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroupPermissions(Forum.Models.EditGroupPermissionsModel model)
        {
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == model.GroupId);

            Database.Permissions newPermissions = GetPermissionsFromDictionary(model.Permissions);

            Database.Group group = model.Group;
            group.Permissions = (int)newPermissions;
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Group", new { id = group.Id });
        }

        //
        // GET: /Admin/EditGroupName

        public ActionResult EditGroupName(int id)
        {
            Models.EditGroupNameModel model = new Models.EditGroupNameModel() { GroupId = id };
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == id);

            return View(model);
        }

        //
        // POST: /Admin/EditGroupName

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroupName(Forum.Models.EditGroupNameModel model)
        {
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == model.GroupId);

            Database.Group group = model.Group;

            // TODO: Invalid when system group
            // TODO: Invalid when empty name

            if (!group.IsSystem)
            {
                group.Name = model.NewName;
                ForumDatabase.SubmitChanges();
            }

            return RedirectToAction("Group", new { id = group.Id });
        }

        //
        // GET: /Admin/CreateGroup

        public ActionResult CreateGroup()
        {
            return View();
        }

        //
        // POST: /Admin/CreateGroup

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGroup(Database.Group group)
        {
            group.IsSystem = false;
            ForumDatabase.Group.InsertOnSubmit(group);
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Group", new { id = group.Id });
        }

        //
        // GET: /Admin/AddGroupMember

        public ActionResult AddGroupMember(int id)
        {
            var model = new Models.AddGroupMemberModel();

            model.GroupId = id;
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == model.GroupId);
            model.Users = ForumDatabase.User;

            return View(model);
        }

        //
        // POST: /Admin/AddGroupMember

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGroupMember(Models.AddGroupMemberModel model)
        {
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == model.GroupId);

            ForumDatabase.UserGroup.InsertOnSubmit(new Database.UserGroup() { GroupId = model.GroupId, UserId = model.UserId });
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Group", new { id = model.GroupId });
        }

        //
        // GET: /Admin/RemoveGroupMember

        public ActionResult RemoveGroupMember(int id)
        {
            var model = new Models.AddGroupMemberModel();

            model.GroupId = id;
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == model.GroupId);
            model.Users = model.Group.UserGroup.Select(ug => ug.User);

            return View(model);
        }

        //
        // POST: /Admin/RemoveGroupMember

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveGroupMember(Models.AddGroupMemberModel model)
        {
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == model.GroupId);

            Database.UserGroup userGroup = model.Group.UserGroup.Single(ug => ug.UserId == model.UserId);
            ForumDatabase.UserGroup.DeleteOnSubmit(userGroup);
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Group", new { id = model.GroupId });
        }

        //
        // GET: /Admin/AddPerForumGroupPermissions

        public ActionResult AddPerForumGroupPermissions(int id)
        {
            Models.AddPerForumGroupPermissionModel model = new Models.AddPerForumGroupPermissionModel() { GroupId = id };
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == id);
            model.Forums = ForumDatabase.Forum;

            return View(model);
        }

        //
        // POST: /Admin/AddPerForumGroupPermissions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPerForumGroupPermissions(Forum.Models.AddPerForumGroupPermissionModel model)
        {
            ForumDatabase.PerForumGroupPermissions.InsertOnSubmit(new Database.PerForumGroupPermissions()
            {
                ForumId = model.ForumId,
                GroupId = model.GroupId,
                Permissions = 0
            });
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Group", new { id = model.GroupId });
        }

        //
        // GET: /Admin/EditPerForumGroupPermissions

        public ActionResult EditPerForumGroupPermissions(int id, int forumId)
        {
            Models.EditPerForumGroupPermissionsModel model = new Models.EditPerForumGroupPermissionsModel() { GroupId = id, ForumId = forumId };
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == id);
            model.Forum = ForumDatabase.Forum.SingleOrDefault(f => f.Id == forumId);

            var perForumPerm = ForumDatabase.PerForumGroupPermissions.SingleOrDefault(pfgp => pfgp.GroupId == id && pfgp.ForumId == forumId);
            WritePermissionsToDictionary((Database.Permissions)perForumPerm.Permissions, model.Permissions);

            return View(model);
        }

        //
        // POST: /Admin/EditPerForumGroupPermissions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPerForumGroupPermissions(Forum.Models.EditPerForumGroupPermissionsModel model)
        {
            model.Group = ForumDatabase.Group.SingleOrDefault(g => g.Id == model.GroupId);
            model.Forum = ForumDatabase.Forum.SingleOrDefault(f => f.Id == model.ForumId);

            Database.Permissions newPermissions = GetPermissionsFromDictionary(model.Permissions);

            var perForumPerm = ForumDatabase.PerForumGroupPermissions.SingleOrDefault(pfgp => pfgp.GroupId == model.GroupId && pfgp.ForumId == model.ForumId);
            perForumPerm.Permissions = (int)newPermissions;
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Group", new { id = model.GroupId });
        }

        //
        // GET: /Admin/DeletePerForumGroupPermissions

        public ActionResult DeletePerForumGroupPermissions(int id, int forumId)
        {
            var perForumPerm = ForumDatabase.PerForumGroupPermissions.SingleOrDefault(pfgp => pfgp.GroupId == id && pfgp.ForumId == forumId);

            ForumDatabase.PerForumGroupPermissions.DeleteOnSubmit(perForumPerm);
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Group", new { id = id });
        }

        private void WritePermissionsToDictionary(Database.Permissions perms, IDictionary<string, bool> dict)
        {
            foreach (var flagName in Enum.GetNames(typeof(Database.Permissions)))
            {
                var flagValue = (Database.Permissions)Enum.Parse(typeof(Database.Permissions), flagName);

                if ((flagValue & flagValue - 1) != 0)
                {
                    continue;
                }

                dict.Add(flagName, (perms & flagValue) != 0);
            }
        }

        private Database.Permissions GetPermissionsFromDictionary(IDictionary<string, bool> dict)
        {
            Database.Permissions newPermissions = 0;

            foreach (var pair in dict)
            {
                Database.Permissions currentPermission;
                if (Enum.TryParse(pair.Key, out currentPermission))
                    if (pair.Value)
                        newPermissions |= currentPermission;
            }

            return newPermissions;
        }
    }
}
