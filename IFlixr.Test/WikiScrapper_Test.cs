using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IFlixr.WebScraper;
using System.Diagnostics;


namespace IFlixr.Test
{
    [TestClass]
    public class WikiScrapper_Test
    {
        [TestMethod]
        public void TestGetMovieNamesUsingOnePageForMovie()
        {
            var scrapper = new WikiScraper();
            var movieNames = scrapper.GetMovieNames(
                scrapper.GetNodesUsingCss("http://en.wikipedia.org/wiki/Malayalam_films_of_2009", 
                "table"), 2009);
            //for (var i = 0; i < movieNames.Count; i++ )
            //{
            //    var mn = movieNames[i];
            //    Debug.WriteLine(String.Format("{0} => Title: {1}, Link: {2}",i, mn.Name, mn.WikiLink));
            //}

            scrapper.GetMovies(movieNames);
        }

        [TestMethod]
        public void TestGetMovieNamesUsingOnePageForMultipleMovies()
        {
            var scrapper = new WikiScraper();
            var movieNames = scrapper.GetMovieNames(
                scrapper.GetNodesUsingXPath("http://en.wikipedia.org/wiki/Malayalam_films_of_2004", "div[3]/div[3]/div[4]/table[8]"),
                2004);
            for (var i = 0; i < movieNames.Count; i++)
            {
                var mn = movieNames[i];
                Debug.WriteLine(String.Format("{0} => Title: {1}, Link: {2}", i, mn.Name, mn.WikiUrl));
            }
        }
    }
}
