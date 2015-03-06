using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IFlixr.WebScraper
{
    public static class Helper
    {
        public static T ChangeType<T>(string data)
        {
            return (T)Convert.ChangeType(data, typeof(T));
        }

        public static void CreateDirectory(this DirectoryInfo instance)
        {
            if (instance.Parent != null)
            {
                CreateDirectory(instance.Parent);
            }
            if (!instance.Exists)
            {
                instance.Create();
            }
        }

       
    }

    
}
