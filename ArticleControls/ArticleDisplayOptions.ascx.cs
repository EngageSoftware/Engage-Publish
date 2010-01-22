// <copyright file="ArticleDisplayOptions.ascx.cs" company="Engage Software">
// Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
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
    using Util;

    
    /// <summary>
    /// A control for setting the settings of the Article Display type
    /// </summary>
    public partial class ArticleDisplayOptions : ModuleSettingsBase
    {

        private int? ArticleId
        {
            set
            {
                if(value.HasValue)
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adArticleId", value.Value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["adArticleId"];
                return o == null ? (int?)null : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }
        }

        private string LastUpdatedFormat
        {
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adLastUpdatedFormat", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["adLastUpdatedFormat"];
                return o == null ? "F" : o.ToString();
            }
        }

        private bool AllowPhotoGalleryDisplay
        {
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adShowPhotoGallery", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["adShowPhotoGallery"];
                return o == null ? false : Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
            }
        }

        private int? MaximumNumberOfThumbnails
        {
            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(TabModuleId, "adNumberOfThumbnails", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(TabModuleId, "adNumberOfThumbnails", string.Empty);
                }
            }

            get
            {
                object o = Settings["adNumberOfThumbnails"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }
        }

        private int? HoverThumbnailHeight
        {
            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(
                        TabModuleId, "adHoverThumbnailHeight", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(TabModuleId, "adHoverThumbnailHeight", string.Empty);
                }
            }

            get
            {
                object o = Settings["adHoverThumbnailHeight"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }
        }

        private int? HoverThumbnailWidth
        {
            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(TabModuleId, "adHoverThumbnailWidth", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(TabModuleId, "adHoverThumbnailWidth", string.Empty);
                }
            }

            get
            {
                object o = Settings["adHoverThumbnailWidth"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }
        }

        private int? GalleryThumbnailHeight
        {
            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(
                        TabModuleId, "adGalleryThumbnailHeight", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(TabModuleId, "adGalleryThumbnailHeight", string.Empty);
                }
            }

            get
            {
                object o = Settings["adGalleryThumbnailHeight"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
            }
        }

        private int? GalleryThumbnailWidth
        {
            set
            {
                if (value.HasValue)
                {
                    new ModuleController().UpdateTabModuleSetting(
                        TabModuleId, "adGalleryThumbnailWidth", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    new ModuleController().UpdateTabModuleSetting(TabModuleId, "adGalleryThumbnailWidth", string.Empty);
                }
            }

            get
            {
                object o = Settings["adGalleryThumbnailWidth"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }

                return null;
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
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adEnableRatings", value.ToString());
            }

            get
            {
                object o = Settings["adEnableRatings"];
                if (o != null && Enum.IsDefined(typeof(RatingDisplayOption), o))
                {
                    return (RatingDisplayOption)Enum.Parse(typeof(RatingDisplayOption), o.ToString());
                }

                return RatingDisplayOption.Enable;
            }
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
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adCommentsLink", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["adCommentsLink"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }
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
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adCommentsDisplay", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["adCommentsDisplay"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }
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
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adCommentDisplayOption", value.ToString());
            }

            get
            {
                object o = Settings["adCommentDisplayOption"];
                if (o != null && Enum.IsDefined(typeof(CommentDisplayOption), o))
                {
                    return (CommentDisplayOption)Enum.Parse(typeof(CommentDisplayOption), o.ToString());
                }

                return CommentDisplayOption.ShowAll;
            }
        }

        /// <summary>
        /// Sets a value indicating whether to display one comment at a time, in a random order.
        /// </summary>
        /// <value>
        /// <c>true</c> if comments should be displayed one-at-a-time in random order; otherwise, <c>false</c>.
        /// </value>
        private bool DisplayRandomComment
        {
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adRandomComment", value.ToString());
            }
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
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adFirstNameCollectOption", value.ToString());
            }

            get
            {
                object o = Settings["adFirstNameCollectOption"];
                if (o != null && Enum.IsDefined(typeof(NameDisplayOption), o))
                {
                    return (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), o.ToString());
                }

                return NameDisplayOption.Full;
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
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adLastNameCollectOption", value.ToString());
            }

            get
            {
                object o = Settings["adLastNameCollectOption"];
                if (o != null && Enum.IsDefined(typeof(NameDisplayOption), o))
                {
                    return (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), o.ToString());
                }

                return NameDisplayOption.Full;
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
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adCollectEmailAddress", value.ToString());
            }

            get
            {
                object o = Settings["adCollectEmailAddress"];
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
        }

        private bool CollectUrl
        {
            set
            {
                new ModuleController().UpdateTabModuleSetting(TabModuleId, "adCollectUrl", value.ToString());
            }

            get
            {
                object o = Settings["adCollectUrl"];

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
        }

        /// <summary>
        /// Loads the settings for this module instance.
        /// </summary>
        public override void LoadSettings()
        {
            FillDropDowns();

            if (!IsPostBack)
            {
                SetInitialValues();
            }

            SetPhotoGalleryOptionsVisibility(ModuleBase.AllowSimpleGalleryIntegrationForPortal(PortalId) || ModuleBase.AllowUltraMediaGalleryIntegrationForPortal(PortalId));
            SetCommentOptionsVisibility(ModuleBase.IsCommentsEnabledForPortal(PortalId));
            SetRatingsOptionsVisibility(ModuleBase.AreRatingsEnabledForPortal(PortalId));
        }

        /// <summary>
        /// Updates the settings for this module instance.
        /// </summary>
        public override void UpdateSettings()
        {
            if (Page.IsValid)
            {
                ArticleId = ArticleSelectorControl.ArticleId ?? Null.NullInteger;

                LastUpdatedFormat = txtLastUpdatedFormat.Text.Trim();
                RatingDisplayOption = (RatingDisplayOption)Enum.Parse(typeof(RatingDisplayOption), ddlDisplayRatings.SelectedValue);
                DisplayCommentSubmission = chkCommentSubmit.Checked;
                DisplayComments = chkCommentDisplay.Checked;
                CommentDisplayOption = (CommentDisplayOption)Enum.Parse(typeof(CommentDisplayOption), ddlDisplayComments.SelectedValue);
                DisplayRandomComment = CommentDisplayOption.Equals(CommentDisplayOption.Paging); ////chkCommentRandom.Checked;
                FirstNameCollectOption = (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), ddlFirstNameCollect.SelectedValue);
                LastNameCollectOption = (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), ddlLastNameCollect.SelectedValue);
                CollectEmailAddress = chkEmailAddressCollect.Checked;
                CollectUrl = chkUrlCollect.Checked;
                AllowPhotoGalleryDisplay = chkDisplayPhotoGallery.Checked;

                int parsedValue;
                MaximumNumberOfThumbnails = int.TryParse(txtPhotoGalleryMaxCount.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                                     ? parsedValue
                                                     : (int?)null;
                HoverThumbnailHeight = int.TryParse(txtPhotoGalleryHoverThumbnailHeight.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                                ? parsedValue
                                                : (int?)null;
                HoverThumbnailWidth = int.TryParse(txtPhotoGalleryHoverThumbnailWidth.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                               ? parsedValue
                                               : (int?)null;
                GalleryThumbnailHeight = int.TryParse(txtPhotoGalleryThumbnailHeight.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                                  ? parsedValue
                                                  : (int?)null;
                GalleryThumbnailWidth = int.TryParse(txtPhotoGalleryThumbnailWidth.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue)
                                                 ? parsedValue
                                                 : (int?)null;
            }
        }

        /// <summary>
        /// Fills the drop downs on this control with their possible values.
        /// </summary>
        private void FillDropDowns()
        {
            ddlDisplayRatings.Items.Clear();
            ddlDisplayRatings.Items.Add(new ListItem(Localization.GetString(RatingDisplayOption.Enable.ToString(), LocalResourceFile), RatingDisplayOption.Enable.ToString()));
            ddlDisplayRatings.Items.Add(new ListItem(Localization.GetString(RatingDisplayOption.ReadOnly.ToString(), LocalResourceFile), RatingDisplayOption.ReadOnly.ToString()));
            ddlDisplayRatings.Items.Add(new ListItem(Localization.GetString(RatingDisplayOption.Disable.ToString(), LocalResourceFile), RatingDisplayOption.Disable.ToString()));

            ddlDisplayComments.Items.Clear();
            ddlDisplayComments.Items.Add(new ListItem(Localization.GetString(CommentDisplayOption.ShowAll.ToString(), LocalResourceFile), CommentDisplayOption.ShowAll.ToString()));
            ddlDisplayComments.Items.Add(new ListItem(Localization.GetString(CommentDisplayOption.Paging.ToString(), LocalResourceFile), CommentDisplayOption.Paging.ToString()));

            ddlFirstNameCollect.Items.Clear();
            ddlFirstNameCollect.Items.Add(new ListItem(Localization.GetString(NameDisplayOption.Full.ToString(), LocalResourceFile), NameDisplayOption.Full.ToString()));
            ddlFirstNameCollect.Items.Add(new ListItem(Localization.GetString(NameDisplayOption.Initial.ToString(), LocalResourceFile), NameDisplayOption.Initial.ToString()));
            ddlFirstNameCollect.Items.Add(new ListItem(Localization.GetString(NameDisplayOption.None.ToString(), LocalResourceFile), NameDisplayOption.None.ToString()));

            ddlLastNameCollect.Items.Clear();
            ddlLastNameCollect.Items.Add(new ListItem(Localization.GetString(NameDisplayOption.Full.ToString(), LocalResourceFile), NameDisplayOption.Full.ToString()));
            ddlLastNameCollect.Items.Add(new ListItem(Localization.GetString(NameDisplayOption.Initial.ToString(), LocalResourceFile), NameDisplayOption.Initial.ToString()));
            ddlLastNameCollect.Items.Add(new ListItem(Localization.GetString(NameDisplayOption.None.ToString(), LocalResourceFile), NameDisplayOption.None.ToString()));
        }

        /// <summary>
        /// Sets the values on this form based on the settings stored for this module instance
        /// </summary>
        private void SetInitialValues()
        {
            ListItem li = ddlDisplayRatings.Items.FindByValue(RatingDisplayOption.ToString());
            if (li != null)
            {
                li.Selected = true;
            }

            li = ddlDisplayComments.Items.FindByValue(CommentDisplayOption.ToString());
            if (li != null)
            {
                li.Selected = true;
            }

            if (ddlFirstNameCollect != null)
                li = ddlFirstNameCollect.Items.FindByValue(FirstNameCollectOption.ToString());
            if (li != null)
            {
                li.Selected = true;
            }

            li = ddlLastNameCollect.Items.FindByValue(LastNameCollectOption.ToString());
            if (li != null)
            {
                li.Selected = true;
            }

            ArticleSelectorControl.ArticleId = ArticleId;

            if (MaximumNumberOfThumbnails.HasValue && MaximumNumberOfThumbnails.Value > 0)
            {
                txtPhotoGalleryMaxCount.Text = MaximumNumberOfThumbnails.Value.ToString(CultureInfo.CurrentCulture);
            }

            if (HoverThumbnailHeight.HasValue && HoverThumbnailHeight.Value > 0)
            {
                txtPhotoGalleryHoverThumbnailHeight.Text = HoverThumbnailHeight.Value.ToString(CultureInfo.CurrentCulture);
            }

            if (HoverThumbnailWidth.HasValue && HoverThumbnailWidth.Value > 0)
            {
                txtPhotoGalleryHoverThumbnailWidth.Text = HoverThumbnailWidth.Value.ToString(CultureInfo.CurrentCulture);
            }

            if (GalleryThumbnailHeight.HasValue && GalleryThumbnailHeight.Value > 0)
            {
                txtPhotoGalleryThumbnailHeight.Text = GalleryThumbnailHeight.Value.ToString(CultureInfo.CurrentCulture);
            }

            if (GalleryThumbnailWidth.HasValue && GalleryThumbnailWidth.Value > 0)
            {
                txtPhotoGalleryThumbnailWidth.Text = GalleryThumbnailWidth.Value.ToString(CultureInfo.CurrentCulture);
            }

            chkDisplayPhotoGallery.Checked = AllowPhotoGalleryDisplay;

            chkCommentDisplay.Checked = DisplayComments;
            chkCommentSubmit.Checked = DisplayCommentSubmission;
            chkEmailAddressCollect.Checked = CollectEmailAddress;
            chkUrlCollect.Checked = CollectUrl;

            if (txtLastUpdatedFormat != null) txtLastUpdatedFormat.Text = LastUpdatedFormat;
        }

        /// <summary>
        /// Sets the visibility of the photo gallery options, showing a message if the settings are not available.
        /// </summary>
        /// <param name="photoGallerySettingEnabled">if set to <c>true</c> the photo gallery settings are enabled.</param>
        private void SetPhotoGalleryOptionsVisibility(bool photoGallerySettingEnabled)
        {
            lblEnablePhotoGallery.Visible = !photoGallerySettingEnabled;
            pnlPhotoGallerySettings.Visible = photoGallerySettingEnabled;
        }

        /// <summary>
        /// Sets the visibility of the ratings options, showing a message if the settings are not available.
        /// </summary>
        /// <param name="ratingsEnabled">if set to <c>true</c> the rating settings are enabled.</param>
        private void SetRatingsOptionsVisibility(bool ratingsEnabled)
        {
            pnlRatingsSettings.Visible = ratingsEnabled;
            lblEnableRatings.Visible = !ratingsEnabled;
        }

        /// <summary>
        /// Sets the visibility of the comment options, showing a message if the settings are not available.
        /// </summary>
        /// <param name="commentsEnabled">if set to <c>true</c> the comments settings are enabled.</param>
        private void SetCommentOptionsVisibility(bool commentsEnabled)
        {
            pnlCommentSettings.Visible = commentsEnabled;
            lblEnableComments.Visible = !commentsEnabled;
        }
    }
}