using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Database;
using Forum.ViewModels;

namespace Forum.ViewModels
{
    public class ForumViewModel
    {
        public Database.Forum Forum { get; set; }
        public int Page { get; set; }
        public UserPermissionManager PermissionManager { get; set; }
    }
}