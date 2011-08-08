//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Admin
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Tabs;
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
            this.lnkUpdate.Click += this.lnkUpdate_Click;
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
            this.rowMaximumRating.Visible = this.chkEnableRatings.Checked;
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
            if (this.Page.IsValid)
            {
                var objHostSettings = new HostSettingsController();

                objHostSettings.UpdateHostSetting(Utility.PublishSetup + this.PortalId.ToString(CultureInfo.InvariantCulture), "true");
                objHostSettings.UpdateHostSetting(
                    Utility.PublishEmail + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEmailNotification.Checked.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishEmailNotificationRole + this.PortalId.ToString(CultureInfo.InvariantCulture), this.ddlEmailRoles.SelectedValue);
                objHostSettings.UpdateHostSetting(
                    Utility.PublishAdminRole + this.PortalId.ToString(CultureInfo.InvariantCulture), this.ddlRoles.SelectedValue);
                objHostSettings.UpdateHostSetting(
                    Utility.PublishAuthorRole + this.PortalId.ToString(CultureInfo.InvariantCulture), this.ddlAuthor.SelectedValue);

                objHostSettings.UpdateHostSetting(
                    Utility.PublishAuthorCategoryEdit + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkAuthorCategoryEdit.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishEnableArticlePaging + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnablePaging.Checked.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishEnableTags + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnableTags.Checked.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishEnableSimpleGalleryIntegration + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnableSimpleGallery.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishEnableUltraMediaGalleryIntegration + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnableUltraMediaGallery.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishEnableVenexusSearch + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnableVenexus.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishEnableViewTracking + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnableViewTracking.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishEnableDisplayNameAsHyperlink + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnableDisplayNameAsHyperlink.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishAllowRichTextDescriptions + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkAllowRichTextDescriptions.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultRichTextDescriptions + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkDefaultRichTextDescriptions.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishUseApprovals + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkUseApprovals.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishUseEmbeddedArticles + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkUseEmbeddedArticles.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishShowItemId + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkShowItemId.Checked.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishCacheTime + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtDefaultCacheTime.Text.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultAdminPagingSize + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtAdminPagingSize.Text.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishDescriptionEditHeight + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtItemDescriptionHeight.Text.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishArticleEditHeight + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtArticleTextHeight.Text.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishDescriptionEditWidth + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtItemDescriptionWidth.Text.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishArticleEditWidth + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtArticleTextWidth.Text.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishEnablePublishFriendlyUrls + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnablePublishFriendlyUrls.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishThumbnailSubdirectory + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtThumbnailSubdirectory.Text.EndsWith("/", StringComparison.Ordinal)
                        ? this.txtThumbnailSubdirectory.Text
                        : this.txtThumbnailSubdirectory.Text + "/");
                objHostSettings.UpdateHostSetting(
                    Utility.PublishThumbnailDisplayOption + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.radThumbnailSelection.SelectedValue.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultDisplayPage + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.ddlDefaultDisplay.SelectedValue.ToString(CultureInfo.InvariantCulture));

                if (this.ddlDefaultTextHtmlCategory.SelectedIndex > 0)
                {
                    objHostSettings.UpdateHostSetting(
                        Utility.PublishDefaultTextHtmlCategory + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                        this.ddlDefaultTextHtmlCategory.SelectedValue.ToString(CultureInfo.InvariantCulture));
                }

                if (this.ddlDefaultCategory.SelectedIndex > 0)
                {
                    objHostSettings.UpdateHostSetting(
                        Utility.PublishDefaultCategory + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                        this.ddlDefaultCategory.SelectedValue.ToString(CultureInfo.InvariantCulture));
                }

                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultTagPage + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.ddlTagList.SelectedValue.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultReturnToList + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkDefaultReturnToList.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultRatings + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkDefaultArticleRatings.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultComments + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkDefaultArticleComments.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishCommentEmailAuthor + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkCommentNotification.Checked.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultEmailAFriend + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkDefaultEmailAFriend.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultPrinterFriendly + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkDefaultPrinterFriendly.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishSessionReturnToList + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkReturnToListSession.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultShowAuthor + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkDefaultShowAuthor.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishDefaultShowTags + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkDefaultShowTags.Checked.ToString(CultureInfo.InvariantCulture));

                /*ratings*/
                objHostSettings.UpdateHostSetting(
                    Utility.PublishRating + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnableRatings.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishRatingMaximum + this.PortalId.ToString(CultureInfo.InvariantCulture), this.txtMaximumRating.Text);

                // objHostSettings.UpdateHostSetting(Utility.PublishRatingAnonymous + PortalId.ToString(CultureInfo.InvariantCulture), chkAnonymousRatings.Checked.ToString(CultureInfo.InvariantCulture));
                /*comments*/
                objHostSettings.UpdateHostSetting(
                    Utility.PublishComment + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnableComments.Checked.ToString(CultureInfo.InvariantCulture));

                /*Ping Settings*/
                objHostSettings.UpdateHostSetting(
                    Utility.PublishEnablePing + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkEnablePing.Checked.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishPingServers + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtPingServers.Text.ToString(CultureInfo.InvariantCulture));
                objHostSettings.UpdateHostSetting(
                    Utility.PublishPingChangedUrl + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtPingChangedUrl.Text.ToString(CultureInfo.InvariantCulture));

                objHostSettings.UpdateHostSetting(
                    Utility.PublishForumProviderType + this.PortalId.ToString(CultureInfo.InvariantCulture), this.ddlCommentsType.SelectedValue);

                // objHostSettings.UpdateHostSetting(Utility.PublishCommentApproval + PortalId.ToString(CultureInfo.InvariantCulture), chkCommentApproval.Checked.ToString(CultureInfo.InvariantCulture));
                // objHostSettings.UpdateHostSetting(Utility.PublishCommentAnonymous + PortalId.ToString(CultureInfo.InvariantCulture), chkAnonymousComment.Checked.ToString(CultureInfo.InvariantCulture));
                // objHostSettings.UpdateHostSetting(Utility.PublishCommentAutoApprove + PortalId.ToString(CultureInfo.InvariantCulture), chkCommentAutoApprove.Checked.ToString(CultureInfo.InvariantCulture));

                // TODO: Remove after DNN 5.5.x fixes DNN-13633 (not clearing all the cache after updates)
                DataCache.ClearHostCache(true);

                string returnUrl = this.Server.UrlDecode(this.Request.QueryString["returnUrl"]);
                if (!Engage.Utility.HasValue(returnUrl))
                {
                    this.Response.Redirect(
                        Globals.NavigateURL(this.TabId, Utility.AdminContainer, "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    this.Response.Redirect(returnUrl, true);
                }
            }
        }

        private new void DefaultRichTextDescriptions()
        {
            if (this.chkAllowRichTextDescriptions.Checked)
            {
                this.chkDefaultRichTextDescriptions.Visible = true;
                this.plDefaultRichTextDescriptions.Visible = true;
            }
            else
            {
                this.chkDefaultRichTextDescriptions.Visible = false;
                this.plDefaultRichTextDescriptions.Visible = false;
            }
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

            this.rowCommentsType.Visible = (new ModuleController()).GetModuleByDefinition(this.PortalId, Utility.ActiveForumsDefinitionModuleName) !=
                                           null;
            if (this.rowCommentsType.Visible)
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
            if (HostSettings.GetHostSetting("UseFriendlyUrls") == "Y")
            {
                this.chkEnablePublishFriendlyUrls.Checked = Utility.GetBooleanPortalSetting(
                    Utility.PublishEnablePublishFriendlyUrls, this.PortalId, true);
            }
            else
            {
                this.lblFriendlyUrlsNotOn.Visible = true;
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
            this.rowMaximumRating.Visible = this.chkEnableRatings.Checked;

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

        private void LocalizeCollapsePanels()
        {
            string expandedImage = ApplicationUrl + Localization.GetString("ExpandedImage.Text", this.LocalSharedResourceFile).Replace("[L]", string.Empty);
            string collapsedImage = ApplicationUrl + Localization.GetString("CollapsedImage.Text", this.LocalSharedResourceFile).Replace("[L]", string.Empty);

            this.clpTagSettings.CollapsedText = Localization.GetString("clpTagSettings.CollapsedText", this.LocalResourceFile);
            this.clpTagSettings.ExpandedText = Localization.GetString("clpTagSettings.ExpandedText", this.LocalResourceFile);
            this.clpTagSettings.ExpandedImage = expandedImage;
            this.clpTagSettings.CollapsedImage = collapsedImage;

            this.clpCommunity.CollapsedText = Localization.GetString("clpCommunity.CollapsedText", this.LocalResourceFile);
            this.clpCommunity.ExpandedText = Localization.GetString("clpCommunity.ExpandedText", this.LocalResourceFile);
            this.clpCommunity.ExpandedImage = expandedImage;
            this.clpCommunity.CollapsedImage = collapsedImage;

            this.clpArticleEditDefaults.CollapsedText = Localization.GetString("clpArticleEditDefaults.CollapsedText", this.LocalResourceFile);
            this.clpArticleEditDefaults.ExpandedText = Localization.GetString("clpArticleEditDefaults.ExpandedText", this.LocalResourceFile);
            this.clpArticleEditDefaults.ExpandedImage = expandedImage;
            this.clpArticleEditDefaults.CollapsedImage = collapsedImage;

            this.clpAdminEdit.CollapsedText = Localization.GetString("clpAdminEdit.CollapsedText", this.LocalResourceFile);
            this.clpAdminEdit.ExpandedText = Localization.GetString("clpAdminEdit.ExpandedText", this.LocalResourceFile);
            this.clpAdminEdit.ExpandedImage = expandedImage;
            this.clpAdminEdit.CollapsedImage = collapsedImage;

            this.clpAddOns.CollapsedText = Localization.GetString("clpAddOns.CollapsedText", this.LocalResourceFile);
            this.clpAddOns.ExpandedText = Localization.GetString("clpAddOns.ExpandedText", this.LocalResourceFile);
            this.clpAddOns.ExpandedImage = expandedImage;
            this.clpAddOns.CollapsedImage = collapsedImage;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.LocalizeCollapsePanels();

                if (!this.IsSetup)
                {
                    // if not setup already display a message notifying they must setup the admin side of things before they can procede.
                    this.lblMessage.Text = Localization.GetString("FirstTime", this.LocalResourceFile);
                }

                if (this.IsPostBack == false)
                {
                    this.LoadSettings();
                }

                if (IsHostMailConfigured == false)
                {
                    this.lblMailNotConfigured.Visible = true;
                    this.lblMailNotConfiguredComment.Visible = true;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}