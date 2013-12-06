using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using System.Security.Principal;
using System.Web.Security;

namespace Forum
{
    // Remarque : pour obtenir des instructions sur l'activation du mode classique IIS6 ou IIS7, 
    // visitez http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void Init()
        {
            base.Init();

            AuthenticateRequest += MvcApplication_AuthenticateRequest;
        }

        void MvcApplication_AuthenticateRequest(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
                return;

            string[] groups = Forum.Controllers.UserController.GetUserRoles(new HttpContextWrapper(Context));

            GenericIdentity identity = new GenericIdentity(User.Identity.Name, User.Identity.AuthenticationType);
            GenericPrincipal principal = new GenericPrincipal(identity, groups);

            Context.User = principal;
        }
    }
}