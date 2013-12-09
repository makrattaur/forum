using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    public class CategoryViewModel
    {
        public Database.Category Category { get; set; }
        public UserPermissionManager PermissionManager { get; set; }
    }
}