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
using IFlixr.Cooking.Base;
using System.IO;

namespace IFlixr.Cooking.Tools
{
    public partial class ApprovalWidget : UserControl
    {
        private Recipe activeRecipe;
        private List<string> cuisineTags;
        private KnownRecipeTagCollection wellknownTagCollection;
        public ApprovalWidget()
        {
            InitializeComponent();
        }
        
        public void Init()
        {

            if (System.ComponentModel.LicenseManager.CurrentContext.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;
            this.cuisineTags = RavenRepository.Create().GetCuisineTags();
            this.wellknownTagCollection = RavenRepository.Create().GetKnownRecipeTags();
            LoadNext();

        }

        private void UpdateTagsUI(Recipe recipe)
        {
            this.checkedListBoxTags.Items.Clear();
            this.checkedListBoxSelectedTags.Items.Clear();
            var title = recipe.Title.ToLower();
            var nonSTags = new List<string>();
            foreach (var tag in this.wellknownTagCollection.KnowsTags)
            {
                if (title.Contains(tag.Name) && !recipe.ActiveTags.Any(x => x == tag.Name))
                    recipe.ActiveTags.Add(tag.Name);
            }

            var tokens = title.Split(' ');
            foreach (var t in tokens)
            {
                if(t.Length > 3 && !recipe.ActiveTags.Any(x => x == t.ToLower()))
                    nonSTags.Add(t.ToLower());
            }
            
            foreach (var tag in this.cuisineTags)
            {
                if(!recipe.ActiveTags.Contains(tag) && !nonSTags.Any(x => x == tag))
                     nonSTags.Add(tag);
            }

            foreach (var tag in recipe.Tags)
            {
                if (!recipe.ActiveTags.Contains(tag))
                {
                    if (title.Contains(tag) || this.wellknownTagCollection.KnowsTags.Any(x => x.Name == tag))
                        recipe.ActiveTags.Add(tag);
                    else if(!nonSTags.Any(x => x == tag))
                        nonSTags.Add(tag);
                }
            }

            foreach (var tag in recipe.ActiveTags)
                this.checkedListBoxSelectedTags.Items.Add(tag, true);
            foreach(var tag in nonSTags)
                this.checkedListBoxTags.Items.Add(tag);

            
        }

        private  void LoadRecipie(Recipe recipe)
        {

            MainFrm.LoadUrl(recipe.Url);
            this.linkUrl.Text = String.Empty;
            this.textBoxTitle.Text = String.Empty;
            UpdateTagsUI(recipe);

            this.flpPictures.Controls.Clear();

            if (recipe == null)
            {
                MessageBox.Show("No more recipe for approval", "Tools");
                return;
            }
            
            this.linkUrl.Text = recipe.Url;
            this.textBoxTitle.Text = recipe.Title;
            foreach (var pic in recipe.Images)
            {
                var pBox = new SelectablePictureBox(pic);
                pBox.SelectChanged += delegate(object sender, SelectChangedEventArgs e)
                {
                    if (e.Selected)
                    {
                        foreach (SelectablePictureBox control in this.flpPictures.Controls)
                        {
                            if (control != (SelectablePictureBox)sender)
                                control.Selected = false;
                        }
                    }
                };

                this.flpPictures.Controls.Add(pBox);
            }
            this.activeRecipe = recipe;
        }

      

        private void buttonNext_Click(object sender, EventArgs e)
        {
            LoadNext();
        }

        private void LoadNext()
        {
            if (activeRecipe != null)
            {
                if (!activeRecipe.Images.Any(x => x.IsDefault))
                {
                    MessageBox.Show("Please select picture"); 
                    return;
                }
                activeRecipe.ApprovalStatus = ApprovalStage.Approved;

                //copy the image
                foreach (var img in activeRecipe.Images.Where(x => x.IsDefault))
                {
                    var folder = PathBuilder.GetRecipeImageFolder(img.ImageOriginal);
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    File.Copy(Path.Combine(GlobalSettings.ImageDownloadFolder, img.ImageThumbnail),
                        PathBuilder.GetRecipeImagePath(img.ImageThumbnail), true);
                    File.Copy(Path.Combine(GlobalSettings.ImageDownloadFolder, img.ImageOriginal),
                        PathBuilder.GetRecipeImagePath(img.ImageOriginal), true);
                }
                foreach (var tag in this.activeRecipe.ActiveTags)
                {
                    if (!this.wellknownTagCollection.KnowsTags.Any(x => x.Name == tag))
                        this.wellknownTagCollection.KnowsTags.Add(new KnownRecipeTag
                        {
                            Name = tag
                        });
                }
                
                RavenRepository.Create().SaveRecipeAndTags(this.activeRecipe);
                activeRecipe = null;
            }
            int total;
            
            var r = RavenRepository.Create().GetPendingRecipies(out total, 1).FirstOrDefault();
            this.labelStat.Text = "Total pending " + total;
            if (r != null)
                LoadRecipie(r);
            else
                MessageBox.Show("Congrats!. No More pending recipes");

        }

        private void buttonAddTag_Click(object sender, EventArgs e)
        {
            if (activeRecipe != null)
            {
                var tag = this.textBoxTag.Text.Trim().ToLower();
                if (tag.Length > 2 && !activeRecipe.ActiveTags.Contains(tag))
                {
                    activeRecipe.ActiveTags.Add(tag);
                    this.checkedListBoxSelectedTags.Items.Add(tag, true);
                }
            }
            this.textBoxTag.Text = String.Empty;
        }
        
        private bool IsTagExists(string tag)
        {
            foreach (var item in this.checkedListBoxSelectedTags.Items)
                if (item.ToString() == tag)
                    return true;

            return false;
        }

       
        private void checkedListBoxTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            int index = clb.SelectedIndex;

            // When you remove an item from the Items collection, it fires the SelectedIndexChanged
            // event again, with SelectedIndex = -1.  Hence the check for index != -1 first, 
            // to prevent an invalid selectedindex error
            if (index != -1 && clb.GetItemCheckState(index) == CheckState.Checked && activeRecipe != null && 
                !activeRecipe.ActiveTags.Contains(this.checkedListBoxTags.SelectedItem.ToString()))
            {
                activeRecipe.ActiveTags.Add(this.checkedListBoxTags.SelectedItem.ToString());
                this.checkedListBoxSelectedTags.Items.Add(this.checkedListBoxTags.SelectedItem.ToString(), true);
                this.checkedListBoxTags.Items.Remove(this.checkedListBoxTags.SelectedItem.ToString());
            }
            
        }

        private void checkedListBoxSelectedTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            int index = clb.SelectedIndex;

            // When you remove an item from the Items collection, it fires the SelectedIndexChanged
            // event again, with SelectedIndex = -1.  Hence the check for index != -1 first, 
            // to prevent an invalid selectedindex error
            if (index != -1 && clb.GetItemCheckState(index) == CheckState.Unchecked && activeRecipe != null )
            {
                if (activeRecipe.ActiveTags.Contains(this.checkedListBoxSelectedTags.SelectedItem.ToString()))
                {
                    activeRecipe.ActiveTags.Remove(this.checkedListBoxSelectedTags.SelectedItem.ToString());
                    this.checkedListBoxTags.Items.Add(this.checkedListBoxSelectedTags.SelectedItem.ToString());
                }
                this.checkedListBoxSelectedTags.Items.Remove(this.checkedListBoxSelectedTags.SelectedItem.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.activeRecipe != null)
            {
                this.activeRecipe.ApprovalStatus = ApprovalStage.Declined;
                RavenRepository.Create().SaveRecipe(this.activeRecipe);
                this.activeRecipe = null;
                LoadNext();
            }
        }

    }
}
