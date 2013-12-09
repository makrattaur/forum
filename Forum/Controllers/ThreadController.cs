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
            var thread = ForumDatabase.Thread.Where(t => t.Id == id).SingleOrDefault();
            SetCurrentLocation(thread);

            return View(thread);
        }

        //
        // GET: /Thread/Reply/

        [Authorize]
        public ActionResult Reply(int id)
        {
            var thread = ForumDatabase.Thread.Where(t => t.Id == id).SingleOrDefault();
            SetCurrentLocation(thread);

            return View(new ThreadReplyModel()
            {
                Thread = thread,
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
            model.Thread = ForumDatabase.Thread.SingleOrDefault(t => t.Id == model.ThreadId);
            SetCurrentLocation(model.Thread);

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
            post.AuthorId = ForumDatabase.User.SingleOrDefault(u => u.Name == User.Identity.Name).Id;
            post.Thread = thread;

            ForumDatabase.Post.InsertOnSubmit(post);
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Index", new { id = thread.Id });
        }

        //
        // GET: /Thread/Create/

        [Authorize]
        public ActionResult Create(int id)
        {
            var forum = ForumDatabase.Forum.SingleOrDefault(f => f.Id == id);
            SetCurrentLocation(forum);

            return View(new Models.ThreadCreationModel()
            {
                ForumId = id,
                Forum = forum
            });
        }

        //
        // POST: /Thread/Create/

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.ThreadCreationModel model)
        {
            model.Forum = ForumDatabase.Forum.SingleOrDefault(f => f.Id == model.ForumId);
            SetCurrentLocation(model.Forum);

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
            thread.AuthorId = post.AuthorId = ForumDatabase.User.SingleOrDefault(u => u.Name == User.Identity.Name).Id;

            thread.ForumId = model.ForumId;
            post.Thread = thread;

            ForumDatabase.Thread.InsertOnSubmit(thread);
            ForumDatabase.Post.InsertOnSubmit(post);
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Index", new { id = thread.Id });
        }
    }
}
