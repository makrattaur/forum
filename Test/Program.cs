using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Database;

namespace Test
{
    class Program
    {
        static void Test01(string[] args)
        {
            var db = new ForumDataContext();
            foreach (var category in db.Category.OrderBy(cat => cat.Order))
            {
                Console.WriteLine("[{1}] {0}:", category.Name, category.Id);
                foreach (var forum in category.Forum.OrderBy(f => f.Order))
                {
                    Console.WriteLine("\t[{1}] {0}", forum.Name, forum.Id);
                }
            }
        }

        static void Test02(string[] args)
        {
            var db = new ForumDataContext();
            var forum = db.Forum.SingleOrDefault(f => f.Id == 1);
            Console.WriteLine("Forum id {0}, name \"{1}\"",
                forum.Id,
                forum.Name
            );
            foreach (var thread in forum.Thread)
            {
                Console.WriteLine("Thread by \"{0}\", title \"{1}\", {2} posts",
                    thread.User.Name,
                    thread.Title,
                    thread.Post.Count
                );

                foreach (var post in thread.Post)
                {
                    Console.WriteLine("\tPost by \"{0}\", content \"{1}\"",
                        post.User.Name,
                        post.Content
                    );
                }
            }
        }

        static void Main(string[] args)
        {
            Test02(args);
        }
    }
}
