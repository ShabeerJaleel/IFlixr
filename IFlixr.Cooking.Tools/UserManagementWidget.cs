using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IFlixr.Cooking.Data;
using IFlixr.Cooking.Repository;

namespace IFlixr.Cooking.Tools
{
    public partial class UserManagementWidget : UserControl
    {
        public UserManagementWidget()
        {
            InitializeComponent();
            Init();
            
        }

        public void Init()
        {
            if (System.ComponentModel.LicenseManager.CurrentContext.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;
            this.comboBoxGender.SelectedIndex = 0;
            var tags = RavenRepository.Create().GetCuisineTags();
            this.checkedListBoxTags.Items.Clear();
            foreach (var tag in tags)
            {
                checkedListBoxTags.Items.Add(tag);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var tags = new List<string>();
                foreach (var item in checkedListBoxTags.SelectedItems)
                {
                    tags.Add(item.ToString());
                }

                var author = new User
                {
                    FirstName = textBoxFirstName.Text.Trim(),
                    LastName = textBoxLastName.Text.Trim(),
                    ProfileName = textBoxProfileName.Text.Trim(),
                    Email = textBoxEmail.Text.Trim(),
                    Gender = comboBoxGender.Text == "Male" ? "m" : "f",
                    HasLocalAccount = true,
                    SystemCreated = this.checkBoxSystemCreated.Checked,
                    Password = textBoxPassword.Text.Trim(),
                    RegisteredOn = dateTimePickerCreatedOn.Value,
                    LoginEnabled = checkBoxLoginEnabled.Checked,
                    ProfileUrl = textBoxProfileUrl.Text.Trim(),
                    Tags = tags,
                    Sites = new List<WebSite>{ new WebSite
                        {
                            Url = textBoxWebSite.Text.Trim(),
                            ScrappingRootUrl = textBoxScrapUrl.Text.Trim(),
                            FeedUrl = textBoxFeedUrl.Text.Trim(),
                            ImageCss = textBoxImageCss.Text.Trim(),
                            LastFeededTime = dateTimePickerLastFeededOn.Value,
                            LastScrappedTime = dateTimePickerLastScrapedOn.Value,
                            FeedScrapeEnabled = true
                        }
                    }
                };
                RavenRepository.Create().CreateUser(author);
                this.textBoxID.Text = author.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxProfileName_TextChanged(object sender, EventArgs e)
        {
            this.textBoxProfileUrl.Text = this.textBoxProfileName.Text.Replace(' ', '-');
        }

       
    }
}
