using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Database;

namespace Forum
{
    public class UserPermissionManager
    {
        ForumDataContext db;
        User user;

        public UserPermissionManager(ForumDataContext db, User user)
        {
            this.db = db;
            this.user = user;
        }

        public bool CanSeeForum(Database.Forum forum)
        {
            if (user == null)
                return true;

            return (GetEffectivePermissions(forum) & Permissions.ViewForum) != 0;
        }

        private Permissions GetEffectivePermissions(Database.Forum forum)
        {
            var perForumGroupPermissions = user.UserGroup
                .Join(db.PerForumGroupPermissions, ug => ug.GroupId, pfgp => pfgp.GroupId, (ug, pfgp) => pfgp)
                .Where(pfgp => pfgp.ForumId == forum.Id);

            if (perForumGroupPermissions.Any())
            {
                var combinedPermissions = perForumGroupPermissions
                    .Select(pfgp => (Database.Permissions)pfgp.Permissions)
                    .Aggregate((state, cur) => state | cur);

                return combinedPermissions;
            }
            else
            {
                var combinedPermissions = user.UserGroup
                    .Join(db.Group, ug => ug.GroupId, g => g.Id, (ug, g) => g)
                    .Select(g => (Database.Permissions)g.Permissions)
                    .Aggregate((state, cur) => state | cur);

                return combinedPermissions;
            }
        }
    }
}