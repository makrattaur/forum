using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum
{
    public abstract class ForumWebViewPage : System.Web.Mvc.WebViewPage
    {
        public ViewModels.LayoutViewModel LayoutModel { get; private set; }

        private void InitLayoutModel()
        {
            LayoutModel = new ViewModels.LayoutViewModel();
            using (var db = new Database.ForumDataContext())
                Controllers.BaseController.InitLayoutViewModel(this, db, LayoutModel);
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            InitLayoutModel();
        }
    }

    public abstract class ForumWebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public ViewModels.LayoutViewModel LayoutModel { get; private set; }

        private void InitLayoutModel()
        {
            LayoutModel = new ViewModels.LayoutViewModel();
            using (var db = new Database.ForumDataContext())
                Controllers.BaseController.InitLayoutViewModel(this, db, LayoutModel);
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            InitLayoutModel();
        }
    }
}