using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IFlixr.ViewModel
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                Js = (bool)HttpContext.Current.Session["jsSupport"];
            else
                Js = true;
        }
        public string Id { get; set; }
        public int IntId { get { return (String.IsNullOrWhiteSpace(Id) ? -1 : Convert.ToInt32(Id.Split('/')[1])); } }
        /// <summary>
        /// Support Javascript or not
        /// </summary>
        public bool Js { get; set; }
    }


    public class ListEx<T> : List<T>
    {
        public ListEx()
        {
            Init();
        }

        public ListEx(IEnumerable<T> collection)
            :base(collection)
        {
            Init();
        }

        private void Init()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                Js = (bool)HttpContext.Current.Session["jsSupport"];
            else
                Js = true;
        }
         /// <summary>
        /// Support Javascript or not
        /// </summary>
        public bool Js { get; set; }
    }
    
}
