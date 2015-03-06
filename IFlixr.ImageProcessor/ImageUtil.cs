using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageMagick;
using System.IO;
using System.Drawing;

namespace IFlixr.ImageProcessor
{
    public class ImageUtil
    {
        public MagickImage Crop(string filePath, CropDimension dim)
        {
            MagickImage img = new MagickImage(filePath);
            var x = (int) (img.Width / 100.0 * dim.Left);
            var y = (int)(img.Height / 100.0 * dim.Top);
            var width = (int) (img.Width - x - ((img.Width / 100.0) * dim.Right));
            var height = (int) (img.Height - y - ((img.Height / 100.0) * dim.Bottom));
          
            img.Crop(new MagickGeometry(x, y, width,height));
            return img;
        }

        public void Crop(MagickImage img, CropDimension dim)
        {
            var x = (int)(img.Width / 100.0 * dim.Left);
            var y = (int)(img.Height / 100.0 * dim.Top);
            var width = (int)(img.Width - x - ((img.Width / 100.0) * dim.Right));
            var height = (int)(img.Height - y - ((img.Height / 100.0) * dim.Bottom));

            img.Crop(new MagickGeometry(x, y, width, height));
        }

        public MagickImage Resize(string filePath, ResizeDimension dim)
        {
            MagickImage img = new MagickImage(filePath);
            if (!IsResizeRequired(img, dim))
                return img;
            if (!IsResizable(img, dim))
                return null;

            if (DoNormalResize(img, dim))
                return img;

            return null;
          
        }

        public MagickImage ResizeBestFit(MagickImage img, ResizeDimension dim)
        {
            if (!IsResizeRequired(img, dim))
                return img;
            if (!IsResizable(img, dim))
                return null;

            if (DoNormalResize(img, dim))
                return img;

            if (img.Width >= img.Height)
            {
                var hPer = CalculateResizePercent(img.Height, dim.MaxHeight);
                hPer = hPer > 100 ? CalculateResizePercent(img.Height, dim.MinHeight) : hPer;
                var height = (int) Math.Ceiling((img.Height / 100.0) * hPer);
                var width = (int) Math.Ceiling((img.Width / 100.0) * hPer);
                img.Resize(new MagickGeometry(width, height));

                var percent = 100 - CalculateResizePercent(img.Width, dim.MaxWidth);
                
                //crop has bug. create new
                img = new MagickImage(img.ToByteArray());
                Crop(img, new CropDimension
                {
                    Left = percent / 2,
                    Right = percent / 2
                });
            }
            else
            {
                var hPer = CalculateResizePercent(img.Width, dim.MaxWidth);
                hPer = hPer > 100 ? CalculateResizePercent(img.Width, dim.MinWidth) : hPer;
                var height = (int)Math.Ceiling((img.Height / 100.0) * hPer);
                var width = (int)Math.Ceiling((img.Width / 100.0) * hPer);
                img.Resize(new MagickGeometry(width, height));

                var percent = 100 - CalculateResizePercent(img.Height, dim.MaxHeight);

                //crop has bug. create new
                img = new MagickImage(img.ToByteArray());
                Crop(img, new CropDimension
                {
                    Top = percent / 2,
                    Bottom = percent / 2
                });
            }

            return img;
        }

        public void Saturate(MagickImage img,int percent )
        {
            if (percent == 100)
                return;
            img.Modulate(100, percent, 100);
        }

        public void Sharpen(MagickImage img)
        {
            img.AdaptiveSharpen();
        }

        public MagickImage Process(string filePath, CropDimension cDim, ResizeDimension rDim,
            int saturatePercent, int maxSize = -1)
        {
            var img = new MagickImage(filePath);
            if (cDim.Top > 0 || cDim.Right > 0 || cDim.Bottom > 0 || cDim.Left > 0)
                Crop(img, cDim);
            img = ResizeBestFit(new MagickImage(img.ToByteArray()), rDim);
            if (img != null)
            {
                //Saturate(img, saturatePercent);
                //Sharpen(img);
                //Optimize(img, maxSize);
                //img.BorderColor = new MagickColor(Color.Black);
                //img.Border(1);
            }
            return img;
        }
        
        public void Optimize(MagickImage img, int maxSize)
        {
            if (maxSize == -1 || GetSize(img) <= maxSize)
                return;
          
            for(var i = 90; i >= 40; i -= 3)
            {
                img.Quality = i;
                if(GetSize(img) <= maxSize)
                    break;
            }

        }

        public long GetSize(MagickImage img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Write(stream);
                return stream.Length;
            }
        }

        #region Private

        private bool DoNormalResize(MagickImage img, ResizeDimension dim)
        {
            if (img.Width > dim.MaxWidth)
            {
                var resizePercentage = CalculateResizePercent(img.Width, dim.MaxWidth);
                var newHeight = CalculateValue(img.Height, resizePercentage);
                if (newHeight >= dim.MinHeight && newHeight <= dim.MaxHeight)
                {
                    img.Resize(new MagickGeometry(new Percentage((int)resizePercentage), new Percentage((int)resizePercentage)));
                    return true;
                }
            }

            if (img.Height > dim.MaxHeight)
            {
                var resizePercentage = CalculateResizePercent(img.Height, dim.MaxHeight);
                var newWidth = CalculateValue(img.Width, resizePercentage);
                if (newWidth >= dim.MinWidth && newWidth <= dim.MaxWidth)
                {
                    img.Scale(new MagickGeometry(new Percentage((int)resizePercentage), new Percentage((int)resizePercentage)));
                    return true;
                }
            }
            return false;
        }

        private bool IsResizeRequired(MagickImage img, ResizeDimension dim)
        {
            if (img.Width >= dim.MinWidth && img.Width <= dim.MaxWidth &&
               img.Height >= dim.MinHeight && img.Height <= dim.MaxHeight)
                return false;
            return true;
        }

        private bool IsResizable(MagickImage img, ResizeDimension dim)
        {
            if (img.Width < dim.MinWidth ||
                img.Height < dim.MinHeight)
                return false;
            return true;
        }

        private float CalculateResizePercent(float value,  float maxValue)
        {
            return (float)(100.0 - ((value - maxValue) / value) * 100.0);
        }

        private float CalculateValue( float value, float percent)
        {
            return (float) ((value / 100.0) * percent);
        }

        #endregion

    }

    public class CropDimension
    {
        public CropDimension()
        {

        }
        public CropDimension(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public CropDimension(int top, int bottom)
            :this(top, 0, bottom, 0)
        {
        }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
    }

    public class ResizeDimension
    {
        public ResizeDimension()
        {

        }
        public ResizeDimension(int minWidth, int maxWidth,int minHeight, int maxHeight)
        {
            MinWidth = minWidth;
            MinHeight = minHeight;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
        }
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
    }
}
