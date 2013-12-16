using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    public class ThreadViewModel
    {
        public Database.Thread Thread { get; set; }
        public UserPermissionManager PermissionManager { get; set; }
    }
}