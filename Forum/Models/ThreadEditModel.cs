using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Database;

namespace Forum.Models
{
    public class ThreadEditModel
    {
        public ThreadEditModel()
        {
            Thread = new Thread();
            Post = new Post();
        }

        public int ForumId { get; set; }
        public Database.Forum Forum { get; set; }
        public int ThreadId { get; set; }
        public Thread Thread { get; set; }
        public Post Post { get; set; }
        public string Error { get; set; }
    }
}