using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    /// <summary>
    /// Wraps a model/viewmodel for use with the layout.
    /// </summary>
    /// <typeparam name="T">The type of the model to wrap.</typeparam>
    public class ViewModelWrapper<T> : BaseViewModel
    {
        public ViewModelWrapper()
        {
            try
            {
                InnerModel = Activator.CreateInstance<T>();
            }
            catch (Exception)
            {
 
            }
        }

        public ViewModelWrapper(T innerModel)
        {
            InnerModel = innerModel;
        }

        public T InnerModel { get; private set; }
    }
}