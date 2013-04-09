// <copyright file="ArticleEdit.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.ArticleControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Controllers;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Security;
    using DotNetNuke.Security.Permissions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.UserControls;
    using DotNetNuke.UI.Utilities;

    using Engage.Dnn.Publish.Controls;
    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Tags;
    using Engage.Dnn.Publish.Util;

    using Globals = DotNetNuke.Common.Globals;

    public partial class ArticleEdit : ModuleBase
    {
        // the relationship controls don't have ID's set in the code below because they will fail if you set them all to the same id.
        // The local resource file for each of the relationship controls is set in the code below.
        protected TextEditor TeArticleText;

        private const string ApprovalControlToLoad = "../controls/ItemApproval.ascx";

        private const string ItemControlToLoad = "../Controls/itemEdit.ascx";

        private const string TagControlToLoad = "../Tags/TagEntry.ascx";

        private readonly string ItemrelationshipResourceFile = "~" + DesktopModuleFolderName + "Controls/App_LocalResources/ItemRelationships";

        private ItemRelationships embeddedArticlesRelationships; // article links

        private ItemApproval itemApprovalStatus; // item approval status

        private ItemEdit itemEditControl; // item edit control

        private ItemRelationships parentCategoryRelationship; // item parent category

        private ItemRelationships relatedArticlesRelationships; // related articles 

        private ItemRelationships relatedCategoryRelationships; // item related categories

        private TagEntry tagEntryControl; // tag entry control

        private int ParentId
        {
            get
            {
                string s = this.Request.QueryString["parentid"];
                return s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.cmdUpdate.Click += this.CmdUpdateClick;
            this.cmdCancel.Click += this.CmdCancelClick;
            this.Load += this.Page_Load;
            this.PreRender += this.Page_PreRender;

            this.LoadControlType();
            base.OnInit(e);
            this.LoadSharedResources();

            // ctlUrlSelection.Url = 
            ItemVersionSetting attachmentSetting = ItemVersionSetting.GetItemVersionSetting(
                this.VersionInfoObject.ItemVersionId, "ArticleSettings", "ArticleAttachment", this.PortalId);
            if (attachmentSetting != null)
            {
                this.ctlUrlSelection.Url = attachmentSetting.PropertyValue;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void chkIncludeRelatedArticles_CheckedChanged(object sender, EventArgs e)
        {
            this.phRelatedArticles.Visible = this.chkIncludeRelatedArticles.Checked;
            this.phEmbeddedArticle.Visible = this.chkIncludeRelatedArticles.Checked && this.UseEmbeddedArticles;

            if (!this.chkIncludeRelatedArticles.Checked)
            {
                // Remove all Related and Embedded Articles if they choose not to include related articles.
                this.relatedArticlesRelationships.Clear();
                this.embeddedArticlesRelationships.Clear();
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void chkUseApprovals_CheckedChanged(object sender, EventArgs e)
        {
            this.phApproval.Visible = this.chkUseApprovals.Checked && this.UseApprovals;
            this.lblNotUsingApprovals.Visible = !this.chkUseApprovals.Checked || !this.UseApprovals;
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            this.txtMessage.Visible = true;
            bool itemExists = false;

            if (this.ItemId > -1)
            {
                // Using GetItemTypeId as substitute for IfExists
                if (Item.GetItemTypeId(this.ItemId, this.PortalId) > -1)
                {
                    itemExists = true;
                    bool inUse = false;
                    var modulesInUse = new StringBuilder();
                    var mc = new ModuleController();
                    ArrayList modules = mc.GetModulesByDefinition(this.PortalId, Utility.DnnFriendlyModuleName);

                    foreach (ModuleInfo module in modules)
                    {
                        Hashtable settings = mc.GetTabModuleSettings(module.TabModuleID);

                        if (settings.ContainsKey("DisplayType") && settings["DisplayType"].ToString() == "ArticleDisplay")
                        {
                            int articleId;
                            if (settings.ContainsKey("adArticleId") && int.TryParse(settings["adArticleId"].ToString(), out articleId))
                            {
                                if (articleId == this.ItemId)
                                {
                                    inUse = true;
                                    modulesInUse.AppendFormat("{0} ({1}){2}", module.ModuleTitle, module.TabID, Environment.NewLine);
                                    break;
                                }
                            }
                        }
                    }

                    ArrayList featuredRelationships = ItemRelationship.GetItemChildRelationships(
                        this.ItemId, RelationshipType.ItemToFeaturedItem.GetId());
                    bool isFeatured = featuredRelationships.Count > 0;

                    if (!inUse && !isFeatured)
                    {
                        // Item.DeleteItem(ItemId);
                        Item.DeleteItem(this.ItemId, this.PortalId);
                        this.txtMessage.Text = Localization.GetString("DeleteSuccess", this.LocalResourceFile);
                    }
                    else
                    {
                        var errorMessage = new StringBuilder();

                        if (inUse)
                        {
                            errorMessage.AppendFormat(
                                "{0}{1}", Localization.GetString("DeleteFailureInUse", this.LocalResourceFile), Environment.NewLine);
                            errorMessage.Append(modulesInUse.ToString());
                        }

                        if (isFeatured)
                        {
                            errorMessage.AppendFormat(
                                "{0}{1}", Localization.GetString("DeleteFailureIsFeatured", this.LocalResourceFile), Environment.NewLine);

                            foreach (ItemRelationship rel in featuredRelationships)
                            {
                                Category parentCategory = Category.GetCategory(rel.ChildItemId, this.PortalId);
                                errorMessage.AppendFormat("{0}{1}", parentCategory.Name, Environment.NewLine);
                            }
                        }

                        this.txtMessage.Text = errorMessage.ToString();
                    }
                }
            }

            if (!itemExists)
            {
                this.txtMessage.Text = Localization.GetString("DeleteFailure", this.LocalResourceFile);
            }

            this.ShowOnlyMessage();
        }

        // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        // protected void chkForceDisplayTab_CheckedChanged(object sender, EventArgs e)
        // {
        // if (chkForceDisplayTab.Checked)
        // {
        // rblDisplayOnCurrentPage.SelectedValue = false.ToString(CultureInfo.InvariantCulture);
        // //populate the list of display pages with all pages configured for Publish, even if they aren't overrideable.
        // }
        // LoadDisplayTabDropDown();
        // }
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void rblDisplayOnCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if display on current page is selected
            if (Convert.ToBoolean(this.rblDisplayOnCurrentPage.SelectedValue, CultureInfo.InvariantCulture))
            {
                this.chkForceDisplayTab.Visible = false;
                this.lblForceDisplayTab.Visible = false;
                this.chkForceDisplayTab.Checked = false;
                this.ddlDisplayTabId.Enabled = false;
            }
            else
            {
                // if display on specific page is selected
                this.chkForceDisplayTab.Visible = true;
                this.lblForceDisplayTab.Visible = true;
                this.ddlDisplayTabId.Enabled = true;
            }

            this.LoadDisplayTabDropDown();
        }

        private void CmdCancelClick(object sender, EventArgs e)
        {
            string returnUrl = this.Server.UrlDecode(this.Request.QueryString["returnUrl"]);
            if (!Engage.Utility.HasValue(returnUrl))
            {
                this.Response.Redirect(this.BuildCategoryListUrl(ItemType.Article), true);
            }
            else
            {
                this.Response.Redirect(returnUrl, true);
            }
        }

        private void CmdUpdateClick(object sender, EventArgs e)
        {
            try
            {
                var av = (Article)this.VersionInfoObject;

                this.txtMessage.Text = string.Empty;
                bool error = false;

                // Removed the check for the Item Description length as we no longer have a restriction on this

                // if (TextBoxMaxLengthExceeded(this.itemEditControl.DescriptionText, "Article Description", 4000))
                // {
                // error = true;
                // }
                if (this.TextBoxMaxLengthExceeded(this.txtVersionDescription.Text, "Version Description", 8000))
                {
                    error = true;
                }

                if (!this.parentCategoryRelationship.IsValid)
                {
                    error = true;
                    this.txtMessage.Text += Localization.GetString("ErrorSelectCategory.Text", this.LocalResourceFile);
                }

                if (!this.itemApprovalStatus.IsValid && this.itemApprovalStatus.Visible)
                {
                    error = true;
                    this.txtMessage.Text += Localization.GetString("ErrorApprovalStatus.Text", this.LocalResourceFile);
                }

                if (!this.itemEditControl.IsValid)
                {
                    error = true;
                    this.txtMessage.Text += this.itemEditControl.ErrorMessage;
                }

                if (Convert.ToInt32(this.ddlDisplayTabId.SelectedValue, CultureInfo.InvariantCulture) > -1)
                {
                    this.VersionInfoObject.DisplayTabId = Convert.ToInt32(this.ddlDisplayTabId.SelectedValue, CultureInfo.InvariantCulture);
                }
                else
                {
                    error = true;
                    this.txtMessage.Text += Localization.GetString("ErrorDisplayPage.Text", this.LocalResourceFile);
                }

                if (error)
                {
                    this.txtMessage.Visible = true;
                    return;
                }

                av.ArticleText = this.TeArticleText.Text;
                av.VersionDescription = this.txtVersionDescription.Text;
                av.VersionNumber = this.txtVersionNumber.Text;
                av.Description = this.itemEditControl.DescriptionText;

                // we need to look at making moduleid be configurable at anytime, not just on item creation, this makes previewing items impossible
                // if (av.IsNew)
                int newModuleId = Utility.GetModuleIdFromDisplayTabId(
                    this.VersionInfoObject.DisplayTabId, this.PortalId, Utility.DnnFriendlyModuleName);
                if (newModuleId > 0)
                {
                    this.VersionInfoObject.ModuleId = newModuleId;
                }
                else
                {
                    newModuleId = Utility.GetModuleIdFromDisplayTabId(
                        this.VersionInfoObject.DisplayTabId, this.PortalId, Utility.DnnFriendlyModuleNameTextHTML);
                    if (newModuleId > 0)
                    {
                        this.VersionInfoObject.ModuleId = newModuleId;
                    }
                }

                // create a relationship
                var irel = new ItemRelationship
                    {
                        RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId()
                    };
                int[] ids = this.parentCategoryRelationship.GetSelectedItemIds();

                // check for parent category, if none then add a relationship for Top Level Item
                if (ids.Length > 0)
                {
                    irel.ParentItemId = ids[0];
                    this.VersionInfoObject.Relationships.Add(irel);
                }

                foreach (int i in this.relatedCategoryRelationships.GetSelectedItemIds())
                {
                    var irco = new ItemRelationship
                        {
                            RelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId(), 
                            ParentItemId = i
                        };
                    av.Relationships.Add(irco);
                }

                foreach (int i in this.relatedArticlesRelationships.GetSelectedItemIds())
                {
                    var irArticleso = new ItemRelationship
                        {
                            RelationshipTypeId = RelationshipType.ItemToRelatedArticle.GetId(), 
                            ParentItemId = i
                        };
                    av.Relationships.Add(irArticleso);
                }

                foreach (int i in this.embeddedArticlesRelationships.GetSelectedItemIds())
                {
                    var irLinksso = new ItemRelationship
                        {
                            RelationshipTypeId = RelationshipType.ItemToArticleLinks.GetId(), 
                            ParentItemId = i
                        };
                    av.Relationships.Add(irLinksso);
                }

                if (this.AllowTags)
                {
                    av.Tags.Clear();

                    // Add the tags to the ItemTagCollection
                    foreach (Tag t in Tag.ParseTags(this.tagEntryControl.TagList, this.PortalId))
                    {
                        ItemTag it = ItemTag.Create();
                        it.TagId = Convert.ToInt32(t.TagId, CultureInfo.InvariantCulture);
                        av.Tags.Add(it);
                    }
                }

                if (av.Description == string.Empty)
                {
                    // trim article text to populate description
                    if (!Engage.Utility.HasValue(av.Description) || !Engage.Utility.HasValue(av.MetaDescription))
                    {
                        string description = HtmlUtils.StripTags(av.ArticleText, false);
                        if (!Engage.Utility.HasValue(av.Description))
                        {
                            av.Description = Utility.TrimDescription(3997, description) + "..."; // description + "...";
                        }
                    }
                }

                // auto populate the meta description if it's not populated already
                if (!Engage.Utility.HasValue(av.MetaDescription))
                {
                    string description = HtmlUtils.StripTags(av.Description, false);
                    av.MetaDescription = Utility.TrimDescription(399, description);
                }

                // Save the ItemVersionSettings
                this.SaveSettings();

                // approval status
                av.ApprovalStatusId = this.chkUseApprovals.Checked && this.UseApprovals
                                          ? this.itemApprovalStatus.ApprovalStatusId
                                          : ApprovalStatus.Approved.GetId();

                this.VersionInfoObject.Save(this.UserId);

                string returnUrl = this.Server.UrlDecode(this.Request.QueryString["returnUrl"]);
                if (!Engage.Utility.HasValue(returnUrl))
                {
                    this.Response.Redirect(
                        Globals.NavigateURL(
                            this.TabId, 
                            string.Empty, 
                            string.Empty, 
                            "ctl=" + Utility.AdminContainer, 
                            "mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture), 
                            "adminType=itemCreated", 
                            "itemId=" + this.VersionInfoObject.ItemId.ToString(CultureInfo.InvariantCulture)), 
                        true);
                }
                else
                {
                    this.Response.Redirect(returnUrl);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void FillPhotoGalleryDropDown()
        {
            var modules = new ModuleController();

            var simpleGalleryAlbums = new SortedList<string, ListItem>(StringComparer.CurrentCultureIgnoreCase);
            if (this.AllowSimpleGalleryIntegration)
            {
                foreach (ModuleInfo module in modules.GetModulesByDefinition(this.PortalId, Utility.SimpleGalleryFriendlyName))
                {
                    if (ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, string.Empty, module))
                    {
                        IDataReader dr = DataProvider.Instance().GetSimpleGalleryAlbums(module.ModuleID);
                        while (dr.Read())
                        {
                            string caption = dr["Caption"].ToString();
                            for (int i = 0; simpleGalleryAlbums.ContainsKey(caption); i++)
                            {
                                caption += i.ToString(CultureInfo.InvariantCulture);
                            }

                            simpleGalleryAlbums.Add(caption, new ListItem(dr["Caption"].ToString(), "s" + dr["AlbumId"]));
                        }
                    }
                }
            }

            var ultaMediaGalleryAlbums = new SortedList<string, ListItem>(StringComparer.CurrentCultureIgnoreCase);
            if (this.AllowUltraMediaGalleryIntegration)
            {
                foreach (ModuleInfo module in modules.GetModulesByDefinition(this.PortalId, Utility.UltraMediaGalleryFriendlyName))
                {
                    if (ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, string.Empty, module))
                    {
                        IDataReader dr = DataProvider.Instance().GetUltraMediaGalleryAlbums(module.ModuleID);
                        while (dr.Read())
                        {
                            string title = dr["Title"].ToString();
                            for (int i = 0; ultaMediaGalleryAlbums.ContainsKey(title); i++)
                            {
                                title += i.ToString(CultureInfo.InvariantCulture);
                            }

                            ultaMediaGalleryAlbums.Add(title, new ListItem(dr["Title"].ToString(), "u" + dr["ItemId"]));
                        }
                    }
                }
            }

            // label which module it's from if there are some from both.
            bool labelModuleType = simpleGalleryAlbums.Count > 0 && ultaMediaGalleryAlbums.Count > 0;

            foreach (ListItem li in simpleGalleryAlbums.Values)
            {
                if (labelModuleType)
                {
                    li.Text += " - " + Localization.GetString("SimpleGallery", this.LocalResourceFile);
                }

                this.ddlPhotoGalleryAlbum.Items.Add(li);
            }

            foreach (ListItem li in ultaMediaGalleryAlbums.Values)
            {
                if (labelModuleType)
                {
                    li.Text += " - " + Localization.GetString("UltraMediaGallery", this.LocalResourceFile);
                }

                this.ddlPhotoGalleryAlbum.Items.Add(li);
            }
        }

        private void LoadControlType()
        {
            this.UseCache = false;
            if (this.ItemVersionId == -1)
            {
                this.BindItemData(true);
                this.trArticleId.Visible = false;
                this.cmdDelete.Visible = false;
            }
            else
            {
                this.BindItemData();
                this.cmdDelete.Visible = this.IsAdmin;
            }

            var av = (Article)this.VersionInfoObject;

            // Item Edit
            this.itemEditControl = (ItemEdit)this.LoadControl(ItemControlToLoad);
            this.itemEditControl.ModuleConfiguration = this.ModuleConfiguration;
            this.itemEditControl.ID = Path.GetFileNameWithoutExtension(ItemControlToLoad);
            this.itemEditControl.VersionInfoObject = this.VersionInfoObject;
            this.phControls.Controls.Add(this.itemEditControl);

            // Article Text Editor
            this.TeArticleText = (TextEditor)this.LoadControl("~/controls/TextEditor.ascx");
            this.TeArticleText.HtmlEncode = false;
            this.TeArticleText.TextRenderMode = "Raw";
            this.TeArticleText.Width = this.ArticleEditWidth; // default values for the editor
            this.TeArticleText.Height = this.ArticleEditHeight; // default values for the editor
            this.TeArticleText.ChooseMode = true;
            this.phArticleText.Controls.Add(this.TeArticleText);
            this.TeArticleText.Text = av.ArticleText;

            // Parent Category Relationship
            this.parentCategoryRelationship = (ItemRelationships)this.LoadControl("../controls/ItemRelationships.ascx");
            this.parentCategoryRelationship.ModuleConfiguration = this.ModuleConfiguration;

            this.parentCategoryRelationship.LocalResourceFile = this.ItemrelationshipResourceFile;
            this.parentCategoryRelationship.VersionInfoObject = this.VersionInfoObject;
            this.parentCategoryRelationship.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.parentCategoryRelationship.CreateRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.parentCategoryRelationship.AvailableSelectionMode = ListSelectionMode.Single;
            this.parentCategoryRelationship.IsRequired = true;
            this.parentCategoryRelationship.FlatView = true;
            this.parentCategoryRelationship.ItemTypeId = ItemType.Category.GetId();
            this.phParentCategory.Controls.Add(this.parentCategoryRelationship);

            // Related Category Relationship
            this.relatedCategoryRelationships = (ItemRelationships)this.LoadControl("../controls/ItemRelationships.ascx");
            this.relatedCategoryRelationships.ModuleConfiguration = this.ModuleConfiguration;
            this.relatedCategoryRelationships.LocalResourceFile = this.ItemrelationshipResourceFile;
            this.relatedCategoryRelationships.VersionInfoObject = this.VersionInfoObject;
            this.relatedCategoryRelationships.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.relatedCategoryRelationships.CreateRelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();
            this.relatedCategoryRelationships.AvailableSelectionMode = ListSelectionMode.Multiple;
            this.relatedCategoryRelationships.IsRequired = false;
            this.relatedCategoryRelationships.FlatView = true;
            this.relatedCategoryRelationships.ItemTypeId = ItemType.Category.GetId();
            this.phRelatedCategories.Controls.Add(this.relatedCategoryRelationships);

            // Related Articles Relationship
            this.relatedArticlesRelationships = (ItemRelationships)this.LoadControl("../controls/ItemRelationships.ascx");
            this.relatedArticlesRelationships.ModuleConfiguration = this.ModuleConfiguration;
            this.relatedArticlesRelationships.VersionInfoObject = this.VersionInfoObject;
            this.relatedArticlesRelationships.LocalResourceFile = this.ItemrelationshipResourceFile;
            this.relatedArticlesRelationships.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.relatedArticlesRelationships.CreateRelationshipTypeId = RelationshipType.ItemToRelatedArticle.GetId();
            this.relatedArticlesRelationships.AvailableSelectionMode = ListSelectionMode.Multiple;
            this.relatedArticlesRelationships.FlatView = true;
            this.relatedArticlesRelationships.EnableDates = false;
            this.relatedArticlesRelationships.AllowSearch = true;
            this.relatedArticlesRelationships.EnableSortOrder = true;
            this.relatedArticlesRelationships.ItemTypeId = ItemType.Article.GetId();
            this.phRelatedArticles.Controls.Add(this.relatedArticlesRelationships);

            // Embedded Articles Relationship
            this.embeddedArticlesRelationships = (ItemRelationships)this.LoadControl("../controls/ItemRelationships.ascx");
            this.embeddedArticlesRelationships.ModuleConfiguration = this.ModuleConfiguration;
            this.embeddedArticlesRelationships.VersionInfoObject = this.VersionInfoObject;
            this.embeddedArticlesRelationships.LocalResourceFile = this.ItemrelationshipResourceFile;
            this.embeddedArticlesRelationships.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.embeddedArticlesRelationships.CreateRelationshipTypeId = RelationshipType.ItemToArticleLinks.GetId();
            this.embeddedArticlesRelationships.AvailableSelectionMode = ListSelectionMode.Single;
            this.embeddedArticlesRelationships.FlatView = true;
            this.embeddedArticlesRelationships.EnableDates = false;
            this.embeddedArticlesRelationships.AllowSearch = true;
            this.embeddedArticlesRelationships.EnableSortOrder = false;
            this.embeddedArticlesRelationships.ItemTypeId = ItemType.Article.GetId();
            this.phEmbeddedArticle.Controls.Add(this.embeddedArticlesRelationships);

            // load approval status
            this.itemApprovalStatus = (ItemApproval)this.LoadControl(ApprovalControlToLoad);
            this.itemApprovalStatus.ModuleConfiguration = this.ModuleConfiguration;
            this.itemApprovalStatus.ID = Path.GetFileNameWithoutExtension(ApprovalControlToLoad);
            this.itemApprovalStatus.VersionInfoObject = this.VersionInfoObject;
            this.phApproval.Controls.Add(this.itemApprovalStatus);

            if (this.AllowTags)
            {
                this.rowTagEntry.Visible = true;
                var tagList = new StringBuilder(255);
                foreach (ItemTag it in this.VersionInfoObject.Tags)
                {
                    tagList.Append(Tag.GetTag(it.TagId, this.PortalId).Name);
                    tagList.Append(";");
                }

                this.tagEntryControl = (TagEntry)this.LoadControl(TagControlToLoad);
                this.tagEntryControl.ModuleConfiguration = this.ModuleConfiguration;
                this.tagEntryControl.ID = Path.GetFileNameWithoutExtension(TagControlToLoad);

                this.tagEntryControl.TagList = tagList.ToString();
                this.phTagEntry.Controls.Add(this.tagEntryControl);
            }
            else
            {
                this.rowTagEntry.Visible = false;
            }
        }

        private void LoadDisplayTabDropDown()
        {
            this.ddlDisplayTabId.Items.Clear();

            var modules = new[]
                {
                    Utility.DnnFriendlyModuleName
                };

            // we're going to get all pages no matter if they have a Publish module on them or not. We'll only highlight Overrideable ones later
            // if (chkForceDisplayTab.Checked)
            // {
            // //if the ForceDisplayTab is checked we need to make sure we get ALL publish modules, not just overrideable ones
            // dt = Utility.GetDisplayTabIdsAll(modules);
            // }
            // else
            // {
            // dt = Utility.GetDisplayTabIds(modules);
            // if (dt.Rows.Count < 1)
            // {
            // //if there are no items in the list, meaning there are no modules set to be overrideable, then get the list of all Publish pages.
            // dt = Utility.GetDisplayTabIdsAll(modules);
            // }
            // }
            DataTable dt = Utility.GetDisplayTabIds(modules);

            // this.ddlDisplayTabId.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
            this.ddlDisplayTabId.DataSource = TabController.GetPortalTabs(this.PortalId, Null.NullInteger, false, true);
            this.ddlDisplayTabId.DataBind();
            foreach (DataRow dr in dt.Rows)
            {
                if (this.ddlDisplayTabId.Items.FindByValue(dr["TabID"].ToString()) != null)
                {
                    this.ddlDisplayTabId.Items.FindByValue(dr["TabID"].ToString()).Text += Localization.GetString(
                        "PublishOverrideable", this.LocalSharedResourceFile);
                }

                // ListItem li = new ListItem(dr["TabName"] + " (" + dr["TabID"] + ")", dr["TabID"].ToString());
                // this.ddlDisplayTabId.Items.Add(li);
            }

            // check if the DisplayTabId should be set.
            var av = (Article)this.VersionInfoObject;
            if (!this.VersionInfoObject.IsNew)
            {
                ListItem li = this.ddlDisplayTabId.Items.FindByValue(av.DisplayTabId.ToString(CultureInfo.InvariantCulture));
                if (li != null)
                {
                    this.ddlDisplayTabId.ClearSelection();
                    li.Selected = true;
                }
                else
                {
                    // if we made it here we've hit an article who is pointing to a page that is no longer overrideable, set the default page.
                    if (this.DefaultDisplayTabId > 0)
                    {
                        li = this.ddlDisplayTabId.Items.FindByValue(this.DefaultDisplayTabId.ToString(CultureInfo.InvariantCulture));
                        if (li != null)
                        {
                            this.ddlDisplayTabId.ClearSelection();
                            li.Selected = true;
                        }
                    }
                }
            }
            else
            {
                Category parent = null;
                if (this.ParentId != -1)
                {
                    parent = Category.GetCategory(this.ParentId, this.PortalId);
                }

                // look for display tab id
                if (parent != null && parent.ChildDisplayTabId > 0)
                {
                    if (this.ddlDisplayTabId.Items.FindByValue(parent.ChildDisplayTabId.ToString(CultureInfo.InvariantCulture)) != null)
                    {
                        this.ddlDisplayTabId.SelectedIndex = -1;
                        this.ddlDisplayTabId.Items.FindByValue(parent.ChildDisplayTabId.ToString(CultureInfo.InvariantCulture)).Selected = true;
                    }
                }
                else
                {
                    // load the default display tab
                    ListItem li = this.ddlDisplayTabId.Items.FindByValue(this.DefaultDisplayTabId.ToString(CultureInfo.InvariantCulture));
                    if (li != null)
                    {
                        this.ddlDisplayTabId.ClearSelection();
                        li.Selected = true;
                    }
                }
            }
        }

        private void LoadPhotoGalleryDropDown(Article av)
        {
            if (this.AllowSimpleGalleryIntegration || this.AllowUltraMediaGalleryIntegration)
            {
                this.rowPhotoGallery.Visible = true;

                this.FillPhotoGalleryDropDown();

                if (this.ddlPhotoGalleryAlbum.Items.Count > 0)
                {
                    ItemVersionSetting simpleGalleryAlbum = ItemVersionSetting.GetItemVersionSetting(
                        av.ItemVersionId, "ddlSimpleGalleryAlbum", "SelectedValue", this.PortalId);
                    if (simpleGalleryAlbum != null && Engage.Utility.HasValue(simpleGalleryAlbum.PropertyValue))
                    {
                        this.ddlPhotoGalleryAlbum.ClearSelection();
                        this.ddlPhotoGalleryAlbum.SelectedValue = "s" + simpleGalleryAlbum.PropertyValue;
                    }
                    else
                    {
                        ItemVersionSetting ultraMediaGalleryAlbum = ItemVersionSetting.GetItemVersionSetting(
                            av.ItemVersionId, "ddlUltraMediaGalleryAlbum", "SelectedValue", this.PortalId);
                        if (ultraMediaGalleryAlbum != null && Engage.Utility.HasValue(ultraMediaGalleryAlbum.PropertyValue))
                        {
                            this.ddlPhotoGalleryAlbum.ClearSelection();
                            this.ddlPhotoGalleryAlbum.SelectedValue = "u" + ultraMediaGalleryAlbum.PropertyValue;
                        }
                    }
                }
                else
                {
                    this.rowPhotoGallery.Visible = false;
                }
            }

            this.ddlPhotoGalleryAlbum.Items.Insert(0, new ListItem(string.Empty));
        }

        private void LoadSharedResources()
        {
            this.lblPublishOverrideable.Text = Localization.GetString("lblPublishOverrideable.Text", this.LocalSharedResourceFile);
        }

        private void LocalizeCollapsePanels()
        {
            this.clpExtended.CollapsedText = Localization.GetString("clpExtended.CollapsedText", this.LocalResourceFile);
            this.clpExtended.ExpandedText = Localization.GetString("clpExtended.ExpandedText", this.LocalResourceFile);
            this.clpExtended.ExpandedImage = ApplicationUrl +
                                             Localization.GetString("ExpandedImage.Text", this.LocalSharedResourceFile).Replace("[L]", string.Empty);
            this.clpExtended.CollapsedImage = ApplicationUrl +
                                              Localization.GetString("CollapsedImage.Text", this.LocalSharedResourceFile).Replace("[L]", string.Empty);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.LocalizeCollapsePanels();

                ClientAPI.AddButtonConfirm(this.cmdDelete, Localization.GetString("DeleteConfirm", this.LocalResourceFile));
                var av = (Article)this.VersionInfoObject;
                if (!this.Page.IsPostBack)
                {
                    // check to see if we're dealing with a new Item, if so set the parentid based on the querystring
                    if (av.IsNew)
                    {
                        if (this.ParentId != -1)
                        {
                            Category parent = Category.GetCategory(this.ParentId, this.PortalId); // = null;
                            this.parentCategoryRelationship.AddToSelectedItems(parent);
                        }
                    }

                    this.trArticleId.Visible = this.ShowItemIds;

                    this.txtArticleId.Text = this.VersionInfoObject.ItemId.ToString(CultureInfo.CurrentCulture) == "-1"
                                                 ? Localization.GetString("NewArticle", this.LocalResourceFile)
                                                 : this.VersionInfoObject.ItemId.ToString(CultureInfo.CurrentCulture);
                    this.txtVersionNumber.Text = av.VersionNumber;
                    this.TeArticleText.Text = av.ArticleText;
                    this.txtPreviousVersionDescription.Text = av.VersionDescription;

                    this.rblDisplayOnCurrentPage.Items.Add(
                        new ListItem(Localization.GetString("CurrentPage", this.LocalResourceFile), true.ToString(CultureInfo.InvariantCulture)));
                    this.rblDisplayOnCurrentPage.Items.Add(
                        new ListItem(Localization.GetString("SpecificPage", this.LocalResourceFile), false.ToString(CultureInfo.InvariantCulture)));

                    // get the pnlPrinterFriendly setting
                    ItemVersionSetting pfSetting = ItemVersionSetting.GetItemVersionSetting(
                        av.ItemVersionId, "pnlPrinterFriendly", "Visible", this.PortalId);
                    var hostController = HostController.Instance;
                    if (pfSetting != null)
                    {
                        this.chkPrinterFriendly.Checked = Convert.ToBoolean(pfSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostPrinterFriendlySetting =
                            hostController.GetString(Utility.PublishDefaultPrinterFriendly + this.PortalId.ToString(CultureInfo.InvariantCulture));
                        this.chkPrinterFriendly.Checked = !Engage.Utility.HasValue(hostPrinterFriendlySetting) ||
                                                          Convert.ToBoolean(hostPrinterFriendlySetting, CultureInfo.InvariantCulture);
                    }

                    // get the pnlEmailAFriend setting
                    ItemVersionSetting efSetting = ItemVersionSetting.GetItemVersionSetting(
                        av.ItemVersionId, "pnlEmailAFriend", "Visible", this.PortalId);
                    if (efSetting != null)
                    {
                        this.chkEmailAFriend.Checked = Convert.ToBoolean(efSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostEmailFriendSetting =
                            hostController.GetString(Utility.PublishDefaultEmailAFriend + this.PortalId.ToString(CultureInfo.InvariantCulture));
                        this.chkEmailAFriend.Checked = !Engage.Utility.HasValue(hostEmailFriendSetting) ||
                                                       Convert.ToBoolean(hostEmailFriendSetting, CultureInfo.InvariantCulture);
                    }

                    // if ratings are enabled show options
                    if (this.AreRatingsEnabled)
                    {
                        // get the upnlRating setting
                        ItemVersionSetting rtSetting = ItemVersionSetting.GetItemVersionSetting(
                            av.ItemVersionId, "upnlRating", "Visible", this.PortalId);
                        if (rtSetting != null)
                        {
                            this.chkRatings.Checked = Convert.ToBoolean(rtSetting.PropertyValue, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            string hostRatingSetting =
                                hostController.GetString(Utility.PublishDefaultRatings + this.PortalId.ToString(CultureInfo.InvariantCulture));
                            this.chkRatings.Checked = !Engage.Utility.HasValue(hostRatingSetting) ||
                                                      Convert.ToBoolean(hostRatingSetting, CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        this.chkRatings.Visible = false;
                    }

                    // if comments are enabled show options.
                    if (this.IsCommentsEnabled)
                    {
                        // get the pnlComments setting
                        ItemVersionSetting ctSetting = ItemVersionSetting.GetItemVersionSetting(
                            av.ItemVersionId, "pnlComments", "Visible", this.PortalId);
                        if (ctSetting != null)
                        {
                            this.chkComments.Checked = Convert.ToBoolean(ctSetting.PropertyValue, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            string hostCommentSetting =
                                hostController.GetString(Utility.PublishDefaultComments + this.PortalId.ToString(CultureInfo.InvariantCulture));
                            this.chkComments.Checked = !Engage.Utility.HasValue(hostCommentSetting) ||
                                                       Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture);
                        }

                        if (this.IsPublishCommentType)
                        {
                            this.chkForumComments.Visible = false;
                        }
                        else
                        {
                            ItemVersionSetting forumCommentSetting = ItemVersionSetting.GetItemVersionSetting(
                                av.ItemVersionId, "chkForumComments", "Checked", this.PortalId);
                            this.chkForumComments.Checked = forumCommentSetting == null ||
                                                            Convert.ToBoolean(forumCommentSetting.PropertyValue, CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        this.chkComments.Visible = false;
                        this.chkForumComments.Visible = false;
                    }

                    // chkIncludeRelatedArticles
                    ItemVersionSetting raSetting = ItemVersionSetting.GetItemVersionSetting(
                        av.ItemVersionId, "ArticleSettings", "IncludeParentCategoryArticles", this.PortalId);
                    this.chkIncludeOtherArticlesFromSameList.Checked = raSetting != null &&
                                                                       Convert.ToBoolean(raSetting.PropertyValue, CultureInfo.InvariantCulture);

                    // chkShowAuthor
                    ItemVersionSetting auSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "pnlAuthor", "Visible", this.PortalId);
                    if (auSetting != null)
                    {
                        this.chkShowAuthor.Checked = Convert.ToBoolean(auSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostAuthorSetting =
                            hostController.GetString(Utility.PublishDefaultShowAuthor + this.PortalId.ToString(CultureInfo.InvariantCulture));
                        this.chkShowAuthor.Checked = Engage.Utility.HasValue(hostAuthorSetting) &&
                                                     Convert.ToBoolean(hostAuthorSetting, CultureInfo.InvariantCulture);
                    }

                    // chkShowTags
                    ItemVersionSetting tagSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "pnlTags", "Visible", this.PortalId);
                    if (tagSetting != null)
                    {
                        this.chkTags.Checked = Convert.ToBoolean(tagSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostTagsSetting =
                            hostController.GetString(Utility.PublishDefaultShowTags + this.PortalId.ToString(CultureInfo.InvariantCulture));
                        this.chkTags.Checked = Engage.Utility.HasValue(hostTagsSetting) && Convert.ToBoolean(hostTagsSetting, CultureInfo.InvariantCulture);
                    }

                    // chkDisplayOnCurrentPage
                    ItemVersionSetting cpSetting = ItemVersionSetting.GetItemVersionSetting(
                        av.ItemVersionId, "ArticleSettings", "DisplayOnCurrentPage", this.PortalId);
                    if (cpSetting != null)
                    {
                        this.rblDisplayOnCurrentPage.SelectedValue = av.DisplayOnCurrentPage().ToString(CultureInfo.InvariantCulture);
                        if (av.DisplayOnCurrentPage())
                        {
                            this.chkForceDisplayTab.Checked = false;
                            this.chkForceDisplayTab.Visible = false;
                            this.lblForceDisplayTab.Visible = false;
                            this.ddlDisplayTabId.Enabled = false;
                        }
                        else
                        {
                            this.chkForceDisplayTab.Visible = true;
                            this.lblForceDisplayTab.Visible = true;
                            this.ddlDisplayTabId.Enabled = true;
                        }
                    }
                    else if (av.DisplayTabId < 0)
                    {
                        this.rblDisplayOnCurrentPage.SelectedValue = false.ToString(CultureInfo.InvariantCulture);
                        this.chkForceDisplayTab.Checked = false;
                        this.chkForceDisplayTab.Visible = true;
                        this.lblForceDisplayTab.Visible = true;
                        this.ddlDisplayTabId.Enabled = true;
                    }
                    else
                    {
                        this.rblDisplayOnCurrentPage.SelectedValue = false.ToString(CultureInfo.InvariantCulture);
                        this.chkForceDisplayTab.Visible = true;
                        this.lblForceDisplayTab.Visible = true;
                        this.ddlDisplayTabId.Enabled = true;
                    }

                    this.chkForceDisplayTab.Checked = av.ForceDisplayOnPage();

                    ItemVersionSetting rlSetting = ItemVersionSetting.GetItemVersionSetting(
                        av.ItemVersionId, "ArticleSettings", "DisplayReturnToList", this.PortalId);
                    if (rlSetting != null)
                    {
                        this.chkReturnList.Checked = Convert.ToBoolean(rlSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostReturnToListSetting =
                            hostController.GetString(Utility.PublishDefaultReturnToList + this.PortalId.ToString(CultureInfo.InvariantCulture));
                        this.chkReturnList.Checked = Engage.Utility.HasValue(hostReturnToListSetting) &&
                                                     Convert.ToBoolean(hostReturnToListSetting, CultureInfo.InvariantCulture);
                    }

                    // use approvals setting
                    ItemVersionSetting useApprovals = ItemVersionSetting.GetItemVersionSetting(
                        av.ItemVersionId, "chkUseApprovals", "Checked", this.PortalId);
                    this.chkUseApprovals.Checked = useApprovals == null || Convert.ToBoolean(useApprovals.PropertyValue, CultureInfo.InvariantCulture);
                    this.chkUseApprovals.Visible = this.IsAdmin && this.UseApprovals;
                    this.phApproval.Visible = this.chkUseApprovals.Checked && this.UseApprovals;
                    this.lblNotUsingApprovals.Visible = !this.chkUseApprovals.Checked || !this.UseApprovals;
                    this.lblNotUsingApprovals.Text = Localization.GetString("ApprovalsDisabled", this.LocalSharedResourceFile);

                    this.LoadPhotoGalleryDropDown(av);
                    this.LoadDisplayTabDropDown();

                    // load the article attachement settings
                }
                else
                {
                    if (this.ddlDisplayTabId.Enabled)
                    {
                        av.DisplayTabId = Convert.ToInt32(this.ddlDisplayTabId.SelectedValue, CultureInfo.InvariantCulture);
                    }

                    av.VersionNumber = this.txtVersionNumber.Text;
                    av.VersionDescription = this.txtVersionDescription.Text;
                    av.ArticleText = this.TeArticleText.Text;
                }

                // load the article attachement settings

                // ctlUrlSelection.Url = 
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            // the ItemRelationships control doesn't bind until after this Page_Load,
            // so we need to use the PreRender to see whether there are articles
            bool thereAreRelatedArticles = this.relatedArticlesRelationships.GetSelectedItemIds().Length > 0;
            bool thereIsAnEmbeddedArtice = this.embeddedArticlesRelationships.GetSelectedItemIds().Length > 0;
            if (thereAreRelatedArticles || thereIsAnEmbeddedArtice || this.chkIncludeOtherArticlesFromSameList.Checked)
            {
                this.chkIncludeRelatedArticles.Checked = true;
                this.phRelatedArticles.Visible = true;
                this.phEmbeddedArticle.Visible = thereIsAnEmbeddedArtice || this.UseEmbeddedArticles;
            }
        }

        private void SaveSettings()
        {
            var av = (Article)this.VersionInfoObject;

            av.VersionSettings.Clear();

            // Printer Friendly
            Setting setting = Setting.PrinterFriendly;
            setting.PropertyValue = this.chkPrinterFriendly.Checked.ToString(CultureInfo.InvariantCulture);
            var itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // AuthorName setting
            setting = Setting.AuthorName;
            setting.PropertyValue = this.itemEditControl.AuthorName;
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // Email A Friend
            setting = Setting.EmailAFriend;
            setting.PropertyValue = this.chkEmailAFriend.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // ratings
            setting = Setting.Rating;
            setting.PropertyValue = this.chkRatings.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // comments
            setting = Setting.Comments;
            setting.PropertyValue = this.chkComments.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // forum comments
            setting = Setting.ForumComments;
            setting.PropertyValue = this.chkForumComments.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // forum comments thread ID
            // just continue forward to the next version, this doesn't get set in the edit screen
            itemVersionSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "ArticleSetting", "CommentForumThreadId", this.PortalId);
            if (itemVersionSetting != null)
            {
                av.VersionSettings.Add(itemVersionSetting);
            }

            // include all articles from the parent category
            setting = Setting.ArticleSettingIncludeCategories;
            setting.PropertyValue = this.chkIncludeOtherArticlesFromSameList.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // display on current page option
            setting = Setting.ArticleSettingCurrentDisplay;
            setting.PropertyValue = this.rblDisplayOnCurrentPage.SelectedValue;
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // force display on specific page
            setting = Setting.ArticleSettingForceDisplay;
            setting.PropertyValue = this.chkForceDisplayTab.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // display return to list
            setting = Setting.ArticleSettingReturnToList;
            setting.PropertyValue = this.chkReturnList.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // show author
            setting = Setting.Author;
            setting.PropertyValue = this.chkShowAuthor.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // show tags
            setting = Setting.ShowTags;
            setting.PropertyValue = this.chkTags.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // use approvals
            setting = Setting.UseApprovals;
            setting.PropertyValue = this.chkUseApprovals.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // simple gallery album
            setting = Setting.UseSimpleGalleryAlbum;
            setting.PropertyValue = this.ddlPhotoGalleryAlbum.SelectedValue.StartsWith("s", StringComparison.Ordinal)
                                        ? this.ddlPhotoGalleryAlbum.SelectedValue.Substring(1)
                                        : string.Empty;
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // ultra media gallery album
            setting = Setting.UseUltraMediaGalleryAlbum;
            setting.PropertyValue = this.ddlPhotoGalleryAlbum.SelectedValue.StartsWith("u", StringComparison.Ordinal)
                                        ? this.ddlPhotoGalleryAlbum.SelectedValue.Substring(1)
                                        : string.Empty;
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            // article attachment
            setting = Setting.ArticleAttachment;
            setting.PropertyValue = this.ctlUrlSelection.Url;
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);
        }

        private void ShowOnlyMessage()
        {
            foreach (Control cntl in this.Controls)
            {
                cntl.Visible = false;
            }

            this.txtMessage.Visible = true;
            this.txtMessage.Parent.Visible = true;
        }

        private bool TextBoxMaxLengthExceeded(string text, string controlName, int length)
        {
            bool b = text.Length > length;
            if (b)
            {
                this.txtMessage.Text += string.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.GetString("MaximumLength", this.LocalResourceFile), 
                    controlName, 
                    length.ToString(CultureInfo.CurrentCulture), 
                    text.Length);
                this.txtMessage.Visible = true;
            }

            return b;
        }
    }
}