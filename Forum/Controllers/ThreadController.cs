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

        public ActionResult Index(int? id)
        {
            if (id == null)
                return ForumError("No thread specified.");

            var thread = ForumDatabase.Thread.Where(t => t.Id == id).SingleOrDefault();
            if (thread == null)
                return ForumError("Invalid thread specified.");

            if (!PermissionManager.CanReadThread(thread))
                return NoPermissionError("read threads in this forum");

            SetCurrentLocation(thread);

            return View(new ThreadViewModel() 
            {
                Thread =  thread,
                PermissionManager = PermissionManager
            });
        }

        //
        // GET: /Thread/Reply/

        [Authorize]
        public ActionResult Reply(int? id)
        {
            if (id == null)
                return ForumError("No thread specified.");

            var thread = ForumDatabase.Thread.Where(t => t.Id == id).SingleOrDefault();
            if (thread == null)
                return ForumError("Invalid thread specified.");

            if (!PermissionManager.CanReplyInThread(thread))
                return NoPermissionError("reply to this thread");

            SetCurrentLocation(thread);

            return View(new ThreadReplyModel()
            {
                Thread = thread,
                ThreadId = id.Value
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
            post.AuthorId = CurrentForumUser.Id;
            post.Thread = thread;

            ForumDatabase.Post.InsertOnSubmit(post);
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Index", new { id = thread.Id });
        }

        //
        // GET: /Thread/Create/

        [Authorize]
        public ActionResult Create(int? id)
        {
            if (id == null)
                return ForumError("No forum specified.");

            var forum = ForumDatabase.Forum.SingleOrDefault(f => f.Id == id);
            if (forum == null)
                return ForumError("Invalid forum specified.");

            if (!PermissionManager.CanCreateThread(forum))
                return NoPermissionError("create a thread in this forum");

            SetCurrentLocation(forum);

            // TODO: No permission error

            return View(new Models.ThreadCreationModel()
            {
                ForumId = id.Value,
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
            thread.AuthorId = post.AuthorId = CurrentForumUser.Id;

            thread.ForumId = model.ForumId;
            post.Thread = thread;

            ForumDatabase.Thread.InsertOnSubmit(thread);
            ForumDatabase.Post.InsertOnSubmit(post);
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Index", new { id = thread.Id });
        }

        //
        // GET: /Thread/Edit/

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return ForumError("No thread specified.");

            var thread = ForumDatabase.Thread.SingleOrDefault(f => f.Id == id);
            if (thread == null)
                return ForumError("Invalid thread specified.");

            var post = thread.Post.First();
            if (!PermissionManager.CanEditPost(post))
                return NoPermissionError("edit a thread in this forum");

            SetCurrentLocation(thread);

            return View(new Models.ThreadEditModel()
            {
                ThreadId = thread.Id,
                NewTitle = thread.Title,
                NewContent = post.Content,
                Thread = thread,
            });
        }

        //
        // POST: /Thread/Edit/

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.ThreadEditModel model)
        {
            model.Thread = ForumDatabase.Thread.SingleOrDefault(f => f.Id == model.ThreadId);
            if (model.Thread == null)
                return ForumError("Invalid thread specified.");

            SetCurrentLocation(model.Thread);

            if (string.IsNullOrWhiteSpace(model.NewTitle))
            {
                model.Error = "The title cannot be empty.";
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.NewContent))
            {
                model.Error = "The content cannot be empty.";
                return View(model);
            }

            var thread = model.Thread;
            var post = thread.Post.First();

            thread.Title = model.NewTitle;
            post.Content = model.NewContent;
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Index", new { id = thread.Id });
        }
    }
}
