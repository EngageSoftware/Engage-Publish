// <copyright file="ArticleDisplayOptions.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// A control for setting the settings of the Article Display type
    /// </summary>
    public partial class ArticleDisplayOptions : OverrideableDisplayOptionsBase
    {
        private bool AllowPhotoGalleryDisplay
        {
            get
            {
                object o = this.Settings["adShowPhotoGallery"];
                return o == null ? false : Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adShowPhotoGallery", value.ToString(CultureInfo.InvariantCulture)); }
        }

        private int? ArticleId
        {
            get
            {
                object o = this.Settings["adArticleId"];
                return o == null ? (int?)null : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adArticleId", value.Value.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display a textbox to collect the email address of the commenter.
        /// </summary>
        /// <value>
        /// <c>true</c> if the email address should be collected; otherwise, <c>false</c>.
        /// </value>
        private bool CollectEmailAddress
        {
            get
            {
                object o = this.Settings["adCollectEmailAddress"];
                if (o != null)
                {
                    bool collectEmailAddress;
                    if (bool.TryParse(o.ToString(), out collectEmailAddress))
                    {
                        return collectEmailAddress;
                    }
                }

                return false;
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adCollectEmailAddress", value.ToString()); }
        }

        private bool CollectUrl
        {
            get
            {
                object o = this.Settings["adCollectUrl"];

                if (o != null)
                {
                    bool value;
                    if (bool.TryParse(o.ToString(), out value))
                    {
                        return value;
                    }
                }

                return true;
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adCollectUrl", value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the comment display option, whether to show all or only some of the comments at a time.
        /// </summary>
        /// <value>
        /// The comment display option.
        /// Defaults to <see cref="CommentDisplayOption"/> if no setting is defined.
        /// </value>
        private CommentDisplayOption CommentDisplayOption
        {
            get
            {
                object o = this.Settings["adCommentDisplayOption"];
                if (o != null && Enum.IsDefined(typeof(CommentDisplayOption), o))
                {
                    return (CommentDisplayOption)Enum.Parse(typeof(CommentDisplayOption), o.ToString());
                }

                return CommentDisplayOption.ShowAll;
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adCommentDisplayOption", value.ToString()); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display the option to create a comment.
        /// </summary>
        /// <value>
        /// <c>true</c> if the option to create a comment is displayed, otherwise <c>false</c>.
        /// Defaults to <c>true</c> if no setting is defined.
        /// </value>
        private bool DisplayCommentSubmission
        {
            get
            {
                object o = this.Settings["adCommentsLink"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adCommentsLink", value.ToString(CultureInfo.InvariantCulture)); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display comments made on this item.
        /// </summary>
        /// <value>
        /// <c>true</c> if comments should be displayed; otherwise, <c>false</c>.
        /// Defaults to <c>true</c> if no setting is defined.
        /// </value>
        private bool DisplayComments
        {
            get
            {
                object o = this.Settings["adCommentsDisplay"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adCommentsDisplay", value.ToString(CultureInfo.InvariantCulture)); }
        }

        /// <summary>
        /// Sets a value indicating whether to display one comment at a time, in a random order.
        /// </summary>
        /// <value>
        /// <c>true</c> if comments should be displayed one-at-a-time in random order; otherwise, <c>false</c>.
        /// </value>
        private bool DisplayRandomComment
        {
            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adRandomComment", value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the first name collection option, whether to ask for the full name, only the initial, or not to ask for the first name at all.
        /// </summary>
        /// <value>
        /// The first name collection option.
        /// Defaults to <see cref="NameDisplayOption.Full"/> if no setting is defined.
        /// </value>
        private NameDisplayOption FirstNameCollectOption
        {
            get
            {
                object o = this.Settings["adFirstNameCollectOption"];
                if (o != null && Enum.IsDefined(typeof(NameDisplayOption), o))
                {
                    return (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), o.ToString());
                }

                return NameDisplayOption.Full;
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adFirstNameCollectOption", value.ToString()); }
        }

        private int? GalleryThumbnailHeight
        {
            get
            {
                object o = this.Settings["adGalleryThumbnailHeight"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(
                        this.TabModuleId, "adGalleryThumbnailHeight", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adGalleryThumbnailHeight", string.Empty);
                }
            }
        }

        private int? GalleryThumbnailWidth
        {
            get
            {
                object o = this.Settings["adGalleryThumbnailWidth"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(
                        this.TabModuleId, "adGalleryThumbnailWidth", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adGalleryThumbnailWidth", string.Empty);
                }
            }
        }

        private int? HoverThumbnailHeight
        {
            get
            {
                object o = this.Settings["adHoverThumbnailHeight"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(
                        this.TabModuleId, "adHoverThumbnailHeight", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adHoverThumbnailHeight", string.Empty);
                }
            }
        }

        private int? HoverThumbnailWidth
        {
            get
            {
                object o = this.Settings["adHoverThumbnailWidth"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(
                        this.TabModuleId, "adHoverThumbnailWidth", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adHoverThumbnailWidth", string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the last name collection option, whether to ask for the full name, only the initial, or not to ask for the last name at all.
        /// </summary>
        /// <value>
        /// The last name collection option.
        /// Defaults to <see cref="NameDisplayOption.Initial"/> if no setting is defined.
        /// </value>
        private NameDisplayOption LastNameCollectOption
        {
            get
            {
                object o = this.Settings["adLastNameCollectOption"];
                if (o != null && Enum.IsDefined(typeof(NameDisplayOption), o))
                {
                    return (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), o.ToString());
                }

                return NameDisplayOption.Full;
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adLastNameCollectOption", value.ToString()); }
        }

        private string LastUpdatedFormat
        {
            get
            {
                object o = this.Settings["adLastUpdatedFormat"];
                return o == null ? "F" : o.ToString();
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adLastUpdatedFormat", value.ToString(CultureInfo.InvariantCulture)); }
        }

        private int? MaximumNumberOfThumbnails
        {
            get
            {
                object o = this.Settings["adNumberOfThumbnails"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(
                        this.TabModuleId, "adNumberOfThumbnails", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adNumberOfThumbnails", string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the rating display option, whether to enable or disable rating submission, 
        /// or make ratings read only.
        /// </summary>
        /// <value>
        /// The rating display option.
        /// Defaults to <see cref="Util.RatingDisplayOption.Enable"/> if no setting is defined.
        /// </value>
        private RatingDisplayOption RatingDisplayOption
        {
            get
            {
                object o = this.Settings["adEnableRatings"];
                if (o != null && Enum.IsDefined(typeof(RatingDisplayOption), o))
                {
                    return (RatingDisplayOption)Enum.Parse(typeof(RatingDisplayOption), o.ToString());
                }

                return RatingDisplayOption.Enable;
            }

            set { new ModuleController().UpdateTabModuleSetting(this.TabModuleId, "adEnableRatings", value.ToString()); }
        }

        /// <summary>
        /// Loads the settings for this module instance.
        /// </summary>
        public override void LoadSettings()
        {
            this.FillDropDowns();

            if (!this.IsPostBack || this.ForceSetInitialValues)
            {
                this.SetInitialValues();
            }

            this.SetPhotoGalleryOptionsVisibility(
                ModuleBase.AllowSimpleGalleryIntegrationForPortal(this.PortalId) ||
                ModuleBase.AllowUltraMediaGalleryIntegrationForPortal(this.PortalId));
            this.SetCommentOptionsVisibility(ModuleBase.IsCommentsEnabledForPortal(this.PortalId));
            this.SetRatingsOptionsVisibility(ModuleBase.AreRatingsEnabledForPortal(this.PortalId));
        }

        /// <summary>
        /// Updates the settings for this module instance.
        /// </summary>
        public override void UpdateSettings()
        {
            if (this.Page.IsValid)
            {
                this.ArticleId = this.ArticleSelectorControl.ArticleId ?? Null.NullInteger;

                this.LastUpdatedFormat = this.txtLastUpdatedFormat.Text.Trim();
                this.RatingDisplayOption = (RatingDisplayOption)Enum.Parse(typeof(RatingDisplayOption), this.ddlDisplayRatings.SelectedValue);
                this.DisplayCommentSubmission = this.chkCommentSubmit.Checked;
                this.DisplayComments = this.chkCommentDisplay.Checked;
                this.CommentDisplayOption = (CommentDisplayOption)Enum.Parse(typeof(CommentDisplayOption), this.ddlDisplayComments.SelectedValue);
                this.DisplayRandomComment = this.CommentDisplayOption.Equals(CommentDisplayOption.Paging); ////chkCommentRandom.Checked;
                this.FirstNameCollectOption = (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), this.ddlFirstNameCollect.SelectedValue);
                this.LastNameCollectOption = (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), this.ddlLastNameCollect.SelectedValue);
                this.CollectEmailAddress = this.chkEmailAddressCollect.Checked;
                this.CollectUrl = this.chkUrlCollect.Checked;
                this.AllowPhotoGalleryDisplay = this.chkDisplayPhotoGallery.Checked;

                int parsedValue;
                this.MaximumNumberOfThumbnails = int.TryParse(
                    this.txtPhotoGalleryMaxCount.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                                     ? parsedValue
                                                     : (int?)null;
                this.HoverThumbnailHeight = int.TryParse(
                    this.txtPhotoGalleryHoverThumbnailHeight.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                                ? parsedValue
                                                : (int?)null;
                this.HoverThumbnailWidth = int.TryParse(
                    this.txtPhotoGalleryHoverThumbnailWidth.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                               ? parsedValue
                                               : (int?)null;
                this.GalleryThumbnailHeight = int.TryParse(
                    this.txtPhotoGalleryThumbnailHeight.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                                  ? parsedValue
                                                  : (int?)null;
                this.GalleryThumbnailWidth = int.TryParse(
                    this.txtPhotoGalleryThumbnailWidth.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                                 ? parsedValue
                                                 : (int?)null;
            }
        }

        /// <summary>
        /// Fills the drop downs on this control with their possible values.
        /// </summary>
        private void FillDropDowns()
        {
            this.ddlDisplayRatings.Items.Clear();
            this.ddlDisplayRatings.Items.Add(
                new ListItem(
                    Localization.GetString(RatingDisplayOption.Enable.ToString(), this.LocalResourceFile), RatingDisplayOption.Enable.ToString()));
            this.ddlDisplayRatings.Items.Add(
                new ListItem(
                    Localization.GetString(RatingDisplayOption.ReadOnly.ToString(), this.LocalResourceFile), RatingDisplayOption.ReadOnly.ToString()));
            this.ddlDisplayRatings.Items.Add(
                new ListItem(
                    Localization.GetString(RatingDisplayOption.Disable.ToString(), this.LocalResourceFile), RatingDisplayOption.Disable.ToString()));

            this.ddlDisplayComments.Items.Clear();
            this.ddlDisplayComments.Items.Add(
                new ListItem(
                    Localization.GetString(CommentDisplayOption.ShowAll.ToString(), this.LocalResourceFile), CommentDisplayOption.ShowAll.ToString()));
            this.ddlDisplayComments.Items.Add(
                new ListItem(
                    Localization.GetString(CommentDisplayOption.Paging.ToString(), this.LocalResourceFile), CommentDisplayOption.Paging.ToString()));

            this.ddlFirstNameCollect.Items.Clear();
            this.ddlFirstNameCollect.Items.Add(
                new ListItem(Localization.GetString(NameDisplayOption.Full.ToString(), this.LocalResourceFile), NameDisplayOption.Full.ToString()));
            this.ddlFirstNameCollect.Items.Add(
                new ListItem(
                    Localization.GetString(NameDisplayOption.Initial.ToString(), this.LocalResourceFile), NameDisplayOption.Initial.ToString()));
            this.ddlFirstNameCollect.Items.Add(
                new ListItem(Localization.GetString(NameDisplayOption.None.ToString(), this.LocalResourceFile), NameDisplayOption.None.ToString()));

            this.ddlLastNameCollect.Items.Clear();
            this.ddlLastNameCollect.Items.Add(
                new ListItem(Localization.GetString(NameDisplayOption.Full.ToString(), this.LocalResourceFile), NameDisplayOption.Full.ToString()));
            this.ddlLastNameCollect.Items.Add(
                new ListItem(
                    Localization.GetString(NameDisplayOption.Initial.ToString(), this.LocalResourceFile), NameDisplayOption.Initial.ToString()));
            this.ddlLastNameCollect.Items.Add(
                new ListItem(Localization.GetString(NameDisplayOption.None.ToString(), this.LocalResourceFile), NameDisplayOption.None.ToString()));
        }

        /// <summary>
        /// Sets the visibility of the comment options, showing a message if the settings are not available.
        /// </summary>
        /// <param name="commentsEnabled">if set to <c>true</c> the comments settings are enabled.</param>
        private void SetCommentOptionsVisibility(bool commentsEnabled)
        {
            this.pnlCommentSettings.Visible = commentsEnabled;
            this.lblEnableComments.Visible = !commentsEnabled;
        }

        /// <summary>
        /// Sets the values on this form based on the settings stored for this module instance
        /// </summary>
        private void SetInitialValues()
        {
            ListItem li = this.ddlDisplayRatings.Items.FindByValue(this.RatingDisplayOption.ToString());
            if (li != null)
            {
                li.Selected = true;
            }

            li = this.ddlDisplayComments.Items.FindByValue(this.CommentDisplayOption.ToString());
            if (li != null)
            {
                li.Selected = true;
            }

            if (this.ddlFirstNameCollect != null)
            {
                li = this.ddlFirstNameCollect.Items.FindByValue(this.FirstNameCollectOption.ToString());
            }

            if (li != null)
            {
                li.Selected = true;
            }

            li = this.ddlLastNameCollect.Items.FindByValue(this.LastNameCollectOption.ToString());
            if (li != null)
            {
                li.Selected = true;
            }

            this.ArticleSelectorControl.ArticleId = this.ArticleId;

            if (this.MaximumNumberOfThumbnails.HasValue && this.MaximumNumberOfThumbnails.Value > 0)
            {
                this.txtPhotoGalleryMaxCount.Text = this.MaximumNumberOfThumbnails.Value.ToString(CultureInfo.CurrentCulture);
            }

            if (this.HoverThumbnailHeight.HasValue && this.HoverThumbnailHeight.Value > 0)
            {
                this.txtPhotoGalleryHoverThumbnailHeight.Text = this.HoverThumbnailHeight.Value.ToString(CultureInfo.CurrentCulture);
            }

            if (this.HoverThumbnailWidth.HasValue && this.HoverThumbnailWidth.Value > 0)
            {
                this.txtPhotoGalleryHoverThumbnailWidth.Text = this.HoverThumbnailWidth.Value.ToString(CultureInfo.CurrentCulture);
            }

            if (this.GalleryThumbnailHeight.HasValue && this.GalleryThumbnailHeight.Value > 0)
            {
                this.txtPhotoGalleryThumbnailHeight.Text = this.GalleryThumbnailHeight.Value.ToString(CultureInfo.CurrentCulture);
            }

            if (this.GalleryThumbnailWidth.HasValue && this.GalleryThumbnailWidth.Value > 0)
            {
                this.txtPhotoGalleryThumbnailWidth.Text = this.GalleryThumbnailWidth.Value.ToString(CultureInfo.CurrentCulture);
            }

            this.chkDisplayPhotoGallery.Checked = this.AllowPhotoGalleryDisplay;

            this.chkCommentDisplay.Checked = this.DisplayComments;
            this.chkCommentSubmit.Checked = this.DisplayCommentSubmission;
            this.chkEmailAddressCollect.Checked = this.CollectEmailAddress;
            this.chkUrlCollect.Checked = this.CollectUrl;

            if (this.txtLastUpdatedFormat != null)
            {
                this.txtLastUpdatedFormat.Text = this.LastUpdatedFormat;
            }
        }

        /// <summary>
        /// Sets the visibility of the photo gallery options, showing a message if the settings are not available.
        /// </summary>
        /// <param name="photoGallerySettingEnabled">if set to <c>true</c> the photo gallery settings are enabled.</param>
        private void SetPhotoGalleryOptionsVisibility(bool photoGallerySettingEnabled)
        {
            this.lblEnablePhotoGallery.Visible = !photoGallerySettingEnabled;
            this.pnlPhotoGallerySettings.Visible = photoGallerySettingEnabled;
        }

        /// <summary>
        /// Sets the visibility of the ratings options, showing a message if the settings are not available.
        /// </summary>
        /// <param name="ratingsEnabled">if set to <c>true</c> the rating settings are enabled.</param>
        private void SetRatingsOptionsVisibility(bool ratingsEnabled)
        {
            this.pnlRatingsSettings.Visible = ratingsEnabled;
            this.lblEnableRatings.Visible = !ratingsEnabled;
        }
    }
}