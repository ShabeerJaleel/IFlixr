namespace IFlixr.WebScraper.Client
{
    partial class ValidatorForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonValidateScrapperDB = new System.Windows.Forms.Button();
            this.buttonValidateLiveDB = new System.Windows.Forms.Button();
            this.tabControlResults = new System.Windows.Forms.TabControl();
            this.tabPageScrapper = new System.Windows.Forms.TabPage();
            this.tabPageLive = new System.Windows.Forms.TabPage();
            this.dataGridViewScrapper = new System.Windows.Forms.DataGridView();
            this.videoModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.selectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.durationInMinutesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uploaderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publishedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.videoIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.indexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.playlistSequenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.playlistIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlResults.SuspendLayout();
            this.tabPageScrapper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScrapper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonValidateLiveDB);
            this.splitContainer1.Panel1.Controls.Add(this.buttonValidateScrapperDB);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlResults);
            this.splitContainer1.Size = new System.Drawing.Size(1199, 659);
            this.splitContainer1.SplitterDistance = 49;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonValidateScrapperDB
            // 
            this.buttonValidateScrapperDB.Location = new System.Drawing.Point(3, 3);
            this.buttonValidateScrapperDB.Name = "buttonValidateScrapperDB";
            this.buttonValidateScrapperDB.Size = new System.Drawing.Size(118, 35);
            this.buttonValidateScrapperDB.TabIndex = 0;
            this.buttonValidateScrapperDB.Text = "Validate Scrapper DB";
            this.buttonValidateScrapperDB.UseVisualStyleBackColor = true;
            this.buttonValidateScrapperDB.Click += new System.EventHandler(this.buttonValidateScrapperDB_Click);
            // 
            // buttonValidateLiveDB
            // 
            this.buttonValidateLiveDB.Location = new System.Drawing.Point(127, 3);
            this.buttonValidateLiveDB.Name = "buttonValidateLiveDB";
            this.buttonValidateLiveDB.Size = new System.Drawing.Size(118, 35);
            this.buttonValidateLiveDB.TabIndex = 1;
            this.buttonValidateLiveDB.Text = "Validate Live DB";
            this.buttonValidateLiveDB.UseVisualStyleBackColor = true;
            // 
            // tabControlResults
            // 
            this.tabControlResults.Controls.Add(this.tabPageScrapper);
            this.tabControlResults.Controls.Add(this.tabPageLive);
            this.tabControlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlResults.Location = new System.Drawing.Point(0, 0);
            this.tabControlResults.Name = "tabControlResults";
            this.tabControlResults.SelectedIndex = 0;
            this.tabControlResults.Size = new System.Drawing.Size(1199, 606);
            this.tabControlResults.TabIndex = 0;
            // 
            // tabPageScrapper
            // 
            this.tabPageScrapper.Controls.Add(this.dataGridViewScrapper);
            this.tabPageScrapper.Location = new System.Drawing.Point(4, 22);
            this.tabPageScrapper.Name = "tabPageScrapper";
            this.tabPageScrapper.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageScrapper.Size = new System.Drawing.Size(1191, 580);
            this.tabPageScrapper.TabIndex = 0;
            this.tabPageScrapper.Text = "Scrapper";
            this.tabPageScrapper.UseVisualStyleBackColor = true;
            // 
            // tabPageLive
            // 
            this.tabPageLive.Location = new System.Drawing.Point(4, 22);
            this.tabPageLive.Name = "tabPageLive";
            this.tabPageLive.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLive.Size = new System.Drawing.Size(1191, 580);
            this.tabPageLive.TabIndex = 1;
            this.tabPageLive.Text = "Live";
            this.tabPageLive.UseVisualStyleBackColor = true;
            // 
            // dataGridViewScrapper
            // 
            this.dataGridViewScrapper.AutoGenerateColumns = false;
            this.dataGridViewScrapper.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScrapper.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectedDataGridViewCheckBoxColumn,
            this.titleDataGridViewTextBoxColumn,
            this.durationInMinutesDataGridViewTextBoxColumn,
            this.uploaderDataGridViewTextBoxColumn,
            this.publishedDateDataGridViewTextBoxColumn,
            this.videoIdDataGridViewTextBoxColumn,
            this.indexDataGridViewTextBoxColumn,
            this.playlistSequenceDataGridViewTextBoxColumn,
            this.playlistIdDataGridViewTextBoxColumn,
            this.typeDataGridViewTextBoxColumn,
            this.sourceDataGridViewTextBoxColumn});
            this.dataGridViewScrapper.DataSource = this.videoModelBindingSource;
            this.dataGridViewScrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewScrapper.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewScrapper.Name = "dataGridViewScrapper";
            this.dataGridViewScrapper.Size = new System.Drawing.Size(1185, 574);
            this.dataGridViewScrapper.TabIndex = 0;
            this.dataGridViewScrapper.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewScrapper_CellContentClick);
            // 
            // videoModelBindingSource
            // 
            this.videoModelBindingSource.DataSource = typeof(IFlixr.WebScraper.Client.Model.VideoModel);
            // 
            // selectedDataGridViewCheckBoxColumn
            // 
            this.selectedDataGridViewCheckBoxColumn.DataPropertyName = "Selected";
            this.selectedDataGridViewCheckBoxColumn.HeaderText = "Selected";
            this.selectedDataGridViewCheckBoxColumn.Name = "selectedDataGridViewCheckBoxColumn";
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            // 
            // durationInMinutesDataGridViewTextBoxColumn
            // 
            this.durationInMinutesDataGridViewTextBoxColumn.DataPropertyName = "DurationInMinutes";
            this.durationInMinutesDataGridViewTextBoxColumn.HeaderText = "DurationInMinutes";
            this.durationInMinutesDataGridViewTextBoxColumn.Name = "durationInMinutesDataGridViewTextBoxColumn";
            this.durationInMinutesDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uploaderDataGridViewTextBoxColumn
            // 
            this.uploaderDataGridViewTextBoxColumn.DataPropertyName = "Uploader";
            this.uploaderDataGridViewTextBoxColumn.HeaderText = "Uploader";
            this.uploaderDataGridViewTextBoxColumn.Name = "uploaderDataGridViewTextBoxColumn";
            // 
            // publishedDateDataGridViewTextBoxColumn
            // 
            this.publishedDateDataGridViewTextBoxColumn.DataPropertyName = "PublishedDate";
            this.publishedDateDataGridViewTextBoxColumn.HeaderText = "PublishedDate";
            this.publishedDateDataGridViewTextBoxColumn.Name = "publishedDateDataGridViewTextBoxColumn";
            // 
            // videoIdDataGridViewTextBoxColumn
            // 
            this.videoIdDataGridViewTextBoxColumn.DataPropertyName = "VideoId";
            this.videoIdDataGridViewTextBoxColumn.HeaderText = "VideoId";
            this.videoIdDataGridViewTextBoxColumn.Name = "videoIdDataGridViewTextBoxColumn";
            this.videoIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.videoIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // indexDataGridViewTextBoxColumn
            // 
            this.indexDataGridViewTextBoxColumn.DataPropertyName = "Index";
            this.indexDataGridViewTextBoxColumn.HeaderText = "Index";
            this.indexDataGridViewTextBoxColumn.Name = "indexDataGridViewTextBoxColumn";
            // 
            // playlistSequenceDataGridViewTextBoxColumn
            // 
            this.playlistSequenceDataGridViewTextBoxColumn.DataPropertyName = "PlaylistSequence";
            this.playlistSequenceDataGridViewTextBoxColumn.HeaderText = "PlaylistSequence";
            this.playlistSequenceDataGridViewTextBoxColumn.Name = "playlistSequenceDataGridViewTextBoxColumn";
            // 
            // playlistIdDataGridViewTextBoxColumn
            // 
            this.playlistIdDataGridViewTextBoxColumn.DataPropertyName = "PlaylistId";
            this.playlistIdDataGridViewTextBoxColumn.HeaderText = "PlaylistId";
            this.playlistIdDataGridViewTextBoxColumn.Name = "playlistIdDataGridViewTextBoxColumn";
            // 
            // typeDataGridViewTextBoxColumn
            // 
            this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
            this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
            // 
            // sourceDataGridViewTextBoxColumn
            // 
            this.sourceDataGridViewTextBoxColumn.DataPropertyName = "Source";
            this.sourceDataGridViewTextBoxColumn.HeaderText = "Source";
            this.sourceDataGridViewTextBoxColumn.Name = "sourceDataGridViewTextBoxColumn";
            // 
            // ValidatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 659);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ValidatorForm";
            this.Text = "Video Validator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlResults.ResumeLayout(false);
            this.tabPageScrapper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScrapper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonValidateScrapperDB;
        private System.Windows.Forms.Button buttonValidateLiveDB;
        private System.Windows.Forms.TabControl tabControlResults;
        private System.Windows.Forms.TabPage tabPageScrapper;
        private System.Windows.Forms.TabPage tabPageLive;
        private System.Windows.Forms.DataGridView dataGridViewScrapper;
        private System.Windows.Forms.BindingSource videoModelBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn durationInMinutesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uploaderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publishedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewLinkColumn videoIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn playlistSequenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn playlistIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceDataGridViewTextBoxColumn;
    }
}