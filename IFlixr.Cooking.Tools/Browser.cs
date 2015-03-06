using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IFlixr.Cooking.Tools
{
    public partial class Browser : Form
    {
        public Browser()
        {
            InitializeComponent();
        }

        public void LoadUrl(string url)
        {
            this.webBrowser1.Url = new Uri(url);
        }
    }
}
