using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IFlixr.Cooking.Repository;
using IFlixr.Cooking.Scraper;
using IFlixr.Cooking.Data;
using System.Net;
using System.IO;
using IFlixr.ImageProcessor;
using System.Threading.Tasks;
using IFlixr.Cooking.Base;

namespace IFlixr.Cooking.Tools
{
    public partial class ScraperWidget : UserControl
    {
        private Task task;
        private FeedScraperService service =  FeedScraperService.Create();

        public ScraperWidget()
        {
            InitializeComponent();
            service.FeedScrapeProgess += new EventHandler<FeedScrapeProgessEventArgs>(service_FeedScrapeProgess);
        }

        void service_FeedScrapeProgess(object sender, FeedScrapeProgessEventArgs e)
        {
            this.InvokeEx(() =>
            {
                this.labelAuthor.Text = e.Author.ProfileName;
                this.labelProgress.Text = e.Message;
            });
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            if (this.task != null)
            {
                this.buttonProcess.Text = "Start";
                this.buttonProcess.Enabled = false;
                service.StopScraping();
            }
            else
            {
                this.buttonProcess.Text = "Stop";
                this.task = Task.Factory.StartNew(() =>
                 {
                     service.ScrapeAll(GlobalSettings.ImageDownloadFolder);
                    
                     this.InvokeEx(() =>
                     {
                         this.buttonProcess.Enabled = true;
                         this.buttonProcess.Text = "Start";
                         this.labelAuthor.Text = String.Empty;
                         this.labelProgress.Text = "Idle";
                     });
                     this.task = null;
                 });
            }
        }
    }
}
