//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
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
    using System.Globalization;
    using System.Web.UI;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;
    using Util;
    public partial class CommentDisplay : CommentDisplayBase
    {
        private int _articleId = -1;
  
        public override int ArticleId
        {
            get
            {
                return _articleId;
            }
            set
            {
                _articleId = value;
            }
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
                object o = Settings["adCommentDisplayOption"];
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
                object o = Settings["adRandomComment"];
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

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            LoadArticle();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            Load += Page_Load;
        }

        private void LoadArticle()
        {
            try
            {
                if (_articleId != -1)
                {
                    SetItemId(_articleId);
                }
                else
                {
                    if (ItemVersionId > 0 && _articleId == -1)
                    {
                        VersionInfoObject = Article.GetArticleVersion(ItemVersionId, PortalId);
                    }
                    else
                    {
                        if (ItemId == -1)
                        {
                            VersionInfoObject = ItemVersionId > 0 ? Article.GetArticleVersion(ItemVersionId, PortalId) : Article.Create(PortalId);
                        }
                        else
                        {
                            VersionInfoObject = Article.GetArticle(ItemId, PortalId);
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

                ScriptManager sm = ScriptManager.GetCurrent(Page);

                if (sm != null)
                {
                    sm.RegisterAsyncPostBackControl(dlCommentText);
                }

                if (!Page.IsPostBack)
                {
                    DisplayComment();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void DisplayComment()
        {
            //get cache
            string cacheKey = Utility.CacheKeyPublishArticleComments + ItemId.ToString(CultureInfo.InvariantCulture);
            var comments = DataCache.GetCache(cacheKey) as List<UserFeedback.Comment>;
            if (comments == null)
            {
                comments = Comment.GetCommentsByItemId(ArticleId, ApprovalStatus.Approved.GetId());
                if (comments != null)
                {
                    DataCache.SetCache(cacheKey, comments, DateTime.Now.AddMinutes(CacheTime));
                    Utility.AddCacheKey(cacheKey, PortalId);
                }
            }
            
            if (comments != null && comments.Count > 0)
            {
                lblNoComments.Visible = false;
                divPager.Visible = CommentDisplayOption == CommentDisplayOption.Paging;

                if (DisplayRandomComment)
                {
                    dlCommentText.DataSource = Utility.GetRandomItem(comments);
                    btnPrevious.Visible = false;
                }
                else //if (CommentDisplayOption == CommentDisplayOptions.Paging)
                {
                    //    PagedDataSource pagedComments = new PagedDataSource();
                    //    pagedComments.DataSource = comments;
                    //    pagedComments.PageSize = CommentsPerPage;

                    //    dlCommentText.DataSource = pagedComments;
                    //}
                    //else
                    //{
                    dlCommentText.DataSource = comments;
                    divPager.Visible = false;
                }
                dlCommentText.DataBind();
                if (dlCommentText.Items.Count < 1)
                {
                    divPager.Visible = false;
                }
            }
            else
            {
                divPager.Visible = false;
                lblNoComments.Visible = true;
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
                sName += firstName.ToString();

            if (lastName!= null)
                sName += " " + lastName;

            if (url != null && url.ToString().Trim() != string.Empty)
            {
                sUrl = url.ToString();
                sComment = string.Format(Localization.GetString("CommentNameDateWithLink", LocalResourceFile), sUrl, sName, Convert.ToDateTime(date.ToString()).ToString(LastUpdatedFormat, CultureInfo.CurrentCulture));

            }
            else
            {
                sComment = string.Format(Localization.GetString("CommentNameDateNoLink", LocalResourceFile), sName, Convert.ToDateTime(date.ToString()).ToString(LastUpdatedFormat, CultureInfo.CurrentCulture));
            }

            return sComment;
        }

        private string LastUpdatedFormat
        {
            get
            {
                object o = Settings["adLastUpdatedFormat"];
                return (o == null ? "MMM yyyy" : o.ToString());
            }
        }

       
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (DisplayRandomComment)
            {
                dlCommentText.DataSource = Utility.GetRandomItem(Comment.GetCommentsByItemId(ArticleId, ApprovalStatus.Approved.GetId()));
            }
            dlCommentText.DataBind();
        }
    }
}

