using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IFlixr.Cooking.Repository;
using IFlixr.Cooking.Data;

namespace IFlixr.Cooking.Tools
{
    public partial class MainFrm : Form
    {
        
        public static MainFrm MainForm;
        private static Browser browserForm;
        public MainFrm()
        {
            InitializeComponent();
            MainForm = this;
            var form = new ManagementForm();
            form.MdiParent = MainForm;
            form.Show();
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        public static void LoadUrl(string url)
        {
            if (browserForm == null || browserForm.IsDisposed)
            {
                browserForm = new Browser();
                browserForm.TopLevel = false;
                MainForm.Controls.Add(browserForm);
                browserForm.Parent = MainForm;
                browserForm.TopMost = true;
                browserForm.Show();
            }
            browserForm.LoadUrl(url);
          

            //var browser = MainForm.MdiChildren.FirstOrDefault(x => x.GetType() == typeof(Browser));
            //if (browser == null)
            //{
            //    browser = new Browser();
            //    browser.MdiParent = MainForm;
            //    browser.Show();
            //}
            //((Browser)browser).LoadUrl(url);
        }

        


    }
}
