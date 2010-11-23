//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.ArticleControls
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using AjaxControlToolkit;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Mail;

    using Engage.Dnn.Publish.Controls;
    using Engage.Dnn.Publish.Forum;
    using Engage.Dnn.Publish.Util;
    using Engage.Dnn.UserFeedback;

    using DataProvider = Engage.Dnn.Publish.Data.DataProvider;
    using Utility = Engage.Dnn.Publish.Util.Utility;

    public partial class ArticleDisplay : ModuleBase, IActionable
    {
        private const string ArticleControlToLoad = "articleDisplay.ascx";

        private const string CommentsControlToLoad = "../Controls/CommentDisplay.ascx";

        private const string EmailControlToLoad = "../Controls/EmailAFriend.ascx";

        private const string PrinterControlToLoad = "../Controls/PrinterFriendlyButton.ascx";

        private const string RelatedArticlesControlToLoad = "../Controls/RelatedArticleLinks.ascx";

        private ArticleDisplay ad;

        private CommentDisplayBase commentDisplay;

        private EmailAFriend ea;

        private PrinterFriendlyButton pf;

        private RelatedArticleLinksBase ral;

        public ArticleDisplay()
        {
            this.DisplayPrinterFriendly = true;
            this.DisplayRelatedLinks = true;
            this.DisplayRelatedArticle = true;
            this.DisplayEmailAFriend = true;
            this.DisplayTitle = true;
        }

        public bool AllowPhotoGalleryIntegration
        {
            get
            {
                if (this.AllowSimpleGalleryIntegration || this.AllowUltraMediaGalleryIntegration)
                {
                    bool value;
                    object o = this.Settings["adShowPhotoGallery"];
                    if (o != null && bool.TryParse(o.ToString(), out value))
                    {
                        return value;
                    }
                }

                return false;
            }
        }

        public bool DisplayEmailAFriend { get; set; }

        public bool DisplayPrinterFriendly { get; set; }

        public bool DisplayRelatedArticle { get; set; }

        public bool DisplayRelatedLinks { get; set; }

        public bool DisplayTitle { get; set; }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString("Settings", this.LocalResourceFile), ModuleActionType.AddContent, 
                            string.Empty, string.Empty, this.EditUrl("Settings"), false, SecurityAccessLevel.Edit, true, false
                            }
                    };
            }
        }

        public bool ShowAuthor { get; set; }

        public bool ShowTags { get; set; }

        protected int? GalleryThumbnailHeight
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
        }

        protected int? GalleryThumbnailWidth
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
        }

        protected int? HoverThumbnailHeight
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
        }

        protected int? HoverThumbnailWidth
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
        }

        protected string PhotoMouseOverText
        {
            get { return Localization.GetString("PhotoMouseover", this.LocalResourceFile); }
        }

        /// <summary>
        /// Gets a value indicating whether to display a textbox to collect the email address of the commenter.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the email address should be collected; otherwise, <c>false</c>.
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
        }

        /// <summary>
        /// Gets whether to display the option to create a comment.
        /// </summary>
        /// <value>
        /// <c>true</c> if the option to create a comment is displayed, otherwise <c>false</c>.
        /// Defaults to <c>true</c> if no setting is defined.
        /// </value>
        private bool DisplayCommentsLink
        {
            get
            {
                if (this.IsCommentsEnabled)
                {
                    object o = this.Settings["adCommentsLink"];
                    return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to display comments made on this item.
        /// </summary>
        /// <value>
        /// <c>true</c> if comments should be displayed; otherwise, <c>false</c>.
        /// Defaults to <c>true</c> if no setting is defined.
        /// </value>
        private bool DisplayPublishComments
        {
            get
            {
                if (this.IsCommentsEnabled)
                {
                    object o = this.Settings["adCommentsDisplay"];
                    if (o != null)
                    {
                        if (!Convert.ToBoolean(o, CultureInfo.InvariantCulture))
                        {
                            return false;
                        }
                    }

                    // else { o = true; }
                    ItemVersionSetting forumCommentSetting =
                        ItemVersionSetting.GetItemVersionSetting(this.VersionInfoObject.ItemVersionId, "chkForumComments", "Checked", this.PortalId) ??
                        new ItemVersionSetting
                            {
                                ControlName = "chkForumComments", 
                                PropertyName = "Checked", 
                                PropertyValue = false.ToString()
                            };

                    return this.IsPublishCommentType || !Convert.ToBoolean(forumCommentSetting.PropertyValue, CultureInfo.InvariantCulture);
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the first name collection option, whether to ask for the full name, only the initial, or not to ask for the first name at all.
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
        }

        /// <summary>
        /// Gets the last name collection option, whether to ask for the full name, only the initial, or not to ask for the last name at all.
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

                return NameDisplayOption.Initial;
            }
        }

        private string LastUpdatedFormat
        {
            get
            {
                object o = this.Settings["adLastUpdatedFormat"];
                return o == null ? "MMM yyyy" : o.ToString();
            }
        }

        private int? NumberOfThumbnails
        {
            get
            {
                int value;
                object o = this.Settings["adNumberOfThumbnails"];
                if (o != null && int.TryParse(o.ToString(), out value))
                {
                    return value;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the setting value for this module, whether to show ratings or not.
        /// </summary>
        /// <value>
        /// The rating display option.
        /// Defaults to <see cref="Util.RatingDisplayOption.Enable"/> if no setting is defined.
        /// </value>
        private RatingDisplayOption RatingDisplayOption
        {
            get
            {
                if (this.AreRatingsEnabled)
                {
                    object o = this.Settings["adEnableRatings"];
                    if (o != null && Enum.IsDefined(typeof(RatingDisplayOption), o))
                    {
                        return (RatingDisplayOption)Enum.Parse(typeof(RatingDisplayOption), o.ToString());
                    }

                    return RatingDisplayOption.Enable;
                }

                return RatingDisplayOption.Disable;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to display a link to make comments on this item in a forum post.
        /// </summary>
        /// <value>
        /// <c>true</c> if comments should be made in a forum; otherwise, <c>false</c>.
        /// Defaults to <c>true</c> if no setting is defined.
        /// </value>
        private bool UseForumComments
        {
            get
            {
                if (this.IsPublishCommentType)
                {
                    return false;
                }

                ItemVersionSetting forumCommentSetting = ItemVersionSetting.GetItemVersionSetting(
                    this.VersionInfoObject.ItemVersionId, "chkForumComments", "Checked", this.PortalId);
                int? categoryForumId = this.GetCategoryForumId();
                if (!categoryForumId.HasValue || categoryForumId < 1)
                {
                    return false;
                }

                return (this.IsCommentsEnabled && !this.IsPublishCommentType) &&
                       (forumCommentSetting != null && Convert.ToBoolean(forumCommentSetting.PropertyValue, CultureInfo.InvariantCulture));

                // {
                // object o = Settings["adCommentsDisplay"];
                // return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
                // }
                // return false;
            }
        }

        protected static string GetPhotoAltText(object dataItem)
        {
            // string clickToView = Localization.GetString("ClickToView", LocalResourceFile);
            var row = dataItem as DataRowView;
            if (row != null)
            {
                // SimpleGallery uses Name, UltraMedia uses Title
                if (row.DataView.Table.Columns.Contains("Name"))
                {
                    return row["Name"].ToString();
                }

                return row["Title"].ToString();
            }

            return string.Empty;
        }

        protected string GetPhotoLink(object dataItem)
        {
            var row = dataItem as DataRowView;
            if (row != null)
            {
                var moduleId = (int)row["ModuleId"];
                int? albumId = this.GetUltraMediaGalleryAlbumId();

                if (albumId.HasValue)
                {
                    // ultra media
                    string popupUrl = this.ResolveUrl(
                        string.Format(
                            CultureInfo.InvariantCulture, 
                            this.Localize("UltraMediaGalleryPopupUrl.Format")
#if DEBUG
                                .Replace("[L]", string.Empty)
#endif
                            , 
                            moduleId, 
                            this.PortalId, 
                            albumId.Value));
                    return string.Format(
                        this.Localize("UltraMediaGalleryImageLink.Format")
#if DEBUG
                            .Replace("[L]", string.Empty)
#endif
                        , 
                        popupUrl, 
                        GetPopupWidth(moduleId), 
                        GetPopupHeight(moduleId));
                }
                else
                {
                    // simple gallery
                    string popupUrl = this.ResolveUrl(
                        string.Format(
                            CultureInfo.InvariantCulture, 
                            this.Localize("SimpleGalleryPopupUrl.Format")
#if DEBUG
                                .Replace("[L]", string.Empty)
#endif
                            , 
                            this.PortalId, 
                            Null.NullInteger, 
                            row["PhotoID"], 
                            GetBorderStyle(moduleId), 
                            GetSortBy(moduleId), 
                            GetSortDirection(moduleId), 
                            GetToolTipSetting(moduleId)));
                    return string.Format(
                        this.Localize("SimpleGalleryImageLink.Format")
#if DEBUG
                            .Replace("[L]", string.Empty)
#endif
                        , 
                        popupUrl, 
                        GetPopupWidth(moduleId), 
                        GetPopupHeight(moduleId));
                }
            }

            return string.Empty;
        }

        protected string GetPhotoPath(object dataItem)
        {
            var row = dataItem as DataRowView;
            if (row != null)
            {
                // SimpleGallery has HomeDirectory column, otherwise just point to the image path.
                if (row.DataView.Table.Columns.Contains("HomeDirectory"))
                {
                    return
                        this.ResolveUrl(
                            "~/DesktopModules/SimpleGallery/ImageHandler.ashx?width=" + row["Width"] + "&height=" + row["Height"] + "&HomeDirectory=" +
                            this.Server.UrlEncode(this.PortalSettings.HomeDirectory + row["HomeDirectory"]) + "&fileName=" +
                            this.Server.UrlEncode(row["FileName"].ToString()) + "&portalid=" + this.PortalId.ToString(CultureInfo.InvariantCulture) +
                            "&i=" + row["PhotoID"] + "&q=1");
                }

                return row["ImagePath"].ToString();
            }

            return string.Empty;
        }

        protected string GetThumbPhotoPath(object dataItem)
        {
            var row = dataItem as DataRowView;
            if (row != null)
            {
                // SimpleGallery has HomeDirectory column, otherwise just point to the image path.
                if (row.DataView.Table.Columns.Contains("HomeDirectory"))
                {
                    return
                        this.ResolveUrl(
                            "~/DesktopModules/SimpleGallery/ImageHandler.ashx?width=" + GetPhotoWidth(row).ToString(CultureInfo.InvariantCulture) +
                            "&height=" + GetPhotoHeight(row).ToString(CultureInfo.InvariantCulture) + "&HomeDirectory=" +
                            this.Server.UrlEncode(this.PortalSettings.HomeDirectory + row["HomeDirectory"]) + "&fileName=" +
                            this.Server.UrlEncode(row["FileName"].ToString()) + "&portalid=" + this.PortalId.ToString(CultureInfo.InvariantCulture) +
                            "&i=" + row["PhotoID"] + "&q=1");
                }

                return row["ImageThumbPath"].ToString();
            }

            return string.Empty;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
            this.LoadArticle();
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void ajaxRating_Changed(object sender, RatingEventArgs e)
        {
            var article = (Article)this.VersionInfoObject;
            article.AddRating(int.Parse(e.Value, CultureInfo.InvariantCulture), this.UserId == -1 ? null : (int?)this.UserId);
            this.ajaxRating.ReadOnly = true;
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void btnCancelComment_Click(object sender, EventArgs e)
        {
            this.ClearCommentInput();
            this.mpeComment.Hide();
            this.mpeForumComment.Hide();
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void btnConfirmationClose_Click(object sender, EventArgs e)
        {
            this.pnlCommentEntry.Visible = true;
            this.pnlCommentConfirmation.Visible = false;
            this.mpeComment.Hide();
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void btnSubmitComment_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                // TODO: we're allowing anonymous comments, we should have a setting for this.
                var objSecurity = new PortalSecurity();
                if (this.UseForumComments)
                {
                    int? categoryForumId = this.GetCategoryForumId();
                    if (categoryForumId.HasValue)
                    {
                        int threadId = ForumProvider.GetInstance(this.PortalId).AddComment(
                            categoryForumId.Value, 
                            this.VersionInfoObject.AuthorUserId, 
                            this.VersionInfoObject.Name, 
                            this.VersionInfoObject.Description, 
                            this.GetItemLinkUrl(this.VersionInfoObject.ItemId, this.PortalId), 
                            objSecurity.InputFilter(this.txtComment.Text, PortalSecurity.FilterFlag.NoScripting), 
                            this.UserId, 
                            this.Request.UserHostAddress);

                        var threadIdSetting = new ItemVersionSetting(Setting.CommentForumThreadId)
                            {
                                PropertyValue = threadId.ToString(CultureInfo.InvariantCulture), 
                                ItemVersionId = this.VersionInfoObject.ItemVersionId
                            };
                        threadIdSetting.Save();

                        // VersionInfoObject.VersionSettings.Add(threadIdSetting);
                        // VersionInfoObject.Save(VersionInfoObject.AuthorUserId);
                        this.Response.Redirect(ForumProvider.GetInstance(this.PortalId).GetThreadUrl(threadId), true);
                    }
                }
                else
                {
                    string urlText = this.txtUrlComment.Text;
                    if (urlText.Trim().Length > 0 && !urlText.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                        !urlText.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        urlText = "http://" + urlText;
                    }

                    int approvalStatusId = ApprovalStatus.Waiting.GetId();
                    if (this.IsAdmin)
                    {
                        // automatically approve admin comments
                        approvalStatusId = ApprovalStatus.Approved.GetId();
                    }

                    Comment.AddComment(
                        this.VersionInfoObject.ItemVersionId, 
                        this.UserId == -1 ? null : (int?)this.UserId, 
                        objSecurity.InputFilter(this.txtComment.Text, PortalSecurity.FilterFlag.NoScripting), 
                        approvalStatusId, 
                        null, 
                        objSecurity.InputFilter(this.txtFirstNameComment.Text, PortalSecurity.FilterFlag.NoScripting), 
                        objSecurity.InputFilter(this.txtLastNameComment.Text, PortalSecurity.FilterFlag.NoScripting), 
                        objSecurity.InputFilter(this.txtEmailAddressComment.Text, PortalSecurity.FilterFlag.NoScripting), 
                        objSecurity.InputFilter(urlText, PortalSecurity.FilterFlag.NoScripting), 
                        DataProvider.ModuleQualifier);

                    // see if comment notification is turned on. Notify the ItemVersion.Author
                    if (this.IsCommentAuthorNotificationEnabled)
                    {
                        var uc = new UserController();

                        UserInfo ui = uc.GetUser(this.PortalId, this.VersionInfoObject.AuthorUserId);

                        if (ui != null)
                        {
                            string emailBody = Localization.GetString("CommentNotificationEmail.Text", this.LocalResourceFile);
                            emailBody = String.Format(
                                emailBody, 
                                this.VersionInfoObject.Name, 
                                this.GetItemLinkUrlExternal(this.VersionInfoObject.ItemId), 
                                objSecurity.InputFilter(this.txtFirstNameComment.Text, PortalSecurity.FilterFlag.NoScripting), 
                                objSecurity.InputFilter(this.txtLastNameComment.Text, PortalSecurity.FilterFlag.NoScripting), 
                                objSecurity.InputFilter(this.txtEmailAddressComment.Text, PortalSecurity.FilterFlag.NoScripting), 
                                objSecurity.InputFilter(this.txtComment.Text, PortalSecurity.FilterFlag.NoScripting));

                            string emailSubject = Localization.GetString("CommentNotificationEmailSubject.Text", this.LocalResourceFile);
                            emailSubject = String.Format(emailSubject, this.VersionInfoObject.Name);

                            Mail.SendMail(
                                this.PortalSettings.Email, 
                                ui.Email, 
                                string.Empty, 
                                emailSubject, 
                                emailBody, 
                                string.Empty, 
                                "HTML", 
                                string.Empty, 
                                string.Empty, 
                                string.Empty, 
                                string.Empty);
                        }
                    }
                }

                this.ConfigureComments();

                this.pnlCommentEntry.Visible = false;
                this.pnlCommentConfirmation.Visible = true;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Compiler doesn't see validation")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member")]
        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
        protected void rpThumbnails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e != null && (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item))
            {
                var pceArticleThumbnail = e.Item.FindControl("pceArticleThumbnail") as PopupControlExtender;
                var pnlLargeImage = e.Item.FindControl("pnlLargeImage") as Panel;
                var pnlSmallImage = e.Item.FindControl("pnlSmallImage") as Panel;
                var imgThumbnail = e.Item.FindControl("imgThumbnail") as HtmlImage;

                if (pceArticleThumbnail != null)
                {
                    pceArticleThumbnail.BehaviorID = pceArticleThumbnail.ClientID;

                    if (pnlLargeImage != null && imgThumbnail != null && pnlSmallImage != null)
                    {
                        // Page.ClientScript.RegisterStartupScript(this.GetType(), imgThumbnail.ClientID, "Engage_ThumbnailHashtable." + imgThumbnail.ClientID + " = ['" + pceArticleThumbnail.ClientID + "', '" + pnlLargeImage.ClientID + "', '" + pnlSmallImage.ClientID + "'];", true);
                        // Page.ClientScript.RegisterStartupScript(this.GetType(), pnlLargeImage.ClientID, "Engage_ThumbnailHashtable." + pnlLargeImage.ClientID + " = Engage_ThumbnailHashtable." + imgThumbnail.ClientID + ";", true);
                        // Page.ClientScript.RegisterStartupScript(this.GetType(), imgThumbnail.ClientID + "Show", "$addHandler($get('" + imgThumbnail.ClientID + "'), 'mouseover', ShowImage);", true);
                        // Page.ClientScript.RegisterStartupScript(this.GetType(), imgThumbnail.ClientID + "Hide", "$addHandler($get('" + imgThumbnail.ClientID + "'), 'mouseout', HideImage);", true);
                        // Page.ClientScript.RegisterStartupScript(this.GetType(), pnlLargeImage.ClientID + "Show", "$addHandler($get('" + pnlLargeImage.ClientID + "'), 'mouseover', ShowImage);", true);
                        // Page.ClientScript.RegisterStartupScript(this.GetType(), pnlLargeImage.ClientID + "Hide", "$addHandler($get('" + pnlLargeImage.ClientID + "'), 'mouseout', HideImage);", true);
                        imgThumbnail.Attributes["onmouseover"] = "ShowImage('" + pceArticleThumbnail.ClientID + "', '" + pnlLargeImage.ClientID + "')";
                        imgThumbnail.Attributes["onmouseout"] = "HideImage('" + pceArticleThumbnail.ClientID + "', '" + pnlLargeImage.ClientID +
                                                                "', '" + pnlSmallImage.ClientID + "')";
                        imgThumbnail.Attributes["onclick"] = this.GetPhotoLink(e.Item.DataItem);

                        if (this.GalleryThumbnailHeight.HasValue)
                        {
                            imgThumbnail.Height = this.GalleryThumbnailHeight.Value;
                        }

                        if (this.GalleryThumbnailWidth.HasValue)
                        {
                            imgThumbnail.Width = this.GalleryThumbnailWidth.Value;
                        }
                    }
                }
            }
        }

        private static string GetBorderStyle(int moduleId)
        {
            return Utility.GetTabModuleSettingAsString(moduleId, "BorderStyle", "White");
        }

        private static int GetMaxThumbnailHeight(int moduleId)
        {
            return Utility.GetTabModuleSettingAsInt(moduleId, "ThumbnailHeight", 125);
        }

        private static int GetMaxThumbnailWidth(int moduleId)
        {
            return Utility.GetTabModuleSettingAsInt(moduleId, "ThumbnailWidth", 125);
        }

        private static int GetPhotoHeight(object dataItem)
        {
            var row = dataItem as DataRowView;
            if (row != null)
            {
                if (row.DataView.Table.Columns.Contains("TnHeight"))
                {
                    // if there is a TnHeight column, it's been calculated by Ultra Media Gallery, just return it.
                    object o = row["TnHeight"];
                    if (o != null)
                    {
                        return int.Parse(o.ToString(), CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    var actualHeight = (int)row["Height"];
                    var actualWidth = (int)row["Width"];
                    int maxThumbnailWidth = GetMaxThumbnailWidth((int)row["ModuleId"]);
                    int maxThumbnailHeight = GetMaxThumbnailHeight((int)row["ModuleId"]);
                    int thumbnailWidth = actualWidth > maxThumbnailWidth ? maxThumbnailWidth : actualWidth;
                    int thumbnailHeight = Convert.ToInt32(actualHeight / ((double)actualWidth / thumbnailWidth));
                    if (thumbnailHeight > maxThumbnailHeight)
                    {
                        thumbnailHeight = maxThumbnailHeight;

                        // thumbnailWidth = Convert.ToInt32(actualWidth / ((double)actualHeight / thumbnailHeight));
                    }

                    return thumbnailHeight;
                }
            }

            return 125;
        }

        private static int GetPhotoWidth(object dataItem)
        {
            var row = dataItem as DataRowView;
            if (row != null)
            {
                if (row.DataView.Table.Columns.Contains("TnWidth"))
                {
                    // if there is a TnWidth column, it's been calculated by Ultra Media Gallery, just return it.
                    object o = row["TnWidth"];
                    if (o != null)
                    {
                        return int.Parse(o.ToString(), CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    var actualHeight = (int)row["Height"];
                    var actualWidth = (int)row["Width"];
                    int maxThumbnailWidth = GetMaxThumbnailWidth((int)row["ModuleId"]);
                    int maxThumbnailHeight = GetMaxThumbnailHeight((int)row["ModuleId"]);
                    int thumbnailWidth = actualWidth > maxThumbnailWidth ? maxThumbnailWidth : actualWidth;

                    if (Convert.ToInt32(actualHeight / ((double)actualWidth / thumbnailWidth)) > maxThumbnailHeight)
                    {
                        thumbnailWidth = Convert.ToInt32(actualWidth / ((double)actualHeight / maxThumbnailHeight));
                    }

                    return thumbnailWidth;
                }
            }

            return 125;
        }

        private static int GetPopupHeight(int moduleId)
        {
            int height;

            // UltraMediaGallery
            object heightObj = (new ModuleController()).GetModuleSettings(moduleId)["GalleryHeight"];
            if (heightObj != null && int.TryParse(heightObj.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out height))
            {
                return height;
            }

            // SimpleGallery
            heightObj = (new ModuleController()).GetModuleSettings(moduleId)["PopupHeight"];
            if (heightObj != null && int.TryParse(heightObj.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out height))
            {
                return height;
            }

            return 600;
        }

        private static int GetPopupWidth(int moduleId)
        {
            int width;

            // UltraMediaGallery
            object widthObj = (new ModuleController()).GetModuleSettings(moduleId)["GalleryWidth"];
            if (widthObj != null && int.TryParse(widthObj.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out width))
            {
                return width;
            }

            widthObj = (new ModuleController()).GetModuleSettings(moduleId)["PopupWidth"];
            if (widthObj != null && int.TryParse(widthObj.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out width))
            {
                return width;
            }

            return 645;
        }

        private static string GetSortBy(int moduleId)
        {
            string defaultValue = "Name";
            List<int> version = null;
            try
            {
                version = Utility.ParseIntegerList(
                    DataProvider.Instance().GetSimpleGalleryVersion().Split(
                        new[]
                            {
                                '.'
                            }));
            }
                
                // if we can't get the version, just swallow the exception. BD
            catch (FormatException)
            {
            }

            if (version != null)
            {
                if (Utility.IsVersionGreaterOrEqual(
                    version, 
                    new List<int>(
                        new[]
                            {
                                2, 2, 0
                            })))
                {
                    defaultValue = "0"; // 0=name, 3=fileName, 1=dateCreated, 2=dateApproved
                }
            }

            return Utility.GetTabModuleSettingAsString(moduleId, "SortBy", defaultValue);
        }

        private static string GetSortDirection(int moduleId)
        {
            string defaultValue = "ASC";
            List<int> version = null;
            try
            {
                version = Utility.ParseIntegerList(
                    DataProvider.Instance().GetSimpleGalleryVersion().Split(
                        new[]
                            {
                                '.'
                            }));
            }
                
                // if we can't get the version, just swallow the exception. BD
            catch (FormatException)
            {
            }

            if (version != null)
            {
                if (Utility.IsVersionGreaterOrEqual(
                    version, 
                    new List<int>(
                        new[]
                            {
                                2, 2, 0
                            })))
                {
                    defaultValue = "1"; // 0=desc, 1=asc
                }
            }

            return Utility.GetTabModuleSettingAsString(moduleId, "SortDirection", defaultValue);
        }

        private static string GetToolTipSetting(int moduleId)
        {
            return Utility.GetTabModuleSettingAsString(moduleId, "EnableTooltip", true.ToString(CultureInfo.InvariantCulture));
        }

        private static DataTable SetSimpleGalleryImagePath(DataTable photos)
        {
            if (photos != null)
            {
                photos.Columns.Add("ImageThumbPath");
                photos.Columns.Add("ImagePath");

                foreach (DataRow row in photos.Rows)
                {
                    row["ImagePath"] = row["ImageThumbPath"] = Path.Combine(row["HomeDirectory"].ToString(), row["FileName"].ToString());
                }
            }

            return photos;
        }

        private void ClearCommentInput()
        {
            this.txtComment.Text = string.Empty;
            this.txtFirstNameComment.Text = string.Empty;
            this.txtLastNameComment.Text = string.Empty;
            this.txtEmailAddressComment.Text = string.Empty;
        }

        private void ConfigureChildControls()
        {
            if (this.VersionInfoObject.IsNew)
            {
                return;
            }

            // check if items are enabled.
            if (this.DisplayEmailAFriend && this.VersionInfoObject.IsNew == false)
            {
                this.ea = (EmailAFriend)this.LoadControl(EmailControlToLoad);
                this.ea.ModuleConfiguration = this.ModuleConfiguration;
                this.ea.ID = Path.GetFileNameWithoutExtension(EmailControlToLoad);
                this.phEmailAFriend.Controls.Add(this.ea);
            }

            if (this.DisplayPrinterFriendly && this.VersionInfoObject.IsNew == false)
            {
                this.pf = (PrinterFriendlyButton)this.LoadControl(PrinterControlToLoad);
                this.pf.ModuleConfiguration = this.ModuleConfiguration;
                this.pf.ID = Path.GetFileNameWithoutExtension(PrinterControlToLoad);
                this.phPrinterFriendly.Controls.Add(this.pf);
            }

            if (this.DisplayRelatedLinks)
            {
                this.ral = (RelatedArticleLinksBase)this.LoadControl(RelatedArticlesControlToLoad);
                this.ral.ModuleConfiguration = this.ModuleConfiguration;
                this.ral.ID = Path.GetFileNameWithoutExtension(RelatedArticlesControlToLoad);
                this.phRelatedArticles.Controls.Add(this.ral);
            }

            if (this.DisplayRelatedArticle)
            {
                Article a = this.VersionInfoObject.GetRelatedArticle(this.PortalId);
                if (a != null)
                {
                    this.ad = (ArticleDisplay)this.LoadControl(ArticleControlToLoad);
                    this.ad.ModuleConfiguration = this.ModuleConfiguration;
                    this.ad.ID = Path.GetFileNameWithoutExtension(ArticleControlToLoad);
                    this.ad.Overrideable = false;
                    this.ad.UseCache = true;
                    this.ad.DisplayPrinterFriendly = false;
                    this.ad.DisplayRelatedArticle = false;
                    this.ad.DisplayRelatedLinks = false;
                    this.ad.DisplayEmailAFriend = false;

                    this.ad.SetItemId(a.ItemId);
                    this.ad.DisplayTitle = false;
                    this.phRelatedArticle.Controls.Add(this.ad);
                    this.divRelatedArticle.Visible = true;
                }
                else
                {
                    this.divRelatedArticle.Visible = false;
                }
            }

            if (this.RatingDisplayOption.Equals(RatingDisplayOption.Enable) || this.RatingDisplayOption.Equals(RatingDisplayOption.ReadOnly))
            {
                // get the upnlRating setting
                ItemVersionSetting rtSetting = ItemVersionSetting.GetItemVersionSetting(
                    this.VersionInfoObject.ItemVersionId, "upnlRating", "Visible", this.PortalId);
                if (rtSetting != null)
                {
                    this.upnlRating.Visible = Convert.ToBoolean(rtSetting.PropertyValue, CultureInfo.InvariantCulture);
                }

                if (this.upnlRating.Visible)
                {
                    this.lblRatingMessage.Visible = true;
                    this.ajaxRating.MaxRating = this.MaximumRating;

                    var avgRating = (int)Math.Round(((Article)this.VersionInfoObject).AverageRating);
                    this.ajaxRating.CurrentRating = avgRating > this.MaximumRating ? this.MaximumRating : (avgRating < 0 ? 0 : avgRating);

                    this.ajaxRating.ReadOnly = this.RatingDisplayOption.Equals(RatingDisplayOption.ReadOnly);
                }
            }

            this.btnComment.Visible = this.DisplayCommentsLink;
            if (this.IsCommentsEnabled)
            {
                if (!this.UseForumComments || (this.DisplayPublishComments && !this.VersionInfoObject.IsNew))
                {
                    this.pnlComments.Visible = this.pnlCommentDisplay.Visible = true;
                    this.commentDisplay = (CommentDisplayBase)this.LoadControl(CommentsControlToLoad);
                    this.commentDisplay.ModuleConfiguration = this.ModuleConfiguration;
                    this.commentDisplay.ID = Path.GetFileNameWithoutExtension(CommentsControlToLoad);
                    this.commentDisplay.ArticleId = this.VersionInfoObject.ItemId;
                    this.phCommentsDisplay.Controls.Add(this.commentDisplay);
                }

                if (this.UseForumComments)
                {
                    this.pnlComments.Visible = true;
                    this.mvCommentDisplay.SetActiveView(this.vwForumComments);
                    ItemVersionSetting forumThreadIdSetting = ItemVersionSetting.GetItemVersionSetting(
                        this.VersionInfoObject.ItemVersionId, "ArticleSetting", "CommentForumThreadId", this.PortalId);
                    if (forumThreadIdSetting != null)
                    {
                        this.lnkGoToForum.Visible = true;
                        this.lnkGoToForum.NavigateUrl =
                            ForumProvider.GetInstance(this.PortalId).GetThreadUrl(
                                Convert.ToInt32(forumThreadIdSetting.PropertyValue, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        this.btnForumComment.Visible = true;
                    }
                }
            }

            this.ConfigureTags();
        }

        private void ConfigureComments()
        {
            bool showNamePanel = false;
            this.ClearCommentInput();
            if (this.mvCommentDisplay.GetActiveView() == this.vwPublishComments)
            {
                switch (this.FirstNameCollectOption)
                {
                    case NameDisplayOption.Initial:
                        this.txtFirstNameComment.MaxLength = 1;
                        this.txtFirstNameComment.Text = (this.UserInfo != null && this.UserInfo.UserID != -1)
                                                            ? this.UserInfo.FirstName.Substring(0, 1)
                                                            : string.Empty;
                        this.lblFirstNameComment.Text = Localization.GetString("FirstInitial", this.LocalResourceFile);
                        showNamePanel = true;
                        break;
                    case NameDisplayOption.None:
                        this.txtFirstNameComment.Visible = false;
                        this.lblFirstNameComment.Visible = false;
                        this.rfvFirstNameComment.Enabled = false;

                        // vceFirstNameComment.Enabled = false;
                        break;

                        // case NameDisplayOption.Full:
                    default:
                        this.txtFirstNameComment.Text = (this.UserInfo != null && this.UserInfo.UserID != -1) ? this.UserInfo.FirstName : string.Empty;
                        this.lblFirstNameComment.Text = Localization.GetString("FirstName", this.LocalResourceFile);
                        showNamePanel = true;
                        break;
                }

                switch (this.LastNameCollectOption)
                {
                    case NameDisplayOption.Initial:
                        this.txtLastNameComment.MaxLength = 1;
                        this.txtLastNameComment.Text = (this.UserInfo != null && this.UserInfo.UserID != -1)
                                                           ? this.UserInfo.LastName.Substring(0, 1)
                                                           : string.Empty;
                        this.lblLastNameComment.Text = Localization.GetString("LastInitial", this.LocalResourceFile);
                        showNamePanel = true;
                        break;
                    case NameDisplayOption.None:
                        this.txtLastNameComment.Visible = false;
                        this.lblLastNameComment.Visible = false;
                        this.rfvLastNameComment.Enabled = false;

                        // vceLastNameComment.Enabled = false;
                        break;

                        // case NameDisplayOption.Full:
                    default:
                        this.txtLastNameComment.Text = (this.UserInfo != null && this.UserInfo.UserID != -1) ? this.UserInfo.LastName : string.Empty;
                        this.lblLastNameComment.Text = Localization.GetString("LastName", this.LocalResourceFile);
                        showNamePanel = true;
                        break;
                }

                this.pnlEmailAddressComment.Visible = this.rfvEmailAddressComment.Enabled = this.CollectEmailAddress;
                this.pnlNameComment.Visible = showNamePanel;
                this.pnlUrlComment.Visible = this.CollectUrl;

                if (this.pnlEmailAddressComment.Visible)
                {
                    this.txtEmailAddressComment.Text = (this.UserInfo != null && this.UserInfo.UserID != -1) ? this.UserInfo.Email : string.Empty;
                }

                if (this.pnlUrlComment.Visible)
                {
                    this.txtUrlComment.Text = (this.UserInfo != null && this.UserInfo.UserID != -1) ? this.UserInfo.Profile.Website : string.Empty;
                }
            }
            else
            {
                this.pnlNameComment.Visible = this.rfvFirstNameComment.Enabled = this.rfvLastNameComment.Enabled = false;
                this.pnlEmailAddressComment.Visible = this.rfvEmailAddressComment.Enabled = false;
                this.pnlUrlComment.Visible = false;
            }
        }

        private void ConfigureSettings()
        {
            // LogBreadcrumb is true by default.  Check to see if we need to turn it off.
            object o = this.Settings["LogBreadCrumb"];
            if (o != null)
            {
                bool logBreadCrumb;
                if (bool.TryParse(o.ToString(), out logBreadCrumb))
                {
                    this.LogBreadcrumb = logBreadCrumb;
                }
            }
        }

        private void ConfigureTags()
        {
            // get the upnlRating setting
            ItemVersionSetting tgSetting = ItemVersionSetting.GetItemVersionSetting(
                this.VersionInfoObject.ItemVersionId, "pnlTags", "Visible", this.PortalId);
            if (tgSetting != null)
            {
                this.pnlTags.Visible = Convert.ToBoolean(tgSetting.PropertyValue, CultureInfo.InvariantCulture);
                if (Convert.ToBoolean(tgSetting.PropertyValue, CultureInfo.InvariantCulture))
                {
                    this.PopulateTagList();
                }
            }
            else
            {
                if (this.VersionInfoObject.Tags.Count > 0)
                {
                    this.pnlTags.Visible = true;
                    this.PopulateTagList();
                }
            }
        }

        private void DisplayArticle()
        {
            if (this.VersionInfoObject.IsNew)
            {
                if (this.IsAdmin || this.IsAuthor)
                {
                    // Default the text to no approved version. if the module isn't configured or no Categories/Articles exist yet then it will be overwritten.
                    this.lblArticleText.Text = Localization.GetString("NoApprovedVersion", this.LocalResourceFile);

                    // Check to see if there are Categories defined. If none are defined this is the first
                    // instance of the Module so we need to notify the user to create categories and articles.
                    int categoryCount = DataProvider.Instance().GetCategories(this.PortalId).Rows.Count;
                    if (categoryCount == 0)
                    {
                        this.lblArticleText.Text = Localization.GetString("NoDataToDisplay", this.LocalResourceFile);
                    }
                    else if (this.IsConfigured == false)
                    {
                        this.lnkConfigure.Text = Localization.GetString("UnableToFindAction", this.LocalResourceFile);
                        this.lnkConfigure.NavigateUrl = this.EditUrl("ModuleId", this.ModuleId.ToString(CultureInfo.InvariantCulture), "Module");
                        this.lnkConfigure.Visible = true;
                        this.lblArticleText.Text = Localization.GetString("UnableToFind", this.LocalResourceFile);
                    }
                }

                return;
            }

            if (Item.GetItemType(this.VersionInfoObject.ItemId, this.PortalId) == "Article")
            {
                this.UseCache = true;

                var article = (Article)this.VersionInfoObject;
                if (this.DisplayTitle)
                {
                    this.SetPageTitle();
                    this.lblArticleTitle.Text = article.Name;
                    this.divArticleTitle.Visible = true;
                    this.divLastUpdated.Visible = true;
                }

                article.ArticleText = Utility.ReplaceTokens(article.ArticleText);
                this.DisplayArticlePaging(article);

                string referrer = string.Empty;
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    referrer = HttpContext.Current.Request.UrlReferrer.ToString();
                }

                string url = string.Empty;
                if (HttpContext.Current.Request.RawUrl != null)
                {
                    url = HttpContext.Current.Request.RawUrl;
                }

                article.AddView(
                    this.UserId, this.TabId, HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserAgent, referrer, url);

                DateTime lastUpdated = Convert.ToDateTime(article.LastUpdated, CultureInfo.InvariantCulture);

                this.lblLastUpdated.Text = Localization.GetString("LastUpdated", this.LocalResourceFile) + " " +
                                           lastUpdated.ToString(this.LastUpdatedFormat, CultureInfo.CurrentCulture);

                // get the pnlAuthor setting
                ItemVersionSetting auSetting = ItemVersionSetting.GetItemVersionSetting(article.ItemVersionId, "pnlAuthor", "Visible", this.PortalId);
                if (auSetting != null)
                {
                    this.ShowAuthor = Convert.ToBoolean(auSetting.PropertyValue, CultureInfo.InvariantCulture);
                }

                if (this.ShowAuthor)
                {
                    this.pnlAuthor.Visible = true;
                    this.lblAuthor.Text = article.Author;
                    if (this.lblAuthor.Text.Trim().Length < 1)
                    {
                        var uc = new UserController();
                        UserInfo ui = uc.GetUser(this.PortalId, article.AuthorUserId);
                        this.lblAuthor.Text = ui.DisplayName;
                    }

                    if (this.lblAuthor.Text.Trim().Length < 1)
                    {
                        this.pnlAuthor.Visible = false;
                    }
                }
                else
                {
                    this.pnlAuthor.Visible = false;
                }

                // get the pnlPrinterFriendly setting
                ItemVersionSetting pfSetting = ItemVersionSetting.GetItemVersionSetting(
                    article.ItemVersionId, "pnlPrinterFriendly", "Visible", this.PortalId);
                if (pfSetting != null)
                {
                    this.pnlPrinterFriendly.Visible = Convert.ToBoolean(pfSetting.PropertyValue, CultureInfo.InvariantCulture);
                }

                // get the pnlEmailAFriend setting
                ItemVersionSetting efSetting = ItemVersionSetting.GetItemVersionSetting(
                    article.ItemVersionId, "pnlEmailAFriend", "Visible", this.PortalId);
                if (efSetting != null)
                {
                    this.pnlEmailAFriend.Visible = Convert.ToBoolean(efSetting.PropertyValue, CultureInfo.InvariantCulture);
                }

                // get the pnlComments setting
                ItemVersionSetting ctSetting = ItemVersionSetting.GetItemVersionSetting(
                    article.ItemVersionId, "pnlComments", "Visible", this.PortalId);
                if (ctSetting != null)
                {
                    this.pnlComments.Visible = Convert.ToBoolean(ctSetting.PropertyValue, CultureInfo.InvariantCulture);
                }

                ////get the upnlRating setting
                // ItemVersionSetting tgSetting = ItemVersionSetting.GetItemVersionSetting(article.ItemVersionId, "pnlTags", "Visible");
                // if (tgSetting != null)
                // {
                // pnlTags.Visible = Convert.ToBoolean(tgSetting.PropertyValue, CultureInfo.InvariantCulture);
                // if (Convert.ToBoolean(tgSetting.PropertyValue, CultureInfo.InvariantCulture))
                // {
                // PopulateTagList();
                // }
                // }
                // else
                // {
                // if (article.Tags.Count > 0)
                // {
                // pnlTags.Visible = true;
                // PopulateTagList();
                // }
                // }
                this.DisplayGalleryIntegration();
                this.DisplayReturnToList(article);
            }
        }

        private void DisplayArticlePaging(Article article)
        {
            // check if we're using paging
            if (this.AllowArticlePaging && (this.PageId > 0))
            {
                this.lblArticleText.Text = article.GetPage(this.PageId).Replace("[PAGE]", string.Empty);

                // lblArticleText.Text = article.GetPage(PageId).Replace("[PAGE]", "");

                // lnkPreviousPage
                if (this.PageId > 1)
                {
                    this.lnkPreviousPage.Text = Localization.GetString("lnkPreviousPage", this.LocalResourceFile);
                    this.lnkPreviousPage.NavigateUrl = UrlGenerator.GetItemLinkUrl(
                        article.ItemId, this.PortalId, this.TabId, this.ModuleId, this.PageId - 1, this.GetCultureName());
                    this.lnkNextPage.Attributes.Add("rel", "prev");
                }

                if (this.PageId < article.GetNumberOfPages)
                {
                    this.lnkNextPage.Text = Localization.GetString("lnkNextPage", this.LocalResourceFile);
                    this.lnkNextPage.NavigateUrl = UrlGenerator.GetItemLinkUrl(
                        article.ItemId, this.PortalId, this.TabId, this.ModuleId, this.PageId + 1, this.GetCultureName());
                    this.lnkNextPage.Attributes.Add("rel", "next");
                }
            }
            else
            {
                this.lblArticleText.Text = article.ArticleText.Replace("[PAGE]", string.Empty);
                this.lnkPreviousPage.Visible = false;
                this.lnkNextPage.Visible = false;
            }
        }

        private void DisplayGalleryIntegration()
        {
            if (this.AllowPhotoGalleryIntegration)
            {
                int? simpleGalleryAlbumId = this.GetSimpleGalleryAlbumId();
                if (simpleGalleryAlbumId.HasValue)
                {
                    this.rpThumbnails.DataSource =
                        SetSimpleGalleryImagePath(DataProvider.Instance().GetSimpleGalleryPhotos(simpleGalleryAlbumId.Value, this.NumberOfThumbnails));
                    this.rpThumbnails.DataBind();
                }
                else
                {
                    int? ultraMediaGalleryAlbumId = this.GetUltraMediaGalleryAlbumId();
                    if (ultraMediaGalleryAlbumId.HasValue)
                    {
                        this.rpThumbnails.DataSource =
                            this.SetUltraMediaGalleryImagePath(
                                DataProvider.Instance().GetUltraMediaGalleryPhotos(ultraMediaGalleryAlbumId.Value, this.NumberOfThumbnails));
                        this.rpThumbnails.DataBind();
                    }
                }
            }
        }

        private void DisplayReturnToList(Article article)
        {
            // lnkReturnToList
            if (article.DisplayReturnToList())
            {
                // check if there's a "list" in session, if so go back to that URL
                if (this.Session["PublishListLink"] != null && Engage.Utility.HasValue(this.Session["PublishListLink"].ToString()))
                {
                    this.lnkReturnToList.NavigateUrl = this.Session["PublishListLink"].ToString().Trim();
                    this.lnkReturnToList.Text = String.Format(
                        CultureInfo.CurrentCulture, Localization.GetString("lnkReturnToList", this.LocalResourceFile), string.Empty);
                }
                else
                {
                    this.pnlReturnToList.Visible = true;

                    int parentItemId = article.GetParentCategoryId();
                    if (parentItemId > 0)
                    {
                        this.lnkReturnToList.NavigateUrl = this.GetItemLinkUrl(parentItemId);

                        // check of the parent category is set to not display on current page, if it isn't, we need to force it to be so here.
                        Category cparent = Category.GetCategory(parentItemId, this.PortalId);

                        this.lnkReturnToList.Text = String.Format(
                            CultureInfo.CurrentCulture, Localization.GetString("lnkReturnToList", this.LocalResourceFile), cparent.Name);
                    }
                    else
                    {
                        this.pnlReturnToList.Visible = false;
                    }
                }
            }
        }

        private int? GetCategoryForumId()
        {
            // TODO: we need to handle items that no longer have a valid parent
            Category pc = Category.GetCategory(Category.GetParentCategory(this.VersionInfoObject.ItemId, this.PortalId), this.PortalId);

            if (pc != null)
            {
                int parentCategoryItemVersionId = pc.ItemVersionId;
                ItemVersionSetting categoryForumSetting = ItemVersionSetting.GetItemVersionSetting(
                    parentCategoryItemVersionId, "CategorySettings", "CommentForumId", this.PortalId);
                int categoryForumId;
                if (categoryForumSetting == null)
                {
                    return null;
                }

                Int32.TryParse(categoryForumSetting.PropertyValue, out categoryForumId);
                return categoryForumId;
            }

            return null;
        }

        private int? GetSimpleGalleryAlbumId()
        {
            ItemVersionSetting simpleGallerySettings = ItemVersionSetting.GetItemVersionSetting(
                this.VersionInfoObject.ItemVersionId, "ddlSimpleGalleryAlbum", "SelectedValue", this.PortalId);
            if (simpleGallerySettings != null && Engage.Utility.HasValue(simpleGallerySettings.PropertyValue))
            {
                int albumId;
                if (int.TryParse(simpleGallerySettings.PropertyValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out albumId))
                {
                    return albumId;
                }
            }

            return null;
        }

        private int? GetUltraMediaGalleryAlbumId()
        {
            ItemVersionSetting ultraMediaGallerySettings = ItemVersionSetting.GetItemVersionSetting(
                this.VersionInfoObject.ItemVersionId, "ddlUltraMediaGalleryAlbum", "SelectedValue", this.PortalId);
            if (ultraMediaGallerySettings != null && Engage.Utility.HasValue(ultraMediaGallerySettings.PropertyValue))
            {
                int albumId;
                if (int.TryParse(ultraMediaGallerySettings.PropertyValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out albumId))
                {
                    return albumId;
                }
            }

            return null;
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
            this.PreRender += this.Page_PreRender;
        }

        private void LoadArticle()
        {
            try
            {
                this.BindItemData();
                this.ConfigureChildControls();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.ConfigureSettings();
                    this.DisplayArticle();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                this.ConfigureComments();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void PopulateTagList()
        {
            var article = (Article)this.VersionInfoObject;
            foreach (ItemTag t in article.Tags)
            {
                var hl = new HyperLink();
                Tag tag = Tag.GetTag(t.TagId, this.PortalId);
                hl.Text = tag.Name;
                hl.NavigateUrl = Globals.NavigateURL(this.DefaultTagDisplayTabId, string.Empty, "&tags=" + tag.Name);
                hl.Attributes.Add("rel", "tag");
                var li = new Literal
                    {
                        Text = ", "
                    };

                this.phTags.Controls.Add(hl);
                this.phTags.Controls.Add(li);
            }

            if (this.phTags.Controls.Count > 1)
            {
                this.phTags.Controls.RemoveAt(this.phTags.Controls.Count - 1);
            }
            else
            {
                this.pnlTags.Visible = false;
            }
        }

        private DataTable SetUltraMediaGalleryImagePath(DataTable photos)
        {
            if (photos != null)
            {
                photos.Columns.Add("ImageThumbPath");
                photos.Columns.Add("ImagePath");

                foreach (DataRow row in photos.Rows)
                {
                    row["ImageThumbPath"] = string.Format(
                        CultureInfo.InvariantCulture, 
                        this.Localize("UltraMediaGalleryThumbPath.Format")
#if DEBUG
                            .Replace("[L]", string.Empty)
#endif
                        , 
                        this.PortalSettings.HomeDirectory, 
                        row["ModuleId"], 
                        row["AlbumId"], 
                        row["Src"]);
                    row["ImagePath"] = string.Format(
                        CultureInfo.InvariantCulture, 
                        this.Localize("UltraMediaGalleryPath.Format")
#if DEBUG
                            .Replace("[L]", string.Empty)
#endif
                        , 
                        this.PortalSettings.HomeDirectory, 
                        row["ModuleId"], 
                        row["AlbumId"], 
                        row["Src"]);
                }
            }

            return photos;
        }
    }
}