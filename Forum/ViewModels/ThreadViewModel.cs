using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Database;
using Forum.ViewModels;

namespace Forum.ViewModels
{
    public class ThreadViewModel
    {
        public Database.Forum Forum { get; set; }
        public UserPermissionManager PermissionManager { get; set; }
    }
}