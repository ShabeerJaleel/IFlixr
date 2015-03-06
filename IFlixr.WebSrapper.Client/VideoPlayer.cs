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
    public partial class VideoPlayer : Form
    {
        private static VideoPlayer TheOnlyPlayer;
        public VideoPlayer()
        {
            InitializeComponent();
        }

        public static void Play(string url)
        {
            if (TheOnlyPlayer != null)
            {
                TheOnlyPlayer.Dispose();
                TheOnlyPlayer = null;
            }

            TheOnlyPlayer = new VideoPlayer();
            TheOnlyPlayer.webBrowser.Url = new Uri(url);
            TheOnlyPlayer.Show();
        }

        private void VideoPlayer_FormClosed(object sender, FormClosedEventArgs e)
        {
            TheOnlyPlayer = null;
        }
    }
}
