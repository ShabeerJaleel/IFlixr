using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AutoMapper;
using IFlixr.WebScraper.Client.Model;
using IFlixr.Data;

namespace IFlixr.WebScraper.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread] 
        static void Main()
        {
          
            Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static void Init()
        {
            Mapper.CreateMap<VideoInfo, VideoModel>();
            Mapper.CreateMap<IFlixr.Data.Movie, Movie>();
           // Mapper.CreateMap<List<YouTubeVideo>, List<VideoModel>>();
        }
    }
}
