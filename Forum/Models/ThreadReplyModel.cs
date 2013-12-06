using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Database;

namespace Forum.Models
{
    public class ThreadReplyModel
    {
        public ThreadReplyModel()
        {
            Post = new Post();
        }

        public Post Post { get; set; }
        public Thread Thread { get; set; }
        public int ThreadId { get; set; }
        public string Error { get; set; }
    }
}