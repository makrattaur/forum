using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class EditGroupModel
    {
        public EditGroupModel()
        {
            Group = new Database.Group();
            Permissions = new Dictionary<string, bool>();
        }

        public int GroupId { get; set; }
        public Database.Group Group { get; set; }
        public IDictionary<string, bool> Permissions { get; set; }
        public string NewName { get; set; }
    }
}