using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Database;
using Forum.ViewModels;

namespace Forum.ViewModels
{
    public class ThreadViewModel
    {
        public ThreadViewModel(Database.Forum forum, IEnumerable<Thread> threads)
        {
            Forum = forum;
            Threads = threads;
        }

        public Database.Forum Forum { get; set; }
        public IEnumerable<Thread> Threads { get; set; }
    }
}