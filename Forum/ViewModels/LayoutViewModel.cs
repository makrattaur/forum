using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    public class LayoutViewModel
    {
        public string CurrentController { get; set; }
        public string CurrentAction { get; set; }
        public string Username { get; set; }
        public Database.User CurrentUser { get; set; }
    }
}