using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IFlixr.WebScraper.Client.Model;
using IFlixr.Data;
using IFlixr.Base;

namespace IFlixr.WebScraper.Client
{
    public partial class SelectablePictureBox : UserControl
    {
        private string fileName;
        public SelectablePictureBox()
        {
            InitializeComponent();
        }

        public SelectablePictureBox(ImageModel model, bool displayOptions)
            : this()
        {
            Init(model.FullPath, displayOptions);
            if (displayOptions)
            {
                this.checkBoxMedium.Checked = (model.SupportedDimensions & (int)ImageDimension.Medium_W220XH320) == 1;
                this.checkBoxSmall.Checked = (model.SupportedDimensions & (int)ImageDimension.Small_W120XH174) == 2;
                this.checkBoxThumb.Checked = (model.SupportedDimensions & (int)ImageDimension.Thumb_W120XH174) == 4;
                this.checkBoxDefault.Checked = model.IsDefault;
            }
        }

        private void Init(string fileName, bool displayOptions)
        {
            this.fileName = fileName;
            this.pictureBox.Image = System.Drawing.Image.FromFile(fileName);
            this.checkBoxMedium.Visible = this.checkBoxSmall.Visible = this.checkBoxThumb.Visible = this.checkBoxDefault.Visible = displayOptions;
            
        }

        public bool Selected
        {
            get { return this.checkBoxSelect.Checked; }
        }

        public string FileName
        {
            get { return this.fileName; }
        }

        public bool Medium { get { return this.checkBoxMedium.Checked; } }
        public bool Small { get { return this.checkBoxSmall.Checked; } }
        public bool Thumbnail { get { return this.checkBoxThumb.Checked; } }
        public bool Default { get { return this.checkBoxDefault.Checked; } }

        public int ImageWidth { get { return this.pictureBox.Image.Width; } }
        public int ImageHeight { get { return this.pictureBox.Image.Height; } }
    }
}
