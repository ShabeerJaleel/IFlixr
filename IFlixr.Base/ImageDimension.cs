using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Base
{
    [Flags]
    public enum ImageDimension
    {
        Medium_W220XH320 = 1,
        Small_W120XH174 = 2,
        Thumb_W120XH174 = 4
    }
}
