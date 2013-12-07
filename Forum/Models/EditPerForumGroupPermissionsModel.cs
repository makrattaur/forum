using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class EditPerForumGroupPermissionsModel : EditGroupPermissionsModel
    {
        public int ForumId { get; set; }
        public Database.Forum Forum { get; set; }
    }
}