//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Framework;
    using DotNetNuke.Security;
    using DotNetNuke.Security.Roles;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Personalization;

    using Engage.Dnn.Publish.Util;

    public partial class ItemEdit : ModuleBase, IActionable
    {
        private string _authorName = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _errorMessage = string.Empty;

        public string AuthorName
        {
            [DebuggerStepThrough]
            get { return this._authorName; }
            [DebuggerStepThrough]
            set { this._authorName = value; }
        }

        public string DescriptionText
        {
            get { return this.txtDescription.Visible ? this.txtDescription.Text : this.teDescription.Text; }
        }

        public string ErrorMessage
        {
            [DebuggerStepThrough]
            get { return this._errorMessage; }
            [DebuggerStepThrough]
            set { this._errorMessage = value; }
        }

        public bool IsValid
        {
            get
            {
                bool returnVal = true;

                DateTime start;
                DateTime end;
                if (DateTime.TryParse(this.txtStartDate.Text, out start) && DateTime.TryParse(this.txtEndDate.Text, out end))
                {
                    returnVal = end > start;
                    this.ErrorMessage = Localization.GetString("DateError", this.LocalResourceFile);
                }
                else if (start == DateTime.MinValue && Engage.Utility.HasValue(this.txtEndDate.Text))
                {
                    returnVal = false;
                    this.ErrorMessage = Localization.GetString("DateError", this.LocalResourceFile);
                }

                if (this.txtName.Text.Trim().Length < 1)
                {
                    returnVal = false;
                    this.ErrorMessage += Localization.GetString("NameError", this.LocalResourceFile);
                }

                return returnVal;
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), 
                            ModuleActionType.AddContent, string.Empty, string.Empty, string.Empty, false, SecurityAccessLevel.Edit, true, false
                            }
                    };
            }
        }

        protected void ChangeParentPage()
        {
            // TODO: what should we do here?
            // switch(typeof(this.Page))
            // {
            // case ArticleControls.ArticleEdit:
            // ((ArticleControls.ArticleEdit)this.Page).UseUrls = chkUrlSelection.Checked;
            // break;
            // case CategoryControls.CategoryEdit:
            // ((CategoryControls.CategoryEdit)this.Page).UseUrls = chkUrlSelection.Checked;
            // break;
            // }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();

            // LoadControlType();
            base.OnInit(e);

            // System.InvalidOperationException: The EnableScriptGlobalization property cannot be changed during async postbacks or after the Init event.
            if (!this.IsPostBack && AJAX.IsInstalled())
            {
                AJAX.RegisterScriptManager();
                AJAX.SetScriptManagerProperty(
                    this.Page, 
                    "EnableScriptGlobalization", 
                    new object[]
                        {
                            true
                        });
                AJAX.SetScriptManagerProperty(
                    this.Page, 
                    "EnableScriptLocalization", 
                    new object[]
                        {
                            true
                        });
            }

            this.teDescription.Width = this.ItemEditDescriptionWidth;
            this.teDescription.Height = this.ItemEditDescriptionHeight;

            if (Engage.Utility.HasValue(this.VersionInfoObject.Url))
            {
                this.ctlUrlSelection.Url = this.VersionInfoObject.Url;
                this.chkNewWindow.Checked = this.VersionInfoObject.NewWindow;
                this.pnlUrlSelection.Visible = true;
                this.chkUrlSelection.Checked = true;
                this.UseUrls = true;
            }

            // TODO: should we allow NewWindow to work even if the URL option isn't chosen
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void btnChangeDescriptionEditorMode_Click(object sender, EventArgs e)
        {
            if (this.txtDescription.Visible)
            {
                this.teDescription.Visible = true;

                this.txtDescription.Visible = false;
                this.teDescription.Text = this.txtDescription.Text;
            }
            else
            {
                this.teDescription.Visible = false;
                this.txtDescription.Visible = true;
                this.txtDescription.Text = this.teDescription.Text;
            }

            this.btnChangeDescriptionEditorMode.Text = Localization.GetString(
                "btnChangeDescriptionEditorMode_" + this.txtDescription.Visible, this.LocalResourceFile);
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void chkSearchEngine_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlSearchEngine.Visible = this.chkSearchEngine.Checked;

            if (!this.chkSearchEngine.Checked)
            {
                // Remove SEO Optimization when they check to not Optimize for Search Engines.  BD
                this.txtMetaDescription.Text = string.Empty;
                this.txtMetaKeywords.Text = string.Empty;
                this.txtMetaTitle.Text = string.Empty;
            }
        }

        protected void chkUrlSelection_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlUrlSelection.Visible = this.chkUrlSelection.Checked;
            this.ChangeParentPage();
        }

        protected void ddlAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtAuthorName.Text = this.ddlAuthor.SelectedItem.Text;
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        private void LoadAuthorsList()
        {
            var roleController = new RoleController();
            ArrayList authorsList = roleController.GetUserRolesByRoleName(this.PortalId, HostSettings.GetHostSetting(Utility.PublishAuthorRole + this.PortalId));
            ArrayList adminsList = roleController.GetUserRolesByRoleName(this.PortalId, HostSettings.GetHostSetting(Utility.PublishAdminRole + this.PortalId));

            // check to make sure we only add authors who aren't already in the list.
            foreach (UserRoleInfo adminUserRole in adminsList)
            {
                bool located = false;
                foreach (UserRoleInfo authorUserRole in authorsList)
                {
                    if (adminUserRole.UserID == authorUserRole.UserID)
                    {
                        located = true;
                        break;
                    }
                }

                if (!located)
                {
                    authorsList.Add(adminUserRole);
                }
            }

            // sort the author list alphabetically 
            authorsList.Sort(new UserRoleInfoComparer(true));
            this.ddlAuthor.DataTextField = "FullName";
            this.ddlAuthor.DataValueField = "UserId";
            this.ddlAuthor.DataSource = authorsList;
            this.ddlAuthor.DataBind();
        }

        private void LocalizeCollapsePanels()
        {
            this.clpAdvanced.CollapsedText = Localization.GetString("clpAdvanced.CollapsedText", this.LocalResourceFile);
            this.clpAdvanced.ExpandedText = Localization.GetString("clpAdvanced.ExpandedText", this.LocalResourceFile);
            this.clpAdvanced.ExpandedImage = ApplicationUrl +
                                             Localization.GetString("ExpandedImage.Text", this.LocalSharedResourceFile).Replace("[L]", string.Empty);
            this.clpAdvanced.CollapsedImage = ApplicationUrl +
                                              Localization.GetString("CollapsedImage.Text", this.LocalSharedResourceFile).Replace("[L]", string.Empty);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.LocalizeCollapsePanels();

                // check VI for null then set information
                if (!this.Page.IsPostBack)
                {
                    this.LoadAuthorsList();

                    // set author
                    this.ddlAuthor.SelectedValue = this.VersionInfoObject.AuthorUserId > 0
                                                       ? this.VersionInfoObject.AuthorUserId.ToString()
                                                       : this.UserId.ToString();

                    // configure the author name (Text) if defined
                    // chkShowAuthor
                    ItemVersionSetting auNameSetting = ItemVersionSetting.GetItemVersionSetting(
                        this.VersionInfoObject.ItemVersionId, "lblAuthorName", "Text", this.PortalId);
                    this.txtAuthorName.Text = auNameSetting != null ? auNameSetting.PropertyValue : this.ddlAuthor.SelectedItem.Text.Trim();

                    if (this.AllowRichTextDescriptions)
                    {
                        if (this.DefaultRichTextDescriptions)
                        {
                            this.teDescription.ChooseMode = true;
                            this.btnChangeDescriptionEditorMode.Visible = false;
                            this.teDescription.Visible = true;
                            this.txtDescription.Visible = false;
                        }
                        else
                        {
                            // if their profile is set to basic text mode, we need to show the radio buttons so they can get to rich text mode.
                            this.teDescription.ChooseMode = (string)Personalization.GetProfile("DotNetNuke.TextEditor", "PreferredTextEditor") ==
                                                            "BASIC";
                            this.btnChangeDescriptionEditorMode.Text =
                                Localization.GetString("btnChangeDescriptionEditorMode_" + this.txtDescription.Visible, this.LocalResourceFile);
                        }
                    }
                    else
                    {
                        this.btnChangeDescriptionEditorMode.Visible = false;
                    }

                    if (Engage.Utility.HasValue(this.VersionInfoObject.MetaTitle) || Engage.Utility.HasValue(this.VersionInfoObject.MetaDescription) ||
                        Engage.Utility.HasValue(this.VersionInfoObject.MetaKeywords))
                    {
                        this.chkSearchEngine.Checked = true;
                        this.pnlSearchEngine.Visible = true;
                    }

                    this.txtDescription.Text = this.VersionInfoObject.Description;
                    this.teDescription.Text = this.VersionInfoObject.Description;

                    this.txtMetaKeywords.Text = this.VersionInfoObject.MetaKeywords;
                    this.txtMetaDescription.Text = this.VersionInfoObject.MetaDescription;
                    this.txtMetaTitle.Text = this.VersionInfoObject.MetaTitle;

                    if (this.EnableDisplayNameAsHyperlink)
                    {
                        this.chkDisplayAsHyperlink.Checked = !this.VersionInfoObject.Disabled;
                    }
                    else
                    {
                        this.lblDisplayAsHyperlink.Visible = false;
                        this.chkDisplayAsHyperlink.Visible = false;
                        this.chkDisplayAsHyperlink.Checked = true;
                    }

                    if (Engage.Utility.HasValue(this.VersionInfoObject.StartDate))
                    {
                        this.txtStartDate.Text = Utility.GetCurrentCultureDateTime(this.VersionInfoObject.StartDate);
                    }

                    if (Engage.Utility.HasValue(this.VersionInfoObject.EndDate))
                    {
                        this.txtEndDate.Text = Utility.GetCurrentCultureDateTime(this.VersionInfoObject.EndDate);
                    }

                    this.txtName.Text = this.VersionInfoObject.Name;

                    this.thumbnailSelector.ThumbnailUrl = this.VersionInfoObject.Thumbnail;
                }
                else
                {
                    this.VersionInfoObject.Name = this.txtName.Text;
                    this.VersionInfoObject.Description = this.DescriptionText;
                    this.VersionInfoObject.Thumbnail = this.thumbnailSelector.ThumbnailUrl; // ctlMediaFile.Url;

                    // define author's name to display
                    this._authorName = this.txtAuthorName.Text.Trim();

                    this.VersionInfoObject.MetaKeywords = this.txtMetaKeywords.Text;
                    this.VersionInfoObject.MetaDescription = this.txtMetaDescription.Text;
                    this.VersionInfoObject.MetaTitle = this.txtMetaTitle.Text;
                    this.VersionInfoObject.Disabled = !this.chkDisplayAsHyperlink.Checked;

                    this.VersionInfoObject.Url = this.ctlUrlSelection.Url;

                    this.VersionInfoObject.NewWindow = this.chkNewWindow.Checked;
                    DateTime dt;
                    if (Engage.Utility.HasValue(this.txtStartDate.Text) && DateTime.TryParse(this.txtStartDate.Text, out dt))
                    {
                        if (!dt.Equals(DateTime.MinValue))
                        {
                            this.VersionInfoObject.StartDate = dt.ToString(CultureInfo.InvariantCulture);
                        }
                    }

                    if (Engage.Utility.HasValue(this.txtEndDate.Text) && DateTime.TryParse(this.txtEndDate.Text, out dt))
                    {
                        if (!dt.Equals(DateTime.MinValue))
                        {
                            this.VersionInfoObject.EndDate = dt.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        this.VersionInfoObject.EndDate = string.Empty;
                    }

                    this.VersionInfoObject.AuthorUserId = Convert.ToInt32(this.ddlAuthor.SelectedValue);
                    this.VersionInfoObject.RevisingUserId = this.UserId;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}