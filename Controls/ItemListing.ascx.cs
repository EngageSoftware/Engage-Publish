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
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Search;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    public partial class ItemListing : ModuleBase, IActionable
    {
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

        protected ArticleViewOption DataDisplayFormat
        {
            get
            {
                object o = this.Settings["ilDataDisplayFormat"];
                return Enum.IsDefined(typeof(ArticleViewOption), o)
                           ? (ArticleViewOption)Enum.Parse(typeof(ArticleViewOption), o.ToString(), true)
                           : ArticleViewOption.Abstract;
            }
        }

        protected string DataType
        {
            get
            {
                object o = this.Settings["ilDataType"];
                return o == null ? "Item Listing" : o.ToString();
            }
        }

        private int CategoryId
        {
            get
            {
                object o = this.Settings["ilCategoryId"];
                return o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }
        }

        private bool EnableRss
        {
            get
            {
                object o = this.Settings["ilEnableRss"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }
        }

        private int ItemTypeId
        {
            get
            {
                object o = this.Settings["ilItemTypeId"];
                return o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }
        }

        private int MaxDisplayItems
        {
            get
            {
                object o = this.Settings["ilMaxDisplayItems"];
                return o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this module is set to show the item's parent category.
        /// </summary>
        /// <value><c>true</c> if the parent category should be shown; otherwise, <c>false</c>.</value>
        private bool ShowParent
        {
            get
            {
                object o = this.Settings["ilShowParent"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }
        }

        // private int MaxDisplaySubItems
        // {
        // get
        // {
        // object o = Settings["ilMaxDisplaySubItems"];
        // return (o == null ? -1 : Convert.ToInt32(o));
        // }
        // }
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "0#", Justification = "Interface Implementation")
        ]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Interface Implementation")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "modInfo", Justification = "Interface implementation")]
        public SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            // included as a stub only so that the core knows this module Implements Entities.Modules.ISearchable
            return null;
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void lstItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e != null)
            {
                var pnlThumbnail = (Panel)e.Item.FindControl("pnlThumbnail");
                var pnlCategory = (Panel)e.Item.FindControl("pnlCategory");

                // Panel pnlTitle = (Panel)e.Item.FindControl("pnlTitle");
                var pnlDescription = (Panel)e.Item.FindControl("pnlDescription");
                var pnlReadMore = (Panel)e.Item.FindControl("pnlReadMore");
                var lnkTitle = (HyperLink)e.Item.FindControl("lnkTitle");

                if (pnlThumbnail != null)
                {
                    pnlThumbnail.Visible = this.DataDisplayFormat == ArticleViewOption.Thumbnail ||
                                           this.DataDisplayFormat == ArticleViewOption.TitleAndThumbnail;
                }

                if (pnlDescription != null)
                {
                    pnlDescription.Visible = this.DataDisplayFormat == ArticleViewOption.Thumbnail ||
                                             this.DataDisplayFormat == ArticleViewOption.Abstract;
                }

                if (pnlReadMore != null)
                {
                    pnlReadMore.Visible = this.DataDisplayFormat == ArticleViewOption.Thumbnail ||
                                          this.DataDisplayFormat == ArticleViewOption.Abstract;
                }

                if (pnlCategory != null)
                {
                    pnlCategory.Visible = this.ShowParent;
                }

                // This code makes sure that an article that is disabled does not get a link.
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                    var childItemId = (int)dr["ChildItemId"];

                    if (Utility.IsDisabled(childItemId, this.PortalId))
                    {
                        lnkTitle.NavigateUrl = string.Empty;
                        if (pnlReadMore != null)
                        {
                            pnlReadMore.Visible = false;
                        }
                    }

                    if (pnlThumbnail != null && (dr["Thumbnail"] == null || !Engage.Utility.HasValue(dr["Thumbnail"].ToString())))
                    {
                        pnlThumbnail.CssClass += " item_listing_nothumbnail";
                    }
                }
            }
        }

        private static DataTable FormatDataTable(DataTable dt)
        {
            var newDataTable = new DataTable(dt.TableName, dt.Namespace)
                {
                    Locale = CultureInfo.InvariantCulture
                };
            newDataTable.Columns.Add("CategoryName", typeof(string), string.Empty);
            newDataTable.Columns.Add("Thumbnail");
            newDataTable.Columns.Add("ChildName");
            newDataTable.Columns.Add("ChildItemId", typeof(int));
            newDataTable.Columns.Add("ChildDescription");

            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = newDataTable.NewRow();

                newRow["Thumbnail"] = row["Thumbnail"];
                newRow["ChildName"] = row["Name"];
                newRow["ChildItemId"] = Convert.ToInt32(row["ItemId"], CultureInfo.InvariantCulture);
                newRow["ChildDescription"] = row["Description"];
                newDataTable.Rows.Add(newRow);
            }

            return newDataTable;
        }

        private DataTable GetData()
        {
            int itemId = this.ItemId; // Since figuring out the Item ID is so expensive, do it only once.

            // this obviously needs refactoring
            string cacheKey = Utility.CacheKeyPublishCategory + "ItemListing_" + this.DataType.Replace(" ", string.Empty) + this.CategoryId;
                
                // +"PageId";
            var dt = DataCache.GetCache(cacheKey) as DataTable;

            if (dt == null)
            {
                switch (this.DataType)
                {
                    case "Item Listing":
                        if (this.CategoryId != -1)
                        {
                            dt = DataProvider.Instance().GetChildrenInCategory(this.CategoryId, this.ItemTypeId, this.MaxDisplayItems, this.PortalId);
                        }
                        else
                        {
                            DataSet ds = DataProvider.Instance().GetItems(
                                TopLevelCategoryItemType.Category.GetId(), 
                                this.PortalId, 
                                RelationshipType.CategoryToTopLevelCategory.GetId(), 
                                this.ItemTypeId);
                            dt = ds.Tables[0];
                            dt = FormatDataTable(dt);
                        }

                        break;
                    case "Most Popular":
                        dt = DataProvider.Instance().GetMostPopular(this.CategoryId, this.ItemTypeId, this.MaxDisplayItems, this.PortalId);
                        break;
                    case "Most Recent":
                        if (this.EnableRss)
                        {
                            this.lnkRss.Visible = true;
                            string rssImage = Localization.GetString("rssImage", this.LocalResourceFile);
#if DEBUG
                            rssImage = rssImage.Replace("[L]", string.Empty);
#endif

                            this.lnkRss.ImageUrl = ApplicationUrl + rssImage; // "/images/xml.gif";
                            this.lnkRss.Attributes.Add("alt", Localization.GetString("rssAlt", this.LocalResourceFile));
                            this.lnkRss.NavigateUrl = GetRssLinkUrl(itemId, this.MaxDisplayItems, this.ItemTypeId, this.PortalId, "ItemListing");

                            this.SetRssUrl(this.lnkRss.NavigateUrl, Localization.GetString("rssText", this.LocalResourceFile));
                        }

                        dt = itemId > -1
                                 ? DataProvider.Instance().GetMostRecentByCategoryId(itemId, this.ItemTypeId, this.MaxDisplayItems, this.PortalId)
                                 : DataProvider.Instance().GetMostRecent(this.ItemTypeId, this.MaxDisplayItems, this.PortalId);
                        break;
                    default:
                        dt = DataProvider.Instance().GetChildrenInCategory(this.CategoryId, this.ItemTypeId, this.MaxDisplayItems, this.PortalId);
                        break;
                }

                // Set the object into cache
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(this.CacheTime));
                    Utility.AddCacheKey(cacheKey, this.PortalId);
                }
            }

            if (this.EnableRss && this.DataType == "Most Recent")
            {
                this.lnkRss.Visible = true;
                string rssImage = Localization.GetString("rssImage", this.LocalResourceFile);
#if DEBUG
                rssImage = rssImage.Replace("[L]", string.Empty);
#endif

                this.lnkRss.ImageUrl = ApplicationUrl + rssImage; // "/images/xml.gif";
                this.lnkRss.Attributes.Add("alt", Localization.GetString("rssAlt", this.LocalResourceFile));
                this.lnkRss.NavigateUrl = GetRssLinkUrl(itemId, this.MaxDisplayItems, this.ItemTypeId, this.PortalId, "ItemListing");

                this.SetRssUrl(this.lnkRss.NavigateUrl, Localization.GetString("rssText", this.LocalResourceFile));
            }

            return dt;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            // try
            // {
            if (this.ItemTypeId == -1)
            {
                this.lblMessage.Text = Localization.GetString("SetupItemType", this.LocalResourceFile);
                this.lblMessage.Visible = true;
                return;
            }

            this.lstItems.DataSource = this.GetData();
            this.lstItems.DataBind();

            // }
            // catch (Exception exc)
            // {
            // Exceptions.ProcessModuleLoadException(this, exc);
            // }
        }
    }
}