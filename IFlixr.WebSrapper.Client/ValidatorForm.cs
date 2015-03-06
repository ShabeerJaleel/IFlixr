using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IFlixr.Data.Scraper;
using IFlixr.WebScraper.Client.Model;
using IFlixr.Base;
using System.Diagnostics;

namespace IFlixr.WebScraper.Client
{
    public partial class ValidatorForm : Form
    {
        #region Constructor

        public ValidatorForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Events
        private void buttonValidateScrapperDB_Click(object sender, EventArgs e)
        {
            var youtubeSraper = new YouTubeScraper();
            var videos = new SortableBindingList<VideoModel>();

            using (var ctx = new ScraperContext())
            {
                foreach (var item in ctx.Videos)
                {
                    if ((VideoSource)item.Source != VideoSource.Youtube)
                        continue;
                    try
                    {
                       var video = youtubeSraper.GetVideoById(item.VideoId);
                       if (video == null)
                           videos.Add(item.Map());
                    }
                    catch (Exception ex)
                    {
                        if (MessageBox.Show(ex.Message + Environment.NewLine + "Do you want to continue?", "IFlixr", MessageBoxButtons.YesNo)
                            == System.Windows.Forms.DialogResult.No)
                            break;
                    }
                }
            }

            this.dataGridViewScrapper.DataSource = videos;
        }
        #endregion

        private void dataGridViewScrapper_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView view = (DataGridView)sender;
            if (e.RowIndex != -1)
            {

                // Open the link in the default browser
                var data = view.DataSource as SortableBindingList<VideoModel>;

                if (view.Columns[e.ColumnIndex].CellType == typeof(DataGridViewLinkCell))
                {
                    if (data != null)
                    {
                        var vm = data.ElementAt(e.RowIndex);
                        Process.Start(vm.Url);
                      
                    }
                }
            }
        }
    }
}
