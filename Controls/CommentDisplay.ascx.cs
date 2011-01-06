//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

//TODO: add last updated date

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI;

    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;

    using Engage.Dnn.Publish.Util;
    using Engage.Dnn.UserFeedback;

    using Comment = Engage.Dnn.Publish.Comment;

    public partial class CommentDisplay : CommentDisplayBase
    {
        private int _articleId = -1;

        public override int ArticleId
        {
            get { return this._articleId; }
            set { this._articleId = value; }
        }

        /// <summary>
        /// Gets the comment display option, whether to show all or only some of the comments at a time.
        /// </summary>
        /// <value>
        /// The comment display option.
#pragma warning disable 1574

        /// Defaults to <see cref="CommentDisplayOptions.ShowAll"/> if no setting is defined.
#pragma warning restore 1574

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
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display one comment at a time, in a random order.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if comments should be displayed one-at-a-time in random order; otherwise, <c>false</c>.
        /// </value>
        private bool DisplayRandomComment
        {
            get
            {
                object o = this.Settings["adRandomComment"];
                if (o != null)
                {
                    bool value;
                    if (bool.TryParse(o.ToString(), out value))
                    {
                        return value;
                    }
                }

                return false;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            this.LoadArticle();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        private void LoadArticle()
        {
            try
            {
                if (this._articleId != -1)
                {
                    this.SetItemId(this._articleId);
                }
                else
                {
                    if (this.ItemVersionId > 0 && this._articleId == -1)
                    {
                        this.VersionInfoObject = Article.GetArticleVersion(this.ItemVersionId, this.PortalId);
                    }
                    else
                    {
                        if (this.ItemId == -1)
                        {
                            this.VersionInfoObject = this.ItemVersionId > 0
                                                         ? Article.GetArticleVersion(this.ItemVersionId, this.PortalId)
                                                         : Article.Create(this.PortalId);
                        }
                        else
                        {
                            this.VersionInfoObject = Article.GetArticle(this.ItemId, this.PortalId);
                        }
                    }
                }
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
                ScriptManager sm = ScriptManager.GetCurrent(this.Page);

                if (sm != null)
                {
                    sm.RegisterAsyncPostBackControl(this.dlCommentText);
                }

                if (!this.Page.IsPostBack)
                {
                    this.DisplayComment();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void DisplayComment()
        {
            // get cache
            var cacheKey = Utility.CacheKeyPublishArticleComments + this.ItemId.ToString(CultureInfo.InvariantCulture);
            var comments = Utility.GetValueFromCache(
                this.PortalId, 
                cacheKey, 
                () => Comment.GetCommentsByItemId(this.ArticleId, ApprovalStatus.Approved.GetId()));

            if (comments != null && comments.Count > 0)
            {
                this.lblNoComments.Visible = false;
                this.divPager.Visible = this.CommentDisplayOption == CommentDisplayOption.Paging;

                if (this.DisplayRandomComment)
                {
                    this.dlCommentText.DataSource = Utility.GetRandomItem(comments);
                    this.btnPrevious.Visible = false;
                }
                else
                {
                    // if (CommentDisplayOption == CommentDisplayOptions.Paging)
                    // PagedDataSource pagedComments = new PagedDataSource();
                    // pagedComments.DataSource = comments;
                    // pagedComments.PageSize = CommentsPerPage;

                    // dlCommentText.DataSource = pagedComments;
                    // }
                    // else
                    // {
                    this.dlCommentText.DataSource = comments;
                    this.divPager.Visible = false;
                }

                this.dlCommentText.DataBind();
                if (this.dlCommentText.Items.Count < 1)
                {
                    this.divPager.Visible = false;
                }
            }
            else
            {
                this.divPager.Visible = false;
                this.lblNoComments.Visible = true;
            }
        }

        public string BuildCommentNameDate(object firstName, object lastName, object url, object date)
        {
            string sComment;
            string sUrl = string.Empty;
            if (sUrl == null)
            {
                throw new NotImplementedException();
            }

            string sName = string.Empty;

            if (firstName != null)
            {
                sName += firstName.ToString();
            }

            if (lastName != null)
            {
                sName += " " + lastName;
            }

            if (url != null && url.ToString().Trim() != string.Empty)
            {
                sUrl = url.ToString();
                sComment = string.Format(
                    Localization.GetString("CommentNameDateWithLink", this.LocalResourceFile), 
                    sUrl, 
                    sName, 
                    Convert.ToDateTime(date.ToString()).ToString(this.LastUpdatedFormat, CultureInfo.CurrentCulture));
            }
            else
            {
                sComment = string.Format(
                    Localization.GetString("CommentNameDateNoLink", this.LocalResourceFile), 
                    sName, 
                    Convert.ToDateTime(date.ToString()).ToString(this.LastUpdatedFormat, CultureInfo.CurrentCulture));
            }

            return sComment;
        }

        private string LastUpdatedFormat
        {
            get
            {
                object o = this.Settings["adLastUpdatedFormat"];
                return o == null ? "MMM yyyy" : o.ToString();
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (this.DisplayRandomComment)
            {
                this.dlCommentText.DataSource = Utility.GetRandomItem(Comment.GetCommentsByItemId(this.ArticleId, ApprovalStatus.Approved.GetId()));
            }

            this.dlCommentText.DataBind();
        }
    }
}