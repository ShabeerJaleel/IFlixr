using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IFlixr.Cooking.ViewModel
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

        #region DB 
        
        public string Id { get; set; }
        public int IntId { get { return (String.IsNullOrWhiteSpace(Id) ? -1 : Convert.ToInt32(Id.Split('/')[1])); } }

        #endregion
        /// <summary>
        /// Support Javascript or not
        /// </summary>
        public bool Js { get; set; }
    }
}
