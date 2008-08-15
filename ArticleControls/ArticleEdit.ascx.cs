//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using Engage.Dnn.Publish.Controls;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Tags;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.ArticleControls
{
    public partial class ArticleEdit : ModuleBase
    {
        //the relationship controls don't have ID's set in the code below because they will fail if you set them all to the same id.
        //The local resource file for each of the relationship controls is set in the code below.

        #region Controls
        protected TextEditor teArticleText;
        private ItemRelationships parentCategoryRelationship; // item parent category
        private ItemRelationships relatedCategoryRelationships; // item related categories
        private ItemRelationships relatedArticlesRelationships; //related articles 
        private ItemRelationships embeddedArticlesRelationships; //article links
        private ItemApproval itemApprovalStatus; //item approval status
        private ItemEdit itemEditControl; //item edit control
        private TagEntry tagEntryControl; //tag entry control
                
        #endregion

        #region Private Const
        private readonly string ItemrelationshipResourceFile = "~" + DesktopModuleFolderName + "Controls/App_LocalResources/ItemRelationships";
        private const string approvalControlToLoad = "../controls/ItemApproval.ascx";
        private const string itemControlToLoad = "../Controls/itemEdit.ascx";

        private const string tagControlToLoad = "../Tags/TagEntry.ascx";
        #endregion

        #region Properties
        private int ParentId
        {
            get
            {
                string s = Request.QueryString["parentid"];
                return (s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture));
            }
        }
        #endregion

        #region Event Handlers
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            LoadControlType();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.cmdUpdate.Click += this.cmdUpdate_Click;
            this.cmdCancel.Click += this.cmdCancel_Click;
            this.Load += this.Page_Load;
            this.PreRender += this.Page_PreRender;
        }

        private void LoadControlType()
        {

            UseCache = false;
            if (ItemVersionId == -1)
            {
                BindItemData(true);
                this.trArticleId.Visible = false;
                this.cmdDelete.Visible = false;
            }
            else
            {
                BindItemData();
                this.cmdDelete.Visible = IsAdmin;
            }

            Article av = (Article)VersionInfoObject;

            //Item Edit
            itemEditControl = (ItemEdit)LoadControl(itemControlToLoad);
            itemEditControl.ModuleConfiguration = ModuleConfiguration;
            itemEditControl.ID = Path.GetFileNameWithoutExtension(itemControlToLoad);
            itemEditControl.VersionInfoObject = VersionInfoObject;
            this.phControls.Controls.Add(itemEditControl);

            //Article Text Editor
            teArticleText = (TextEditor)LoadControl("~/controls/TextEditor.ascx");
            teArticleText.HtmlEncode = false;
            teArticleText.TextRenderMode = "Raw";
            teArticleText.Width = ArticleEditWidth; //default values for the editor
            teArticleText.Height = ArticleEditHeight; //default values for the editor
            teArticleText.ChooseMode = false;
            this.phArticleText.Controls.Add(teArticleText);
            teArticleText.Text = av.ArticleText;

            //Parent Category Relationship
            this.parentCategoryRelationship = (ItemRelationships)LoadControl("../controls/ItemRelationships.ascx");
            this.parentCategoryRelationship.ModuleConfiguration = ModuleConfiguration;
            
            this.parentCategoryRelationship.LocalResourceFile = ItemrelationshipResourceFile;
            this.parentCategoryRelationship.VersionInfoObject = VersionInfoObject;
            this.parentCategoryRelationship.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.parentCategoryRelationship.CreateRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.parentCategoryRelationship.AvailableSelectionMode = ListSelectionMode.Single;
            this.parentCategoryRelationship.IsRequired = true;
            this.parentCategoryRelationship.FlatView = true;
            this.parentCategoryRelationship.ItemTypeId = ItemType.Category.GetId();
            this.phParentCategory.Controls.Add(this.parentCategoryRelationship);

            //Related Category Relationship
            this.relatedCategoryRelationships = (ItemRelationships)LoadControl("../controls/ItemRelationships.ascx");
            this.relatedCategoryRelationships.ModuleConfiguration = ModuleConfiguration;
            this.relatedCategoryRelationships.LocalResourceFile = ItemrelationshipResourceFile;
            this.relatedCategoryRelationships.VersionInfoObject = VersionInfoObject;
            this.relatedCategoryRelationships.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.relatedCategoryRelationships.CreateRelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();
            this.relatedCategoryRelationships.AvailableSelectionMode = ListSelectionMode.Multiple;
            this.relatedCategoryRelationships.IsRequired = false;
            this.relatedCategoryRelationships.FlatView = true;
            this.relatedCategoryRelationships.ItemTypeId = ItemType.Category.GetId();
            this.phRelatedCategories.Controls.Add(this.relatedCategoryRelationships);

            //Related Articles Relationship
            this.relatedArticlesRelationships = (ItemRelationships)LoadControl("../controls/ItemRelationships.ascx");
            this.relatedArticlesRelationships.ModuleConfiguration = ModuleConfiguration;
            this.relatedArticlesRelationships.VersionInfoObject = VersionInfoObject;
            this.relatedArticlesRelationships.LocalResourceFile = ItemrelationshipResourceFile;
            this.relatedArticlesRelationships.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.relatedArticlesRelationships.CreateRelationshipTypeId = RelationshipType.ItemToRelatedArticle.GetId();
            this.relatedArticlesRelationships.AvailableSelectionMode = ListSelectionMode.Multiple;
            this.relatedArticlesRelationships.FlatView = true;
            this.relatedArticlesRelationships.EnableDates = false;
            this.relatedArticlesRelationships.AllowSearch = true;
            this.relatedArticlesRelationships.EnableSortOrder = true;
            this.relatedArticlesRelationships.ItemTypeId = ItemType.Article.GetId();
            this.phRelatedArticles.Controls.Add(this.relatedArticlesRelationships);

            //Embedded Articles Relationship
            this.embeddedArticlesRelationships = (ItemRelationships)LoadControl("../controls/ItemRelationships.ascx");
            this.embeddedArticlesRelationships.ModuleConfiguration = ModuleConfiguration;
            this.embeddedArticlesRelationships.VersionInfoObject = VersionInfoObject;
            this.embeddedArticlesRelationships.LocalResourceFile = ItemrelationshipResourceFile;
            this.embeddedArticlesRelationships.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.embeddedArticlesRelationships.CreateRelationshipTypeId = RelationshipType.ItemToArticleLinks.GetId();
            this.embeddedArticlesRelationships.AvailableSelectionMode = ListSelectionMode.Single;
            this.embeddedArticlesRelationships.FlatView = true;
            this.embeddedArticlesRelationships.EnableDates = false;
            this.embeddedArticlesRelationships.AllowSearch = true;
            this.embeddedArticlesRelationships.EnableSortOrder = false;
            this.embeddedArticlesRelationships.ItemTypeId = ItemType.Article.GetId();
            this.phEmbeddedArticle.Controls.Add(this.embeddedArticlesRelationships);

            //load approval status
            itemApprovalStatus = (ItemApproval)LoadControl(approvalControlToLoad);
            itemApprovalStatus.ModuleConfiguration = ModuleConfiguration;
            itemApprovalStatus.ID = Path.GetFileNameWithoutExtension(approvalControlToLoad);
            itemApprovalStatus.VersionInfoObject = VersionInfoObject;
            this.phApproval.Controls.Add(itemApprovalStatus);


            if (AllowTags)
            {
                rowTagEntry.Visible = true;
                StringBuilder tagList = new StringBuilder(255);
                foreach (ItemTag it in VersionInfoObject.Tags)
                {
                    tagList.Append(Tag.GetTag(it.TagId,PortalId).Name);
                    tagList.Append(";");
                }

                tagEntryControl = (TagEntry)LoadControl(tagControlToLoad);
                tagEntryControl.ModuleConfiguration = ModuleConfiguration;
                tagEntryControl.ID = Path.GetFileNameWithoutExtension(tagControlToLoad);


                tagEntryControl.TagList = tagList.ToString();
                this.phTagEntry.Controls.Add(tagEntryControl);

            }
            else
            {
                rowTagEntry.Visible = false;
            }

        }

       

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {

                LocalizeCollapsePanels();

                DotNetNuke.UI.Utilities.ClientAPI.AddButtonConfirm(cmdDelete, Localization.GetString("DeleteConfirm", LocalResourceFile));
                Article av = (Article)VersionInfoObject;
                if (!Page.IsPostBack)
                {
                    //check to see if we're dealing with a new Item, if so set the parentid based on the querystring
                    if (av.IsNew)
                    {
                        if (ParentId != -1)
                        {
                            Category parent = Category.GetCategory(ParentId, PortalId);// = null;
                            this.parentCategoryRelationship.AddToSelectedItems(parent);
                        }
                    }

                    trArticleId.Visible = ShowItemIds;

                    this.txtArticleId.Text = VersionInfoObject.ItemId.ToString(CultureInfo.CurrentCulture);
                    this.txtVersionNumber.Text = av.VersionNumber;
                    this.teArticleText.Text = av.ArticleText;
                    this.txtPreviousVersionDescription.Text = av.VersionDescription;

                    this.rblDisplayOnCurrentPage.Items.Add(new ListItem(Localization.GetString("CurrentPage", LocalResourceFile), true.ToString(CultureInfo.InvariantCulture)));
                    this.rblDisplayOnCurrentPage.Items.Add(new ListItem(Localization.GetString("SpecificPage", LocalResourceFile), false.ToString(CultureInfo.InvariantCulture)));

                    //get the pnlPrinterFriendly setting
                    ItemVersionSetting pfSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "pnlPrinterFriendly", "Visible", PortalId);
                    if (pfSetting != null)
                    {
                        chkPrinterFriendly.Checked = Convert.ToBoolean(pfSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostPrinterFriendlySetting = HostSettings.GetHostSetting(Utility.PublishDefaultPrinterFriendly + PortalId.ToString(CultureInfo.InvariantCulture));
                        chkPrinterFriendly.Checked = !Utility.HasValue(hostPrinterFriendlySetting) || Convert.ToBoolean(hostPrinterFriendlySetting, CultureInfo.InvariantCulture);
                    }

                    //get the pnlEmailAFriend setting
                    ItemVersionSetting efSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "pnlEmailAFriend", "Visible", PortalId);
                    if (efSetting != null)
                    {
                        chkEmailAFriend.Checked = Convert.ToBoolean(efSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostEmailFriendSetting = HostSettings.GetHostSetting(Utility.PublishDefaultEmailAFriend + PortalId.ToString(CultureInfo.InvariantCulture));
                        chkEmailAFriend.Checked = !Utility.HasValue(hostEmailFriendSetting) || Convert.ToBoolean(hostEmailFriendSetting, CultureInfo.InvariantCulture);
                    }

                    //if ratings are enabled show options
                    if (AreRatingsEnabled)
                    {
                        //get the upnlRating setting
                        ItemVersionSetting rtSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "upnlRating", "Visible", PortalId);
                        if (rtSetting != null)
                        {
                            chkRatings.Checked = Convert.ToBoolean(rtSetting.PropertyValue, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            string hostRatingSetting = HostSettings.GetHostSetting(Utility.PublishDefaultRatings + PortalId.ToString(CultureInfo.InvariantCulture));
                            chkRatings.Checked = !Utility.HasValue(hostRatingSetting) || Convert.ToBoolean(hostRatingSetting, CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        chkRatings.Visible = false;
                    }

                    //if comments are enabled show options.
                    if (IsCommentsEnabled)
                    {
                        //get the pnlComments setting
                        ItemVersionSetting ctSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "pnlComments", "Visible", PortalId);
                        if (ctSetting != null)
                        {
                            chkComments.Checked = Convert.ToBoolean(ctSetting.PropertyValue, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            string hostCommentSetting = HostSettings.GetHostSetting(Utility.PublishDefaultComments + PortalId.ToString(CultureInfo.InvariantCulture));
                            chkComments.Checked = !Utility.HasValue(hostCommentSetting) || Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture);
                        }

                        if (IsPublishCommentType)
                        {
                            chkForumComments.Visible = false;
                        }
                        else
                        {
                            ItemVersionSetting forumCommentSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "chkForumComments", "Checked", PortalId);
                            chkForumComments.Checked = forumCommentSetting == null || Convert.ToBoolean(forumCommentSetting.PropertyValue, CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        chkComments.Visible = false;
                        chkForumComments.Visible = false;
                    }

                    //chkIncludeRelatedArticles
                    ItemVersionSetting raSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "ArticleSettings", "IncludeParentCategoryArticles", PortalId);
                    chkIncludeOtherArticlesFromSameList.Checked = raSetting != null && Convert.ToBoolean(raSetting.PropertyValue, CultureInfo.InvariantCulture);

                    //chkShowAuthor
                    ItemVersionSetting auSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "pnlAuthor", "Visible", PortalId);
                    if (auSetting != null)
                    {
                        chkShowAuthor.Checked = Convert.ToBoolean(auSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostAuthorSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowAuthor + PortalId.ToString(CultureInfo.InvariantCulture));
                        chkShowAuthor.Checked = Utility.HasValue(hostAuthorSetting) && Convert.ToBoolean(hostAuthorSetting, CultureInfo.InvariantCulture);
                    }

                    //chkShowTags
                    ItemVersionSetting tagSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "pnlTags", "Visible", PortalId);
                    if (tagSetting != null)
                    {
                        chkTags.Checked = Convert.ToBoolean(tagSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostTagsSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowTags + PortalId.ToString(CultureInfo.InvariantCulture));
                        chkTags.Checked = Utility.HasValue(hostTagsSetting) && Convert.ToBoolean(hostTagsSetting, CultureInfo.InvariantCulture);
                    }

                    //chkDisplayOnCurrentPage
                    ItemVersionSetting cpSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "ArticleSettings", "DisplayOnCurrentPage", PortalId);
                    if (cpSetting != null)
                    {
                        //TODO: cache display on currentpage setting somewhere?
                        rblDisplayOnCurrentPage.SelectedValue = av.DisplayOnCurrentPage().ToString(CultureInfo.InvariantCulture);
                        if (av.DisplayOnCurrentPage())
                        {
                            chkForceDisplayTab.Checked = false;
                            chkForceDisplayTab.Visible = false;
                            lblForceDisplayTab.Visible = false;
                            ddlDisplayTabId.Enabled = false;
                        }
                        else
                        {
                            chkForceDisplayTab.Visible = true;
                            lblForceDisplayTab.Visible = true;
                            ddlDisplayTabId.Enabled = true;
                        }
                    }
                    else if (av.DisplayTabId < 0)
                    {
                        rblDisplayOnCurrentPage.SelectedValue = false.ToString(CultureInfo.InvariantCulture);
                        chkForceDisplayTab.Checked = false;
                        chkForceDisplayTab.Visible = true;
                        lblForceDisplayTab.Visible = true;
                        ddlDisplayTabId.Enabled = true;
                    }
                    else
                    {
                        rblDisplayOnCurrentPage.SelectedValue = false.ToString(CultureInfo.InvariantCulture);
                        chkForceDisplayTab.Visible = true;
                        lblForceDisplayTab.Visible = true;
                        ddlDisplayTabId.Enabled = true;
                    }

                    chkForceDisplayTab.Checked = av.ForceDisplayOnPage();

                    ItemVersionSetting rlSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "ArticleSettings", "DisplayReturnToList", PortalId);
                    if (rlSetting != null)
                    {
                        chkReturnList.Checked = Convert.ToBoolean(rlSetting.PropertyValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        string hostReturnToListSetting = HostSettings.GetHostSetting(Utility.PublishDefaultReturnToList + PortalId.ToString(CultureInfo.InvariantCulture));
                        chkReturnList.Checked = Utility.HasValue(hostReturnToListSetting) && Convert.ToBoolean(hostReturnToListSetting, CultureInfo.InvariantCulture);
                    }

                    //use approvals setting
                    ItemVersionSetting useApprovals = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "chkUseApprovals", "Checked", PortalId);
                    chkUseApprovals.Checked = useApprovals == null || Convert.ToBoolean(useApprovals.PropertyValue, CultureInfo.InvariantCulture);
                    chkUseApprovals.Visible = IsAdmin && UseApprovals;
                    phApproval.Visible = chkUseApprovals.Checked && UseApprovals;
                    lblNotUsingApprovals.Visible = !chkUseApprovals.Checked || !UseApprovals;

                    LoadPhotoGalleryDropDown(av);
                    LoadDisplayTabDropDown();
                }
                else
                {
                    if (ddlDisplayTabId.SelectedIndex > 0 && ddlDisplayTabId.Enabled)
                    {
                        av.DisplayTabId = Convert.ToInt32(ddlDisplayTabId.SelectedValue, CultureInfo.InvariantCulture);
                    }

                    av.VersionNumber = this.txtVersionNumber.Text;
                    av.VersionDescription = this.txtVersionDescription.Text;
                    av.ArticleText = this.teArticleText.Text;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            
            //the ItemRelationships control doesn't bind until after this Page_Load,
            //so we need to use the PreRender to see whether there are articles
            bool thereAreRelatedArticles = relatedArticlesRelationships.GetSelectedItemIds().Length > 0;
            bool thereIsAnEmbeddedArtice = embeddedArticlesRelationships.GetSelectedItemIds().Length > 0;
            if (thereAreRelatedArticles || thereIsAnEmbeddedArtice || chkIncludeOtherArticlesFromSameList.Checked)
            {
                chkIncludeRelatedArticles.Checked = true;
                phRelatedArticles.Visible = true;
                phEmbeddedArticle.Visible = thereIsAnEmbeddedArtice || UseEmbeddedArticles;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void chkIncludeRelatedArticles_CheckedChanged(object sender, EventArgs e)
        {
            phRelatedArticles.Visible = chkIncludeRelatedArticles.Checked;
            phEmbeddedArticle.Visible = chkIncludeRelatedArticles.Checked && UseEmbeddedArticles;

            if (!chkIncludeRelatedArticles.Checked)
            {
                //Remove all Related and Embedded Articles if they choose not to include related articles.
                relatedArticlesRelationships.Clear();
                embeddedArticlesRelationships.Clear();
            }
        }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        //protected void chkForceDisplayTab_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkForceDisplayTab.Checked)
        //    {
        //        rblDisplayOnCurrentPage.SelectedValue = false.ToString(CultureInfo.InvariantCulture);
        //        //populate the list of display pages with all pages configured for Publish, even if they aren't overrideable.
        //    }
        //    LoadDisplayTabDropDown();
        //}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void rblDisplayOnCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if display on current page is selected
            if (Convert.ToBoolean(rblDisplayOnCurrentPage.SelectedValue, CultureInfo.InvariantCulture))
            {
                chkForceDisplayTab.Visible = false;
                lblForceDisplayTab.Visible = false;
                chkForceDisplayTab.Checked = false;
                ddlDisplayTabId.Enabled = false;
            }
            else //if display on specific page is selected
            {
                chkForceDisplayTab.Visible = true;
                lblForceDisplayTab.Visible = true;
                ddlDisplayTabId.Enabled = true;
            }
            LoadDisplayTabDropDown();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            string returnUrl = Server.UrlDecode(Request.QueryString["returnUrl"]);
            if (!Utility.HasValue(returnUrl))
            {
                Response.Redirect(BuildCategoryListUrl(ItemType.Article), true);
            }
            else
            {
                Response.Redirect(returnUrl, true);
            }
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Article av = (Article)VersionInfoObject;

                this.txtMessage.Text = string.Empty;
                bool error = false;

                if (TextBoxMaxLengthExceeded(this.itemEditControl.DescriptionText, "Article Description", 4000))
                {
                    error = true;
                }
                if (TextBoxMaxLengthExceeded(this.txtVersionDescription.Text, "Version Description", 8000))
                {
                    error = true;
                }

                if (!this.parentCategoryRelationship.IsValid)
                {
                    error = true;
                    this.txtMessage.Text += Localization.GetString("ErrorSelectCategory.Text", LocalResourceFile);
                }

                if (!this.itemApprovalStatus.IsValid && this.itemApprovalStatus.Visible)
                {
                    error = true;
                    this.txtMessage.Text += Localization.GetString("ErrorApprovalStatus.Text", LocalResourceFile);
                }

                if (!this.itemEditControl.IsValid)
                {
                    error = true;
                    this.txtMessage.Text += this.itemEditControl.ErrorMessage;
                }

                if (Convert.ToInt32(ddlDisplayTabId.SelectedValue, CultureInfo.InvariantCulture) > -1)
                {
                    VersionInfoObject.DisplayTabId = Convert.ToInt32(ddlDisplayTabId.SelectedValue, CultureInfo.InvariantCulture);
                }
                else
                {
                    error = true;
                    this.txtMessage.Text += Localization.GetString("ErrorDisplayPage.Text", LocalResourceFile);
                }

                if (error)
                {
                    this.txtMessage.Visible = true;
                    return;
                }

                av.ArticleText = teArticleText.Text;
                av.VersionDescription = txtVersionDescription.Text;
                av.VersionNumber = txtVersionNumber.Text;
                av.Description = itemEditControl.DescriptionText;
                if (av.IsNew) av.ModuleId = ModuleId;

                //create a relationship
                ItemRelationship irel = new ItemRelationship();
                irel.RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
                int[] ids = this.parentCategoryRelationship.GetSelectedItemIds();

                //check for parent category, if none then add a relationship for Top Level Item
                if (ids.Length >0)
                {
                    irel.ParentItemId = ids[0];
                    VersionInfoObject.Relationships.Add(irel);
                }

                foreach (int i in this.relatedCategoryRelationships.GetSelectedItemIds())
                {
                    ItemRelationship irco = new ItemRelationship();
                    irco.RelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();
                    irco.ParentItemId = i;
                    av.Relationships.Add(irco);
                }

                foreach (int i in this.relatedArticlesRelationships.GetSelectedItemIds())
                {
                    ItemRelationship irArticleso = new ItemRelationship();
                    irArticleso.RelationshipTypeId = RelationshipType.ItemToRelatedArticle.GetId();
                    irArticleso.ParentItemId = i;
                    av.Relationships.Add(irArticleso);
                }

                foreach (int i in this.embeddedArticlesRelationships.GetSelectedItemIds())
                {
                    ItemRelationship irLinksso = new ItemRelationship();
                    irLinksso.RelationshipTypeId = RelationshipType.ItemToArticleLinks.GetId();
                    irLinksso.ParentItemId = i;
                    av.Relationships.Add(irLinksso);
                }

                if (AllowTags)
                {
                    av.Tags.Clear();
                    //Add the tags to the ItemTagCollection
                    foreach (Tag t in Tag.ParseTags(tagEntryControl.TagList.ToString(), PortalId))
                    {
                        ItemTag it = ItemTag.Create();
                        it.TagId = Convert.ToInt32(t.TagId, CultureInfo.InvariantCulture);
                        av.Tags.Add(it);
                    }
                }

                //TODO: add a checkbox to enable this functionality
                //check to see if av.Description == string.empty
                if (av.Description == string.Empty)
                {
                    //trim article text to populate description
                    //if our article is over 8k characters be sure to trim it
                    if (!Utility.HasValue(av.Description) || !Utility.HasValue(av.MetaDescription))
                    {
                        string description = DotNetNuke.Common.Utilities.HtmlUtils.StripTags(av.ArticleText, false);

                        if (!Utility.HasValue(av.MetaDescription))
                            av.MetaDescription = Utility.TrimDescription(399, description);

                        if (!Utility.HasValue(av.Description))
                            av.Description = Utility.TrimDescription(3997, description) + "...";// description + "...";
                    }
                }

                //Save the ItemVersionSettings
                SaveSettings();

                //approval status
                av.ApprovalStatusId = chkUseApprovals.Checked && UseApprovals ? itemApprovalStatus.ApprovalStatusId : ApprovalStatus.Approved.GetId();

                VersionInfoObject.Save(UserId);


                //remove all the cache items for this publish item
                //DataCache.RemoveCache(Utility.CacheKeyPublishArticle + VersionInfoObject.ItemId.ToString(CultureInfo.InvariantCulture));
                

                string returnUrl = Server.UrlDecode(Request.QueryString["returnUrl"]);
                if (!Utility.HasValue(returnUrl))
                {
                    Response.Redirect(Globals.NavigateURL(TabId, "", "", "ctl=" + Utility.AdminContainer, "mid=" + ModuleId.ToString(CultureInfo.InvariantCulture),
                        "adminType=itemCreated", "itemId=" + VersionInfoObject.ItemId.ToString(CultureInfo.InvariantCulture)), true);
                }
                else
                {
                    Response.Redirect(returnUrl);
                }
            }

            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            this.txtMessage.Visible = true;
            bool itemExists = false;

            if (ItemId > -1)
            {
                //Using GetItemTypeId as substitute for IfExists
                if (Item.GetItemTypeId(ItemId, PortalId) > -1)
                {
                    itemExists = true;
                    bool inUse = false;
                    StringBuilder modulesInUse = new StringBuilder();
                    ModuleController mc = new ModuleController();
                    ArrayList modules = mc.GetModulesByDefinition(PortalId, Utility.DnnFriendlyModuleName);

                    foreach (ModuleInfo module in modules)
                    {
                        Hashtable settings = mc.GetTabModuleSettings(module.TabModuleID);

                        if (settings.ContainsKey("DisplayType") && settings["DisplayType"].ToString() == "ArticleDisplay")
                        {
                            int articleId;
                            if (settings.ContainsKey("adArticleId") && int.TryParse(settings["adArticleId"].ToString(), out articleId))
                            {
                                if (articleId == ItemId)
                                {
                                    inUse = true;
                                    modulesInUse.AppendFormat("{0} ({1}){2}", module.ModuleTitle, module.TabID, Environment.NewLine);
                                    break;
                                }
                            }
                        }
                    }

                    ArrayList featuredRelationships = ItemRelationship.GetItemChildRelationships(ItemId, RelationshipType.ItemToFeaturedItem.GetId());
                    bool isFeatured = featuredRelationships.Count > 0;

                    if (!inUse && !isFeatured)
                    {
                        Item.DeleteItem(ItemId);
                        txtMessage.Text = Localization.GetString("DeleteSuccess", LocalResourceFile);

                    }
                    else
                    {
                        StringBuilder errorMessage = new StringBuilder();

                        if (inUse)
                        {
                            errorMessage.AppendFormat("{0}{1}", Localization.GetString("DeleteFailureInUse", LocalResourceFile), Environment.NewLine);
                            errorMessage.Append(modulesInUse.ToString());
                        }
                        if (isFeatured)
                        {
                            errorMessage.AppendFormat("{0}{1}", Localization.GetString("DeleteFailureIsFeatured", LocalResourceFile), Environment.NewLine);

                            foreach (ItemRelationship rel in featuredRelationships)
                            {
                                Category parentCategory = Category.GetCategory(rel.ChildItemId, PortalId);
                                errorMessage.AppendFormat("{0}{1}", parentCategory.Name, Environment.NewLine);
                            }
                        }
                        txtMessage.Text = errorMessage.ToString();
                    }
                }
            }
            if (!itemExists)
            {
                this.txtMessage.Text = Localization.GetString("DeleteFailure", LocalResourceFile);
            }
            ShowOnlyMessage();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void chkUseApprovals_CheckedChanged(object sender, EventArgs e)
        {
            phApproval.Visible = chkUseApprovals.Checked && UseApprovals;
            lblNotUsingApprovals.Visible = !chkUseApprovals.Checked || !UseApprovals;
        }
        #endregion

        private void LoadDisplayTabDropDown()
        {
            ddlDisplayTabId.Items.Clear();

            string[] modules = new string[] { Utility.DnnFriendlyModuleName };
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            if (chkForceDisplayTab.Checked)
            {
                //if the ForceDisplayTab is checked we need to make sure we get ALL publish modules, not just overrideable ones
                dt = Utility.GetDisplayTabIdsAll(modules);
            }
            else
            {
                dt = Utility.GetDisplayTabIds(modules);
                if (dt.Rows.Count < 1)
                {
                    //if there are no items in the list, meaning there are no modules set to be overrideable, then get the list of all Publish pages.
                    dt = Utility.GetDisplayTabIdsAll(modules);
                }

            }

            this.ddlDisplayTabId.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));


            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["TabName"] + " (" + dr["TabID"] + ")", dr["TabID"].ToString());
                this.ddlDisplayTabId.Items.Add(li);
            }

            //check if the DisplayTabId should be set.
            Article av = (Article)VersionInfoObject;
            if (!VersionInfoObject.IsNew)
            {
                ListItem li = ddlDisplayTabId.Items.FindByValue(av.DisplayTabId.ToString(CultureInfo.InvariantCulture));
                if (li != null)
                {
                    ddlDisplayTabId.ClearSelection();
                    li.Selected = true;
                }
                else
                {
                    //if we made it here we've hit an article who is pointing to a page that is no longer overrideable, set the default page.
                    if (DefaultDisplayTabId > 0)
                    {
                        li = ddlDisplayTabId.Items.FindByValue(DefaultDisplayTabId.ToString(CultureInfo.InvariantCulture));
                        if (li != null)
                        {
                            ddlDisplayTabId.ClearSelection();
                            li.Selected = true;
                        }
                    }
                }
            }
            else
            {
                Category parent = null;
                if (ParentId != -1)
                {
                    parent = Category.GetCategory(ParentId, PortalId);
                }

                //look for display tab id
                if (parent != null && parent.ChildDisplayTabId > 0)
                {
                    if (ddlDisplayTabId.Items.FindByValue(parent.ChildDisplayTabId.ToString(CultureInfo.InvariantCulture)) != null)
                    {
                        ddlDisplayTabId.SelectedIndex = -1;
                        ddlDisplayTabId.Items.FindByValue(parent.ChildDisplayTabId.ToString(CultureInfo.InvariantCulture)).Selected = true;
                    }
                }
                
                else
                {
                    //load the default display tab
                    ListItem li = ddlDisplayTabId.Items.FindByValue(DefaultDisplayTabId.ToString(CultureInfo.InvariantCulture));
                    if (li != null)
                    {
                        ddlDisplayTabId.ClearSelection();
                        li.Selected = true;
                    }
                }
            }
        }

        private void LoadPhotoGalleryDropDown(Article av)
        {
            if (AllowSimpleGalleryIntegration || AllowUltraMediaGalleryIntegration)
            {
                rowPhotoGallery.Visible = true;

                FillPhotoGalleryDropDown();

                if (ddlPhotoGalleryAlbum.Items.Count > 0)
                {
                    ItemVersionSetting simpleGalleryAlbum = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "ddlSimpleGalleryAlbum", "SelectedValue", PortalId);
                    if (simpleGalleryAlbum != null && Utility.HasValue(simpleGalleryAlbum.PropertyValue))
                    {
                        ddlPhotoGalleryAlbum.ClearSelection();
                        ddlPhotoGalleryAlbum.SelectedValue = "s" + simpleGalleryAlbum.PropertyValue;
                    }
                    else
                    {
                        ItemVersionSetting ultraMediaGalleryAlbum = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "ddlUltraMediaGalleryAlbum", "SelectedValue", PortalId);
                        if (ultraMediaGalleryAlbum != null && Utility.HasValue(ultraMediaGalleryAlbum.PropertyValue))
                        {
                            ddlPhotoGalleryAlbum.ClearSelection();
                            ddlPhotoGalleryAlbum.SelectedValue = "u" + ultraMediaGalleryAlbum.PropertyValue;
                        }
                    }
                }
                else
                {
                    rowPhotoGallery.Visible = false;
                }
            }
            ddlPhotoGalleryAlbum.Items.Insert(0, new ListItem(string.Empty));
        }

        private void FillPhotoGalleryDropDown()
        {
            ModuleController modules = new ModuleController();

            SortedList<string, ListItem> simpleGalleryAlbums = new SortedList<string, ListItem>(StringComparer.CurrentCultureIgnoreCase);
            if (AllowSimpleGalleryIntegration)
            {
                foreach (ModuleInfo module in modules.GetModulesByDefinition(PortalId, Utility.SimpleGalleryFriendlyName))
                {
                    if (PortalSecurity.IsInRoles(module.AuthorizedEditRoles))
                    {
                        IDataReader dr = DataProvider.Instance().GetSimpleGalleryAlbums(module.ModuleID);
                        while (dr.Read())
                        {
                            string caption = dr["Caption"].ToString();
                            for (int i = 0; simpleGalleryAlbums.ContainsKey(caption); i++)
                            {
                                caption += i.ToString(CultureInfo.InvariantCulture);
                            }
                            simpleGalleryAlbums.Add(caption, new ListItem(dr["Caption"].ToString(), "s" + dr["AlbumId"].ToString()));
                        }
                    }
                }
            }

            SortedList<string, ListItem> ultaMediaGalleryAlbums = new SortedList<string, ListItem>(StringComparer.CurrentCultureIgnoreCase);
            if (AllowUltraMediaGalleryIntegration)
            {
                foreach (ModuleInfo module in modules.GetModulesByDefinition(PortalId, Utility.UltraMediaGalleryFriendlyName))
                {
                    if (PortalSecurity.IsInRoles(module.AuthorizedEditRoles))
                    {
                        IDataReader dr = DataProvider.Instance().GetUltraMediaGalleryAlbums(module.ModuleID);
                        while (dr.Read())
                        {
                            string title = dr["Title"].ToString();
                            for (int i = 0; ultaMediaGalleryAlbums.ContainsKey(title); i++)
                            {
                                title += i.ToString(CultureInfo.InvariantCulture);
                            }
                            ultaMediaGalleryAlbums.Add(title, new ListItem(dr["Title"].ToString(), "u" + dr["ItemId"].ToString()));
                        }
                    }
                }
            }

            //label which module it's from if there are some from both.
            bool labelModuleType = simpleGalleryAlbums.Count > 0 && ultaMediaGalleryAlbums.Count > 0;

            foreach (ListItem li in simpleGalleryAlbums.Values)
            {
                if (labelModuleType)
                {
                    li.Text += " - " + Localization.GetString("SimpleGallery", LocalResourceFile);
                }
                ddlPhotoGalleryAlbum.Items.Add(li);
            }

            foreach (ListItem li in ultaMediaGalleryAlbums.Values)
            {
                if (labelModuleType)
                {
                    li.Text += " - " + Localization.GetString("UltraMediaGallery", LocalResourceFile);
                }
                ddlPhotoGalleryAlbum.Items.Add(li);
            }
        }

        private bool TextBoxMaxLengthExceeded(string text, string controlName, int length)
        {
            bool b = (text.Length > length);
            if (b)
            {
                this.txtMessage.Text += String.Format(CultureInfo.CurrentCulture, Localization.GetString("MaximumLength", LocalResourceFile), controlName, length.ToString(CultureInfo.CurrentCulture), text.Length);
                this.txtMessage.Visible = true;
            }

            return b;
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

        private void SaveSettings()
        {
            Article av = (Article)VersionInfoObject;

            //Printer Friendly
            Setting setting = Setting.PrinterFriendly;
            setting.PropertyValue = chkPrinterFriendly.Checked.ToString(CultureInfo.InvariantCulture);
            ItemVersionSetting itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //Email A Friend
            setting = Setting.EmailAFriend;
            setting.PropertyValue = chkEmailAFriend.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //ratings
            setting = Setting.Rating;
            setting.PropertyValue = chkRatings.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //comments
            setting = Setting.Comments;
            setting.PropertyValue = chkComments.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //forum comments
            setting = Setting.ForumComments;
            setting.PropertyValue = chkForumComments.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //forum comments thread ID
            //just continue forward to the next version, this doesn't get set in the edit screen
            itemVersionSetting = ItemVersionSetting.GetItemVersionSetting(av.ItemVersionId, "ArticleSetting", "CommentForumThreadId", PortalId);
            if (itemVersionSetting != null)
            {
                av.VersionSettings.Add(itemVersionSetting);
            }

            //include all articles from the parent category
            setting = Setting.ArticleSettingIncludeCategories;
            setting.PropertyValue = chkIncludeOtherArticlesFromSameList.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //display on current page option
            setting = Setting.ArticleSettingCurrentDisplay;
            setting.PropertyValue = rblDisplayOnCurrentPage.SelectedValue;
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //force display on specific page
            setting = Setting.ArticleSettingForceDisplay;
            setting.PropertyValue = chkForceDisplayTab.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //display return to list
            setting = Setting.ArticleSettingReturnToList;
            setting.PropertyValue = chkReturnList.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);
            
            //show author
            setting = Setting.Author;
            setting.PropertyValue = chkShowAuthor.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //show tags
            setting = Setting.ShowTags;
            setting.PropertyValue = chkTags.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);
            
            
            //use approvals
            setting = Setting.UseApprovals;
            setting.PropertyValue = chkUseApprovals.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //simple gallery album
            setting = Setting.UseSimpleGalleryAlbum;
            setting.PropertyValue = ddlPhotoGalleryAlbum.SelectedValue.StartsWith("s", StringComparison.Ordinal) ? ddlPhotoGalleryAlbum.SelectedValue.Substring(1) : string.Empty;
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

            //ultra media gallery album
            setting = Setting.UseUltraMediaGalleryAlbum;
            setting.PropertyValue = ddlPhotoGalleryAlbum.SelectedValue.StartsWith("u", StringComparison.Ordinal) ? ddlPhotoGalleryAlbum.SelectedValue.Substring(1) : string.Empty;
            itemVersionSetting = new ItemVersionSetting(setting);
            av.VersionSettings.Add(itemVersionSetting);

        }

        private void LocalizeCollapsePanels()
        {
            clpExtended.CollapsedText = Localization.GetString("clpExtended.CollapsedText", LocalResourceFile);
            clpExtended.ExpandedText = Localization.GetString("clpExtended.ExpandedText", LocalResourceFile);
            clpExtended.ExpandedImage = ApplicationUrl.ToString() + Localization.GetString("ExpandedImage.Text", GlobalResourceFile).Replace("[L]", "");
            clpExtended.CollapsedImage = ApplicationUrl.ToString() + Localization.GetString("CollapsedImage.Text", GlobalResourceFile).Replace("[L]", "");

        }

     
    }
}

