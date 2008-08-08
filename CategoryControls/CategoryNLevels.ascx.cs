//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Util;

//TODO: allow for paging?
//TODO: allow sort order - possible admin function (1-10 ranking)
namespace Engage.Dnn.Publish.CategoryControls
{
    public partial class CategoryNLevels : ModuleBase, IActionable
    {
        //private category id set from display loader
        private const string HighlightCssClass = "NLevelCurrentItem";
        private int setCategoryId;
        
        private int nLevels = -1;
        //private int mItems = -1;
        private bool highlightCurrentItem;// = false;
        private bool showParentItem = true;
        private bool showAll;// = false;

        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);

            BindItemData();
            SetupOptions();
            SetPageTitle();
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                RecordView();
                //N Levels M Items
                BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        #endregion
        
        private void BindData()
        {
           //TODO: NLevels number of items is not currently used

           //ToDO: figure out how to get the treeview to use the sorted datatable

            System.Windows.Forms.TreeNode root;

            string cacheKey = Utility.CacheKeyPublishCategoryNLevels + ItemId; // +"PageId";
            root = DataCache.GetCache(cacheKey) as System.Windows.Forms.TreeNode;


            if (root == null)
            {
                //if (Settings.Contains("nSortOrder"))
                //{
                //    if (!String.IsNullOrEmpty(Settings["nSortOrder"].ToString()))
                //    {
                //        DataTable dtchildren = ItemRelationship.GetAllChildrenNLevelsInDataTable(ItemId, nLevels, -1, PortalId);
                //        DataTable dt = Utility.SortDataTable(dtchildren, Settings["nSortOrder"].ToString());
                //        root = ItemRelationship.BuildHierarchy(dt);
                //    }
                //    else
                //    {
                //        root = ItemRelationship.GetAllChildrenNLevels(ItemId, nLevels, -1, PortalId);
                //    }
                //}
                //else
                //{

                    root = ItemRelationship.GetAllChildrenNLevels(ItemId, nLevels, -1, PortalId);
                //}

                if (root != null)
                {
                    DataCache.SetCache(cacheKey, root, DateTime.Now.AddMinutes(CacheTime));
                    Utility.AddCacheKey(cacheKey, PortalId);
                }

            }

           

           //add the parent category to the list first.
           if (showParentItem)
           {
               
               //TODO: get the category from cache
                Category parentItem = Category.GetCategory(ItemId, PortalId);
                #region showparent
                if (parentItem != null)
                {
                    phNLevels.Controls.Add(new LiteralControl("<ul>"));
                    phNLevels.Controls.Add(new LiteralControl("<li>"));

                    HyperLink hlParent = new HyperLink();

                    if (parentItem.Disabled == false)
                    {
                        hlParent.NavigateUrl = GetItemLinkUrl(parentItem.ItemId);
                    }

                    hlParent.Text = parentItem.Name;

                    if (highlightCurrentItem)
                    {
                        int itemId;//=0;
                        object o = Request.QueryString["ItemId"];
                        if (o != null && int.TryParse(o.ToString(), out itemId))
                        {
                            if (itemId == parentItem.ItemId)
                            {
                                hlParent.CssClass = HighlightCssClass;
                            }
                        }
                    }
                    phNLevels.Controls.Add(hlParent);
                    phNLevels.Controls.Add(new LiteralControl("</li>"));

                    FillNLevelList(root, phNLevels);
                    phNLevels.Controls.Add(new LiteralControl("</ul>"));
                }
                else
                {
                    FillNLevelList(root, phNLevels);
                }
               #endregion

           }
           else
           {
               FillNLevelList(root, phNLevels);
           }

            
        }


        private void FillNLevelList(System.Windows.Forms.TreeNode root, PlaceHolder ph)
        {

            for (int i = 0; i < root.Nodes.Count; i++)
            {
                
                System.Windows.Forms.TreeNode child = root.Nodes[i];
                if (child.Text.Length > 0)
                {
                    HyperLink hl = new HyperLink();
                    int itemId = Convert.ToInt32(child.Tag, CultureInfo.InvariantCulture);

                    if (!Utility.IsDisabled(itemId, PortalId))
                    {
                        Item curItem = this.BindItemData(itemId);
                        hl.NavigateUrl = GetItemLinkUrl(itemId);
                        if (curItem.NewWindow) hl.Target = "_blank";
                    }
                    hl.Text = child.Text;

                    ph.Controls.Add(new LiteralControl("<li>"));

                    //check if we should hightlight the current item, if so set the CSSClass
                    if (highlightCurrentItem)
                    {
                        int querystringItemId;
                        object o = Request.QueryString["ItemId"];
                        if (o != null && int.TryParse(o.ToString(), out querystringItemId))
                        {
                            if (querystringItemId == itemId)
                            {
                                hl.CssClass = HighlightCssClass;
                            }
                        }
                    }
                    ph.Controls.Add(hl);
                    ph.Controls.Add(new LiteralControl("</li>"));
                }
                if (child.Nodes.Count > 0)
                {
                    ph.Controls.Add(new LiteralControl("<ul>"));
                    FillNLevelList(child, ph);
                    ph.Controls.Add(new LiteralControl("</ul>"));
                }
                
            }

        }

        #region Interface Members

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString("Settings", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("Settings"), false, SecurityAccessLevel.Edit, true, false);
                return actions;
            }
        }

        #endregion

        //private int CategoryId
        //{
        //    get
        //    {
        //        string s = Request.QueryString["catid"];
        //        return (s == null ? -1 : Convert.ToInt32(s));
        //    }
        //}

        private void SetupOptions()
        {
            object o = Settings["nLevels"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                nLevels = Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            //object m = Settings["mItems"];
            //if (m != null && !String.IsNullOrEmpty(m.ToString()))
            //{
            //    mItems = Convert.ToInt32(m, CultureInfo.InvariantCulture);
            //}

            o = Settings["HighlightCurrentItem"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                highlightCurrentItem = Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
            }

            o = Settings["ShowParentItem"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                showParentItem = Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Record a Viewing.
        /// </summary>
        private void RecordView()
        {
            if (!VersionInfoObject.IsNew)
            {
                string referrer = string.Empty;
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    referrer = HttpContext.Current.Request.UrlReferrer.ToString();
                }
                string url = string.Empty;
                if (HttpContext.Current.Request.RawUrl != null)
                {
                    url = HttpContext.Current.Request.RawUrl.ToString();
                }

                
                this.VersionInfoObject.AddView(UserId, TabId, HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserAgent, referrer, url);
            }
        }

        public int SetCategoryId
        {
            get
            {
                return this.setCategoryId;
            }
            set
            {
                this.setCategoryId = value;
            }
        }

                /// <summary>
        /// This method is exclusively used for cases where there is no "context" for the Category that a user has 
        /// linked to. If this is set to true, when the control loads both child categories and child articles are displayed. hk
        /// </summary>
        public bool ShowAll
        {
            get
            {
                return showAll;
            }
            set
            {
                this.showAll = value;
            }
        }
       
    }
}

