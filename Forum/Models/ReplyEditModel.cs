using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Database;

namespace Forum.Models
{
    public class ReplyEditModel
    {
        public int PostId { get; set; }
        public string NewContent { get; set; }
        public string Error { get; set; }

        // Display members

        public Post Post { get; set; }
    }
}