using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Forum.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Forum.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult Index(int? id)
        {
            Database.User user = id == null ? CurrentForumUser : ForumDatabase.User.SingleOrDefault(u => u.Id == id);

            if (user == null)
                return ForumError("Invalid user specified.");

            return View(user);
        }

        //
        // GET: /User/LogOff

        [Authorize]
        public ActionResult LogOff()
        {
            HttpContext.Cache.Remove("userGroups_authSess-" + Request.Cookies[FormsAuthentication.FormsCookieName].Value);
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /User/Login

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        //
        // POST: /User/Login

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.LoginModel model, string ReturnUrl)
        {
            Database.User user = ForumDatabase.User.SingleOrDefault(u => u.Name == model.User.Name);
            if (user == null)
            {
                model.Error = "Invalid login information.";
                return View(model);
            }

            string hash = HashPassword(model.Password);
            if(user.PasswordHash != hash)
            {
                model.Error = "Invalid login information.";
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(model.User.Name, model.KeepConnected);
            //GetUserRoles(HttpContext);

            if(string.IsNullOrEmpty(ReturnUrl))
                return RedirectToAction("Index", "Home");
            else
                return Redirect(ReturnUrl);
        }

        static string GetCachedRoleKey(HttpContextBase httpContext)
        {
            return "userGroups_authSess-" + httpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value;
        }

        [NonAction]
        public static string[] GetUserRoles(HttpContextBase httpContext)
        {
            string key = GetCachedRoleKey(httpContext);
            string[] cachedRoles = (string[])httpContext.Cache.Get(key);

            if (cachedRoles != null)
                return cachedRoles;

            using (var db = new Database.ForumDataContext())
            {
                Database.User user = db.User.SingleOrDefault(u => u.Name == httpContext.User.Identity.Name);
                // TODO: null check
                string[] roles = user.UserGroup.Select(ug => ug.Group.Name).ToArray();
                httpContext.Cache.Add(key,
                    roles,
                    null,
                    DateTime.Now.Add(FormsAuthentication.Timeout),
                    System.Web.Caching.Cache.NoSlidingExpiration,
                    System.Web.Caching.CacheItemPriority.Default,
                    null
                );

                return roles;
            }
        }

        //
        // GET: /User/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new Models.RegisterModel());
        }

        //
        // POST: /User/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(Forum.Models.RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.User.Name))
            {
                model.Error = "The username cannot be empty.";
                return View(model);
            }

            if (string.IsNullOrEmpty(model.User.Email))
            {
                model.Error = "The email address cannot be empty.";
                return View(model);
            }

            if (ForumDatabase.User.Any(u => u.Name == model.User.Name))
            {
                model.Error = "The username is alrady taken.";
                return View(model);
            }

            if (model.Password != model.PasswordConfirmation)
            {
                model.Error = "The passwords do not match.";
                return View(model);
            }

            Database.User newUser = model.User;
            newUser.PasswordHash = HashPassword(model.Password);
            newUser.JoinTime = DateTime.Now;

            Database.Group userGroup = ForumDatabase.Group.SingleOrDefault(g => g.Name == "Users" && g.IsSystem);
            ForumDatabase.UserGroup.InsertOnSubmit(new Database.UserGroup() { Group = userGroup, User = newUser });

            ForumDatabase.User.InsertOnSubmit(model.User);
            ForumDatabase.SubmitChanges();
            FormsAuthentication.SetAuthCookie(model.User.Name, true);

            return RedirectToAction("Index", "Home");
        }

        string HashPassword(string password)
        {
            SHA1 hasher = new SHA1CryptoServiceProvider();
            hasher.ComputeHash(Encoding.UTF8.GetBytes(password));

            return string.Concat(hasher.Hash.Select(b => b.ToString("x2")));
        }
    }
}
