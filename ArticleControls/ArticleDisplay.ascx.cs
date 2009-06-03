//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Controls;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Forum;
using Engage.Dnn.Publish.Util;
using DotNetNuke.Services.Mail;

namespace Engage.Dnn.Publish.ArticleControls
{
    using DotNetNuke.Common.Utilities;

    public partial class ArticleDisplay : ModuleBase, IActionable
    {
        private CommentDisplayBase commentDisplay;
        private EmailAFriend ea;
        private PrinterFriendlyButton pf;
        private RelatedArticleLinksBase ral;
        private ArticleDisplay ad;
        private bool displayPrinterFriendly = true;
        private bool displayEmailAFriend = true;
        private bool displayRelatedLinks = true;
        private bool displayRelatedArticle = true;
        private bool displayTitle = true;
        private bool showAuthor;// = false;
        private bool showTags;// = false;

        private const string commentsControlToLoad = "../Controls/CommentDisplay.ascx";
        private const string emailControlToLoad = "../Controls/EmailAFriend.ascx";
        private const string printerControlToLoad = "../Controls/PrinterFriendlyButton.ascx";
        private const string relatedArticlesControlToLoad = "../Controls/RelatedArticleLinks.ascx";
        private const string articleControlToLoad = "articleDisplay.ascx";

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
            LoadArticle();
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
            this.PreRender += this.Page_PreRender;
        }

        public bool DisplayPrinterFriendly
        {
            get { return this.displayPrinterFriendly; }
            set { this.displayPrinterFriendly = value; }
        }

        public bool DisplayRelatedLinks
        {
            get { return this.displayRelatedLinks; }
            set { this.displayRelatedLinks = value; }
        }

        public bool DisplayRelatedArticle
        {
            get { return this.displayRelatedArticle; }
            set { this.displayRelatedArticle = value; }
        }

        public bool DisplayEmailAFriend
        {
            get { return this.displayEmailAFriend; }
            set { this.displayEmailAFriend = value; }
        }

        public bool DisplayTitle
        {
            get { return this.displayTitle; }
            set { this.displayTitle = value; }
        }

        public bool ShowAuthor
        {
            get { return this.showAuthor; }
            set { this.showAuthor = value; }
        }

        public bool ShowTags
        {
            get { return this.showTags; }
            set { this.showTags = value; }
        }


        public bool AllowPhotoGalleryIntegration
        {
            get
            {
                if (AllowSimpleGalleryIntegration || AllowUltraMediaGalleryIntegration)
                {
                    bool value;
                    object o = Settings["adShowPhotoGallery"];
                    if (o != null && bool.TryParse(o.ToString(), out value))
                    {
                        return value;
                    }
                }
                return false;
            }
        }

        private int? NumberOfThumbnails
        {
            get
            {

                int value;
                object o = Settings["adNumberOfThumbnails"];
                if (o != null && int.TryParse(o.ToString(), out value))
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
                object o = Settings["adHoverThumbnailHeight"];
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
                object o = Settings["adHoverThumbnailWidth"];
                int value;
                if (o != null && int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }
                return null;
            }
        }

        protected int? GalleryThumbnailHeight
        {
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

        protected int? GalleryThumbnailWidth
        {
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
                if (AreRatingsEnabled)
                {
                    object o = Settings["adEnableRatings"];
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
                if (IsCommentsEnabled)
                {
                    object o = Settings["adCommentsLink"];
                    return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
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
                if (IsCommentsEnabled)
                {
                    object o = Settings["adCommentsDisplay"];
                    if (o != null)
                    {
                        if (!Convert.ToBoolean(o, CultureInfo.InvariantCulture))
                        {
                            return false;
                        }
                    }
                    //else { o = true; }

                    ItemVersionSetting forumCommentSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "chkForumComments", "Checked", PortalId);
                    if (forumCommentSetting == null)
                    {
                        forumCommentSetting = new ItemVersionSetting();
                        forumCommentSetting.ControlName = "chkForumComments";
                        forumCommentSetting.PropertyName = "Checked";
                        forumCommentSetting.PropertyValue = false.ToString();
                    }

                    return IsPublishCommentType || !Convert.ToBoolean(forumCommentSetting.PropertyValue, CultureInfo.InvariantCulture);
                }
                return false;
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

                if (IsPublishCommentType) return false;
                ItemVersionSetting forumCommentSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "chkForumComments", "Checked", PortalId);
                int? categoryForumId = GetCategoryForumId();
                if (!categoryForumId.HasValue || categoryForumId < 1) return false;
                return (IsCommentsEnabled && !IsPublishCommentType)
                    && (forumCommentSetting != null && Convert.ToBoolean(forumCommentSetting.PropertyValue, CultureInfo.InvariantCulture));
                //{
                //    object o = Settings["adCommentsDisplay"];
                //    return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
                //}
                //return false;
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
                object o = Settings["adFirstNameCollectOption"];
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
                object o = Settings["adLastNameCollectOption"];
                if (o != null && Enum.IsDefined(typeof(NameDisplayOption), o))
                {
                    return (NameDisplayOption)Enum.Parse(typeof(NameDisplayOption), o.ToString());
                }
                return NameDisplayOption.Initial;
            }
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

        private string LastUpdatedFormat
        {
            get
            {
                object o = Settings["adLastUpdatedFormat"];
                return (o == null ? "MMM yyyy" : o.ToString());
            }
        }

        private int? GetCategoryForumId()
        {
            //TODO: we need to handle items that no longer have a valid parent
            Category pc = Category.GetCategory(Category.GetParentCategory(VersionInfoObject.ItemId, PortalId), PortalId);

            if (pc != null)
            {
                int parentCategoryItemVersionId = pc.ItemVersionId;
                ItemVersionSetting categoryForumSetting = ItemVersionSetting.GetItemVersionSetting(parentCategoryItemVersionId, "CategorySettings", "CommentForumId", PortalId);
                int categoryForumId;
                if (categoryForumSetting == null)
                    return null;
                Int32.TryParse(categoryForumSetting.PropertyValue, out categoryForumId);
                return categoryForumId;
            }
            return null;
        }

        private void LoadArticle()
        {
            try
            {
                BindItemData();
                ConfigureChildControls();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        #region Event Handlers

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ConfigureSettings();
                    DisplayArticle();
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
                ConfigureComments();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void ajaxRating_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
        {
            Article article = (Article)VersionInfoObject;
            article.AddRating(int.Parse(e.Value, CultureInfo.InvariantCulture), UserId == -1 ? null : (int?)UserId);
            ajaxRating.ReadOnly = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnSubmitComment_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //TODO: we're allowing anonymous comments, we should have a setting for this.
                DotNetNuke.Security.PortalSecurity objSecurity = new DotNetNuke.Security.PortalSecurity();
                if (UseForumComments)
                {
                    int? categoryForumId = GetCategoryForumId();
                    if (categoryForumId.HasValue)
                    {
                        int threadId = ForumProvider.GetInstance(PortalId).AddComment(categoryForumId.Value, VersionInfoObject.AuthorUserId,
                            VersionInfoObject.Name, VersionInfoObject.Description, GetItemLinkUrl(VersionInfoObject.ItemId, PortalId),
                            objSecurity.InputFilter(txtComment.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting), UserId,
                            Request.UserHostAddress);

                        ItemVersionSetting threadIdSetting = new ItemVersionSetting(Setting.CommentForumThreadId);
                        threadIdSetting.PropertyValue = threadId.ToString(CultureInfo.InvariantCulture);
                        threadIdSetting.ItemVersionId = VersionInfoObject.ItemVersionId;
                        threadIdSetting.Save();
                        //VersionInfoObject.VersionSettings.Add(threadIdSetting);
                        //VersionInfoObject.Save(VersionInfoObject.AuthorUserId);
                        Response.Redirect(ForumProvider.GetInstance(PortalId).GetThreadUrl(threadId), true);
                    }
                }
                else
                {
                    string urlText = txtUrlComment.Text;
                    if (urlText.Trim().Length > 0 && !urlText.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !urlText.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        urlText = "http://" + urlText;
                    }

                    int approvalStatusId = ApprovalStatus.Waiting.GetId();
                    if (IsAdmin)
                    {//automatically approve admin comments
                        approvalStatusId = ApprovalStatus.Approved.GetId();                        
                    }

                    UserFeedback.Comment.AddComment(VersionInfoObject.ItemVersionId, (UserId == -1 ? null : (int?)UserId),
                        objSecurity.InputFilter(txtComment.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting), approvalStatusId,
                        null, objSecurity.InputFilter(txtFirstNameComment.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting),
                        objSecurity.InputFilter(txtLastNameComment.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting),
                        objSecurity.InputFilter(txtEmailAddressComment.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting),
                        objSecurity.InputFilter(urlText, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting),
                        DataProvider.ModuleQualifier);

                    //see if comment notification is turned on. Notify the ItemVersion.Author
                    if (IsCommentAuthorNotificationEnabled)
                    {
                        UserController uc = new UserController();

                        UserInfo ui = uc.GetUser(PortalId, VersionInfoObject.AuthorUserId);

                        if (ui != null)
                        {
                            string emailBody = Localization.GetString("CommentNotificationEmail.Text", LocalResourceFile);
                            emailBody = String.Format(emailBody
                                , VersionInfoObject.Name
                                , this.GetItemLinkUrlExternal(VersionInfoObject.ItemId)
                                , objSecurity.InputFilter(txtFirstNameComment.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting)
                                , objSecurity.InputFilter(txtLastNameComment.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting)
                                , objSecurity.InputFilter(txtEmailAddressComment.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting)
                                , objSecurity.InputFilter(txtComment.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting)

                                );

                            string emailSubject = Localization.GetString("CommentNotificationEmailSubject.Text", LocalResourceFile);
                            emailSubject = String.Format(emailSubject, VersionInfoObject.Name);

                            Mail.SendMail(PortalSettings.Email, ui.Email, string.Empty, emailSubject, emailBody, string.Empty, "HTML", string.Empty, string.Empty, string.Empty, string.Empty);
                        }
                    }

                }
                ConfigureComments();

                pnlCommentEntry.Visible = false;
                pnlCommentConfirmation.Visible = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnCancelComment_Click(object sender, EventArgs e)
        {
            ClearCommentInput();
            mpeComment.Hide();
            mpeForumComment.Hide();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnConfirmationClose_Click(object sender, EventArgs e)
        {
            pnlCommentEntry.Visible = true;
            pnlCommentConfirmation.Visible = false;
            mpeComment.Hide();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Compiler doesn't see validation"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
        protected void rpThumbnails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e != null && (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item))
            {
                AjaxControlToolkit.PopupControlExtender pceArticleThumbnail = e.Item.FindControl("pceArticleThumbnail") as AjaxControlToolkit.PopupControlExtender;
                Panel pnlLargeImage = e.Item.FindControl("pnlLargeImage") as Panel;
                Panel pnlSmallImage = e.Item.FindControl("pnlSmallImage") as Panel;
                HtmlImage imgThumbnail = e.Item.FindControl("imgThumbnail") as HtmlImage;

                if (pceArticleThumbnail != null)
                {
                    pceArticleThumbnail.BehaviorID = pceArticleThumbnail.ClientID;

                    if (pnlLargeImage != null && imgThumbnail != null && pnlSmallImage != null)
                    {
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), imgThumbnail.ClientID, "Engage_ThumbnailHashtable." + imgThumbnail.ClientID + " = ['" + pceArticleThumbnail.ClientID + "', '" + pnlLargeImage.ClientID + "', '" + pnlSmallImage.ClientID + "'];", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), pnlLargeImage.ClientID, "Engage_ThumbnailHashtable." + pnlLargeImage.ClientID + " = Engage_ThumbnailHashtable." + imgThumbnail.ClientID + ";", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), imgThumbnail.ClientID + "Show", "$addHandler($get('" + imgThumbnail.ClientID + "'), 'mouseover', ShowImage);", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), imgThumbnail.ClientID + "Hide", "$addHandler($get('" + imgThumbnail.ClientID + "'), 'mouseout', HideImage);", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), pnlLargeImage.ClientID + "Show", "$addHandler($get('" + pnlLargeImage.ClientID + "'), 'mouseover', ShowImage);", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), pnlLargeImage.ClientID + "Hide", "$addHandler($get('" + pnlLargeImage.ClientID + "'), 'mouseout', HideImage);", true);

                        imgThumbnail.Attributes["onmouseover"] = "ShowImage('" + pceArticleThumbnail.ClientID + "', '" + pnlLargeImage.ClientID + "')";
                        imgThumbnail.Attributes["onmouseout"] = "HideImage('" + pceArticleThumbnail.ClientID + "', '" + pnlLargeImage.ClientID + "', '" + pnlSmallImage.ClientID + "')";
                        imgThumbnail.Attributes["onclick"] = GetPhotoLink(e.Item.DataItem);


                        if (GalleryThumbnailHeight.HasValue)
                        {
                            imgThumbnail.Height = GalleryThumbnailHeight.Value;
                        }
                        if (GalleryThumbnailWidth.HasValue)
                        {
                            imgThumbnail.Width = GalleryThumbnailWidth.Value;
                        }
                    }
                }
            }
        }
        #endregion

        private void ConfigureComments()
        {
            bool showNamePanel = false;
            ClearCommentInput();
            if (mvCommentDisplay.GetActiveView() == vwPublishComments)
            {
                switch (FirstNameCollectOption)
                {
                    case NameDisplayOption.Initial:
                        txtFirstNameComment.MaxLength = 1;
                        txtFirstNameComment.Text = (UserInfo != null && UserInfo.UserID != -1) ? UserInfo.FirstName.Substring(0, 1) : string.Empty;
                        lblFirstNameComment.Text = Localization.GetString("FirstInitial", LocalResourceFile);
                        showNamePanel = true;
                        break;
                    case NameDisplayOption.None:
                        txtFirstNameComment.Visible = false;
                        lblFirstNameComment.Visible = false;
                        rfvFirstNameComment.Enabled = false;
                        //vceFirstNameComment.Enabled = false;
                        break;
                    //case NameDisplayOption.Full:
                    default:
                        txtFirstNameComment.Text = (UserInfo != null && UserInfo.UserID != -1) ? UserInfo.FirstName : string.Empty;
                        lblFirstNameComment.Text = Localization.GetString("FirstName", LocalResourceFile);
                        showNamePanel = true;
                        break;
                }

                switch (LastNameCollectOption)
                {
                    case NameDisplayOption.Initial:
                        txtLastNameComment.MaxLength = 1;
                        txtLastNameComment.Text = (UserInfo != null && UserInfo.UserID != -1) ? UserInfo.LastName.Substring(0, 1) : string.Empty;
                        lblLastNameComment.Text = Localization.GetString("LastInitial", LocalResourceFile);
                        showNamePanel = true;
                        break;
                    case NameDisplayOption.None:
                        txtLastNameComment.Visible = false;
                        lblLastNameComment.Visible = false;
                        rfvLastNameComment.Enabled = false;
                        //vceLastNameComment.Enabled = false;
                        break;
                    //case NameDisplayOption.Full:
                    default:
                        txtLastNameComment.Text = (UserInfo != null && UserInfo.UserID != -1) ? UserInfo.LastName : string.Empty;
                        lblLastNameComment.Text = Localization.GetString("LastName", LocalResourceFile);
                        showNamePanel = true;
                        break;
                }

                pnlEmailAddressComment.Visible = rfvEmailAddressComment.Enabled = CollectEmailAddress;
                pnlNameComment.Visible = showNamePanel;
                pnlUrlComment.Visible = CollectUrl;

                if (pnlEmailAddressComment.Visible)
                {
                    txtEmailAddressComment.Text = (UserInfo != null && UserInfo.UserID != -1) ? UserInfo.Email : string.Empty;
                }
                if (pnlUrlComment.Visible)
                {
                    txtUrlComment.Text = (UserInfo != null && UserInfo.UserID != -1) ? UserInfo.Profile.Website : string.Empty;
                }
            }
            else
            {
                pnlNameComment.Visible = rfvFirstNameComment.Enabled = rfvLastNameComment.Enabled = false;
                pnlEmailAddressComment.Visible = rfvEmailAddressComment.Enabled = false;
                pnlUrlComment.Visible = false;
            }
        }

        private void ConfigureChildControls()
        {
            if (VersionInfoObject.IsNew) return;

            //check if items are enabled.
            if (displayEmailAFriend && VersionInfoObject.IsNew == false)
            {
                ea = (EmailAFriend)LoadControl(emailControlToLoad);
                ea.ModuleConfiguration = ModuleConfiguration;
                ea.ID = Path.GetFileNameWithoutExtension(emailControlToLoad);
                this.phEmailAFriend.Controls.Add(ea);
            }
            if (displayPrinterFriendly && VersionInfoObject.IsNew == false)
            {
                pf = (PrinterFriendlyButton)LoadControl(printerControlToLoad);
                pf.ModuleConfiguration = ModuleConfiguration;
                pf.ID = Path.GetFileNameWithoutExtension(printerControlToLoad);
                this.phPrinterFriendly.Controls.Add(pf);
            }

            if (displayRelatedLinks)
            {
                ral = (RelatedArticleLinksBase)LoadControl(relatedArticlesControlToLoad);
                ral.ModuleConfiguration = ModuleConfiguration;
                ral.ID = Path.GetFileNameWithoutExtension(relatedArticlesControlToLoad);
                this.phRelatedArticles.Controls.Add(ral);
            }

            if (displayRelatedArticle)
            {
                Article a = VersionInfoObject.GetRelatedArticle(PortalId);
                if (a != null)
                {
                    ad = (ArticleDisplay)LoadControl(articleControlToLoad);
                    ad.ModuleConfiguration = ModuleConfiguration;
                    ad.ID = Path.GetFileNameWithoutExtension(articleControlToLoad);
                    ad.Overrideable = false;
                    ad.UseCache = true;
                    ad.DisplayPrinterFriendly = false;
                    ad.DisplayRelatedArticle = false;
                    ad.DisplayRelatedLinks = false;
                    ad.DisplayEmailAFriend = false;



                    this.ad.SetItemId(a.ItemId);
                    ad.DisplayTitle = false;
                    this.phRelatedArticle.Controls.Add(ad);
                    divRelatedArticle.Visible = true;
                }
                else
                {
                    divRelatedArticle.Visible = false;
                }
            }

            if (RatingDisplayOption.Equals(RatingDisplayOption.Enable) || RatingDisplayOption.Equals(RatingDisplayOption.ReadOnly))
            {
                //get the upnlRating setting
                ItemVersionSetting rtSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "upnlRating", "Visible", PortalId);
                if (rtSetting != null)
                {

                    upnlRating.Visible = Convert.ToBoolean(rtSetting.PropertyValue, CultureInfo.InvariantCulture);
                }
                if (upnlRating.Visible)
                {
                    lblRatingMessage.Visible = true;
                    ajaxRating.MaxRating = MaximumRating;

                    int avgRating = (int)Math.Round(((Article)VersionInfoObject).AverageRating);
                    ajaxRating.CurrentRating = (avgRating > MaximumRating ? MaximumRating : (avgRating < 0 ? 0 : avgRating));

                    ajaxRating.ReadOnly = RatingDisplayOption.Equals(RatingDisplayOption.ReadOnly);
                }
            }

            btnComment.Visible = DisplayCommentsLink;
            if (IsCommentsEnabled)
            {
                if (!UseForumComments || (DisplayPublishComments && !VersionInfoObject.IsNew))
                {
                    pnlComments.Visible = pnlCommentDisplay.Visible = true;
                    commentDisplay = (CommentDisplayBase)LoadControl(commentsControlToLoad);
                    commentDisplay.ModuleConfiguration = ModuleConfiguration;
                    commentDisplay.ID = Path.GetFileNameWithoutExtension(commentsControlToLoad);
                    commentDisplay.ArticleId = VersionInfoObject.ItemId;
                    this.phCommentsDisplay.Controls.Add(commentDisplay);
                }

                if (UseForumComments)
                {
                    pnlComments.Visible = true;
                    mvCommentDisplay.SetActiveView(vwForumComments);
                    ItemVersionSetting forumThreadIdSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "ArticleSetting", "CommentForumThreadId", PortalId);
                    if (forumThreadIdSetting != null)
                    {
                        lnkGoToForum.Visible = true;
                        lnkGoToForum.NavigateUrl = ForumProvider.GetInstance(PortalId).GetThreadUrl(Convert.ToInt32(forumThreadIdSetting.PropertyValue, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        btnForumComment.Visible = true;
                    }
                }
            }
            ConfigureTags();
        }


        private void ConfigureTags()
        {
            //get the upnlRating setting
            ItemVersionSetting tgSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "pnlTags", "Visible", PortalId);
            if (tgSetting != null)
            {
                pnlTags.Visible = Convert.ToBoolean(tgSetting.PropertyValue, CultureInfo.InvariantCulture);
                if (Convert.ToBoolean(tgSetting.PropertyValue, CultureInfo.InvariantCulture))
                {
                    PopulateTagList();
                }
            }
            else
            {
                if (VersionInfoObject.Tags.Count > 0)
                {
                    pnlTags.Visible = true;
                    PopulateTagList();
                }
            }
        }


        private void ConfigureSettings()
        {
            // LogBreadcrumb is true by default.  Check to see if we need to turn it off.
            object o = Settings["LogBreadCrumb"];
            if (o != null)
            {
                bool logBreadCrumb;
                if (bool.TryParse(o.ToString(), out logBreadCrumb))
                {
                    this.LogBreadcrumb = logBreadCrumb;
                }
            }
        }

        private void DisplayArticle()
        {
            if (VersionInfoObject.IsNew)
            {
                if (IsAdmin || IsAuthor)
                {
                    //Default the text to no approved version. if the module isn't configured or no Categories/Articles exist yet then it will be overwritten.
                    lblArticleText.Text = Localization.GetString("NoApprovedVersion", LocalResourceFile);

                    //Check to see if there are Categories defined. If none are defined this is the first
                    //instance of the Module so we need to notify the user to create categories and articles.
                    int categoryCount = DataProvider.Instance().GetCategories(PortalId).Rows.Count;
                    if (categoryCount == 0)
                    {
                        lblArticleText.Text = Localization.GetString("NoDataToDisplay", LocalResourceFile);
                    }
                    else if (IsConfigured == false)
                    {
                        lnkConfigure.Text = Localization.GetString("UnableToFindAction", LocalResourceFile);
                        lnkConfigure.NavigateUrl = EditUrl("ModuleId", ModuleId.ToString(CultureInfo.InvariantCulture), "Module");
                        lnkConfigure.Visible = true;
                        lblArticleText.Text = Localization.GetString("UnableToFind", LocalResourceFile);
                    }
                }
                return;
            }


            if (Item.GetItemType(VersionInfoObject.ItemId, PortalId) == "Article")
            {
                UseCache = true;

                Article article = (Article)VersionInfoObject;
                if (displayTitle)
                {
                    SetPageTitle();
                    lblArticleTitle.Text = article.Name;
                    divArticleTitle.Visible = true;
                    divLastUpdated.Visible = true;
                }

                article.ArticleText = Utility.ReplaceTokens(article.ArticleText);
                DisplayArticlePaging(article);

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
                article.AddView(UserId, TabId, HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserAgent, referrer, url);

                DateTime lastUpdated = Convert.ToDateTime(article.LastUpdated, CultureInfo.InvariantCulture);

                lblLastUpdated.Text = Localization.GetString("LastUpdated", LocalResourceFile) + " " + lastUpdated.ToString(LastUpdatedFormat, CultureInfo.CurrentCulture);

                //get the pnlAuthor setting
                ItemVersionSetting auSetting = ItemVersionSetting.GetItemVersionSetting(article.ItemVersionId, "pnlAuthor", "Visible", PortalId);
                if (auSetting != null)
                {
                    ShowAuthor = Convert.ToBoolean(auSetting.PropertyValue, CultureInfo.InvariantCulture);
                }

                if (ShowAuthor)
                {
                    pnlAuthor.Visible = true;
                    lblAuthor.Text = article.Author;
                    if (lblAuthor.Text.Trim().Length < 1)
                    {
                        UserController uc = new UserController();
                        UserInfo ui = uc.GetUser(PortalId, article.AuthorUserId);
                        lblAuthor.Text = ui.DisplayName;
                    }

                    if (lblAuthor.Text.Trim().Length < 1)
                    {
                        pnlAuthor.Visible = false;
                    }
                }
                else
                {
                    pnlAuthor.Visible = false;
                }

                //get the pnlPrinterFriendly setting
                ItemVersionSetting pfSetting = ItemVersionSetting.GetItemVersionSetting(article.ItemVersionId, "pnlPrinterFriendly", "Visible", PortalId);
                if (pfSetting != null)
                {
                    pnlPrinterFriendly.Visible = Convert.ToBoolean(pfSetting.PropertyValue, CultureInfo.InvariantCulture);
                }

                //get the pnlEmailAFriend setting
                ItemVersionSetting efSetting = ItemVersionSetting.GetItemVersionSetting(article.ItemVersionId, "pnlEmailAFriend", "Visible", PortalId);
                if (efSetting != null)
                {
                    pnlEmailAFriend.Visible = Convert.ToBoolean(efSetting.PropertyValue, CultureInfo.InvariantCulture);
                }

                //get the pnlComments setting
                ItemVersionSetting ctSetting = ItemVersionSetting.GetItemVersionSetting(article.ItemVersionId, "pnlComments", "Visible", PortalId);
                if (ctSetting != null)
                {
                    pnlComments.Visible = Convert.ToBoolean(ctSetting.PropertyValue, CultureInfo.InvariantCulture);
                }


                ////get the upnlRating setting
                //ItemVersionSetting tgSetting = ItemVersionSetting.GetItemVersionSetting(article.ItemVersionId, "pnlTags", "Visible");
                //if (tgSetting != null)
                //{
                //    pnlTags.Visible = Convert.ToBoolean(tgSetting.PropertyValue, CultureInfo.InvariantCulture);
                //    if (Convert.ToBoolean(tgSetting.PropertyValue, CultureInfo.InvariantCulture))
                //    {
                //        PopulateTagList();
                //    }
                //}
                //else
                //{
                //    if (article.Tags.Count > 0)
                //    {
                //        pnlTags.Visible = true;
                //        PopulateTagList();
                //    }
                //}

                DisplayGalleryIntegration();
                DisplayReturnToList(article);
            }
        }

        private void PopulateTagList()
        {
            Article article = (Article)VersionInfoObject;
            foreach (ItemTag t in article.Tags)
            {
                HyperLink hl = new HyperLink();
                Tag tag = Tag.GetTag(t.TagId, PortalId);
                hl.Text = tag.Name;
                hl.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(DefaultTagDisplayTabId, string.Empty, "&tags=" + tag.Name);
                hl.Attributes.Add("rel", "tag");
                Literal li = new Literal();
                li.Text = ", ";

                phTags.Controls.Add(hl);
                phTags.Controls.Add(li);
            }
            if (phTags.Controls.Count > 1)
            {
                phTags.Controls.RemoveAt(phTags.Controls.Count - 1);
            }
            else
            {
                pnlTags.Visible = false;
            }
        }

        //private static void ReplaceTokens(Article a)
        //{
        //    //TODO: change : to | (pipe) sometime
        //    int youTubeLocation = a.ArticleText.ToUpperInvariant().IndexOf("[YOUTUBE:", StringComparison.Ordinal);
        //    if (youTubeLocation >= 0)
        //    {
        //        string afterToken = a.ArticleText.Substring(youTubeLocation + 9);
        //        int youTubeFinishLocation = afterToken.IndexOf("]", StringComparison.Ordinal);
        //        string youTubeId = afterToken.Substring(0, youTubeFinishLocation);

        //        string youTubeEmbed = "<span class=\"Publish_Video\"><object width=\"425\" height=\"355\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + youTubeId + "\"></param><param name=\"wmode\" value=\"transparent\"></param><embed src=\"http://www.youtube.com/v/" + youTubeId + "\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"425\" height=\"355\"></embed></object></span>";
        //        string fullArticleText = a.ArticleText.Substring(0, youTubeLocation);
        //        fullArticleText += youTubeEmbed;
        //        fullArticleText += a.ArticleText.Substring(youTubeLocation + youTubeId.Length + youTubeFinishLocation - 1);
        //        a.ArticleText = fullArticleText;
        //        ReplaceTokens(a);
        //    }
        //    //flickr
        //    int flickrLocation = a.ArticleText.ToUpperInvariant().IndexOf("[FLICKR|", StringComparison.Ordinal);
        //    if (flickrLocation >= 0)
        //    {

        //        string afterToken = a.ArticleText.Substring(flickrLocation + 8);
        //        int flickrFinishLocation = afterToken.IndexOf("]", StringComparison.Ordinal);
        //        string flickrId = afterToken.Substring(0, flickrFinishLocation);

        //        string flickrEmbed = string.Format(CultureInfo.InvariantCulture, "<object type=\"application/x-shockwave-flash\" width=\"499\" height=\"333\" data=\"http://www.flickr.com/apps/video/stewart.swf?v=1.173\" classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\"> <param name=\"flashvars\" value=\"intl_lang=en-us&amp;photo_secret=d70f929acb&amp;photo_id={0}&amp;show_info_box=true\"></param> <param name=\"movie\" value=\"http://www.flickr.com/apps/video/stewart.swf?v=1.173\"></param> <param name=\"bgcolor\" value=\"#000000\"></param> <param name=\"allowFullScreen\" value=\"true\"></param><embed type=\"application/x-shockwave-flash\" src=\"http://www.flickr.com/apps/video/stewart.swf?v=1.173\" bgcolor=\"#000000\" allowfullscreen=\"true\" flashvars=\"intl_lang=en-us&amp;photo_secret=d70f929acb&amp;photo_id={0}&amp;flickr_show_info_box=true\" height=\"333\" width=\"499\"></embed></object>", flickrId);
        //        string fullArticleText = a.ArticleText.Substring(0, flickrLocation);
        //        fullArticleText += flickrEmbed;
        //        fullArticleText += a.ArticleText.Substring(flickrLocation + flickrId.Length + flickrFinishLocation - 1);
        //        a.ArticleText = fullArticleText;
        //        ReplaceTokens(a);
        //    }
        //}

        private void DisplayArticlePaging(Article article)
        {
            //check if we're using paging
            if (this.AllowArticlePaging && (this.PageId > 0))
            {
                this.lblArticleText.Text = article.GetPage(this.PageId).Replace("[PAGE]", string.Empty);

                //lblArticleText.Text = article.GetPage(PageId).Replace("[PAGE]", "");

                //lnkPreviousPage

                if (this.PageId > 1)
                {
                    this.lnkPreviousPage.Text = Localization.GetString("lnkPreviousPage", this.LocalResourceFile);
                    this.lnkPreviousPage.NavigateUrl = Utility.GetItemLinkUrl(article.ItemId, this.PortalId, this.TabId, this.ModuleId, this.PageId - 1, this.GetCultureName());
                    this.lnkNextPage.Attributes.Add("rel", "prev");
                }

                if (this.PageId < article.GetNumberOfPages)
                {
                    this.lnkNextPage.Text = Localization.GetString("lnkNextPage", this.LocalResourceFile);
                    this.lnkNextPage.NavigateUrl = Utility.GetItemLinkUrl(article.ItemId, this.PortalId, this.TabId, this.ModuleId, this.PageId + 1, this.GetCultureName());
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
            if (AllowPhotoGalleryIntegration)
            {
                int? simpleGalleryAlbumId = GetSimpleGalleryAlbumId();
                if (simpleGalleryAlbumId.HasValue)
                {
                    rpThumbnails.DataSource = SetSimpleGalleryImagePath(DataProvider.Instance().GetSimpleGalleryPhotos(simpleGalleryAlbumId.Value, NumberOfThumbnails));
                    rpThumbnails.DataBind();
                }
                else
                {
                    int? ultraMediaGalleryAlbumId = GetUltraMediaGalleryAlbumId();
                    if (ultraMediaGalleryAlbumId.HasValue)
                    {
                        rpThumbnails.DataSource = SetUltraMediaGalleryImagePath(DataProvider.Instance().GetUltraMediaGalleryPhotos(ultraMediaGalleryAlbumId.Value, NumberOfThumbnails));
                        rpThumbnails.DataBind();
                    }
                }
            }
        }

        private void DisplayReturnToList(Article article)
        {
            //lnkReturnToList
            if (article.DisplayReturnToList())
            {
                //check if there's a "list" in session, if so go back to that URL
                if (Session["PublishListLink"] != null && Utility.HasValue(Session["PublishListLink"].ToString()))
                {
                    lnkReturnToList.NavigateUrl = Session["PublishListLink"].ToString().Trim();
                    lnkReturnToList.Text = String.Format(CultureInfo.CurrentCulture, Localization.GetString("lnkReturnToList", LocalResourceFile), string.Empty);
                }
                else
                {
                    pnlReturnToList.Visible = true;

                    int parentItemId = article.GetParentCategoryId();
                    if (parentItemId > 0)
                    {
                        lnkReturnToList.NavigateUrl = GetItemLinkUrl(parentItemId);

                        //check of the parent category is set to not display on current page, if it isn't, we need to force it to be so here.
                        Category cparent = Category.GetCategory(parentItemId, PortalId);


                        lnkReturnToList.Text = String.Format(CultureInfo.CurrentCulture, Localization.GetString("lnkReturnToList", LocalResourceFile), cparent.Name);
                    }
                    else
                    {
                        pnlReturnToList.Visible = false;
                    }
                }
            }
        }

        private void ClearCommentInput()
        {
            txtComment.Text = string.Empty;
            txtFirstNameComment.Text = string.Empty;
            txtLastNameComment.Text = string.Empty;
            txtEmailAddressComment.Text = string.Empty;
        }

        #region Photo Gallery Helper Functions
        protected string GetPhotoPath(object dataItem)
        {
            DataRowView row = dataItem as DataRowView;
            if (row != null)
            {
                //SimpleGallery has HomeDirectory column, otherwise just point to the image path.
                if (row.DataView.Table.Columns.Contains("HomeDirectory"))
                {
                    return ResolveUrl("~/DesktopModules/SimpleGallery/ImageHandler.ashx?width=" + row["Width"] + "&height=" + row["Height"] + "&HomeDirectory=" + Server.UrlEncode(PortalSettings.HomeDirectory + row["HomeDirectory"]) + "&fileName=" + this.Server.UrlEncode(row["FileName"].ToString()) + "&portalid=" + PortalId.ToString(CultureInfo.InvariantCulture) + "&i=" + row["PhotoID"] + "&q=1");
                }
                return row["ImagePath"].ToString();
            }
            return string.Empty;
        }

        protected string GetThumbPhotoPath(object dataItem)
        {
            DataRowView row = dataItem as DataRowView;
            if (row != null)
            {
                //SimpleGallery has HomeDirectory column, otherwise just point to the image path.
                if (row.DataView.Table.Columns.Contains("HomeDirectory"))
                {
                    return ResolveUrl("~/DesktopModules/SimpleGallery/ImageHandler.ashx?width=" + GetPhotoWidth(row).ToString(CultureInfo.InvariantCulture) + "&height=" + GetPhotoHeight(row).ToString(CultureInfo.InvariantCulture) + "&HomeDirectory=" + Server.UrlEncode(PortalSettings.HomeDirectory + row["HomeDirectory"]) + "&fileName=" + this.Server.UrlEncode(row["FileName"].ToString()) + "&portalid=" + PortalId.ToString(CultureInfo.InvariantCulture) + "&i=" + row["PhotoID"] + "&q=1");
                }
                return row["ImageThumbPath"].ToString();
            }
            return string.Empty;
        }

        protected static string GetPhotoAltText(object dataItem)
        {
            //string clickToView = Localization.GetString("ClickToView", LocalResourceFile);

            DataRowView row = dataItem as DataRowView;
            if (row != null)
            {
                //SimpleGallery uses Name, UltraMedia uses Title
                if (row.DataView.Table.Columns.Contains("Name"))
                {
                    return row["Name"].ToString();
                }
                return row["Title"].ToString();
            }
            return string.Empty;
        }

        protected string PhotoMouseOverText
        {
            get
            {
                return Localization.GetString("PhotoMouseover", LocalResourceFile);
            }
        }

        private static int GetPhotoWidth(object dataItem)
        {
            DataRowView row = dataItem as DataRowView;
            if (row != null)
            {
                if (row.DataView.Table.Columns.Contains("TnWidth"))
                {
                    //if there is a TnWidth column, it's been calculated by Ultra Media Gallery, just return it.
                    object o = row["TnWidth"];
                    if (o != null)
                    {
                        return int.Parse(o.ToString(), CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    int actualHeight = (int)row["Height"];
                    int actualWidth = (int)row["Width"];
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

        private static int GetPhotoHeight(object dataItem)
        {
            DataRowView row = dataItem as DataRowView;
            if (row != null)
            {
                if (row.DataView.Table.Columns.Contains("TnHeight"))
                {

                    //if there is a TnHeight column, it's been calculated by Ultra Media Gallery, just return it.
                    object o = row["TnHeight"];
                    if (o != null)
                    {
                        return int.Parse(o.ToString(), CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    int actualHeight = (int)row["Height"];
                    int actualWidth = (int)row["Width"];
                    int maxThumbnailWidth = GetMaxThumbnailWidth((int)row["ModuleId"]);
                    int maxThumbnailHeight = GetMaxThumbnailHeight((int)row["ModuleId"]);
                    int thumbnailWidth = actualWidth > maxThumbnailWidth ? maxThumbnailWidth : actualWidth;
                    int thumbnailHeight = Convert.ToInt32(actualHeight / ((double)actualWidth / thumbnailWidth));
                    if (thumbnailHeight > maxThumbnailHeight)
                    {
                        thumbnailHeight = maxThumbnailHeight;
                        //thumbnailWidth = Convert.ToInt32(actualWidth / ((double)actualHeight / thumbnailHeight));
                    }
                    return thumbnailHeight;
                }
            }
            return 125;
        }

        protected string GetPhotoLink(object dataItem)
        {
            DataRowView row = dataItem as DataRowView;
            if (row != null)
            {
                int moduleId = (int)row["ModuleId"];
                int? albumId = GetUltraMediaGalleryAlbumId();

                if (albumId.HasValue) //ultra media
                {
                    string popupUrl = "http://" + PortalSettings.PortalAlias.HTTPAlias + "/DesktopModules/BizModules - UltraPhotoGallery/Popup.aspx?ModuleId=" + moduleId.ToString(CultureInfo.InvariantCulture) + "&portalId=" + PortalId.ToString(CultureInfo.InvariantCulture) + "&AlbumId=" + albumId.Value.ToString(CultureInfo.InvariantCulture);
                    return "window.open('" + popupUrl + "','UPG_POPUP','location=no,status=no,scrollbars=no,toolbar=no,menubar=no,directories=no,resizable=yes,width=" + GetPopupWidth(moduleId).ToString(CultureInfo.InvariantCulture) + ",height=" + GetPopupHeight(moduleId).ToString(CultureInfo.InvariantCulture) + "')";
                }
                else //simple gallery
                {
                    string popupUrl = ResolveUrl("~/DesktopModules/SimpleGallery/SlideShowPopup.aspx?PortalID=" + PortalId.ToString(CultureInfo.InvariantCulture) + "&TagID=" + Null.NullInteger.ToString(CultureInfo.InvariantCulture) + "&ItemID=" + row["PhotoID"] + "&Border=" + GetBorderStyle(moduleId) + "&sb=" + GetSortBy(moduleId) + "&sd=" + GetSortDirection(moduleId) + "&tt=" + GetToolTipSetting(moduleId));
                    return "window.open('" + popupUrl + "','smallscreen','location=no,status=no,scrollbars=no,toolbar=no,menubar=no,directories=no,resizable=yes,width=" + GetPopupWidth(moduleId).ToString(CultureInfo.InvariantCulture) + ",height=" + GetPopupHeight(moduleId).ToString(CultureInfo.InvariantCulture) + "')";
                }
            }
            return string.Empty;
        }

        private int? GetSimpleGalleryAlbumId()
        {
            ItemVersionSetting simpleGallerySettings = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "ddlSimpleGalleryAlbum", "SelectedValue", PortalId);
            if (simpleGallerySettings != null && Utility.HasValue(simpleGallerySettings.PropertyValue))
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
            ItemVersionSetting ultraMediaGallerySettings = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "ddlUltraMediaGalleryAlbum", "SelectedValue", PortalId);
            if (ultraMediaGallerySettings != null && Utility.HasValue(ultraMediaGallerySettings.PropertyValue))
            {
                int albumId;
                if (int.TryParse(ultraMediaGallerySettings.PropertyValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out albumId))
                {
                    return albumId;
                }
            }
            return null;
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

        private DataTable SetUltraMediaGalleryImagePath(DataTable photos)
        {
            if (photos != null)
            {
                photos.Columns.Add("ImageThumbPath");
                photos.Columns.Add("ImagePath");

                foreach (DataRow row in photos.Rows)
                {
                    row["ImageThumbPath"] = PortalSettings.HomeDirectory + "UltraPhotoGallery/" + row["ModuleId"] + "/" + row["AlbumId"] + "/" + "thumbs" + "/" + row["Src"];
                    row["ImagePath"] = PortalSettings.HomeDirectory + "UltraPhotoGallery/" + row["ModuleId"] + "/" + row["AlbumId"] + "/" + "large" + "/" + row["Src"];
                }
            }
            return photos;
        }

        private static int GetMaxThumbnailHeight(int moduleId)
        {
            return Utility.GetTabModuleSettingAsInt(moduleId, "ThumbnailHeight", 125);
        }

        private static int GetMaxThumbnailWidth(int moduleId)
        {
            return Utility.GetTabModuleSettingAsInt(moduleId, "ThumbnailWidth", 125);
        }

        private static int GetPopupWidth(int moduleId)
        {
            int width;
            //UltraMediaGallery
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

        private static int GetPopupHeight(int moduleId)
        {
            int height;
            //UltraMediaGallery
            object heightObj = (new ModuleController()).GetModuleSettings(moduleId)["GalleryHeight"];
            if (heightObj != null && int.TryParse(heightObj.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out height))
            {
                return height;
            }
            //SimpleGallery
            heightObj = (new ModuleController()).GetModuleSettings(moduleId)["PopupHeight"];
            if (heightObj != null && int.TryParse(heightObj.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out height))
            {
                return height;
            }
            return 600;
        }

        private static string GetSortBy(int moduleId)
        {
            string defaultValue = "Name";
            List<int> version = null;
            try
            {
                version = Utility.ParseIntegerList(DataProvider.Instance().GetSimpleGalleryVersion().Split(new char[] { '.' }));
            }
            //if we can't get the version, just swallow the exception. BD
            catch (FormatException) { }

            if (version != null)
            {
                if (Utility.IsVersionGreaterOrEqual(version, new List<int>(new int[] { 2, 2, 0 })))
                {
                    defaultValue = "0";//0=name, 3=fileName, 1=dateCreated, 2=dateApproved
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
                version = Utility.ParseIntegerList(DataProvider.Instance().GetSimpleGalleryVersion().Split(new char[] { '.' }));
            }
            //if we can't get the version, just swallow the exception. BD
            catch (FormatException) { }

            if (version != null)
            {
                if (Utility.IsVersionGreaterOrEqual(version, new List<int>(new int[] { 2, 2, 0 })))
                {
                    defaultValue = "1";//0=desc, 1=asc
                }
            }
            return Utility.GetTabModuleSettingAsString(moduleId, "SortDirection", defaultValue);
        }

        private static string GetToolTipSetting(int moduleId)
        {
            return Utility.GetTabModuleSettingAsString(moduleId, "EnableTooltip", true.ToString(CultureInfo.InvariantCulture));
        }

        private static string GetBorderStyle(int moduleId)
        {
            return Utility.GetTabModuleSettingAsString(moduleId, "BorderStyle", "White");
        }
        #endregion

        #region Optional Interfaces

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                DotNetNuke.Entities.Modules.Actions.ModuleActionCollection actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString("Settings", LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, string.Empty, string.Empty, EditUrl("Settings"), false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
                return actions;
            }
        }


        #endregion
    }
}

