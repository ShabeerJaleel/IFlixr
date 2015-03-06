using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Base
{
    public enum Language : byte
    {
        Any = 255,
        Malayalam = 0
    }

    public static class KnownLanguages
    {
        public static readonly string Malayalam = "ml";
        public static readonly string English = "en";
        public static readonly string Hindi = "hi";
        public static readonly string Tamil = "ta";
        public static readonly string Telugu = "te";
    }
}
