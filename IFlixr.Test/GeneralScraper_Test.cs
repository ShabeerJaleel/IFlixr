using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IFlixr.WebScraper;

namespace IFlixr.Test
{
    [TestClass]
    public class GeneralScraper_Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            new GenericScraper().Crawl();
        }
    }
}
