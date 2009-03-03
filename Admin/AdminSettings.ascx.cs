//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish;
using Engage.Dnn.Publish.Util;
using DotNetNuke.Entities.Tabs;

namespace Engage.Dnn.Publish.Admin
{
    public partial class AdminSettings : ModuleBase
    {
        protected PlaceHolder phSettingsTable;

        #region Event Handlers
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
            this.lnkUpdate.Click += this.lnkUpdate_Click;
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                LocalizeCollapsePanels();

                if (!IsSetup)
                {
                    //if not setup already display a message notifying they must setup the admin side of things before they can procede.
                    lblMessage.Text = Localization.GetString("FirstTime", LocalResourceFile);
                }

                if (IsPostBack == false)
                {
                    LoadSettings();
                }
                if (IsHostMailConfigured == false)
                {
                    lblMailNotConfigured.Visible = true;
                    lblMailNotConfiguredComment.Visible = true;
                }

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                HostSettingsController objHostSettings = new HostSettingsController();

                objHostSettings.UpdateHostSetting(Utility.PublishSetup + PortalId.ToString(CultureInfo.InvariantCulture), "true");
                objHostSettings.UpdateHostSetting(Utility.PublishEmail + PortalId.ToString(CultureInfo.InvariantCulture), chkEmailNotification.Checked.ToString(CultureInfo.InvariantCulture));
               
                objHostSettings.UpdateHostSetting(Utility.PublishEmailNotificationRole + PortalId.ToString(CultureInfo.InvariantCulture), ddlEmailRoles.SelectedValue);
                objHostSettings.UpdateHostSetting(Utility.PublishAdminRole + PortalId.ToString(CultureInfo.InvariantCulture), ddlRoles.SelectedValue);
                objHostSettings.UpdateHostSetting(Utility.PublishAuthorRole + PortalId.ToString(CultureInfo.InvariantCulture), ddlAuthor.SelectedValue);

                objHostSettings.UpdateHostSetting(Utility.PublishAuthorCategoryEdit + PortalId.ToString(CultureInfo.InvariantCulture), chkAuthorCategoryEdit.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishEnableArticlePaging + PortalId.ToString(CultureInfo.InvariantCulture), chkEnablePaging.Checked.ToString(CultureInfo.InvariantCulture));
                
                objHostSettings.UpdateHostSetting(Utility.PublishEnableTags + PortalId.ToString(CultureInfo.InvariantCulture), chkEnableTags.Checked.ToString(CultureInfo.InvariantCulture));
                
                objHostSettings.UpdateHostSetting(Utility.PublishEnableSimpleGalleryIntegration + PortalId.ToString(CultureInfo.InvariantCulture), chkEnableSimpleGallery.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishEnableUltraMediaGalleryIntegration + PortalId.ToString(CultureInfo.InvariantCulture), chkEnableUltraMediaGallery.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishEnableVenexusSearch + PortalId.ToString(CultureInfo.InvariantCulture), chkEnableVenexus.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishEnableViewTracking + PortalId.ToString(CultureInfo.InvariantCulture), chkEnableViewTracking.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishEnableDisplayNameAsHyperlink + PortalId.ToString(CultureInfo.InvariantCulture), chkEnableDisplayNameAsHyperlink.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishAllowRichTextDescriptions + PortalId.ToString(CultureInfo.InvariantCulture), chkAllowRichTextDescriptions.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishDefaultRichTextDescriptions + PortalId.ToString(CultureInfo.InvariantCulture), chkDefaultRichTextDescriptions.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishUseApprovals + PortalId.ToString(CultureInfo.InvariantCulture), chkUseApprovals.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishUseEmbeddedArticles + PortalId.ToString(CultureInfo.InvariantCulture), chkUseEmbeddedArticles.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishShowItemId + PortalId.ToString(CultureInfo.InvariantCulture), chkShowItemId.Checked.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(Utility.PublishCacheTime + PortalId.ToString(CultureInfo.InvariantCulture), txtDefaultCacheTime.Text.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishDefaultAdminPagingSize + PortalId.ToString(CultureInfo.InvariantCulture), txtAdminPagingSize.Text.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(Utility.PublishDescriptionEditHeight + PortalId.ToString(CultureInfo.InvariantCulture), txtItemDescriptionHeight.Text.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishArticleEditHeight + PortalId.ToString(CultureInfo.InvariantCulture), txtArticleTextHeight.Text.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(Utility.PublishDescriptionEditWidth + PortalId.ToString(CultureInfo.InvariantCulture), txtItemDescriptionWidth.Text.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishArticleEditWidth + PortalId.ToString(CultureInfo.InvariantCulture), txtArticleTextWidth.Text.ToString(CultureInfo.InvariantCulture));
                

                objHostSettings.UpdateHostSetting(Utility.PublishEnablePublishFriendlyUrls + PortalId.ToString(CultureInfo.InvariantCulture), chkEnablePublishFriendlyUrls.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishThumbnailSubdirectory + PortalId.ToString(CultureInfo.InvariantCulture), txtThumbnailSubdirectory.Text.EndsWith("/", StringComparison.Ordinal) ? txtThumbnailSubdirectory.Text : txtThumbnailSubdirectory.Text + "/");
                objHostSettings.UpdateHostSetting(Utility.PublishThumbnailDisplayOption + PortalId.ToString(CultureInfo.InvariantCulture), radThumbnailSelection.SelectedValue.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishDefaultDisplayPage + PortalId.ToString(CultureInfo.InvariantCulture), ddlDefaultDisplay.SelectedValue.ToString(CultureInfo.InvariantCulture));

                if(ddlDefaultTextHtmlCategory.SelectedIndex>0)
                objHostSettings.UpdateHostSetting(Utility.PublishDefaultTextHtmlCategory+ PortalId.ToString(CultureInfo.InvariantCulture), ddlDefaultTextHtmlCategory.SelectedValue.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(Utility.PublishDefaultTagPage + PortalId.ToString(CultureInfo.InvariantCulture), ddlTagList.SelectedValue.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(Utility.PublishDefaultReturnToList + PortalId.ToString(CultureInfo.InvariantCulture), chkDefaultReturnToList.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishDefaultRatings + PortalId.ToString(CultureInfo.InvariantCulture), chkDefaultArticleRatings.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishDefaultComments + PortalId.ToString(CultureInfo.InvariantCulture), chkDefaultArticleComments.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishCommentEmailAuthor + PortalId.ToString(CultureInfo.InvariantCulture), chkCommentNotification.Checked.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(Utility.PublishDefaultEmailAFriend + PortalId.ToString(CultureInfo.InvariantCulture), chkDefaultEmailAFriend.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishDefaultPrinterFriendly+ PortalId.ToString(CultureInfo.InvariantCulture), chkDefaultPrinterFriendly.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishSessionReturnToList + PortalId.ToString(CultureInfo.InvariantCulture), chkReturnToListSession.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishDefaultShowAuthor + PortalId.ToString(CultureInfo.InvariantCulture), chkDefaultShowAuthor.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishDefaultShowTags + PortalId.ToString(CultureInfo.InvariantCulture), chkDefaultShowTags.Checked.ToString(CultureInfo.InvariantCulture));

                /*ratings*/
                objHostSettings.UpdateHostSetting(Utility.PublishRating + PortalId.ToString(CultureInfo.InvariantCulture), chkEnableRatings.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishRatingMaximum + PortalId.ToString(CultureInfo.InvariantCulture), txtMaximumRating.Text);
                //objHostSettings.UpdateHostSetting(Utility.PublishRatingAnonymous + PortalId.ToString(CultureInfo.InvariantCulture), chkAnonymousRatings.Checked.ToString(CultureInfo.InvariantCulture));
                /*comments*/
                objHostSettings.UpdateHostSetting(Utility.PublishComment + PortalId.ToString(CultureInfo.InvariantCulture), chkEnableComments.Checked.ToString(CultureInfo.InvariantCulture));


                /*Ping Settings*/
                objHostSettings.UpdateHostSetting(Utility.PublishEnablePing + PortalId.ToString(CultureInfo.InvariantCulture), chkEnablePing.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishPingServers + PortalId.ToString(CultureInfo.InvariantCulture), txtPingServers.Text.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(Utility.PublishPingChangedUrl + PortalId.ToString(CultureInfo.InvariantCulture), txtPingChangedUrl.Text.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(Utility.PublishForumProviderType + PortalId.ToString(CultureInfo.InvariantCulture), ddlCommentsType.SelectedValue);
    
                //objHostSettings.UpdateHostSetting(Utility.PublishCommentApproval + PortalId.ToString(CultureInfo.InvariantCulture), chkCommentApproval.Checked.ToString(CultureInfo.InvariantCulture));
                //objHostSettings.UpdateHostSetting(Utility.PublishCommentAnonymous + PortalId.ToString(CultureInfo.InvariantCulture), chkAnonymousComment.Checked.ToString(CultureInfo.InvariantCulture));
                //objHostSettings.UpdateHostSetting(Utility.PublishCommentAutoApprove + PortalId.ToString(CultureInfo.InvariantCulture), chkCommentAutoApprove.Checked.ToString(CultureInfo.InvariantCulture));

                string returnUrl = Server.UrlDecode(Request.QueryString["returnUrl"]);
                if (!Utility.HasValue(returnUrl))
                {
                    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(TabId, Utility.AdminContainer, "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    Response.Redirect(returnUrl, true);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", Justification = "cv is not an acronym, it is a control prefix"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void cvEmailNotification_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args != null)
            {
                args.IsValid = IsHostMailConfigured || !chkEmailNotification.Checked;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEnableRatings control,
        /// showing the ratings options if they are enabled, hiding them if not.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void chkEnableRatings_CheckedChanged(object source, EventArgs args)
        {
            //rowAnonymousRatings.Visible = chkEnableRatings.Checked;
            rowMaximumRating.Visible = chkEnableRatings.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEnableComments control,
        /// showing the comments options if they are enabled, hiding them if not.
        /// </summary>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        //protected void chkEnableComments_CheckedChanged(object source, EventArgs args)
        //{
        //    rowCommentAnonymous.Visible = chkEnableComments.Checked;
        //    rowCommentApproval.Visible = chkEnableComments.Checked;
        //    rowCommentAutoApprove.Visible = chkEnableComments.Checked;

        //    chkCommentAutoApprove.Enabled = chkCommentApproval.Checked && chkAnonymousComment.Checked;
        //    if (!chkCommentAutoApprove.Enabled)
        //    {
        //        chkCommentAutoApprove.Checked = !chkCommentApproval.Checked;
        //    }
        //}

        /// <summary>
        /// Handles the CheckedChanged event of the chkCommentApproval and chkAnonymousComment controls, 
        /// showing rowAutoApprove if both are checked, and hiding it if either aren't checked.
        /// </summary>
        //protected void ShowCommentAutoApprove(object source, EventArgs args)
        //{
        //    chkCommentAutoApprove.Enabled = chkCommentApproval.Checked && chkAnonymousComment.Checked;
        //    if (!chkCommentAutoApprove.Enabled)
        //    {
        //        chkCommentAutoApprove.Checked = !chkCommentApproval.Checked;
        //    }
        //}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", Justification = "cv is not an acronym, it is a control prefix")]
        protected void cvMaximumRating_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args != null)
            {
                int max;
                args.IsValid = int.TryParse(args.Value.ToString(), out max);
            }
        }

        #endregion

        private void LoadSettings()
        {
            FillListControls();

            Utility.SetSettingListValue(Utility.PublishThumbnailDisplayOption, PortalId, radThumbnailSelection);
            Utility.SetSettingListValue(Utility.PublishAdminRole, PortalId, ddlRoles);
            Utility.SetSettingListValue(Utility.PublishAuthorRole, PortalId, ddlAuthor);
            Utility.SetSettingListValue(Utility.PublishEmailNotificationRole, PortalId, ddlEmailRoles);
            Utility.SetSettingListValue(Utility.PublishDefaultTagPage, PortalId, ddlTagList);
            Utility.SetSettingListValue(Utility.PublishDefaultDisplayPage, PortalId, ddlDefaultDisplay);

            Utility.SetSettingListValue(Utility.PublishDefaultTextHtmlCategory, PortalId, ddlDefaultTextHtmlCategory);
            
            Utility.SetSettingListValue(Utility.PublishForumProviderType, PortalId, ddlCommentsType);

            txtDefaultCacheTime.Text = Utility.GetStringPortalSetting(Utility.PublishCacheTime, PortalId, "5");
            txtAdminPagingSize.Text = Utility.GetStringPortalSetting(Utility.PublishDefaultAdminPagingSize, PortalId, "25");

            txtArticleTextHeight.Text = Utility.GetStringPortalSetting(Utility.PublishArticleEditHeight, PortalId, "500");
            txtArticleTextWidth.Text = Utility.GetStringPortalSetting(Utility.PublishArticleEditWidth, PortalId, "500");
            txtItemDescriptionHeight.Text = Utility.GetStringPortalSetting(Utility.PublishDescriptionEditHeight, PortalId, "300");
            txtItemDescriptionWidth.Text = Utility.GetStringPortalSetting(Utility.PublishDescriptionEditWidth, PortalId, "500");
            txtThumbnailSubdirectory.Text = Utility.GetStringPortalSetting(Utility.PublishThumbnailSubdirectory, PortalId, "PublishThumbnails/");
            txtMaximumRating.Text = Utility.GetStringPortalSetting(Utility.PublishRatingMaximum, PortalId, Rating.DefaultMaximumRating.ToString(CultureInfo.CurrentCulture));
            txtPingServers.Text = Utility.GetStringPortalSetting(Utility.PublishPingServers, PortalId, Localization.GetString("DefaultPingServers", LocalResourceFile));
            txtPingChangedUrl.Text = Utility.GetStringPortalSetting(Utility.PublishPingChangedUrl, PortalId);

            chkEmailNotification.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEmail, PortalId, true);
            chkAuthorCategoryEdit.Checked = Utility.GetBooleanPortalSetting(Utility.PublishAuthorCategoryEdit, PortalId, false);
            chkEnableRatings.Checked = Utility.GetBooleanPortalSetting(Utility.PublishRating, PortalId, false);
            chkEnablePaging.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableArticlePaging, PortalId, true);

            chkEnableTags.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableTags, PortalId, false);
            chkEnableVenexus.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableVenexusSearch, PortalId, false);
            chkEnableViewTracking.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableViewTracking, PortalId, false);
            chkEnableDisplayNameAsHyperlink.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableDisplayNameAsHyperlink, PortalId, false);
            chkAllowRichTextDescriptions.Checked = Utility.GetBooleanPortalSetting(Utility.PublishAllowRichTextDescriptions, PortalId, true);
            DefaultRichTextDescriptions();
            chkDefaultRichTextDescriptions.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultRichTextDescriptions, PortalId, false);
            chkUseApprovals.Checked = Utility.GetBooleanPortalSetting(Utility.PublishUseApprovals, PortalId, true);
            chkUseEmbeddedArticles.Checked = Utility.GetBooleanPortalSetting(Utility.PublishUseEmbeddedArticles, PortalId, false);
            chkShowItemId.Checked = Utility.GetBooleanPortalSetting(Utility.PublishShowItemId, PortalId, true);
            chkEnableComments.Checked = Utility.GetBooleanPortalSetting(Utility.PublishComment, PortalId, false);
            chkReturnToListSession.Checked = Utility.GetBooleanPortalSetting(Utility.PublishSessionReturnToList, PortalId, false);
            chkDefaultShowAuthor.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultShowAuthor, PortalId, true);
            chkDefaultShowTags.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultShowTags, PortalId, false);
            chkEnablePing.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnablePing, PortalId, false);

            chkDefaultEmailAFriend.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultEmailAFriend, PortalId, true);
            chkDefaultPrinterFriendly.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultPrinterFriendly, PortalId, true);
            chkDefaultArticleRatings.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultRatings, PortalId, true);
            chkDefaultArticleComments.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultComments, PortalId, true);
            chkCommentNotification.Checked = Utility.GetBooleanPortalSetting(Utility.PublishCommentEmailAuthor, PortalId, true);

            chkDefaultReturnToList.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultReturnToList, PortalId, false);

            if (Utility.IsSimpleGalleryInstalled)
            {
                chkEnableSimpleGallery.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableSimpleGalleryIntegration, PortalId, false);
            }
            else
            {
                chkEnableSimpleGallery.Enabled = false;
            }

            if (Utility.IsUltraMediaGalleryInstalled)
            {
                chkEnableUltraMediaGallery.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableUltraMediaGalleryIntegration, PortalId, false);
            }
            else
            {
                chkEnableUltraMediaGallery.Enabled = false;
            }

            //default to using Publish URLs, unless FriendlyUrls aren't on, then disable the option and show a message.
            if (HostSettings.GetHostSetting("UseFriendlyUrls") == "Y")
            {
                chkEnablePublishFriendlyUrls.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnablePublishFriendlyUrls, PortalId, true);
            }
            else
            {
                lblFriendlyUrlsNotOn.Visible = true;
                chkEnablePublishFriendlyUrls.Checked = false;
                chkEnablePublishFriendlyUrls.Enabled = false;
            }

            DefaultRichTextDescriptions();

            //s = HostSettings.GetHostSetting(Utility.PublishRatingAnonymous + PortalId.ToString(CultureInfo.InvariantCulture));
            //if (Utility.HasValue(s))
            //{
            //    chkAnonymousRatings.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            //}

            //s = HostSettings.GetHostSetting(Utility.PublishCommentAnonymous + PortalId.ToString(CultureInfo.InvariantCulture));
            //if (Utility.HasValue(s))
            //{
            //    chkAnonymousComment.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            //}

            //s = HostSettings.GetHostSetting(Utility.PublishCommentApproval + PortalId.ToString(CultureInfo.InvariantCulture));
            //if (Utility.HasValue(s))
            //{
            //    chkCommentApproval.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            //}
            //else
            //{
            //    chkCommentApproval.Checked = true;
            //}

            //s = HostSettings.GetHostSetting(Utility.PublishCommentAutoApprove + PortalId.ToString(CultureInfo.InvariantCulture));
            //if (Utility.HasValue(s))
            //{
            //    chkCommentAutoApprove.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            //}

            //rowAnonymousRatings.Visible = chkEnableRatings.Checked;
            rowMaximumRating.Visible = chkEnableRatings.Checked;
            //rowCommentApproval.Visible = chkEnableComments.Checked;
            //rowCommentAnonymous.Visible = chkEnableComments.Checked;
            //rowCommentAutoApprove.Visible = chkEnableComments.Checked;

            //chkCommentAutoApprove.Enabled = chkCommentApproval.Checked && chkAnonymousComment.Checked;
            //if (!chkCommentAutoApprove.Enabled)
            //{
            //    chkCommentAutoApprove.Checked = !chkCommentApproval.Checked;
            //}
        }

        private void LoadThumbnailSelectionRadioButtonList()
        {
            //Load thumbnail options
            radThumbnailSelection.Items.Add(new ListItem(Localization.GetString(ThumbnailDisplayOption.DotNetNuke.ToString(), LocalResourceFile), ThumbnailDisplayOption.DotNetNuke.ToString()));
            radThumbnailSelection.Items.Add(new ListItem(Localization.GetString(ThumbnailDisplayOption.EngagePublish.ToString(), LocalResourceFile), ThumbnailDisplayOption.EngagePublish.ToString()));
            //default the setting to DNN
            radThumbnailSelection.SelectedIndex = 0;
        }

        private void FillListControls()
        {
            //Load the roles for admin/author dropdowns
            ArrayList portalRoles = (new RoleController()).GetPortalRoles(PortalId);
            //load the display dropdown list
            LoadAdminRolesDropDown(portalRoles);
            LoadNotificationRolesDropDown(new RoleController().GetPortalRoles(PortalId));
            LoadAuthorRoleDropDown(new RoleController().GetPortalRoles(PortalId));
            LoadDisplayTabDropDown();
            LoadTagDropDown();
            LoadCommentTypesDropDown();
            LoadThumbnailSelectionRadioButtonList();

            LoadDefaultTextHtmlCategoryDropDown();
        }

        private void LoadAdminRolesDropDown(ArrayList portalRoles)
        {
            ddlRoles.DataSource = portalRoles;
            ddlRoles.DataBind();
        }

        private void LoadNotificationRolesDropDown(ArrayList portalRoles)
        {
            ddlEmailRoles.DataSource = portalRoles;
            ddlEmailRoles.DataBind();
        }

        private void LoadAuthorRoleDropDown(ArrayList portalRoles)
        {
            ddlAuthor.DataSource = portalRoles;
            ddlAuthor.DataBind();
        }

        private void LoadCommentTypesDropDown()
        {
            ddlCommentsType.Items.Clear();
            ddlCommentsType.Items.Add(new ListItem(Localization.GetString("PublishCommentType", LocalResourceFile), string.Empty));

            rowCommentsType.Visible = (new ModuleController()).GetModuleByDefinition(PortalId, Utility.ActiveForumsDefinitionModuleName) != null;
            if (rowCommentsType.Visible)
            {
                ddlCommentsType.Items.Add(new ListItem(Localization.GetString("ActiveForumsCommentType", LocalResourceFile), "Engage.Dnn.Publish.Forum.ActiveForumsProvider"));
            }
        }

        private void LoadLinkFormat()
        {

        }

        private void LoadDisplayTabDropDown()
        {
            string[] modules = new string[] { Utility.DnnFriendlyModuleName };
            DataTable dt = Utility.GetDisplayTabIds(modules);

            this.ddlDefaultDisplay.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));

            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem(dr["TabName"] + " (" + dr["TabID"] + ")", dr["TabID"].ToString());
                this.ddlDefaultDisplay.Items.Add(li);
            }
        }

        private void LoadDefaultTextHtmlCategoryDropDown()
        {
            ItemRelationship.DisplayCategoryHierarchy(ddlDefaultTextHtmlCategory, -1, PortalId, false);
            ListItem li = new ListItem(Localization.GetString("ChooseOne", LocalSharedResourceFile), "-1");
            this.ddlDefaultTextHtmlCategory.Items.Insert(0, li);

        }


        private void LoadTagDropDown()
        {
            ModuleController mc = new ModuleController();
            TabController tc = new TabController();
            ArrayList al = mc.GetModulesByDefinition(PortalId, Utility.DnnTagsFriendlyModuleName);

            this.ddlTagList.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));

            foreach (ModuleInfo mi in al)
            {
                TabInfo ti = tc.GetTab(mi.TabID, mi.PortalID, false);
                ListItem li = new ListItem(ti.TabName + " (" + ti.TabID + ")", ti.TabID.ToString(CultureInfo.InvariantCulture));
                this.ddlTagList.Items.Add(li);
            }
        }

        private void LocalizeCollapsePanels()
        {

            string expandedImage = ApplicationUrl.ToString() + Localization.GetString("ExpandedImage.Text", LocalSharedResourceFile).Replace("[L]", "");
            string collapsedImage = ApplicationUrl.ToString() + Localization.GetString("CollapsedImage.Text", LocalSharedResourceFile).Replace("[L]", "");

            clpTagSettings.CollapsedText = Localization.GetString("clpTagSettings.CollapsedText", LocalResourceFile);
            clpTagSettings.ExpandedText = Localization.GetString("clpTagSettings.ExpandedText", LocalResourceFile);
            clpTagSettings.ExpandedImage = expandedImage;
            clpTagSettings.CollapsedImage=collapsedImage;

            clpCommunity.CollapsedText = Localization.GetString("clpCommunity.CollapsedText", LocalResourceFile);
            clpCommunity.ExpandedText = Localization.GetString("clpCommunity.ExpandedText", LocalResourceFile);
            clpCommunity.ExpandedImage = expandedImage;
            clpCommunity.CollapsedImage=collapsedImage;


            clpArticleEditDefaults.CollapsedText = Localization.GetString("clpArticleEditDefaults.CollapsedText", LocalResourceFile);
            clpArticleEditDefaults.ExpandedText = Localization.GetString("clpArticleEditDefaults.ExpandedText", LocalResourceFile);
            clpArticleEditDefaults.ExpandedImage = expandedImage;
            clpArticleEditDefaults.CollapsedImage = collapsedImage;

            clpAdminEdit.CollapsedText = Localization.GetString("clpAdminEdit.CollapsedText", LocalResourceFile);
            clpAdminEdit.ExpandedText = Localization.GetString("clpAdminEdit.ExpandedText", LocalResourceFile);
            clpAdminEdit.ExpandedImage = expandedImage;
            clpAdminEdit.CollapsedImage = collapsedImage;

            clpAddOns.CollapsedText = Localization.GetString("clpAddOns.CollapsedText", LocalResourceFile);
            clpAddOns.ExpandedText = Localization.GetString("clpAddOns.ExpandedText", LocalResourceFile);
            clpAddOns.ExpandedImage = expandedImage;
            clpAddOns.CollapsedImage = collapsedImage;


        }

        protected void chkAllowRichTextDescriptions_CheckedChanged(object sender, EventArgs e)
        {
            DefaultRichTextDescriptions();
        }

        private void DefaultRichTextDescriptions()
        {
            if (chkAllowRichTextDescriptions.Checked)
            {
                chkDefaultRichTextDescriptions.Visible = true;
                plDefaultRichTextDescriptions.Visible = true;
            }
            else
            {
                chkDefaultRichTextDescriptions.Visible = false;
                plDefaultRichTextDescriptions.Visible = false;
            }
        }

    }
}

