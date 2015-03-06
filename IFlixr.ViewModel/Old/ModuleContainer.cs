using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.ViewModel.Base;

namespace IFlixr.ViewModel
{
    public class ModuleContainer : BaseModel
    {
        public bool FixedWidth { get; set; }
        public bool FixedHeight { get; set; }
        public int WidthInPercentage { get; set; }
        public int Height { get; set; }
    }
}
