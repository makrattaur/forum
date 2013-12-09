using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Database;
using Forum.ViewModels;
using Forum.Models;

namespace Forum.Controllers
{
    public class ThreadController : BaseController
    {
        //
        // GET: /Thread/

        public ActionResult Index(int id)
        {
            return View(db.Thread.Where(t => t.Id == id).SingleOrDefault());
        }

        //
        // GET: /Thread/Reply/

        [Authorize]
        public ActionResult Reply(int id)
        {
            return View(new ThreadReplyModel()
            {
                Thread = db.Thread.SingleOrDefault(t => t.Id == id),
                ThreadId = id
            });
        }

        //
        // POST: /Thread/Reply/

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(Models.ThreadReplyModel model)
        {
            model.Thread = db.Thread.SingleOrDefault(t => t.Id == model.ThreadId);
            if (model.Thread == null)
            {
                // show generic error.
                return RedirectToAction("Index", new { id = model.Thread.Id });
            }

            if (string.IsNullOrWhiteSpace(model.Post.Content))
            {
                model.Error = "The content cannot be empty.";
                return View(model);
            }

            Thread thread = model.Thread;
            Post post = model.Post;

            post.PostTime = DateTime.Now;
            post.AuthorId = db.User.SingleOrDefault(u => u.Name == User.Identity.Name).Id;
            post.Thread = thread;

            db.Post.InsertOnSubmit(post);
            db.SubmitChanges();

            return RedirectToAction("Index", new { id = thread.Id });
        }

        //
        // GET: /Thread/Create/

        [Authorize]
        public ActionResult Create(int id)
        {
            return View(new Models.ThreadCreationModel()
            {
                ForumId = id,
                Forum = db.Forum.SingleOrDefault(f => f.Id == id)
            });
        }

        //
        // POST: /Thread/Create/

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.ThreadCreationModel model)
        {
            model.Forum = db.Forum.SingleOrDefault(f => f.Id == model.ForumId);

            if (string.IsNullOrWhiteSpace(model.Thread.Title))
            {
                model.Error = "The title cannot be empty.";
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Post.Content))
            {
                model.Error = "The content cannot be empty.";
                return View(model);
            }

            Thread thread = model.Thread;
            Post post = model.Post;

            thread.CreationTime = post.PostTime = DateTime.Now;
            thread.AuthorId = post.AuthorId = db.User.SingleOrDefault(u => u.Name == User.Identity.Name).Id;

            thread.ForumId = model.ForumId;
            post.Thread = thread;

            db.Thread.InsertOnSubmit(thread);
            db.Post.InsertOnSubmit(post);
            db.SubmitChanges();

            return RedirectToAction("Index", new { id = thread.Id });
        }
    }
}
