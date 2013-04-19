// <copyright file="AdminSettings.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Admin
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Entities.Controllers;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Framework;
    using DotNetNuke.Security.Roles;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;
    using Engage.Dnn.UserFeedback;

    public partial class AdminSettings : ModuleBase
    {
        protected PlaceHolder SettingsTablePlaceHolder;

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        protected void chkAllowRichTextDescriptions_CheckedChanged(object sender, EventArgs e)
        {
            this.DefaultRichTextDescriptions();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEnableRatings control,
        /// showing the ratings options if they are enabled, hiding them if not.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void chkEnableRatings_CheckedChanged(object source, EventArgs args)
        {
            // rowAnonymousRatings.Visible = chkEnableRatings.Checked;
            this.MaximumRatingPanel.Visible = this.chkEnableRatings.Checked;
        }

        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", 
            Justification = "cv is not an acronym, it is a control prefix")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void cvEmailNotification_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args != null)
            {
                args.IsValid = IsHostMailConfigured || !this.chkEmailNotification.Checked;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkEnableComments control,
        /// showing the comments options if they are enabled, hiding them if not.
        /// </summary>
        // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        // protected void chkEnableComments_CheckedChanged(object source, EventArgs args)
        // {
        // rowCommentAnonymous.Visible = chkEnableComments.Checked;
        // rowCommentApproval.Visible = chkEnableComments.Checked;
        // rowCommentAutoApprove.Visible = chkEnableComments.Checked;
        // chkCommentAutoApprove.Enabled = chkCommentApproval.Checked && chkAnonymousComment.Checked;
        // if (!chkCommentAutoApprove.Enabled)
        // {
        // chkCommentAutoApprove.Checked = !chkCommentApproval.Checked;
        // }
        // }
        /// <summary>
        /// Handles the CheckedChanged event of the chkCommentApproval and chkAnonymousComment controls, 
        /// showing rowAutoApprove if both are checked, and hiding it if either aren't checked.
        /// </summary>
        // protected void ShowCommentAutoApprove(object source, EventArgs args)
        // {
        // chkCommentAutoApprove.Enabled = chkCommentApproval.Checked && chkAnonymousComment.Checked;
        // if (!chkCommentAutoApprove.Enabled)
        // {
        // chkCommentAutoApprove.Checked = !chkCommentApproval.Checked;
        // }
        // }
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", 
            Justification = "cv is not an acronym, it is a control prefix")]
        protected void cvMaximumRating_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args != null)
            {
                int max;
                args.IsValid = int.TryParse(args.Value, out max);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            this.UpdateHostSetting(Utility.PublishSetup, true);
            this.UpdateHostSetting(Utility.PublishEmail, this.chkEmailNotification.Checked);

            this.UpdateHostSetting(Utility.PublishEmailNotificationRole, this.ddlEmailRoles.SelectedValue);
            this.UpdateHostSetting(Utility.PublishAdminRole, this.ddlRoles.SelectedValue);
            this.UpdateHostSetting(Utility.PublishAuthorRole, this.ddlAuthor.SelectedValue);

            this.UpdateHostSetting(Utility.PublishAuthorCategoryEdit, this.chkAuthorCategoryEdit.Checked);
            this.UpdateHostSetting(Utility.PublishEnableArticlePaging, this.chkEnablePaging.Checked);

            this.UpdateHostSetting(Utility.PublishEnableTags, this.chkEnableTags.Checked);

            this.UpdateHostSetting(Utility.PublishEnableSimpleGalleryIntegration, this.chkEnableSimpleGallery.Checked);
            this.UpdateHostSetting(Utility.PublishEnableUltraMediaGalleryIntegration, this.chkEnableUltraMediaGallery.Checked);
            this.UpdateHostSetting(Utility.PublishEnableVenexusSearch, this.chkEnableVenexus.Checked);
            this.UpdateHostSetting(Utility.PublishEnableViewTracking, this.chkEnableViewTracking.Checked);
            this.UpdateHostSetting(Utility.PublishEnableDisplayNameAsHyperlink, this.chkEnableDisplayNameAsHyperlink.Checked);
            this.UpdateHostSetting(Utility.PublishAllowRichTextDescriptions, this.chkAllowRichTextDescriptions.Checked);
            this.UpdateHostSetting(Utility.PublishDefaultRichTextDescriptions, this.chkDefaultRichTextDescriptions.Checked);
            this.UpdateHostSetting(Utility.PublishUseApprovals, this.chkUseApprovals.Checked);
            this.UpdateHostSetting(Utility.PublishUseEmbeddedArticles, this.chkUseEmbeddedArticles.Checked);
            this.UpdateHostSetting(Utility.PublishShowItemId, this.chkShowItemId.Checked);
            this.UpdateHostSetting(Utility.PublishCacheTime, this.txtDefaultCacheTime.Text);
            this.UpdateHostSetting(Utility.PublishDefaultAdminPagingSize, this.txtAdminPagingSize.Text);

            this.UpdateHostSetting(Utility.PublishDescriptionEditHeight, this.txtItemDescriptionHeight.Text);
            this.UpdateHostSetting(Utility.PublishArticleEditHeight, this.txtArticleTextHeight.Text);

            this.UpdateHostSetting(Utility.PublishDescriptionEditWidth, this.txtItemDescriptionWidth.Text);
            this.UpdateHostSetting(Utility.PublishArticleEditWidth, this.txtArticleTextWidth.Text);

            this.UpdateHostSetting(Utility.PublishEnablePublishFriendlyUrls, this.chkEnablePublishFriendlyUrls.Checked);
            this.UpdateHostSetting(Utility.PublishThumbnailSubdirectory, this.txtThumbnailSubdirectory.Text.EndsWith("/", StringComparison.Ordinal) ? this.txtThumbnailSubdirectory.Text : this.txtThumbnailSubdirectory.Text + "/");
            this.UpdateHostSetting(Utility.PublishThumbnailDisplayOption, this.radThumbnailSelection.SelectedValue);
            this.UpdateHostSetting(Utility.PublishDefaultDisplayPage, this.ddlDefaultDisplay.SelectedValue);
            
            if (this.ddlDefaultTextHtmlCategory.SelectedIndex > 0)
            {
                this.UpdateHostSetting(Utility.PublishDefaultTextHtmlCategory, this.ddlDefaultTextHtmlCategory.SelectedValue);
            }

            if (this.ddlDefaultCategory.SelectedIndex > 0)
            {
                this.UpdateHostSetting(Utility.PublishDefaultCategory, this.ddlDefaultCategory.SelectedValue);
            }

            this.UpdateHostSetting(Utility.PublishDefaultTagPage, this.ddlTagList.SelectedValue);

            this.UpdateHostSetting(Utility.PublishDefaultReturnToList, this.chkDefaultReturnToList.Checked);
            this.UpdateHostSetting(Utility.PublishDefaultRatings, this.chkDefaultArticleRatings.Checked);
            this.UpdateHostSetting(Utility.PublishDefaultComments, this.chkDefaultArticleComments.Checked);
            this.UpdateHostSetting(Utility.PublishCommentEmailAuthor, this.chkCommentNotification.Checked);
            
            this.UpdateHostSetting(Utility.PublishDefaultEmailAFriend, this.chkDefaultEmailAFriend.Checked);
            this.UpdateHostSetting(Utility.PublishDefaultPrinterFriendly, this.chkDefaultPrinterFriendly.Checked);
            this.UpdateHostSetting(Utility.PublishSessionReturnToList, this.chkReturnToListSession.Checked);
            this.UpdateHostSetting(Utility.PublishDefaultShowAuthor, this.chkDefaultShowAuthor.Checked);
            this.UpdateHostSetting(Utility.PublishDefaultShowTags, this.chkDefaultShowTags.Checked);

            /*ratings*/
            this.UpdateHostSetting(Utility.PublishRating, this.chkEnableRatings.Checked);
            this.UpdateHostSetting(Utility.PublishRatingMaximum, this.txtMaximumRating.Text);

            /*comments*/
            this.UpdateHostSetting(Utility.PublishComment, this.chkEnableComments.Checked);

            /*Ping Settings*/
            this.UpdateHostSetting(Utility.PublishEnablePing, this.chkEnablePing.Checked);
            this.UpdateHostSetting(Utility.PublishPingServers, this.txtPingServers.Text);
            this.UpdateHostSetting(Utility.PublishPingChangedUrl, this.txtPingChangedUrl.Text);

            this.UpdateHostSetting(Utility.PublishEnableInvisibleCaptcha, this.chkEnableInvisibleCaptcha.Checked);
            this.UpdateHostSetting(Utility.PublishEnableTimedCaptcha, this.chkEnableTimedCaptcha.Checked);
            this.UpdateHostSetting(Utility.PublishEnableStandardCaptcha, this.chkEnableStandardCaptcha.Checked);

            this.UpdateHostSetting(Utility.PublishForumProviderType, this.ddlCommentsType.SelectedValue);

            var returnUrl = HttpUtility.UrlDecode(this.Request.QueryString["returnUrl"]);
            if (!Engage.Utility.HasValue(returnUrl))
            {
                returnUrl = Globals.NavigateURL(this.TabId, Utility.AdminContainer, "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture));
            }

            this.Response.Redirect(returnUrl);
        }

        /// <summary>Updates (or creates) the setting with the given name in the Host Settings store (but for the current portal).</summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="settingValue">The setting value.</param>
        private void UpdateHostSetting(string settingName, bool settingValue)
        {
            this.UpdateHostSetting(settingName, settingValue.ToString());
        }

        /// <summary>Updates (or creates) the setting with the given name in the Host Settings store (but for the current portal).</summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="settingValue">The setting value.</param>
        private void UpdateHostSetting(string settingName, string settingValue)
        {
            HostController.Instance.Update(
                settingName + this.PortalId.ToString(CultureInfo.InvariantCulture),
                settingValue);
        }

        private new void DefaultRichTextDescriptions()
        {
            this.DefaultRichTextDescriptionsPanel.Visible = this.chkAllowRichTextDescriptions.Checked;
        }

        private void FillListControls()
        {
            // Load the roles for admin/author dropdowns
            ArrayList portalRoles = (new RoleController()).GetPortalRoles(this.PortalId);

            // load the display dropdown list
            this.LoadAdminRolesDropDown(portalRoles);
            this.LoadNotificationRolesDropDown(new RoleController().GetPortalRoles(this.PortalId));
            this.LoadAuthorRoleDropDown(new RoleController().GetPortalRoles(this.PortalId));
            this.LoadDisplayTabDropDown();
            this.LoadTagDropDown();
            this.LoadCommentTypesDropDown();
            this.LoadThumbnailSelectionRadioButtonList();

            this.LoadDefaultTextHtmlCategoryDropDown();
            this.LoadDefaultCategoryDropDown();
        }

        private void LoadAdminRolesDropDown(ArrayList portalRoles)
        {
            this.ddlRoles.DataSource = portalRoles;
            this.ddlRoles.DataBind();
        }

        private void LoadAuthorRoleDropDown(ArrayList portalRoles)
        {
            this.ddlAuthor.DataSource = portalRoles;
            this.ddlAuthor.DataBind();
        }

        private void LoadCommentTypesDropDown()
        {
            this.ddlCommentsType.Items.Clear();
            this.ddlCommentsType.Items.Add(new ListItem(Localization.GetString("PublishCommentType", this.LocalResourceFile), string.Empty));

            this.CommentsTypePanel.Visible = (new ModuleController()).GetModuleByDefinition(this.PortalId, Utility.ActiveForumsDefinitionModuleName) !=
                                           null;
            if (this.CommentsTypePanel.Visible)
            {
                this.ddlCommentsType.Items.Add(
                    new ListItem(
                        Localization.GetString("ActiveForumsCommentType", this.LocalResourceFile), "Engage.Dnn.Publish.Forum.ActiveForumsProvider"));
            }
        }

        private void LoadDefaultCategoryDropDown()
        {
            ItemRelationship.DisplayCategoryHierarchy(this.ddlDefaultCategory, -1, this.PortalId, false);
            var li = new ListItem(Localization.GetString("ChooseOne", this.LocalSharedResourceFile), "-1");
            this.ddlDefaultCategory.Items.Insert(0, li);
        }

        private void LoadDefaultTextHtmlCategoryDropDown()
        {
            ItemRelationship.DisplayCategoryHierarchy(this.ddlDefaultTextHtmlCategory, -1, this.PortalId, false);
            var li = new ListItem(Localization.GetString("ChooseOne", this.LocalSharedResourceFile), "-1");
            this.ddlDefaultTextHtmlCategory.Items.Insert(0, li);
        }

        private void LoadDisplayTabDropDown()
        {
            var modules = new[]
                {
                    Utility.DnnFriendlyModuleName
                };
            DataTable dt = Utility.GetDisplayTabIds(modules);

            this.ddlDefaultDisplay.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), "-1"));

            foreach (DataRow dr in dt.Rows)
            {
                var li = new ListItem(dr["TabName"] + " (" + dr["TabID"] + ")", dr["TabID"].ToString());
                this.ddlDefaultDisplay.Items.Add(li);
            }
        }

        private void LoadNotificationRolesDropDown(ArrayList portalRoles)
        {
            this.ddlEmailRoles.DataSource = portalRoles;
            this.ddlEmailRoles.DataBind();
        }

        private void LoadSettings()
        {
            this.FillListControls();

            Utility.SetSettingListValue(Utility.PublishThumbnailDisplayOption, this.PortalId, this.radThumbnailSelection);
            Utility.SetSettingListValue(Utility.PublishAdminRole, this.PortalId, this.ddlRoles);
            Utility.SetSettingListValue(Utility.PublishAuthorRole, this.PortalId, this.ddlAuthor);
            Utility.SetSettingListValue(Utility.PublishEmailNotificationRole, this.PortalId, this.ddlEmailRoles);
            Utility.SetSettingListValue(Utility.PublishDefaultTagPage, this.PortalId, this.ddlTagList);
            Utility.SetSettingListValue(Utility.PublishDefaultDisplayPage, this.PortalId, this.ddlDefaultDisplay);

            Utility.SetSettingListValue(Utility.PublishDefaultTextHtmlCategory, this.PortalId, this.ddlDefaultTextHtmlCategory);
            Utility.SetSettingListValue(Utility.PublishDefaultCategory, this.PortalId, this.ddlDefaultCategory);

            Utility.SetSettingListValue(Utility.PublishForumProviderType, this.PortalId, this.ddlCommentsType);

            this.txtDefaultCacheTime.Text = Utility.GetStringPortalSetting(Utility.PublishCacheTime, this.PortalId, "5");
            this.txtAdminPagingSize.Text = Utility.GetStringPortalSetting(Utility.PublishDefaultAdminPagingSize, this.PortalId, "25");

            this.txtArticleTextHeight.Text = Utility.GetStringPortalSetting(Utility.PublishArticleEditHeight, this.PortalId, "500");
            this.txtArticleTextWidth.Text = Utility.GetStringPortalSetting(Utility.PublishArticleEditWidth, this.PortalId, "500");
            this.txtItemDescriptionHeight.Text = Utility.GetStringPortalSetting(Utility.PublishDescriptionEditHeight, this.PortalId, "300");
            this.txtItemDescriptionWidth.Text = Utility.GetStringPortalSetting(Utility.PublishDescriptionEditWidth, this.PortalId, "500");
            this.txtThumbnailSubdirectory.Text = Utility.GetStringPortalSetting(
                Utility.PublishThumbnailSubdirectory, this.PortalId, "PublishThumbnails/");
            this.txtMaximumRating.Text = Utility.GetStringPortalSetting(
                Utility.PublishRatingMaximum, this.PortalId, Rating.DefaultMaximumRating.ToString(CultureInfo.CurrentCulture));
            this.txtPingServers.Text = Utility.GetStringPortalSetting(
                Utility.PublishPingServers, this.PortalId, Localization.GetString("DefaultPingServers", this.LocalResourceFile));
            this.txtPingChangedUrl.Text = Utility.GetStringPortalSetting(Utility.PublishPingChangedUrl, this.PortalId);

            this.chkEmailNotification.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEmail, this.PortalId, true);
            this.chkAuthorCategoryEdit.Checked = Utility.GetBooleanPortalSetting(Utility.PublishAuthorCategoryEdit, this.PortalId, false);
            this.chkEnableRatings.Checked = Utility.GetBooleanPortalSetting(Utility.PublishRating, this.PortalId, false);
            this.chkEnablePaging.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableArticlePaging, this.PortalId, true);

            this.chkEnableTags.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableTags, this.PortalId, false);
            this.chkEnableVenexus.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableVenexusSearch, this.PortalId, false);
            this.chkEnableViewTracking.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableViewTracking, this.PortalId, false);
            this.chkEnableDisplayNameAsHyperlink.Checked = Utility.GetBooleanPortalSetting(
                Utility.PublishEnableDisplayNameAsHyperlink, this.PortalId, false);
            this.chkAllowRichTextDescriptions.Checked = Utility.GetBooleanPortalSetting(Utility.PublishAllowRichTextDescriptions, this.PortalId, true);
            this.DefaultRichTextDescriptions();
            this.chkDefaultRichTextDescriptions.Checked = Utility.GetBooleanPortalSetting(
                Utility.PublishDefaultRichTextDescriptions, this.PortalId, false);
            this.chkUseApprovals.Checked = Utility.GetBooleanPortalSetting(Utility.PublishUseApprovals, this.PortalId, true);
            this.chkUseEmbeddedArticles.Checked = Utility.GetBooleanPortalSetting(Utility.PublishUseEmbeddedArticles, this.PortalId, false);
            this.chkShowItemId.Checked = Utility.GetBooleanPortalSetting(Utility.PublishShowItemId, this.PortalId, true);
            this.chkEnableComments.Checked = Utility.GetBooleanPortalSetting(Utility.PublishComment, this.PortalId, false);
            this.chkReturnToListSession.Checked = Utility.GetBooleanPortalSetting(Utility.PublishSessionReturnToList, this.PortalId, false);
            this.chkDefaultShowAuthor.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultShowAuthor, this.PortalId, true);
            this.chkDefaultShowTags.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultShowTags, this.PortalId, false);
            this.chkEnablePing.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnablePing, this.PortalId, false);
            this.chkEnableInvisibleCaptcha.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableInvisibleCaptcha, this.PortalId, true);
            this.chkEnableTimedCaptcha.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableTimedCaptcha, this.PortalId, true);
            this.chkEnableStandardCaptcha.Checked = Utility.GetBooleanPortalSetting(Utility.PublishEnableStandardCaptcha, this.PortalId, false);

            this.chkDefaultEmailAFriend.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultEmailAFriend, this.PortalId, true);
            this.chkDefaultPrinterFriendly.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultPrinterFriendly, this.PortalId, true);
            this.chkDefaultArticleRatings.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultRatings, this.PortalId, true);
            this.chkDefaultArticleComments.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultComments, this.PortalId, true);
            this.chkCommentNotification.Checked = Utility.GetBooleanPortalSetting(Utility.PublishCommentEmailAuthor, this.PortalId, true);

            this.chkDefaultReturnToList.Checked = Utility.GetBooleanPortalSetting(Utility.PublishDefaultReturnToList, this.PortalId, false);

            if (Utility.IsSimpleGalleryInstalled)
            {
                this.chkEnableSimpleGallery.Checked = Utility.GetBooleanPortalSetting(
                    Utility.PublishEnableSimpleGalleryIntegration, this.PortalId, false);
            }
            else
            {
                this.chkEnableSimpleGallery.Enabled = false;
            }

            if (Utility.IsUltraMediaGalleryInstalled)
            {
                this.chkEnableUltraMediaGallery.Checked = Utility.GetBooleanPortalSetting(
                    Utility.PublishEnableUltraMediaGalleryIntegration, this.PortalId, false);
            }
            else
            {
                this.chkEnableUltraMediaGallery.Enabled = false;
            }

            // default to using Publish URLs, unless FriendlyUrls aren't on, then disable the option and show a message.
            if (Host.UseFriendlyUrls)
            {
                this.chkEnablePublishFriendlyUrls.Checked = Utility.GetBooleanPortalSetting(
                    Utility.PublishEnablePublishFriendlyUrls, this.PortalId, true);
            }
            else
            {
                this.FriendlyUrlsNotOnPanel.Visible = true;
                this.chkEnablePublishFriendlyUrls.Checked = false;
                this.chkEnablePublishFriendlyUrls.Enabled = false;
            }

            this.DefaultRichTextDescriptions();

            // s = HostSettings.GetHostSetting(Utility.PublishRatingAnonymous + PortalId.ToString(CultureInfo.InvariantCulture));
            // if (Utility.HasValue(s))
            // {
            // chkAnonymousRatings.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            // }

            // s = HostSettings.GetHostSetting(Utility.PublishCommentAnonymous + PortalId.ToString(CultureInfo.InvariantCulture));
            // if (Utility.HasValue(s))
            // {
            // chkAnonymousComment.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            // }

            // s = HostSettings.GetHostSetting(Utility.PublishCommentApproval + PortalId.ToString(CultureInfo.InvariantCulture));
            // if (Utility.HasValue(s))
            // {
            // chkCommentApproval.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            // }
            // else
            // {
            // chkCommentApproval.Checked = true;
            // }

            // s = HostSettings.GetHostSetting(Utility.PublishCommentAutoApprove + PortalId.ToString(CultureInfo.InvariantCulture));
            // if (Utility.HasValue(s))
            // {
            // chkCommentAutoApprove.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            // }

            // rowAnonymousRatings.Visible = chkEnableRatings.Checked;
            this.MaximumRatingPanel.Visible = this.chkEnableRatings.Checked;

            // rowCommentApproval.Visible = chkEnableComments.Checked;
            // rowCommentAnonymous.Visible = chkEnableComments.Checked;
            // rowCommentAutoApprove.Visible = chkEnableComments.Checked;

            // chkCommentAutoApprove.Enabled = chkCommentApproval.Checked && chkAnonymousComment.Checked;
            // if (!chkCommentAutoApprove.Enabled)
            // {
            // chkCommentAutoApprove.Checked = !chkCommentApproval.Checked;
            // }
        }

        private void LoadTagDropDown()
        {
            var mc = new ModuleController();
            var tc = new TabController();
            ArrayList al = mc.GetModulesByDefinition(this.PortalId, Utility.DnnTagsFriendlyModuleName);

            this.ddlTagList.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), "-1"));

            foreach (ModuleInfo mi in al)
            {
                TabInfo ti = tc.GetTab(mi.TabID, mi.PortalID, false);
                var li = new ListItem(ti.TabName + " (" + ti.TabID + ")", ti.TabID.ToString(CultureInfo.InvariantCulture));
                this.ddlTagList.Items.Add(li);
            }
        }

        private void LoadThumbnailSelectionRadioButtonList()
        {
            // Load thumbnail options
            this.radThumbnailSelection.Items.Add(
                new ListItem(
                    Localization.GetString(ThumbnailDisplayOption.DotNetNuke.ToString(), this.LocalResourceFile), 
                    ThumbnailDisplayOption.DotNetNuke.ToString()));
            this.radThumbnailSelection.Items.Add(
                new ListItem(
                    Localization.GetString(ThumbnailDisplayOption.EngagePublish.ToString(), this.LocalResourceFile), 
                    ThumbnailDisplayOption.EngagePublish.ToString()));

            // default the setting to DNN
            this.radThumbnailSelection.SelectedIndex = 0;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.FirstTimeMessagePanel.Visible = !this.IsSetup;

                jQuery.RequestDnnPluginsRegistration();
                if (!this.IsPostBack)
                {
                    this.LoadSettings();
                }

                if (IsHostMailConfigured)
                {
                    return;
                }

                this.MailNotConfiguredPanel.Visible = true;
                this.MailNotConfiguredCommentPanel.Visible = true;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}