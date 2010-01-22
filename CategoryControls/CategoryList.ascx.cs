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
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Data;
    using Util;

    public partial class CategoryList : ModuleBase, IActionable
    {
        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboItemType.SelectedIndexChanged += new System.EventHandler(this.cboItemType_SelectedIndexChanged);
            this.cboWorkflow.SelectedIndexChanged += new EventHandler(cboWorkFlow_SelectedIndexChanged);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion

        #region Event Handlers

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ConfigureAddLink();
                    FillDropDowns();
                    BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion


        #region Optional Interfaces

        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                           {
                                   {
                                           GetNextActionID(),
                                           Localization.GetString(
                                           ModuleActionType.AddContent, LocalResourceFile),
                                           DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "",
                                           "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true,
                                           false
                                           }
                           };
            }
        }


        #endregion

        private void cboItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void cboWorkFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.Web.UI.ITextControl.set_Text(System.String)", Justification = "Literal is HTML"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.Web.UI.WebControls.TableCell.set_Text(System.String)", Justification = "Literal is HTML")]
        private void BindData()
        {
            int itemId = Convert.ToInt32(cboItemType.SelectedValue, CultureInfo.InvariantCulture);
            if (CategoryId > -1)
            {
                //user clicked on a subcategory.
                itemId = CategoryId;
            }
            //set the approval status ID to approved by default, if we're using approvals look for the selected value
            int approvalStatusId = ApprovalStatus.Approved.GetId();

            if (UseApprovals)
            {
                approvalStatusId = Convert.ToInt32(cboWorkflow.SelectedValue, CultureInfo.InvariantCulture);
            }

            var qsp = new QueryStringParameters();
            DataSet ds;

            if (txtArticleSearch.Text.Trim() != string.Empty)
            {
                var objSecurity = new DotNetNuke.Security.PortalSecurity();
                string searchKey = objSecurity.InputFilter(txtArticleSearch.Text.Trim(), DotNetNuke.Security.PortalSecurity.FilterFlag.NoSQL);
                //
                ds = itemId == -1 ? DataProvider.Instance().GetAdminItemListingSearchKey(TopLevelCategoryItemType.Category.GetId(), ItemType.Category.GetId(), RelationshipType.CategoryToTopLevelCategory.GetId(), RelationshipType.ItemToRelatedCategory.GetId(), approvalStatusId, " vi.createddate desc ", searchKey, PortalId) : DataProvider.Instance().GetAdminItemListingSearchKey(itemId, ItemType.Category.GetId(), RelationshipType.ItemToParentCategory.GetId(), RelationshipType.ItemToRelatedCategory.GetId(), approvalStatusId, " vi.createddate desc ", searchKey, PortalId);
            }
            else
            {
                ds = itemId == -1 ? DataProvider.Instance().GetAdminItemListing(TopLevelCategoryItemType.Category.GetId(), ItemType.Category.GetId(), RelationshipType.CategoryToTopLevelCategory.GetId(), approvalStatusId, PortalId) : DataProvider.Instance().GetAdminItemListing(itemId, ItemType.Category.GetId(), RelationshipType.ItemToParentCategory.GetId(), RelationshipType.ItemToRelatedCategory.GetId(), approvalStatusId, PortalId);
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
                                BorderColor = System.Drawing.Color.Gray,
                                BorderStyle = BorderStyle.Solid,
                                BorderWidth = Unit.Pixel(1)
                            };

                var row = new TableRow { CssClass = "listing_table_head_row" };
                t.Rows.Add(row);
                var cell = new TableCell();

                row.Cells.Add(cell);
                cell.Text = Localization.GetString("ID", LocalResourceFile);

                cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = Localization.GetString("Name", LocalResourceFile);

                cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = Localization.GetString("Description", LocalResourceFile);

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

                    //check if the category has any sub categories, if not don't display link
                    var hl = new HyperLink();

                    if (Convert.ToInt32(r["ChildCount"], CultureInfo.InvariantCulture) > 0)
                    {
                        cell.Controls.Add(hl);

                        qsp.ClearKeys();
                        qsp.Add("ctl", Utility.AdminContainer);
                        qsp.Add("mid", ModuleId.ToString(CultureInfo.InvariantCulture));
                        qsp.Add("adminType", "categorylist");
                        qsp.Add("itemId", r["ItemId"]);
                        //qsp.Add("category", r["Name"]);
                        qsp.Add("parentId", itemId);
                        if (TopLevelId == -1)
                        {
                            qsp.Add("topLevelId", cboItemType.SelectedValue);
                        }
                        else
                        {
                            qsp.Add("topLevelId", TopLevelId);
                        }


                        hl.NavigateUrl = BuildLinkUrl(qsp.ToString());

                        hl.Text = Localization.GetString("SubCategories", LocalResourceFile);

                    }
                    else
                    {
                        var l1 = new Label { Text = " <br /> " };
                        cell.Controls.Add(l1);
                    }
                    //Add the CategorySort link
                    cell = new TableCell();
                    row.Cells.Add(cell);
                    hl = new HyperLink();
                    cell.Controls.Add(hl);
                    qsp.ClearKeys();
                    qsp.Add("ctl", Utility.AdminContainer);
                    qsp.Add("mid", ModuleId.ToString(CultureInfo.InvariantCulture));
                    qsp.Add("adminType", "categorysort");
                    qsp.Add("itemid", r["ItemId"]);

                    hl.NavigateUrl = BuildLinkUrl(qsp.ToString());

                    hl.Text = Localization.GetString("CategorySort", LocalResourceFile);

                    cell = new TableCell();
                    row.Cells.Add(cell);





                    cell = new TableCell();
                    row.Cells.Add(cell);
                    hl = new HyperLink();
                    cell.Controls.Add(hl);
                    qsp.ClearKeys();
                    qsp.Add("ctl", Utility.AdminContainer);
                    qsp.Add("mid", ModuleId.ToString(CultureInfo.InvariantCulture));
                    qsp.Add("adminType", "versionslist");
                    qsp.Add("itemid", r["ItemId"]);

                    hl.NavigateUrl = BuildLinkUrl(qsp.ToString());

                    hl.Text = Localization.GetString("Versions", LocalSharedResourceFile);

                    cell = new TableCell();
                    row.Cells.Add(cell);
                    hl = new HyperLink();
                    cell.Controls.Add(hl);
                    qsp.ClearKeys();
                    qsp.Add("ctl", Utility.AdminContainer);
                    qsp.Add("mid", ModuleId.ToString(CultureInfo.InvariantCulture));
                    qsp.Add("adminType", "categoryEdit");
                    qsp.Add("versionid", r["ItemVersionId"]);
                    //qsp.Add("modid", r["ModuleId"]);
                    qsp.Add("parentId", itemId);
                    if (TopLevelId == -1)
                    {
                        qsp.Add("topLevelId", 
                            cboItemType.SelectedValue);
                    }
                    else
                    {
                        qsp.Add("topLevelId", TopLevelId);
                    }

                    hl.NavigateUrl = BuildLinkUrl(qsp.ToString());
                    hl.Text = Localization.GetString("Edit", LocalResourceFile);
                }

                phList.Controls.Add(t);

                if (!cboItemType.SelectedValue.Equals("-1"))
                {
                    lblMessage.Text = Localization.GetString("SubCategoriesFor", LocalResourceFile) + " " + cboItemType.SelectedItem;
                }
            }

            else
            {
                if (!cboItemType.SelectedValue.Equals("-1"))
                {
                    lblMessage.Text = Localization.GetString("NoSubcategoriesFor", LocalResourceFile) + " " + cboItemType.SelectedItem;
                }
            }

        }

        private int CategoryId
        {
            get
            {
                string s = Request.QueryString["itemid"];
                return (s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture));
            }
        }

        private int TopLevelId
        {
            get
            {
                string s = Request.QueryString["topLevelId"];
                return (s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture));
            }
        }

        //private string CategoryName
        //{
        //    get
        //    {
        //        Category c = Category.GetCategory(ItemId, PortalId);
        //        return c.Name;
        //    }
        //}

        private void FillDropDowns()
        {
            ItemRelationship.DisplayCategoryHierarchy(cboItemType, CategoryId, PortalId, false);

            var li = new ListItem(Localization.GetString("ChooseOne", LocalSharedResourceFile), "-1");
            cboItemType.Items.Insert(0, li);

            cboWorkflow.Visible = UseApprovals;
            lblWorkflow.Visible = UseApprovals;
            if (UseApprovals)
            {
                cboWorkflow.DataSource = DataProvider.Instance().GetApprovalStatusTypes(PortalId);
                cboWorkflow.DataValueField = "ApprovalStatusID";
                cboWorkflow.DataTextField = "ApprovalStatusName";
                cboWorkflow.DataBind();
                li = cboWorkflow.Items.FindByText(ApprovalStatus.Approved.Name);
                if (li != null) li.Selected = true;
            }

        }

        private void ConfigureAddLink()
        {
            //has a top level been selected?
            if (TopLevelId == -1)
            {
                string s = cboItemType.SelectedValue;
                int id = (Utility.HasValue(s) ? Convert.ToInt32(s, CultureInfo.InvariantCulture) : -1);
                lnkAddNewCategory.NavigateUrl = id == -1 ? BuildLinkUrl("&ctl=" + Utility.AdminContainer + "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&adminType=categoryEdit") : BuildLinkUrl("&ctl=" + Utility.AdminContainer + "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&adminType=categoryEdit&topLevelId=" + id.ToString(CultureInfo.InvariantCulture) + "&parentId=" + id.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                lnkAddNewCategory.NavigateUrl = BuildLinkUrl("&ctl=" + Utility.AdminContainer + "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&adminType=categoryEdit&topLevelId=" + TopLevelId.ToString(CultureInfo.InvariantCulture) + "&parentId=" + CategoryId.ToString(CultureInfo.InvariantCulture));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindData();
        }

    }
}