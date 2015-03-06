using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Base
{
    public enum CarouselSize : byte
    {
        Large = 0,
        Medium,
        Small,
        Youtube
    }

    public enum CarouselType : byte
    {
        Image = 0,
        Video = 1,
        Mixed = 2
    }
}
