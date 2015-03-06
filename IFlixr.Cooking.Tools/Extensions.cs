using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IFlixr.Cooking.Tools
{
    public static class Extensions
    {
        public static void InvokeEx<TControl>(this TControl control, Action action)
     where TControl : Control
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action();

        }
    }
}
