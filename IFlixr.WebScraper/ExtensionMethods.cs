using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.WebScraper
{
    public static class ExtensionMethods
    {
        public static bool ContainsAnyString(this string text, IEnumerable<string> tokens)
        {
            foreach (var token in tokens)
            {
                if (text.ToLower().Contains(token.ToLower()))
                    return true;
            }
            return false;
        }

        public static bool ContainsAllStrings(this string text, IEnumerable<string> tokens)
        {
            foreach (var token in tokens)
            {
                if (!text.ToLower().Contains(token.ToLower()))
                    return false;
            }
            return true;
        }
    }
}
