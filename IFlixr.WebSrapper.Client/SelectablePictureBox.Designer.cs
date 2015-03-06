namespace IFlixr.WebScraper.Client
{
    partial class SelectablePictureBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxSelect = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.checkBoxMedium = new System.Windows.Forms.CheckBox();
            this.checkBoxSmall = new System.Windows.Forms.CheckBox();
            this.checkBoxThumb = new System.Windows.Forms.CheckBox();
            this.checkBoxDefault = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxSelect
            // 
            this.checkBoxSelect.AutoSize = true;
            this.checkBoxSelect.Location = new System.Drawing.Point(3, 0);
            this.checkBoxSelect.Name = "checkBoxSelect";
            this.checkBoxSelect.Size = new System.Drawing.Size(55, 17);
            this.checkBoxSelect.TabIndex = 0;
            this.checkBoxSelect.Text = "Select";
            this.checkBoxSelect.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxSelect);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxDefault);
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxThumb);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox);
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxSmall);
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxMedium);
            this.splitContainer1.Size = new System.Drawing.Size(291, 354);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(220, 320);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // checkBoxMedium
            // 
            this.checkBoxMedium.AutoSize = true;
            this.checkBoxMedium.Location = new System.Drawing.Point(229, 3);
            this.checkBoxMedium.Name = "checkBoxMedium";
            this.checkBoxMedium.Size = new System.Drawing.Size(62, 17);
            this.checkBoxMedium.TabIndex = 1;
            this.checkBoxMedium.Text = "Medium";
            this.checkBoxMedium.UseVisualStyleBackColor = true;
            // 
            // checkBoxSmall
            // 
            this.checkBoxSmall.AutoSize = true;
            this.checkBoxSmall.Location = new System.Drawing.Point(229, 26);
            this.checkBoxSmall.Name = "checkBoxSmall";
            this.checkBoxSmall.Size = new System.Drawing.Size(50, 17);
            this.checkBoxSmall.TabIndex = 2;
            this.checkBoxSmall.Text = "Small";
            this.checkBoxSmall.UseVisualStyleBackColor = true;
            // 
            // checkBoxThumb
            // 
            this.checkBoxThumb.AutoSize = true;
            this.checkBoxThumb.Location = new System.Drawing.Point(229, 49);
            this.checkBoxThumb.Name = "checkBoxThumb";
            this.checkBoxThumb.Size = new System.Drawing.Size(58, 17);
            this.checkBoxThumb.TabIndex = 3;
            this.checkBoxThumb.Text = "Thumb";
            this.checkBoxThumb.UseVisualStyleBackColor = true;
            // 
            // checkBoxDefault
            // 
            this.checkBoxDefault.AutoSize = true;
            this.checkBoxDefault.Location = new System.Drawing.Point(229, 72);
            this.checkBoxDefault.Name = "checkBoxDefault";
            this.checkBoxDefault.Size = new System.Drawing.Size(61, 17);
            this.checkBoxDefault.TabIndex = 4;
            this.checkBoxDefault.Text = "Default";
            this.checkBoxDefault.UseVisualStyleBackColor = true;
            // 
            // SelectablePictureBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SelectablePictureBox";
            this.Size = new System.Drawing.Size(291, 354);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSelect;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.CheckBox checkBoxThumb;
        private System.Windows.Forms.CheckBox checkBoxSmall;
        private System.Windows.Forms.CheckBox checkBoxMedium;
        private System.Windows.Forms.CheckBox checkBoxDefault;
    }
}
