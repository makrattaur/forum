using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace Forum
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute()
        {
            NoRedirect = false;
            SilentError = false;
        }

        public bool NoRedirect { get; set; }
        public bool SilentError { get; set; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (NoRedirect)
            {
                if (SilentError)
                    filterContext.Result = new HttpStatusCodeResult(404);
                else
                    filterContext.Result = new HttpStatusCodeResult(401);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}