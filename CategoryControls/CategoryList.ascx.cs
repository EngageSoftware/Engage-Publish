//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.CategoryControls
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    public partial class CategoryList : ModuleBase, IActionable
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

        private int CategoryId
        {
            get
            {
                string s = this.Request.QueryString["itemid"];
                return s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
        }

        private int TopLevelId
        {
            get
            {
                string s = this.Request.QueryString["topLevelId"];
                return s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", 
            MessageId = "System.Web.UI.ITextControl.set_Text(System.String)", Justification = "Literal is HTML")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", 
            MessageId = "System.Web.UI.WebControls.TableCell.set_Text(System.String)", Justification = "Literal is HTML")]
        private void BindData()
        {
            int itemId = Convert.ToInt32(this.cboItemType.SelectedValue, CultureInfo.InvariantCulture);
            if (this.CategoryId > -1)
            {
                // user clicked on a subcategory.
                itemId = this.CategoryId;
            }

            // set the approval status ID to approved by default, if we're using approvals look for the selected value
            int approvalStatusId = ApprovalStatus.Approved.GetId();

            if (this.UseApprovals)
            {
                approvalStatusId = Convert.ToInt32(this.cboWorkflow.SelectedValue, CultureInfo.InvariantCulture);
            }

            var qsp = new QueryStringParameters();
            DataSet ds;

            if (this.txtArticleSearch.Text.Trim() != string.Empty)
            {
                var objSecurity = new PortalSecurity();
                string searchKey = objSecurity.InputFilter(this.txtArticleSearch.Text.Trim(), PortalSecurity.FilterFlag.NoSQL);
                ds = itemId == -1
                         ? DataProvider.Instance().GetAdminItemListingSearchKey(
                             TopLevelCategoryItemType.Category.GetId(), 
                             ItemType.Category.GetId(), 
                             RelationshipType.CategoryToTopLevelCategory.GetId(), 
                             RelationshipType.ItemToRelatedCategory.GetId(), 
                             approvalStatusId, 
                             " vi.createddate desc ", 
                             searchKey, 
                             this.PortalId)
                         : DataProvider.Instance().GetAdminItemListingSearchKey(
                             itemId, 
                             ItemType.Category.GetId(), 
                             RelationshipType.ItemToParentCategory.GetId(), 
                             RelationshipType.ItemToRelatedCategory.GetId(), 
                             approvalStatusId, 
                             " vi.createddate desc ", 
                             searchKey, 
                             this.PortalId);
            }
            else
            {
                ds = itemId == -1
                         ? DataProvider.Instance().GetAdminItemListing(
                             TopLevelCategoryItemType.Category.GetId(), 
                             ItemType.Category.GetId(), 
                             RelationshipType.CategoryToTopLevelCategory.GetId(), 
                             approvalStatusId, 
                             this.PortalId)
                         : DataProvider.Instance().GetAdminItemListing(
                             itemId, 
                             ItemType.Category.GetId(), 
                             RelationshipType.ItemToParentCategory.GetId(), 
                             RelationshipType.ItemToRelatedCategory.GetId(), 
                             approvalStatusId, 
                             this.PortalId);
            }

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                var t = new Table
                    {
                        Width = Unit.Percentage(100), 
                        CssClass = "Normal", 
                        CellPadding = 4, 
                        CellSpacing = 0, 
                        GridLines = GridLines.Horizontal, 
                        BorderColor = Color.Gray, 
                        BorderStyle = BorderStyle.Solid, 
                        BorderWidth = Unit.Pixel(1)
                    };

                var row = new TableRow
                    {
                        CssClass = "listing_table_head_row"
                    };
                t.Rows.Add(row);
                var cell = new TableCell();

                row.Cells.Add(cell);
                cell.Text = Localization.GetString("ID", this.LocalResourceFile);

                cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = Localization.GetString("Name", this.LocalResourceFile);

                cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = Localization.GetString("Description", this.LocalResourceFile);

                cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = "&nbsp;";

                cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = "&nbsp;";

                cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = "&nbsp;";

                cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = "&nbsp;";

                cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = "&nbsp;";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];

                    row = new TableRow
                        {
                            CssClass = i % 2 == 0 ? "adminItemSearchDarkRow" : "adminItemSearchLightRow", 
                            VerticalAlign = VerticalAlign.Top
                        };

                    t.Rows.Add(row);
                    cell = new TableCell();

                    row.Cells.Add(cell);
                    cell.Text = r["ItemID"].ToString();

                    cell = new TableCell();
                    row.Cells.Add(cell);
                    cell.Text = r["Name"].ToString();

                    cell = new TableCell();
                    row.Cells.Add(cell);
                    cell.Text = HtmlUtils.Shorten(HtmlUtils.Clean(r["Description"].ToString(), true), 200, string.Empty) + "&nbsp;";

                    cell = new TableCell();
                    row.Cells.Add(cell);

                    // check if the category has any sub categories, if not don't display link
                    var hl = new HyperLink();

                    if (Convert.ToInt32(r["ChildCount"], CultureInfo.InvariantCulture) > 0)
                    {
                        cell.Controls.Add(hl);

                        qsp.ClearKeys();
                        qsp.Add("ctl", Utility.AdminContainer);
                        qsp.Add("mid", this.ModuleId.ToString(CultureInfo.InvariantCulture));
                        qsp.Add("adminType", "categorylist");
                        qsp.Add("itemId", r["ItemId"]);

                        // qsp.Add("category", r["Name"]);
                        qsp.Add("parentId", itemId);
                        if (this.TopLevelId == -1)
                        {
                            qsp.Add("topLevelId", this.cboItemType.SelectedValue);
                        }
                        else
                        {
                            qsp.Add("topLevelId", this.TopLevelId);
                        }

                        hl.NavigateUrl = this.BuildLinkUrl(qsp.ToString());

                        hl.Text = Localization.GetString("SubCategories", this.LocalResourceFile);
                    }
                    else
                    {
                        var l1 = new Label
                            {
                                Text = " <br /> "
                            };
                        cell.Controls.Add(l1);
                    }

                    // Add the CategorySort link
                    cell = new TableCell();
                    row.Cells.Add(cell);
                    hl = new HyperLink();
                    cell.Controls.Add(hl);
                    qsp.ClearKeys();
                    qsp.Add("ctl", Utility.AdminContainer);
                    qsp.Add("mid", this.ModuleId.ToString(CultureInfo.InvariantCulture));
                    qsp.Add("adminType", "categorysort");
                    qsp.Add("itemid", r["ItemId"]);

                    hl.NavigateUrl = this.BuildLinkUrl(qsp.ToString());

                    hl.Text = Localization.GetString("CategorySort", this.LocalResourceFile);

                    cell = new TableCell();
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    row.Cells.Add(cell);
                    hl = new HyperLink();
                    cell.Controls.Add(hl);
                    qsp.ClearKeys();
                    qsp.Add("ctl", Utility.AdminContainer);
                    qsp.Add("mid", this.ModuleId.ToString(CultureInfo.InvariantCulture));
                    qsp.Add("adminType", "versionslist");
                    qsp.Add("itemid", r["ItemId"]);

                    hl.NavigateUrl = this.BuildLinkUrl(qsp.ToString());

                    hl.Text = Localization.GetString("Versions", this.LocalSharedResourceFile);

                    cell = new TableCell();
                    row.Cells.Add(cell);
                    hl = new HyperLink();
                    cell.Controls.Add(hl);
                    qsp.ClearKeys();
                    qsp.Add("ctl", Utility.AdminContainer);
                    qsp.Add("mid", this.ModuleId.ToString(CultureInfo.InvariantCulture));
                    qsp.Add("adminType", "categoryEdit");
                    qsp.Add("versionid", r["ItemVersionId"]);

                    // qsp.Add("modid", r["ModuleId"]);
                    qsp.Add("parentId", itemId);
                    if (this.TopLevelId == -1)
                    {
                        qsp.Add("topLevelId", this.cboItemType.SelectedValue);
                    }
                    else
                    {
                        qsp.Add("topLevelId", this.TopLevelId);
                    }

                    hl.NavigateUrl = this.BuildLinkUrl(qsp.ToString());
                    hl.Text = Localization.GetString("Edit", this.LocalResourceFile);
                }

                this.phList.Controls.Add(t);

                if (!this.cboItemType.SelectedValue.Equals("-1"))
                {
                    this.lblMessage.Text = Localization.GetString("SubCategoriesFor", this.LocalResourceFile) + " " + this.cboItemType.SelectedItem;
                }
            }
            else
            {
                if (!this.cboItemType.SelectedValue.Equals("-1"))
                {
                    this.lblMessage.Text = Localization.GetString("NoSubcategoriesFor", this.LocalResourceFile) + " " + this.cboItemType.SelectedItem;
                }
            }
        }

        private void ConfigureAddLink()
        {
            // has a top level been selected?
            if (this.TopLevelId == -1)
            {
                string s = this.cboItemType.SelectedValue;
                int id = Utility.HasValue(s) ? Convert.ToInt32(s, CultureInfo.InvariantCulture) : -1;
                this.lnkAddNewCategory.NavigateUrl = id == -1
                                                         ? this.BuildLinkUrl(
                                                             "&ctl=" + Utility.AdminContainer + "&mid=" +
                                                             this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&adminType=categoryEdit")
                                                         : this.BuildLinkUrl(
                                                             "&ctl=" + Utility.AdminContainer + "&mid=" +
                                                             this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                                                             "&adminType=categoryEdit&topLevelId=" + id.ToString(CultureInfo.InvariantCulture) +
                                                             "&parentId=" + id.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                this.lnkAddNewCategory.NavigateUrl =
                    this.BuildLinkUrl(
                        "&ctl=" + Utility.AdminContainer + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                        "&adminType=categoryEdit&topLevelId=" + this.TopLevelId.ToString(CultureInfo.InvariantCulture) + "&parentId=" +
                        this.CategoryId.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void FillDropDowns()
        {
            ItemRelationship.DisplayCategoryHierarchy(this.cboItemType, this.CategoryId, this.PortalId, false);

            var li = new ListItem(Localization.GetString("ChooseOne", this.LocalSharedResourceFile), "-1");
            this.cboItemType.Items.Insert(0, li);

            this.cboWorkflow.Visible = this.UseApprovals;
            this.lblWorkflow.Visible = this.UseApprovals;
            if (this.UseApprovals)
            {
                this.cboWorkflow.DataSource = DataProvider.Instance().GetApprovalStatusTypes(this.PortalId);
                this.cboWorkflow.DataValueField = "ApprovalStatusID";
                this.cboWorkflow.DataTextField = "ApprovalStatusName";
                this.cboWorkflow.DataBind();
                li = this.cboWorkflow.Items.FindByText(ApprovalStatus.Approved.Name);
                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboItemType.SelectedIndexChanged += new EventHandler(this.cboItemType_SelectedIndexChanged);
            this.cboWorkflow.SelectedIndexChanged += new EventHandler(cboWorkFlow_SelectedIndexChanged);
            this.Load += new EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    this.ConfigureAddLink();
                    this.FillDropDowns();
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void cboItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void cboWorkFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }
    }
}