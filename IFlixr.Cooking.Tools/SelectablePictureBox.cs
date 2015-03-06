using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IFlixr.Cooking.Data;
using System.IO;
using IFlixr.Cooking.Base;

namespace IFlixr.Cooking.Tools
{
    public partial class SelectablePictureBox : UserControl
    {
        public event EventHandler<SelectChangedEventArgs> SelectChanged;
        private RecipeImage image;
        public SelectablePictureBox()
        {
            InitializeComponent();
        }

        public SelectablePictureBox(RecipeImage ri)
            : this()
        {
            this.image = ri;
            Init(Path.Combine(GlobalSettings.ImageDownloadFolder, ri.ImageThumbnail));
        }

        private void Init(string imageUrl)
        {
            this.pictureBox.Load(imageUrl);
            
        }

        public bool Selected
        {
            get { return this.checkBoxSelect.Checked; }
            set { this.checkBoxSelect.Checked = false; }
        }

        public int ImageWidth { get { return this.pictureBox.Image.Width; } }
        public int ImageHeight { get { return this.pictureBox.Image.Height; } }

        private void checkBoxSelect_CheckedChanged(object sender, EventArgs e)
        {
            this.image.IsDefault = this.checkBoxSelect.Checked;
            if (SelectChanged != null)
                this.SelectChanged(this, new SelectChangedEventArgs(this.checkBoxSelect.Checked));
        }
    }

    public class SelectChangedEventArgs : EventArgs
    {
        public SelectChangedEventArgs (bool selected)
	    {
            Selected = selected;
	    }

        public bool Selected { get; set; }
    }
}
