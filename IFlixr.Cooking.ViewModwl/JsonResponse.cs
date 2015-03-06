using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Cooking.ViewModel
{
    public class JsonResponse
    {
        public JsonResponse()
        {
            Message = String.Empty;
        }
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
