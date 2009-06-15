//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics;
using System.Globalization;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Personalization;
using Engage.Dnn.Publish.Util;
using System.Web.UI.WebControls;
using System.Collections;
using DotNetNuke.Entities.Users;

namespace Engage.Dnn.Publish.Controls
{
	public partial class ItemEdit :  ModuleBase, IActionable
	{
		#region Event Handlers
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
            //LoadControlType();
			base.OnInit(e);

            //System.InvalidOperationException: The EnableScriptGlobalization property cannot be changed during async postbacks or after the Init event.
            if (!IsPostBack && DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
                DotNetNuke.Framework.AJAX.SetScriptManagerProperty(Page, "EnableScriptGlobalization", new object[] { true });
                DotNetNuke.Framework.AJAX.SetScriptManagerProperty(Page, "EnableScriptLocalization", new object[] { true });
            }

            teDescription.Width = ItemEditDescriptionWidth;
            teDescription.Height = ItemEditDescriptionHeight;

            if (Utility.HasValue(VersionInfoObject.Url))
            {
                this.ctlUrlSelection.Url = VersionInfoObject.Url;
                chkNewWindow.Checked    = VersionInfoObject.NewWindow;
                pnlUrlSelection.Visible = true;
                chkUrlSelection.Checked = true;
                UseUrls = true;
            }

            //TODO: should we allow NewWindow to work even if the URL option isn't chosen
		}

        private void InitializeComponent()
		{
			this.Load += this.Page_Load;
		}

        private void LocalizeCollapsePanels()
        {
            clpAdvanced.CollapsedText = Localization.GetString("clpAdvanced.CollapsedText", LocalResourceFile);
            clpAdvanced.ExpandedText = Localization.GetString("clpAdvanced.ExpandedText", LocalResourceFile);
            clpAdvanced.ExpandedImage = ApplicationUrl.ToString() + Localization.GetString("ExpandedImage.Text", LocalSharedResourceFile).Replace("[L]", "");
            clpAdvanced.CollapsedImage = ApplicationUrl.ToString() + Localization.GetString("CollapsedImage.Text", LocalSharedResourceFile).Replace("[L]", "");
        }
 
		private void Page_Load(object sender, EventArgs e)
		{
 			try 
			{

                LocalizeCollapsePanels();

				//check VI for null then set information
				if (!Page.IsPostBack)
				{

                    LoadAuthorsList();
                    //set author
                    if (VersionInfoObject.AuthorUserId > 0)
                    {
                        ddlAuthor.SelectedValue = VersionInfoObject.AuthorUserId.ToString();
                    }
                    else
                    {
                        ddlAuthor.SelectedValue = UserId.ToString();
                    }

                    //configure the author name (Text) if defined
                    //chkShowAuthor
                    ItemVersionSetting auNameSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "lblAuthorName", "Text", PortalId);
                    if (auNameSetting != null)
                    {
                        txtAuthorName.Text = auNameSetting.PropertyValue.ToString();
                    }
                    else
                    {
                        txtAuthorName.Text = ddlAuthor.SelectedItem.Text.Trim().ToString();
                    }
                    

                    if (AllowRichTextDescriptions)
                    {

                        if (DefaultRichTextDescriptions)
                        {
                            teDescription.ChooseMode = true;
                            btnChangeDescriptionEditorMode.Visible = false;
                            teDescription.Visible = true;
                            txtDescription.Visible = false;

                        }
                        else
                        {
                            //if their profile is set to basic text mode, we need to show the radio buttons so they can get to rich text mode.
                            teDescription.ChooseMode = (string)Personalization.GetProfile("DotNetNuke.TextEditor", "PreferredTextEditor") == "BASIC";
                            btnChangeDescriptionEditorMode.Text = Localization.GetString("btnChangeDescriptionEditorMode_" + txtDescription.Visible, LocalResourceFile);
                        }
                    }
                    else
                    {
                        btnChangeDescriptionEditorMode.Visible = false;
                    }

                    if (Utility.HasValue(VersionInfoObject.MetaTitle) || Utility.HasValue(VersionInfoObject.MetaDescription) || Utility.HasValue(VersionInfoObject.MetaKeywords))
                    {
                        chkSearchEngine.Checked = true;
                        pnlSearchEngine.Visible = true;
                    }

                    txtDescription.Text = VersionInfoObject.Description;
                    teDescription.Text = VersionInfoObject.Description;

					txtMetaKeywords.Text = VersionInfoObject.MetaKeywords;
					txtMetaDescription.Text = VersionInfoObject.MetaDescription;
					txtMetaTitle.Text = VersionInfoObject.MetaTitle;

                    if (EnableDisplayNameAsHyperlink)
                    {
                        chkDisplayAsHyperlink.Checked = !VersionInfoObject.Disabled;
                    }
                    else
                    {
                        lblDisplayAsHyperlink.Visible = false;
                        chkDisplayAsHyperlink.Visible = false;
                        chkDisplayAsHyperlink.Checked = true;
                    }
                    if (Utility.HasValue(VersionInfoObject.StartDate))
                    {
                        txtStartDate.Text = Utility.GetCurrentCultureDateTime(VersionInfoObject.StartDate);
                    }
                    if (Utility.HasValue(VersionInfoObject.EndDate))
                    {
                        txtEndDate.Text = Utility.GetCurrentCultureDateTime(VersionInfoObject.EndDate);
                    }
					txtName.Text = VersionInfoObject.Name.ToString();

                    ////TODO: check why this isn't working.
                    //if (Utility.HasValue(VersionInfoObject.Url))
                    //{
                    //    this.ctlUrlSelection.Url = VersionInfoObject.Url;
                    //    chkUrlSelection.Checked = true;
                    //    pnlUrlSelection.Visible = true;
                    //    UseUrls = true;
                    //}
                    
                    thumbnailSelector.ThumbnailUrl = VersionInfoObject.Thumbnail;
				}
				else
                {

                    VersionInfoObject.Name = txtName.Text;
                    VersionInfoObject.Description = DescriptionText;
                    VersionInfoObject.Thumbnail = thumbnailSelector.ThumbnailUrl;//ctlMediaFile.Url;

                    //define author's name to display
                    authorName = txtAuthorName.Text.Trim();

                    VersionInfoObject.MetaKeywords = txtMetaKeywords.Text;
                    VersionInfoObject.MetaDescription = txtMetaDescription.Text;
                    VersionInfoObject.MetaTitle = txtMetaTitle.Text;
                    VersionInfoObject.Disabled = !chkDisplayAsHyperlink.Checked;

                    VersionInfoObject.Url = ctlUrlSelection.Url;


                    VersionInfoObject.NewWindow = chkNewWindow.Checked;
                    DateTime dt;
                    if (Utility.HasValue(txtStartDate.Text) && DateTime.TryParse(txtStartDate.Text, out dt))
                    {
                        if (!dt.Equals(DateTime.MinValue))
                            VersionInfoObject.StartDate = dt.ToString(CultureInfo.InvariantCulture);
                    }

                    if (Utility.HasValue(txtEndDate.Text) && DateTime.TryParse(txtEndDate.Text, out dt))
                    {
                        if (!dt.Equals(DateTime.MinValue))
                            VersionInfoObject.EndDate = dt.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        VersionInfoObject.EndDate = "";
                    }


                    VersionInfoObject.AuthorUserId = Convert.ToInt32(ddlAuthor.SelectedValue);
                    VersionInfoObject.RevisingUserId = UserId;
                }
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnChangeDescriptionEditorMode_Click(object sender, EventArgs e)
        {
            if (txtDescription.Visible)
            {
                teDescription.Visible = true;
                
                txtDescription.Visible = false;
                teDescription.Text = txtDescription.Text;
            }
            else
            {
                teDescription.Visible = false;
                txtDescription.Visible = true;
                txtDescription.Text = teDescription.Text;
            }
            btnChangeDescriptionEditorMode.Text = Localization.GetString("btnChangeDescriptionEditorMode_" + txtDescription.Visible, LocalResourceFile);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void chkSearchEngine_CheckedChanged(object sender, EventArgs e)
        {
            pnlSearchEngine.Visible = chkSearchEngine.Checked;

            if (!chkSearchEngine.Checked)
            {
                //Remove SEO Optimization when they check to not Optimize for Search Engines.  BD
                txtMetaDescription.Text = string.Empty;
                txtMetaKeywords.Text = string.Empty;
                txtMetaTitle.Text = string.Empty;
            }
        }
        protected void chkUrlSelection_CheckedChanged(object sender, EventArgs e)
        {
            pnlUrlSelection.Visible = chkUrlSelection.Checked;
            changeParentPage();
        }

        protected void changeParentPage()
        {
            //TODO: what should we do here?
            //switch(typeof(this.Page))
            //{
            //    case ArticleControls.ArticleEdit:
            //        ((ArticleControls.ArticleEdit)this.Page).UseUrls = chkUrlSelection.Checked;
            //        break;
            //    case CategoryControls.CategoryEdit:
            //        ((CategoryControls.CategoryEdit)this.Page).UseUrls = chkUrlSelection.Checked;
            //        break;
            //}
        }
        
		#endregion

        private void LoadAuthorsList()
        {
            //load authors role
            //load admins role
            DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
            ArrayList al = rc.GetUserRolesByRoleName(PortalId, DotNetNuke.Entities.Host.HostSettings.GetHostSetting(Utility.PublishAuthorRole + PortalId));
            ArrayList alAdmin = rc.GetUserRolesByRoleName(PortalId, DotNetNuke.Entities.Host.HostSettings.GetHostSetting(Utility.PublishAdminRole + PortalId));

            //check to make sure we only add authors who aren't already in the list.
            foreach (UserRoleInfo uri in alAdmin)
            {
                bool located = false;
                foreach (UserRoleInfo ur in al)
                {
                    if (uri.UserRoleID == ur.UserRoleID)
                        located = true;
                        break;
                }
                if (!located)
                {
                    al.Add(uri);
                }
            }

            //sort the author list alphabetically 
            al.Sort(new UserRoleInfoComparer(true));
            ddlAuthor.DataTextField = "FullName";
            ddlAuthor.DataValueField = "UserId";
            ddlAuthor.DataSource = al;
            ddlAuthor.DataBind();
        }

        #region Public Properties
        public bool IsValid
        {
            get
            {
                bool returnVal = true;
                
                DateTime start;
                DateTime end;
                if (DateTime.TryParse(txtStartDate.Text, out start) && DateTime.TryParse(txtEndDate.Text, out end))
                {
                    returnVal = end > start;
                    ErrorMessage = Localization.GetString("DateError", LocalResourceFile);
                }
                else if (start == DateTime.MinValue && Utility.HasValue(this.txtEndDate.Text))
                {
                    returnVal = false;
                    ErrorMessage = Localization.GetString("DateError", LocalResourceFile);
                }

                if (txtName.Text.Trim().Length < 1)
                {
                    returnVal = false;
                    ErrorMessage += Localization.GetString("NameError", LocalResourceFile);
                }                

                return returnVal;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            [DebuggerStepThrough]
            get
            {
                return errorMessage;
            }
            [DebuggerStepThrough]
            set
            {
                errorMessage = value;
            }
        }

        public string DescriptionText
        {
            get
            {
                return txtDescription.Visible ? txtDescription.Text : teDescription.Text;
            }
        }


        private string authorName = string.Empty;
        public string AuthorName
        {
            [DebuggerStepThrough]
            get
            {
                return authorName;
            }
            [DebuggerStepThrough]
            set
            {
                authorName = value;
            }
        }



        #endregion

        #region Optional Interfaces
        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
		{
			get 
			{
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
				return actions;
			}
		}
		#endregion

        protected void ddlAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAuthorName.Text = ddlAuthor.SelectedItem.Text.ToString();
        }
	}
}

