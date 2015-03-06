using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IFlixr.Cooking.Repository;
using IFlixr.Cooking.Data;

namespace IFlixr.Cooking.Tools
{
    public partial class CuisineManagementWidget : UserControl
    {
        public CuisineManagementWidget()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (System.ComponentModel.LicenseManager.CurrentContext.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;
            var repo = RavenRepository.Create();
            foreach (var t in repo.GetCuisineTags())
            {
                this.checkedListBoxExisting.Items.Add(t);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = RavenRepository.Create();
                repo.SaveCuisine(new KnownCuisine
                {
                    Description = this.textBoxDescription.Text.Trim(),
                    Tag = this.textBoxTag.Text.Trim().ToLower()

                });
                this.checkedListBoxExisting.Items.Add(this.textBoxTag.Text.Trim().ToLower());
                ManagementForm.UserWidget.Init();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
}
