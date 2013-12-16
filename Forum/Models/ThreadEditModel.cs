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
        }

        public int ThreadId { get; set; }
        public string NewTitle { get; set; }
        public string NewContent { get; set; }
        public string Error { get; set; }

        // Display members

        public Thread Thread { get; set; }
    }
}