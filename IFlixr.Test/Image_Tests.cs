using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IFlixr.ImageProcessor;
using System.Drawing;
using ImageMagick;

namespace IFlixr.Test
{
    [TestClass]
    public class Image_Tests
    {
        [TestMethod]
        public void CropTest()
        {
            var img = new MagickImage(@"c:\test.jpg");
            new ImageUtil().Crop(img, new CropDimension
            { 
                 Top = 10,
                 Bottom = 10,
                 Left = 10,
                 Right = 10
            });
            img.ToBitmap().Save(@"c:\test_crop.jpg");
        }

        [TestMethod]
        public void CompeteImageTest()
        {
            var util = new ImageUtil();
            var img = util.Process(@"c:\test.jpg", new CropDimension(),
            new ResizeDimension(270, 270, 270, 270),
            140, 20 * 1024);
            if(img != null)
                img.Write(@"c:\test_complete1.jpg");
        }

        [TestMethod]
        public void ResizeTest()
        {
            var img = new ImageUtil().Resize(@"c:\Chai1.jpg", new ResizeDimension
            {
               MinWidth = 270,
               MaxWidth = 300,
               MinHeight = 270,
               MaxHeight = 300
            });
            img.Write(@"c:\test_crop.jpg");
        }

        [TestMethod]
        public void ResizeBestFitTest()
        {
            var img = new MagickImage(@"c:\test.jpg");
            new ImageUtil().ResizeBestFit(img, new ResizeDimension
            {
                MinWidth = 270,
                MaxWidth = 270,
                MinHeight = 270,
                MaxHeight = 270
            });
            img.Write(@"c:\test_crop270.jpg");
            img = new MagickImage(@"c:\test.jpg");
            new ImageUtil().ResizeBestFit(img, new ResizeDimension
            {
                MinWidth = 270,
                MaxWidth = 300,
                MinHeight = 270,
                MaxHeight = 300
            });
            img.Write(@"c:\test_crop300.jpg");
        }

        [TestMethod]
        public void SaturationTest()
        {
            var util = new ImageUtil();
            var img = new MagickImage(@"c:\test.jpg");
            util.ResizeBestFit(img, new ResizeDimension
            {
                MinWidth = 270,
                MaxWidth = 270,
                MinHeight = 270,
                MaxHeight = 270
            });
            util.Saturate(img, 120);
            img.Write(@"c:\test_saturation.jpg");
        }
    }
}
