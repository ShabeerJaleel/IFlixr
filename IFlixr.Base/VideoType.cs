using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Base
{
    public enum VideoType : byte
    {
        Movie = 0,
        Music = 1,
        Trailer = 2,

        Clip = 50,
        Comedy = 51,
        Interview = 52,
        Shooting = 53
    }
}
