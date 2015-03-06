using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IFlixr.WebScraper.Client
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void aggregatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var agg = new AggregatorForm();
            agg.MdiParent = this;
            agg.Show();
        }

        private void validatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var validatory = new ValidatorForm();
            validatory.MdiParent = this;
            validatory.Show();
        }
    }
}
