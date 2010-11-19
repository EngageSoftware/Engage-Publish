//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.CategoryControls
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;

    using Engage.Dnn.Publish.Controls;
    using Engage.Dnn.Publish.Forum;
    using Engage.Dnn.Publish.Security;
    using Engage.Dnn.Publish.Util;

    using Globals = DotNetNuke.Common.Globals;

    public partial class CategoryEdit : ModuleBase
    {
        private const string ApprovalControlToLoad = "../controls/ItemApproval.ascx";

        private const string ItemControlToLoad = "../Controls/itemEdit.ascx";

        private readonly string ItemRelationshipResourceFile = "~" + DesktopModuleFolderName + "Controls/App_LocalResources/ItemRelationships";

        private CategoryPermissions categoryPermissions;

        private ItemRelationships featuredArticlesRelationships;

        private ItemApproval itemApprovalStatus;

        private ItemEdit itemEditControl;

        private ItemRelationships parentCategoryRelationships;

        private int ParentId
        {
            get
            {
                string s = this.Request.QueryString["parentid"];
                return s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
        }

        // [Obsolete("This is not used")]
        // private int TopLevelId
        // {
        // get
        // {
        // string s = Request.QueryString["topLevelId"];
        // return (s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture));
        // }
        // }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            // int tli = TopLevelId;
            this.LoadControlType();
            base.OnInit(e);
            this.LoadSharedResources();
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void chkForceDisplayTab_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkForceDisplayTab.Checked)
            {
                this.rblDisplayOnCurrentPage.SelectedValue = false.ToString(CultureInfo.InvariantCulture);

                // populate the list of display pages with all pages configured for Publish, even if they aren't overrideable.
            }

            this.LoadCategoryDisplayTabDropDown();
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
            bool itemExists = false;
            this.txtMessage.Visible = true;

            if (this.ItemId > -1)
            {
                // Using GetItemTypeId as substitute for IfExists
                if (Item.GetItemTypeId(this.ItemId, this.PortalId) > -1)
                {
                    itemExists = true;
                    DataSet children = ItemRelationship.GetAllChildren(this.ItemId, RelationshipType.ItemToParentCategory.GetId(), this.PortalId);
                    bool hasChildren = children.Tables.Count > 0 && children.Tables[0].Rows.Count > 0;

                    if (!hasChildren)
                    {
                        // Item.DeleteItem(ItemId);
                        Item.DeleteItem(this.ItemId, this.PortalId);
                        this.txtMessage.Text = Localization.GetString("DeleteSuccess", this.LocalResourceFile);

                        // Util.Utility.ClearPublishCache(PortalId);
                    }
                    else
                    {
                        var errorMessage = new StringBuilder();
                        errorMessage.AppendFormat(
                            "{0}{1}", Localization.GetString("DeleteFailureHasChildren", this.LocalResourceFile), Environment.NewLine);

                        foreach (DataRow row in children.Tables[0].Rows)
                        {
                            int itemId; // = 0;
                            if (int.TryParse(row["itemId"].ToString(), out itemId))
                            {
                                errorMessage.AppendFormat(
                                    CultureInfo.CurrentCulture, 
                                    "{0} ({1}, id: {2}){3}", 
                                    row["name"], 
                                    Item.GetItemType(this.ItemId, this.PortalId), 
                                    itemId, 
                                    Environment.NewLine);
                            }
                            else
                            {
                                errorMessage.AppendFormat(
                                    CultureInfo.CurrentCulture, "{0} (id: {1}){2}", row["name"], row["itemId"], Environment.NewLine);
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

            this.LoadCategoryDisplayTabDropDown();
        }

        private void CmdCancelClick(object sender, EventArgs e)
        {
            string returnUrl = this.Server.UrlDecode(this.Request.QueryString["returnUrl"]);
            if (!Engage.Utility.HasValue(returnUrl))
            {
                this.Response.Redirect(this.BuildCategoryListUrl(ItemType.Category), true);
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
                this.txtMessage.Text = string.Empty;
                bool error = false;

                // create a relationship
                var irel = new ItemRelationship
                    {
                        RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId()
                    };
                int[] ids = this.parentCategoryRelationships.GetSelectedItemIds();

                // check for parent category, if none then add a relationship for Top Level Item
                if (ids.Length == 0)
                {
                    // add relationship to TLC
                    irel.RelationshipTypeId = RelationshipType.CategoryToTopLevelCategory.GetId();
                    irel.ParentItemId = TopLevelCategoryItemType.Category.GetId();
                    this.VersionInfoObject.Relationships.Add(irel);
                }
                else
                {
                    irel.ParentItemId = ids[0];
                    this.VersionInfoObject.Relationships.Add(irel);
                }

                // check for parent category, if none then add a relationship for Top Level Item
                // foreach (int i in this.irRelated.GetSelectedItemIds())
                // {
                // ItemRelationship irco = ItemRelationship.Create();
                // irco.RelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();
                // irco.ParentItemId = i;
                // VersionInfoObject.Relationships.Add(irco);
                // }
                if (this.itemEditControl.IsValid == false)
                {
                    error = true;
                    this.txtMessage.Text += this.itemEditControl.ErrorMessage;
                }

                if (Convert.ToInt32(this.ddlDisplayTabId.SelectedValue, CultureInfo.InvariantCulture) < -1)
                {
                    error = true;

                    this.txtMessage.Text += Localization.GetString("ChooseAPage", this.LocalResourceFile);
                }

                if (Convert.ToInt32(this.ddlChildDisplayTabId.SelectedValue, CultureInfo.InvariantCulture) == -1)
                {
                    error = true;
                    this.txtMessage.Text += Localization.GetString("ChooseChildPage", this.LocalResourceFile);
                }

                if (!this.itemApprovalStatus.IsValid)
                {
                    this.txtMessage.Text += Localization.GetString("ChooseApprovalStatus", this.LocalResourceFile);
                }

                if (error)
                {
                    this.txtMessage.Visible = true;
                    return;
                }

                this.VersionInfoObject.Description = this.itemEditControl.DescriptionText;

                // auto populate the meta description if it's not populated already
                if (!Engage.Utility.HasValue(this.VersionInfoObject.MetaDescription))
                {
                    string description = HtmlUtils.StripTags(this.VersionInfoObject.Description, false);
                    this.VersionInfoObject.MetaDescription = Utility.TrimDescription(399, description);
                }

                if (this.VersionInfoObject.IsNew)
                {
                    this.VersionInfoObject.ModuleId = this.ModuleId;
                }

                int sortCount = 0;

                foreach (int i in this.featuredArticlesRelationships.GetSelectedItemIds())
                {
                    var irArticleso = new ItemRelationship
                        {
                            RelationshipTypeId = RelationshipType.ItemToFeaturedItem.GetId(), 
                            ParentItemId = i
                        };

                    if (
                        Engage.Utility.HasValue(this.featuredArticlesRelationships.GetAdditionalSetting("startDate", i.ToString(CultureInfo.InvariantCulture))))
                    {
                        irArticleso.StartDate = this.featuredArticlesRelationships.GetAdditionalSetting(
                            "startDate", i.ToString(CultureInfo.InvariantCulture));
                    }

                    if (Engage.Utility.HasValue(this.featuredArticlesRelationships.GetAdditionalSetting("endDate", i.ToString(CultureInfo.InvariantCulture))))
                    {
                        irArticleso.EndDate = this.featuredArticlesRelationships.GetAdditionalSetting(
                            "endDate", i.ToString(CultureInfo.InvariantCulture));
                    }

                    irArticleso.SortOrder = sortCount;

                    sortCount++;
                    this.VersionInfoObject.Relationships.Add(irArticleso);
                }

                this.SaveSettings();

                // approval status
                if (this.chkUseApprovals.Checked && this.UseApprovals)
                {
                    this.VersionInfoObject.ApprovalStatusId = this.itemApprovalStatus.ApprovalStatusId;
                }
                else
                {
                    this.VersionInfoObject.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                }

                this.VersionInfoObject.Save(this.UserId);

                if (SecurityFilter.IsSecurityEnabled(this.PortalId))
                {
                    this.categoryPermissions.CategoryId = this.VersionInfoObject.ItemId;
                    this.categoryPermissions.Save();
                }

                if (this.chkResetChildDisplayTabs.Checked)
                {
                    ((Category)this.VersionInfoObject).CascadeChildDisplayTab(this.UserId);
                }

                string returnUrl = this.Server.UrlDecode(this.Request.QueryString["returnUrl"]);
                if (!Engage.Utility.HasValue(returnUrl))
                {
                    this.Response.Redirect(
                        Globals.NavigateURL(
                            this.TabId, 
                            string.Empty, 
                            string.Empty, 
                            "ctl=" + Utility.AdminContainer, 
                            "mid=" + this.ModuleId, 
                            "adminType=itemCreated", 
                            "itemId=" + this.VersionInfoObject.ItemId), 
                        true);
                }
                else
                {
                    this.Response.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
            this.cmdUpdate.Click += this.CmdUpdateClick;
            this.cmdCancel.Click += this.CmdCancelClick;
        }

        private void LoadCategoryDisplayTabDropDown()
        {
            this.ddlDisplayTabId.Items.Clear();

            var modules = new[]
                {
                    Utility.DnnFriendlyModuleName
                };

            // ListItem l = new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1");
            // this.ddlDisplayTabId.Items.Insert(0, l);

            // foreach (DataRow dr in dt.Rows)
            // {
            // ListItem li = new ListItem(dr["TabName"] + " (" + dr["TabID"] + ")", dr["TabID"].ToString());
            // this.ddlDisplayTabId.Items.Add(li);
            // }
            DataTable dt = Utility.GetDisplayTabIds(modules);

            // this.ddlDisplayTabId.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
            this.ddlDisplayTabId.DataSource = Globals.GetPortalTabs(this.PortalSettings.DesktopTabs, false, true);
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

            if (!this.VersionInfoObject.IsNew)
            {
                ListItem li = this.ddlDisplayTabId.Items.FindByValue(this.VersionInfoObject.DisplayTabId.ToString(CultureInfo.InvariantCulture));
                if (li != null)
                {
                    this.ddlDisplayTabId.ClearSelection();
                    li.Selected = true;
                }
            }
            else
            {
                Category parent = null;
                if (this.ParentId != -1)
                {
                    parent = Category.GetCategory(this.ParentId, this.PortalId);
                    this.parentCategoryRelationships.AddToSelectedItems(parent);
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

        private void LoadChildDisplayTabDropDown()
        {
            this.ddlChildDisplayTabId.Items.Clear();
            var cv = (Category)this.VersionInfoObject;
            var modules = new[]
                {
                    Utility.DnnFriendlyModuleName
                };
            DataTable dt = Utility.GetDisplayTabIds(modules);

            this.ddlChildDisplayTabId.DataSource = Globals.GetPortalTabs(this.PortalSettings.DesktopTabs, false, true);
            this.ddlChildDisplayTabId.DataBind();

            foreach (DataRow dr in dt.Rows)
            {
                if (this.ddlChildDisplayTabId.Items.FindByValue(dr["TabID"].ToString()) != null)
                {
                    this.ddlChildDisplayTabId.Items.FindByValue(dr["TabID"].ToString()).Text += Localization.GetString(
                        "PublishOverrideable", this.LocalSharedResourceFile);
                }

                // ListItem li = new ListItem(dr["TabName"] + " (" + dr["TabID"] + ")", dr["TabID"].ToString());
                // this.ddlDisplayTabId.Items.Add(li);
            }

            ListItem child = this.ddlChildDisplayTabId.Items.FindByValue(cv.ChildDisplayTabId.ToString(CultureInfo.InvariantCulture));
            if (child != null && child.Value != "-1")
            {
                child.Selected = true;
            }
            else
            {
                this.ddlChildDisplayTabId.SelectedIndex = 0;
            }
        }

        private void LoadCommentForumsDropDown()
        {
            if (this.IsCommentsEnabled && !this.IsPublishCommentType)
            {
                this.ddlCommentForum.Items.Clear();
                foreach (KeyValuePair<int, string> pair in ForumProvider.GetInstance(this.PortalId).GetForums())
                {
                    this.ddlCommentForum.Items.Add(new ListItem(pair.Value, pair.Key.ToString(CultureInfo.InvariantCulture)));
                }

                this.ddlCommentForum.Items.Insert(0, new ListItem(Localization.GetString("NoForum", this.LocalResourceFile), "-1"));

                ItemVersionSetting commentForumSetting = ItemVersionSetting.GetItemVersionSetting(
                    this.VersionInfoObject.ItemVersionId, "CategorySettings", "CommentForumId", this.PortalId);
                if (commentForumSetting != null)
                {
                    this.ddlCommentForum.SelectedValue = commentForumSetting.PropertyValue;
                }
            }
            else
            {
                this.rowCommentForum.Visible = false;
            }
        }

        private void LoadControlType()
        {
            this.UseCache = false;
            if (this.ItemVersionId == -1)
            {
                this.BindItemData(true);
            }
            else
            {
                this.BindItemData();
            }

            // Item Edit
            this.itemEditControl = (ItemEdit)this.LoadControl(ItemControlToLoad);
            this.itemEditControl.ModuleConfiguration = this.ModuleConfiguration;
            this.itemEditControl.ID = Path.GetFileNameWithoutExtension(ItemControlToLoad);
            this.itemEditControl.VersionInfoObject = this.VersionInfoObject;
            this.phItemEdit.Controls.Add(this.itemEditControl);

            if (SecurityFilter.IsSecurityEnabled(this.PortalId))
            {
                this.trCategoryPermissions.Visible = true;

                this.categoryPermissions = (CategoryPermissions)this.LoadControl("../CategoryControls/CategoryPermissions.ascx");
                this.categoryPermissions.CategoryId = this.VersionInfoObject.ItemId;
                this.categoryPermissions.ModuleConfiguration = this.ModuleConfiguration;
                this.phCategoryPermissions.Controls.Add(this.categoryPermissions);
            }

            // Parent Category
            this.parentCategoryRelationships = (ItemRelationships)this.LoadControl("../controls/ItemRelationships.ascx");
            this.parentCategoryRelationships.ExcludeCircularRelationships = true;
            this.parentCategoryRelationships.ModuleConfiguration = this.ModuleConfiguration;
            this.parentCategoryRelationships.LocalResourceFile = this.ItemRelationshipResourceFile;
            this.parentCategoryRelationships.VersionInfoObject = this.VersionInfoObject;
            this.parentCategoryRelationships.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.parentCategoryRelationships.CreateRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.parentCategoryRelationships.AvailableSelectionMode = ListSelectionMode.Single;
            this.parentCategoryRelationships.FlatView = true;
            this.parentCategoryRelationships.ItemTypeId = ItemType.Category.GetId();
            this.phParentCategory.Controls.Add(this.parentCategoryRelationships);

            // Related Categories
            // this.irRelated = (ItemRelationships)LoadControl("../controls/ItemRelationships.ascx");
            // this.irRelated.ModuleConfiguration = ModuleConfiguration;
            // this.irRelated.LocalResourceFile = ItemRelationshipResourceFile;
            // this.irRelated.VersionInfoObject = VersionInfoObject;
            // this.irRelated.ListRelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();
            // this.irRelated.CreateRelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();
            // this.irRelated.AvailableSelectionMode = ListSelectionMode.Multiple;
            // this.irRelated.FlatView = true;
            // this.irRelated.ItemTypeId = ItemType.Category.GetId();
            // this.phParentCategory.Controls.Add(this.irRelated);

            // Featured Articles
            this.featuredArticlesRelationships = (ItemRelationships)this.LoadControl("../controls/ItemRelationships.ascx");
            this.featuredArticlesRelationships.ModuleConfiguration = this.ModuleConfiguration;
            this.featuredArticlesRelationships.VersionInfoObject = this.VersionInfoObject;
            this.featuredArticlesRelationships.LocalResourceFile = this.ItemRelationshipResourceFile;
            this.featuredArticlesRelationships.ListRelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            this.featuredArticlesRelationships.CreateRelationshipTypeId = RelationshipType.ItemToFeaturedItem.GetId();
            this.featuredArticlesRelationships.AvailableSelectionMode = ListSelectionMode.Multiple;
            this.featuredArticlesRelationships.FlatView = true;
            this.featuredArticlesRelationships.EnableDates = true;
            this.featuredArticlesRelationships.AllowSearch = true;
            this.featuredArticlesRelationships.EnableSortOrder = true;
            this.featuredArticlesRelationships.ItemTypeId = ItemType.Article.GetId();
            this.phFeaturedArticles.Controls.Add(this.featuredArticlesRelationships);

            // load approval status
            this.itemApprovalStatus = (ItemApproval)this.LoadControl(ApprovalControlToLoad);
            this.itemApprovalStatus.ModuleConfiguration = this.ModuleConfiguration;
            this.itemApprovalStatus.ID = Path.GetFileNameWithoutExtension(ApprovalControlToLoad);
            this.itemApprovalStatus.VersionInfoObject = this.VersionInfoObject;
            this.phApproval.Controls.Add(this.itemApprovalStatus);
        }

        private void LoadSharedResources()
        {
            this.lblPublishOverrideable.Text = Localization.GetString("lblPublishOverrideable.Text", this.LocalSharedResourceFile);
            this.lblPublishOverrideableChild.Text = Localization.GetString("lblPublishOverrideable.Text", this.LocalSharedResourceFile);
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

                // because we're in the Edit options turn off caching
                ClientAPI.AddButtonConfirm(this.cmdDelete, Localization.GetString("DeleteConfirm", this.LocalResourceFile));
                var cv = (Category)this.VersionInfoObject;

                if (!this.Page.IsPostBack)
                {
                    this.rblDisplayOnCurrentPage.Items.Add(
                        new ListItem(Localization.GetString("CurrentPage", this.LocalResourceFile), true.ToString(CultureInfo.InvariantCulture)));
                    this.rblDisplayOnCurrentPage.Items.Add(
                        new ListItem(Localization.GetString("SpecificPage", this.LocalResourceFile), false.ToString(CultureInfo.InvariantCulture)));

                    this.txtSortOrder.Text = cv.SortOrder.ToString(CultureInfo.CurrentCulture);
                    this.txtCategoryId.Text = cv.ItemId.ToString(CultureInfo.CurrentCulture);
                    this.cmdDelete.Visible = false;

                    // check if new or edit
                    if (this.VersionInfoObject.IsNew)
                    {
                        this.txtCategoryId.Visible = false;
                        this.lblCategoryId.Visible = false;

                        if (this.ParentId != -1)
                        {
                            Category parent = Category.GetCategory(this.ParentId, this.PortalId);
                            this.parentCategoryRelationships.AddToSelectedItems(parent);
                        }
                    }

                    this.trCategoryId.Visible = this.ShowItemIds;

                    // check if the DisplayTabId should be set.

                    // chkDisplayOnCurrentPage
                    ItemVersionSetting cpSetting = ItemVersionSetting.GetItemVersionSetting(
                        this.VersionInfoObject.ItemVersionId, "CategorySettings", "DisplayOnCurrentPage", this.PortalId);
                    if (cpSetting != null)
                    {
                        this.rblDisplayOnCurrentPage.SelectedValue =
                            this.VersionInfoObject.DisplayOnCurrentPage().ToString(CultureInfo.InvariantCulture);
                        if (this.VersionInfoObject.DisplayOnCurrentPage())
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
                    else if (this.VersionInfoObject.DisplayTabId < 0)
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

                    this.chkForceDisplayTab.Checked = this.VersionInfoObject.ForceDisplayOnPage();

                    this.LoadCommentForumsDropDown();
                    this.LoadCategoryDisplayTabDropDown();
                    this.LoadChildDisplayTabDropDown();

                    ItemVersionSetting useApprovals = ItemVersionSetting.GetItemVersionSetting(
                        this.VersionInfoObject.ItemVersionId, "chkUseApprovals", "Checked", this.PortalId);
                    this.chkUseApprovals.Checked = useApprovals == null || Convert.ToBoolean(useApprovals.PropertyValue, CultureInfo.InvariantCulture);
                    this.chkUseApprovals.Visible = this.IsAdmin && this.UseApprovals;
                    this.phApproval.Visible = this.UseApprovals && this.chkUseApprovals.Checked;
                    this.lblNotUsingApprovals.Visible = !this.UseApprovals || !this.chkUseApprovals.Checked;

                    // itemversionsetting for external RSS feed
                    // provide the ability to define an external RSS feed for a category.
                    ItemVersionSetting rssSetting = ItemVersionSetting.GetItemVersionSetting(
                        this.VersionInfoObject.ItemVersionId, "CategorySettings", "RssUrl", this.PortalId);
                    if (rssSetting != null)
                    {
                        this.txtRssUrl.Text = rssSetting.PropertyValue;
                    }
                }
                else
                {
                    cv.SortOrder = Convert.ToInt32(this.txtSortOrder.Text, CultureInfo.InvariantCulture);
                    this.VersionInfoObject.DisplayTabId = Convert.ToInt32(this.ddlDisplayTabId.SelectedValue, CultureInfo.InvariantCulture);
                    cv.ChildDisplayTabId = Convert.ToInt32(this.ddlChildDisplayTabId.SelectedValue, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SaveSettings()
        {
            // use approvals
            Setting setting = Setting.UseApprovals;
            setting.PropertyValue = this.chkUseApprovals.Checked.ToString(CultureInfo.InvariantCulture);
            var itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionInfoObject.VersionSettings.Add(itemVersionSetting);

            // AuthorName setting
            setting = Setting.AuthorName;
            setting.PropertyValue = this.itemEditControl.AuthorName;
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionInfoObject.VersionSettings.Add(itemVersionSetting);

            // display on current page option
            setting = Setting.CategorySettingsCurrentDisplay;
            setting.PropertyValue = this.rblDisplayOnCurrentPage.SelectedValue;
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionInfoObject.VersionSettings.Add(itemVersionSetting);

            // force display on specific page
            setting = Setting.CategorySettingsForceDisplay;
            setting.PropertyValue = this.chkForceDisplayTab.Checked.ToString(CultureInfo.InvariantCulture);
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionInfoObject.VersionSettings.Add(itemVersionSetting);

            // external RSS Url for category
            setting = Setting.CategorySettingsRssUrl;
            setting.PropertyValue = this.txtRssUrl.Text.Trim();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionInfoObject.VersionSettings.Add(itemVersionSetting);

            if (this.rowCommentForum.Visible)
            {
                setting = Setting.CategorySettingsCommentForumId;
                setting.PropertyValue = this.ddlCommentForum.SelectedValue;
                itemVersionSetting = new ItemVersionSetting(setting);
                this.VersionInfoObject.VersionSettings.Add(itemVersionSetting);
            }
        }

        /// <summary>
        /// Hides all controls except for the message box.
        /// </summary>
        private void ShowOnlyMessage()
        {
            foreach (Control cntl in this.Controls)
            {
                cntl.Visible = false;
            }

            this.txtMessage.Visible = true;
            this.txtMessage.Parent.Visible = true;
        }
    }
}