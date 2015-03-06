using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Base
{
    public enum EntertainmentCategory : byte
    {
        Any = 255,
        Movie = 0,
        Music,
        Trailer,
        Clip,
        TVShow
    }
}
