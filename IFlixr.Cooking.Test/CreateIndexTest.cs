using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IFlixr.Cooking.Repository;

namespace IFlixr.Cooking.Test
{
    [TestClass]
    public class CreateIndexTest
    {
        [TestMethod]
        public void CreateAllIndexes()
        {
            RavenRepository.Create().CreateIndexes();
        }
    }
}
