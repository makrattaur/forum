using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    public class BoardIndexViewModel
    {
        public IEnumerable<Database.Category> Categories { get; set; }
        public UserPermissionManager PermissionManager { get; set; }
    }
}