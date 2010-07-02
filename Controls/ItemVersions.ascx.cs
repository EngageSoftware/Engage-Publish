//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class ItemVersions : ModuleBase, IActionable
    {
        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), 
                            ModuleActionType.AddContent, string.Empty, string.Empty, string.Empty, false, SecurityAccessLevel.Edit, true, false
                            }
                    };
            }
        }

        protected static string GetDates(object row)
        {
            if (row != null)
            {
                var view = (DataRowView)row;

                object start = view["StartDate"];
                string dates = start is DBNull ? string.Empty : Convert.ToDateTime(start, CultureInfo.CurrentCulture).ToShortDateString() + "-";
                object end = view["EndDate"];
                dates += end is DBNull ? string.Empty : Convert.ToDateTime(end, CultureInfo.CurrentCulture).ToShortDateString();

                return dates;
            }

            return string.Empty;
        }

        protected static string GetStatus(object approvalStatusId)
        {
            string status = ApprovalStatus.Waiting.Name;
            int statusId = Convert.ToInt32(approvalStatusId, CultureInfo.InvariantCulture);

            if (statusId == ApprovalStatus.Approved.GetId())
            {
                status = ApprovalStatus.Approved.Name;
            }
            else if (statusId == ApprovalStatus.Edit.GetId())
            {
                status = ApprovalStatus.Edit.Name;
            }
            else if (statusId == ApprovalStatus.Archived.GetId())
            {
                status = ApprovalStatus.Archived.Name;
            }

            return status;
        }

        protected string GetAuthorName(object authorId)
        {
            if (authorId != null)
            {
                string author = string.Empty;
                var uc = new UserController();
                UserInfo ui = uc.GetUser(this.PortalId, Convert.ToInt32(authorId, CultureInfo.InvariantCulture));
                if (ui != null)
                {
                    author = authorId is DBNull ? string.Empty : ui.Username;
                }

                return author;
            }

            return string.Empty;
        }

        protected string GetItemDescription(object row)
        {
            if (row != null)
            {
                var view = (DataRowView)row;
                string description;
                if (this.TypeOfItem == ItemType.Article)
                {
                    Article a = Article.GetArticleVersion(Convert.ToInt32(view["ItemVersionId"], CultureInfo.InvariantCulture), this.PortalId);
                    description = HtmlUtils.Shorten(HtmlUtils.Clean(a.VersionDescription, true), 200, string.Empty) + "&nbsp;";
                }
                else
                {
                    description = HtmlUtils.Shorten(HtmlUtils.Clean(view["Description"].ToString(), true), 200, string.Empty) + "&nbsp;";
                }

                return description;
            }

            return string.Empty;
        }

        protected string GetItemDescriptionFull(object row)
        {
            if (row != null)
            {
                var view = (DataRowView)row;
                string description;
                if (this.TypeOfItem == ItemType.Article)
                {
                    Article a = Article.GetArticleVersion(Convert.ToInt32(view["ItemVersionId"], CultureInfo.InvariantCulture), this.PortalId);
                    description = a.VersionDescription;
                }
                else
                {
                    description = view["Description"].ToString();
                }

                if (!string.IsNullOrEmpty(description))
                {
                    return description + "...";
                }
            }

            return Localization.GetString("NoDescription", this.LocalResourceFile);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does not return class state information")]
        protected string GetLocalizedEditText()
        {
            return Localization.GetString("Edit", this.LocalResourceFile);
        }

        protected string GetPreviewUrl(object row)
        {
            if (row != null)
            {
                var view = (DataRowView)row;

                // call GetItemLinkUrl() and it will figure out where to send the user AND if the display 
                // page isn't set it will automatically appear in our ItemPreview.ascx control.
                // int itemVersionId = Convert.ToInt32(view["ItemVersionId"]);
                // string href = GetItemLinkUrl(itemVersionId);

                ////For now, we can test the version to see if the item is linkable. hk
                // int itemVersionId = 
                // int displayTabId = Convert.ToInt32(view["DisplayTabId"], CultureInfo.InvariantCulture);
                string href = this.GetItemVersionLinkUrl(view["ItemVersionId"]);

                return href;
            }

            return string.Empty;
        }

        protected string GetVersionEditUrl(object row)
        {
            var view = (DataRowView)row;
            var qsp = new QueryStringParameters();

            qsp.ClearKeys();
            qsp.Add("ctl", Utility.AdminContainer);
            qsp.Add("mid", this.ModuleId.ToString(CultureInfo.InvariantCulture));
            qsp.Add("adminType", view["adminType"]);
            qsp.Add("versionid", view["ItemVersionId"]);

            // todo: why would we need modid on the URL for editing a version?
            // qsp.Add("modid", view["ModuleId"]);
            return this.BuildLinkUrl(qsp.ToString());
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void cmdBack_Click(object sender, EventArgs e)
        {
            string categoryId = this.Request.QueryString["categoryid"];
            this.Response.Redirect(this.EditUrl("categoryid", categoryId, Utility.AdminContainer), true);
        }

        private void BindData()
        {
            Localization.LocalizeDataGrid(ref this.dgVersions, this.LocalResourceFile);

            DataSet ds = Item.GetItemVersions(this.ItemId, this.PortalId);
            this.dgVersions.DataSource = ds;
            this.dgVersions.DataBind();
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}