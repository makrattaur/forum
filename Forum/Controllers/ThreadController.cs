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
            var forum = db.Forum.SingleOrDefault(f => f.Id == id);
            return TypedView(new ThreadViewModel(forum, forum.Thread.ToList()));
        }

        //
        // GET: /Thread/Show/

        public ActionResult Show(int id)
        {
            return TypedView(db.Thread.Where(t => t.Id == id).SingleOrDefault());
        }

        //
        // GET: /Thread/Reply/

        [Authorize]
        public ActionResult Reply(int id)
        {
            return TypedView(new ThreadReplyModel()
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
        public ActionResult Reply(ViewModels.ViewModelWrapper<Models.ThreadReplyModel> model)
        {
            model.InnerModel.Thread = db.Thread.SingleOrDefault(t => t.Id == model.InnerModel.ThreadId);
            if (model.InnerModel.Thread == null)
            {
                // show generic error.
                return RedirectToAction("Show", new { id = model.InnerModel.Thread.Id });
            }

            if (string.IsNullOrWhiteSpace(model.InnerModel.Post.Content))
            {
                model.InnerModel.Error = "The content cannot be empty.";
                return TypedView(model);
            }

            Thread thread = model.InnerModel.Thread;
            Post post = model.InnerModel.Post;

            InitBaseViewModel(model);

            post.PostTime = DateTime.Now;
            post.AuthorId = model.LayoutModel.CurrentUser.Id;
            post.Thread = thread;

            db.Post.InsertOnSubmit(post);
            db.SubmitChanges();

            return RedirectToAction("Show", new { id = thread.Id });
        }

        //
        // GET: /Thread/Create/

        [Authorize]
        public ActionResult Create(int id)
        {
            return TypedView(new Models.ThreadCreationModel() { ForumId = id });
        }

        //
        // POST: /Thread/Create/

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModels.ViewModelWrapper<Models.ThreadCreationModel> model)
        {
            if (string.IsNullOrWhiteSpace(model.InnerModel.Thread.Title))
            {
                model.InnerModel.Error = "The title cannot be empty.";
                return TypedView(model);
            }

            if (string.IsNullOrWhiteSpace(model.InnerModel.Post.Content))
            {
                model.InnerModel.Error = "The content cannot be empty.";
                return TypedView(model);
            }

            Thread thread = model.InnerModel.Thread;
            Post post = model.InnerModel.Post;

            InitBaseViewModel(model);

            thread.CreationTime = post.PostTime = DateTime.Now;
            thread.AuthorId = post.AuthorId = model.LayoutModel.CurrentUser.Id;

            thread.ForumId = model.InnerModel.ForumId;
            post.Thread = thread;

            db.Thread.InsertOnSubmit(thread);
            db.Post.InsertOnSubmit(post);
            db.SubmitChanges();

            return RedirectToAction("Show", new { id = thread.Id });
        }
    }
}
