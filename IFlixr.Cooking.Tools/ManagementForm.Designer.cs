namespace IFlixr.Cooking.Tools
{
    partial class ManagementForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageApproval = new System.Windows.Forms.TabPage();
            this.tabPageFeedScrape = new System.Windows.Forms.TabPage();
            this.tabPageUserManagement = new System.Windows.Forms.TabPage();
            this.tabPageCuisine = new System.Windows.Forms.TabPage();
            this.approvalWidget1 = new IFlixr.Cooking.Tools.ApprovalWidget();
            this.scraperWidget1 = new IFlixr.Cooking.Tools.ScraperWidget();
            this.userManagementWidget = new IFlixr.Cooking.Tools.UserManagementWidget();
            this.cuisineManagementWidget1 = new IFlixr.Cooking.Tools.CuisineManagementWidget();
            this.tabControl1.SuspendLayout();
            this.tabPageApproval.SuspendLayout();
            this.tabPageFeedScrape.SuspendLayout();
            this.tabPageUserManagement.SuspendLayout();
            this.tabPageCuisine.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageApproval);
            this.tabControl1.Controls.Add(this.tabPageFeedScrape);
            this.tabControl1.Controls.Add(this.tabPageUserManagement);
            this.tabControl1.Controls.Add(this.tabPageCuisine);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1482, 785);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageApproval
            // 
            this.tabPageApproval.Controls.Add(this.approvalWidget1);
            this.tabPageApproval.Location = new System.Drawing.Point(4, 22);
            this.tabPageApproval.Name = "tabPageApproval";
            this.tabPageApproval.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApproval.Size = new System.Drawing.Size(1474, 759);
            this.tabPageApproval.TabIndex = 0;
            this.tabPageApproval.Text = "Approve Recipe";
            this.tabPageApproval.UseVisualStyleBackColor = true;
            // 
            // tabPageFeedScrape
            // 
            this.tabPageFeedScrape.Controls.Add(this.scraperWidget1);
            this.tabPageFeedScrape.Location = new System.Drawing.Point(4, 22);
            this.tabPageFeedScrape.Name = "tabPageFeedScrape";
            this.tabPageFeedScrape.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFeedScrape.Size = new System.Drawing.Size(1474, 759);
            this.tabPageFeedScrape.TabIndex = 1;
            this.tabPageFeedScrape.Text = "Scrape Feeds";
            this.tabPageFeedScrape.UseVisualStyleBackColor = true;
            // 
            // tabPageUserManagement
            // 
            this.tabPageUserManagement.Controls.Add(this.userManagementWidget);
            this.tabPageUserManagement.Location = new System.Drawing.Point(4, 22);
            this.tabPageUserManagement.Name = "tabPageUserManagement";
            this.tabPageUserManagement.Size = new System.Drawing.Size(1474, 759);
            this.tabPageUserManagement.TabIndex = 2;
            this.tabPageUserManagement.Text = "User Management";
            this.tabPageUserManagement.UseVisualStyleBackColor = true;
            // 
            // tabPageCuisine
            // 
            this.tabPageCuisine.Controls.Add(this.cuisineManagementWidget1);
            this.tabPageCuisine.Location = new System.Drawing.Point(4, 22);
            this.tabPageCuisine.Name = "tabPageCuisine";
            this.tabPageCuisine.Size = new System.Drawing.Size(1474, 759);
            this.tabPageCuisine.TabIndex = 3;
            this.tabPageCuisine.Text = "Cuisine Management";
            this.tabPageCuisine.UseVisualStyleBackColor = true;
            // 
            // approvalWidget1
            // 
            this.approvalWidget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.approvalWidget1.Location = new System.Drawing.Point(3, 3);
            this.approvalWidget1.Name = "approvalWidget1";
            this.approvalWidget1.Size = new System.Drawing.Size(1468, 753);
            this.approvalWidget1.TabIndex = 0;
            // 
            // scraperWidget1
            // 
            this.scraperWidget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scraperWidget1.Location = new System.Drawing.Point(3, 3);
            this.scraperWidget1.Name = "scraperWidget1";
            this.scraperWidget1.Size = new System.Drawing.Size(1468, 753);
            this.scraperWidget1.TabIndex = 0;
            // 
            // userManagementWidget
            // 
            this.userManagementWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userManagementWidget.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userManagementWidget.Location = new System.Drawing.Point(0, 0);
            this.userManagementWidget.Name = "userManagementWidget";
            this.userManagementWidget.Size = new System.Drawing.Size(1474, 759);
            this.userManagementWidget.TabIndex = 0;
            // 
            // cuisineManagementWidget1
            // 
            this.cuisineManagementWidget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cuisineManagementWidget1.Location = new System.Drawing.Point(0, 0);
            this.cuisineManagementWidget1.Name = "cuisineManagementWidget1";
            this.cuisineManagementWidget1.Size = new System.Drawing.Size(1474, 759);
            this.cuisineManagementWidget1.TabIndex = 0;
            // 
            // ManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1482, 785);
            this.Controls.Add(this.tabControl1);
            this.Name = "ManagementForm";
            this.Text = "ManagementForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabControl1.ResumeLayout(false);
            this.tabPageApproval.ResumeLayout(false);
            this.tabPageFeedScrape.ResumeLayout(false);
            this.tabPageUserManagement.ResumeLayout(false);
            this.tabPageCuisine.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageApproval;
        private ApprovalWidget approvalWidget1;
        private System.Windows.Forms.TabPage tabPageFeedScrape;
        private ScraperWidget scraperWidget1;
        private System.Windows.Forms.TabPage tabPageUserManagement;
        private UserManagementWidget userManagementWidget;
        private System.Windows.Forms.TabPage tabPageCuisine;
        private CuisineManagementWidget cuisineManagementWidget1;

    }
}