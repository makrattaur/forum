using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class AddGroupMemberModel
    {
        public int GroupId { get; set; }
        public Database.Group Group { get; set; }
        public IEnumerable<Database.User> Users { get; set; }
        public int UserId { get; set; }
    }
}