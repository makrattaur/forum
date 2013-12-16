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
            return HasPermission(forum, Permissions.ViewForum);
        }

        public bool CanSeeForumThreads(Database.Forum forum)
        {
            return HasPermission(forum, Permissions.SeeThread);
        }

        public bool CanReadThread(Thread thread)
        {
            return HasPermission(thread.Forum, Permissions.ReadThread);
        }

        public bool CanCreateThread(Database.Forum forum)
        {
            return HasPermission(forum, Permissions.CreateThread);
        }

        public bool CanEditThread(Thread thread)
        {
            return HasPermission(thread.Forum, Permissions.EditOwnThread) ||
                HasPermission(thread.Forum, Permissions.EditThread);
        }

        public bool CanDeleteThread(Thread thread)
        {
            return HasPermission(thread.Forum, Permissions.DeleteOwnThread) ||
                HasPermission(thread.Forum, Permissions.DeleteThread);
        }

        public bool CanReplyInThread(Thread thread)
        {
            return HasPermission(thread.Forum, Permissions.ReplyThread);
        }

        private bool IsPermissionSet(Permissions perms, Permissions permToTest)
        {
            return (perms & permToTest) != 0;
        }

        private bool HasPermission(Database.Forum forum, Permissions permToTest)
        {
            return IsPermissionSet(GetEffectivePermissions(forum), permToTest);
        }

        private Permissions GetEffectivePermissions(Database.Forum forum)
        {
            IQueryable<Group> groupMembership;

            if (user == null)
            {
                groupMembership = db.Group.Where(g => g.Name == "Guests");
            }
            else
            {
                var groupMembershipIds = user.UserGroup.Select(ug => ug.GroupId);
                groupMembership = db.Group.Where(g => groupMembershipIds.Contains(g.Id));
            }

            var perForumGroupPermissions = groupMembership
                .Join(db.PerForumGroupPermissions, g => g.Id, pfgp => pfgp.GroupId, (ug, pfgp) => pfgp)
                .Where(pfgp => pfgp.ForumId == forum.Id);

            if (perForumGroupPermissions.Any())
            {
                return perForumGroupPermissions
                    .Select(pfgp => (Database.Permissions)pfgp.Permissions)
                    .ToList() // Aggregate in-memory, cannot do it on the IQueryable.
                    .Aggregate((state, cur) => state | cur);
            }
            else
            {
                return groupMembership
                    .Select(g => (Database.Permissions)g.Permissions)
                    .ToList() // Aggregate in-memory, cannot do it on the IQueryable.
                    .Aggregate((state, cur) => state | cur);
            }
        }
    }
}