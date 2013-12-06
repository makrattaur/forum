using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Forum.ViewModels;
using Database;

namespace Forum.Controllers
{
    public class BaseController : Controller
    {
        protected ForumDataContext db = new ForumDataContext();

        public static void InitLayoutViewModel(HttpContextBase context, ForumDataContext db, LayoutViewModel lvm)
        {
            lvm.Username = "foobar";

            if (context.Request.IsAuthenticated)
                lvm.CurrentUser = db.User.SingleOrDefault(u => u.Name == context.User.Identity.Name);

            lvm.CurrentController = context.Request.RequestContext.RouteData.GetRequiredString("controller");
            lvm.CurrentAction = context.Request.RequestContext.RouteData.GetRequiredString("action");
        }

        private void InitLayoutViewModel(LayoutViewModel lvm)
        {
            InitLayoutViewModel(HttpContext, db, lvm);
        }


        protected void InitBaseViewModel(BaseViewModel bvm)
        {
            if (bvm.Initialized)
                return;

            bvm.Initialized = true;
            InitLayoutViewModel(bvm.LayoutModel);
        }

#if false
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var viewResult = filterContext.Result as ViewResultBase;
            if (viewResult != null)
            {
                var model = viewResult.ViewData.Model;
                if (model != null)
                {
                    Type t = model.GetType();
                    if(!t.IsSubclassOf(typeof(Forum.ViewModels.LayoutViewModel)))
                    {
                        var genType = typeof(Forum.ViewModels.ViewModelWrapperEnumerable<>).MakeGenericType(new[] { t });
                        object wrappedModel = Activator.CreateInstance(genType, model);
                        FillLayoutViewModel((LayoutViewModel)wrappedModel);

                        viewResult.ViewData.Model = wrappedModel;
                    }
                }
            }
        }
#endif
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var viewResult = filterContext.Result as ViewResultBase;
            if (viewResult != null)
            {
                var model = viewResult.ViewData.Model;
                if (model != null)
                {
                    var baseViewModel = model as BaseViewModel;
                    if (baseViewModel != null)
                    {
                        InitBaseViewModel(baseViewModel);
                    }
                }
            }
        }
    }
}
