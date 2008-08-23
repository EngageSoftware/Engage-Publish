//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.ArticleControls
{
    public partial class ArticleDisplayOptions : ModuleSettingsBase
    {
        #region Event Handlers

        public override void LoadSettings()
        {
            BindData();

            //if (!IsPostBack)
            //{
                ListItem li = ddlArticleList.Items.FindByValue(ArticleId.ToString(CultureInfo.InvariantCulture));
                if (li != null)
                {
                    li.Selected = true;
                }
                li = ddlDisplayRatings.Items.FindByValue(RatingDisplayOption.ToString());
                if (li != null)
                {
                    li.Selected = true;
                }
                li = ddlDisplayComments.Items.FindByValue(CommentDisplayOption.ToString());
                if (li != null)
                {
                    li.Selected = true;
                }
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

                //chkPhotoGalleryShowAll.Checked = !MaximumNumberOfThumbnails.HasValue;
                chkDisplayPhotoGallery.Checked = AllowPhotoGalleryDisplay;
                //SetPhotoGallerySettingsEnabled(chkDisplayPhotoGallery.Checked);
                //SetThumbnailMaxCountSettingEnabled(chkDisplayPhotoGallery.Checked && !chkPhotoGalleryShowAll.Checked);

                chkCommentDisplay.Checked = DisplayComments;
                chkCommentSubmit.Checked = DisplayCommentSubmission;
                //txtCommentPaging.Text = CommentsPerPage.ToString(CultureInfo.CurrentCulture);
                //chkCommentRandom.Checked = DisplayRandomComment;
                chkEmailAddressCollect.Checked = CollectEmailAddress;
                chkUrlCollect.Checked = CollectUrl;

                txtLastUpdatedFormat.Text = LastUpdatedFormat;
            //}

            SetPhotoGalleryOptionsVisiblity(ModuleBase.AllowSimpleGalleryIntegrationForPortal(PortalId) || ModuleBase.AllowUltraMediaGalleryIntegrationForPortal(PortalId));
            SetCommentOptionsVisibility(ModuleBase.IsCommentsEnabledForPortal(PortalId));
            SetRatingsOptionsVisibility(ModuleBase.AreRatingsEnabledForPortal(PortalId));
        }

        private void BindData()
        {
            ddlArticleList.DataTextField = "Name";
            ddlArticleList.DataValueField = "ItemId";
            ddlArticleList.DataSource = Article.GetArticles(PortalId);
            ddlArticleList.DataBind();
            ddlArticleList.Items.Insert(0, new ListItem(Localization.GetString("ChooseAnArticle", LocalResourceFile), "-1"));

            ddlDisplayRatings.Items.Clear();
            ddlDisplayRatings.Items.Add(new ListItem(Localization.GetString(Util.RatingDisplayOption.Enable.ToString(), LocalResourceFile), Util.RatingDisplayOption.Enable.ToString()));
            ddlDisplayRatings.Items.Add(new ListItem(Localization.GetString(Util.RatingDisplayOption.ReadOnly.ToString(), LocalResourceFile), Util.RatingDisplayOption.ReadOnly.ToString()));
            ddlDisplayRatings.Items.Add(new ListItem(Localization.GetString(Util.RatingDisplayOption.Disable.ToString(), LocalResourceFile), Util.RatingDisplayOption.Disable.ToString()));

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

        //private void SetPhotoGallerySettingsEnabled(bool photoGalleryEnabled)
        //{
        //    //chkPhotoGalleryShowAll.Enabled = photoGalleryEnabled;
        //    SetThumbnailMaxCountSettingEnabled(photoGalleryEnabled);
        //}

        //private void SetThumbnailMaxCountSettingEnabled(bool maxThumbnailEnabled)
        //{
        //    txtPhotoGalleryMaxCount.Enabled = maxThumbnailEnabled;
        //    //rfvPhotoGalleryMaxCount.Enabled = maxThumbnailEnabled;
        //    //cvPhotoGalleryMaxCount.Enabled = maxThumbnailEnabled;
        //}

        private void SetPhotoGalleryOptionsVisiblity(bool photoGallerySettingEnabled)
        {
            lblEnablePhotoGallery.Visible = !photoGallerySettingEnabled;
            pnlPhotoGallerySettings.Visible = photoGallerySettingEnabled;
        }

        private void SetRatingsOptionsVisibility(bool ratingsEnabled)
        {
            pnlRatingsSettings.Visible = ratingsEnabled;
            lblEnableRatings.Visible = !ratingsEnabled;
        }

        private void SetCommentOptionsVisibility(bool commentsEnabled)
        {
            pnlCommentSettings.Visible = commentsEnabled;
            lblEnableComments.Visible = !commentsEnabled;
        }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member")]
        //protected void chkDisplayPhotoGallery_CheckedChanged(object sender, EventArgs e)
        //{
        //    SetPhotoGallerySettingsEnabled(chkDisplayPhotoGallery.Checked);
        //}

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member")]
        //protected void chkPhotoGalleryShowAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    SetThumbnailMaxCountSettingEnabled(chkDisplayPhotoGallery.Checked && !chkPhotoGalleryShowAll.Checked);
        //}
        #endregion

        private void save()
        {
            //save the new setting
//            ModuleController modules = new ModuleController();
//            modules.UpdateTabModuleSetting(this.TabModuleId, "adArticleId", this.ddlArticleList.SelectedValue.ToString());

            if (Page.IsValid)
            {
                ArticleId = int.Parse(ddlArticleList.SelectedValue, CultureInfo.InvariantCulture);
                LastUpdatedFormat = txtLastUpdatedFormat.Text.Trim();
                RatingDisplayOption = (RatingDisplayOption)Enum.Parse(typeof(RatingDisplayOption), ddlDisplayRatings.SelectedValue);
                DisplayCommentSubmission = chkCommentSubmit.Checked;
                DisplayComments = chkCommentDisplay.Checked;
                CommentDisplayOption = (CommentDisplayOption)Enum.Parse(typeof(CommentDisplayOption), ddlDisplayComments.SelectedValue);
                //CommentsPerPage = int.Parse(txtCommentPaging.Text, CultureInfo.InvariantCulture);
                DisplayRandomComment = CommentDisplayOption.Equals(CommentDisplayOption.Paging);//chkCommentRandom.Checked;
                FirstNameCollectOption = (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), ddlFirstNameCollect.SelectedValue);
                LastNameCollectOption = (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), ddlLastNameCollect.SelectedValue);
                CollectEmailAddress = chkEmailAddressCollect.Checked;
                CollectUrl = chkUrlCollect.Checked;
                AllowPhotoGalleryDisplay = chkDisplayPhotoGallery.Checked;

                int parsedValue;
                MaximumNumberOfThumbnails = int.TryParse(txtPhotoGalleryMaxCount.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue) ? parsedValue : (int?)null;
                HoverThumbnailHeight = int.TryParse(txtPhotoGalleryHoverThumbnailHeight.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue) ? parsedValue : (int?)null;
                HoverThumbnailWidth = int.TryParse(txtPhotoGalleryHoverThumbnailWidth.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue) ? parsedValue : (int?)null;
                GalleryThumbnailHeight = int.TryParse(txtPhotoGalleryThumbnailHeight.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue) ? parsedValue : (int?)null;
                GalleryThumbnailWidth = int.TryParse(txtPhotoGalleryThumbnailWidth.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue) ? parsedValue : (int?)null;
            }
        }

        public override void UpdateSettings()
        {
            save();
        }

        private int ArticleId
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adArticleId", value.ToString(CultureInfo.InvariantCulture));
            }
            get
            {
                object o = Settings["adArticleId"];
                return (o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture));
            }
        }

        private string LastUpdatedFormat
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adLastUpdatedFormat", value.ToString(CultureInfo.InvariantCulture));

            }

            get
            {
                object o = Settings["adLastUpdatedFormat"];
                return (o == null ? "F" : o.ToString());
            }
        }

        private bool AllowPhotoGalleryDisplay
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adShowPhotoGallery", value.ToString(CultureInfo.InvariantCulture));
            }
            get
            {
                object o = Settings["adShowPhotoGallery"];
                return (o == null ? false : Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture));
            }
        }

        private int? MaximumNumberOfThumbnails
        {
            set
            {
                ModuleController modules = new ModuleController();
                if (value.HasValue)
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adNumberOfThumbnails", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adNumberOfThumbnails", string.Empty);
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
                ModuleController modules = new ModuleController();
                if (value.HasValue)
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adHoverThumbnailHeight", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adHoverThumbnailHeight", string.Empty);
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
                ModuleController modules = new ModuleController();
                if (value.HasValue)
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adHoverThumbnailWidth", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adHoverThumbnailWidth", string.Empty);
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
                ModuleController modules = new ModuleController();
                if (value.HasValue)
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adGalleryThumbnailHeight", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adGalleryThumbnailHeight", string.Empty);
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
                ModuleController modules = new ModuleController();
                if (value.HasValue)
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adGalleryThumbnailWidth", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "adGalleryThumbnailWidth", string.Empty);
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
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adEnableRatings", value.ToString());
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
        /// Gets or sets whether to display the option to create a comment.
        /// </summary>
        /// <value>
        /// <c>true</c> if the option to create a comment is displayed, otherwise <c>false</c>.
        /// Defaults to <c>true</c> if no setting is defined.
        /// </value>
        private bool DisplayCommentSubmission
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adCommentsLink", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["adCommentsLink"];
                return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
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
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adCommentsDisplay", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["adCommentsDisplay"];
                return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets or sets the comment display option, whether to show all or only some of the comments at a time.
        /// </summary>
        /// <value>
        /// The comment display option.
        /// Defaults to <see cref="CommentDisplayOption.ShowAll"/> if no setting is defined.
        /// </value>
        private CommentDisplayOption CommentDisplayOption
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adCommentDisplayOption", value.ToString());
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

        ///// <summary>
        ///// Gets or sets the number of comments per page, if comments are paged.
        ///// </summary>
        ///// <value>The number of comments per page.  Defaults to 1 if no setting is defined.</value>
        //private int CommentsPerPage
        //{
        //    set
        //    {
        //        ModuleController modules = new ModuleController();
        //        modules.UpdateTabModuleSetting(this.TabModuleId, "adCommentsPerPage", value.ToString(CultureInfo.InvariantCulture));
        //    }

        //    get
        //    {
        //        object o = Settings["adCommentsPerPage"];
        //        int commentsPerPage = 1;

        //        if (o != null)
        //        {
        //            int.TryParse(o.ToString(), out commentsPerPage);
        //        }
        //        return commentsPerPage;
        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether to display one comment at a time, in a random order.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if comments should be displayed one-at-a-time in random order; otherwise, <c>false</c>.
        /// </value>
        private bool DisplayRandomComment
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adRandomComment", value.ToString());
            }

            //get
            //{
            //    object o = Settings["adRandomComment"];
            //    bool displayRandomComment = false;

            //    if (o != null)
            //    {
            //        bool.TryParse(o.ToString(), out displayRandomComment);
            //    }
            //    return displayRandomComment;
            //}
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
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adFirstNameCollectOption", value.ToString());
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
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adLastNameCollectOption", value.ToString());
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
        /// 	<c>true</c> if the email address should be collected; otherwise, <c>false</c>.
        /// </value>
        private bool CollectEmailAddress
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adCollectEmailAddress", value.ToString());
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
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "adCollectUrl", value.ToString());
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

        //protected void cvCommentPaging_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    int max = 0;
        //    args.IsValid = int.TryParse(args.Value.ToString(), out max);
        //    if (args.IsValid)
        //    {
        //        if (max <= 0)
        //        {
        //            args.IsValid = false;
        //            cvCommentPaging.Text = Localization.GetString("cvCommentPaging_Range", LocalResourceFile);
        //        }
        //    }
        //}

        //protected void chkCommentRandom_CheckedChanged(object sender, EventArgs e)
        //{
        //    txtCommentPaging.Enabled = !chkCommentRandom.Checked;
        //}
    }
}

