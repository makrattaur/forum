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
        public string CurrentCategory { get; set; }
        public int CurrentCategoryId { get; set; }
        public string CurrentForum { get; set; }
        public int CurrentForumId { get; set; }
    }
}