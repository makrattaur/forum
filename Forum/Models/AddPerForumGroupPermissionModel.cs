using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class AddPerForumGroupPermissionModel
    {
        public int GroupId { get; set; }
        public Database.Group Group { get; set; }
        public IEnumerable<Database.Forum> Forums { get; set; }
        public int ForumId { get; set; }
    }
}