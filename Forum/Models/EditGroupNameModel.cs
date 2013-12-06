using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class EditGroupNameModel
    {
        public EditGroupNameModel()
        {
            Group = new Database.Group();
        }

        public int GroupId { get; set; }
        public Database.Group Group { get; set; }
        public string NewName { get; set; }
    }
}