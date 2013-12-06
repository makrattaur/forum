using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            LayoutModel = new LayoutViewModel();
            Initialized = false;
        }

        public bool Initialized { get; set; }
        public LayoutViewModel LayoutModel { get; set; }
    }
}