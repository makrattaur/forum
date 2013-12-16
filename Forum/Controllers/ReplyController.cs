using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum.Controllers
{
    public class ReplyController : BaseController
    {
        //
        // GET: /Reply/Delete/

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return ForumError("No reply specified.");

            var post = ForumDatabase.Post.SingleOrDefault(p => p.Id == id);
            if (post == null)
                return ForumError("Invalid reply specified.");

            if (!PermissionManager.CanDeletePost(post))
                return NoPermissionError("delete a reply");

            SetCurrentLocation(post);

            return View(post);
        }

        //
        // POST: /Reply/Delete/

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteSubmitted(int? id)
        {
            var post = ForumDatabase.Post.SingleOrDefault(p => p.Id == id);
            if (post == null)
                return ForumError("Invalid reply specified.");

            var threadId = post.ThreadId;
            ForumDatabase.Post.DeleteOnSubmit(post);
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Index", "Thread", new { id = threadId });
        }

        //
        // GET: /Reply/Edit/

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return ForumError("No reply specified.");

            var post = ForumDatabase.Post.SingleOrDefault(p => p.Id == id);
            if (post == null)
                return ForumError("Invalid reply specified.");

            if (!PermissionManager.CanEditPost(post))
                return NoPermissionError("edit a reply");

            SetCurrentLocation(post);

            return View(new Models.ReplyEditModel()
            {
                PostId = post.Id,
                NewContent = post.Content,
                Post = post,
            });
        }

        //
        // POST: /Reply/Edit/

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.ReplyEditModel model)
        {
            model.Post = ForumDatabase.Post.SingleOrDefault(p => p.Id == model.PostId);
            if (model.Post == null)
                return ForumError("Invalid reply specified.");

            SetCurrentLocation(model.Post);

            if (string.IsNullOrWhiteSpace(model.NewContent))
            {
                model.Error = "The content cannot be empty.";
                return View(model);
            }

            var post = model.Post;
            post.Content = model.NewContent;
            ForumDatabase.SubmitChanges();

            return RedirectToAction("Index", "Thread", new { id = post.ThreadId });
        }
    }
}
