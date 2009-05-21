//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Text;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System.Data;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Controls
{
    public partial class Breadcrumb : ModuleBase, IActionable
    {
        //		protected System.Web.UI.WebControls.Label lblBreadcrumb;
        //		protected System.Web.UI.WebControls.Label lblYouAreHere;

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);

        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += this.Page_Load;

        }
        #endregion

        private BreadcrumbCollection bci = new BreadcrumbCollection();

        private int levels;
        private bool includeCurrent = true;

        public string Levels
        {
            get { return levels.ToString(); }
            set { levels = Convert.ToInt32(value); }
        }

        public string IncludeCurrent
        {
            get { return includeCurrent.ToString(); }
            set { includeCurrent = Convert.ToBoolean(value); }
        }

        #region Event Handlers

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //check VI for null then set information
                //if itemid<=5 then we're dealing with a toplevelitemtype, ignore for breadcrumb
                if (!Page.IsPostBack && ItemId>5)
                {
                    StringBuilder sb = new StringBuilder(100);
                    loadParents(ItemId);
                    loadSelf(ItemId);

                    sb.Append(LoadBreadcrumb());

                    lblBreadcrumb.Text = sb.ToString();
                    if (lblBreadcrumb.Text.Length == 0)
                    {
                        lblYouAreHere.Visible = false;
                    }
                    else
                    {
                        lblYouAreHere.Text = Localization.GetString("lblYouAreHere", LocalSharedResourceFile);
                    }
                }
                else
                {

                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        #endregion

        private string LoadBreadcrumb()
        {
            if (bci.Count > 0)
            {
                StringBuilder sb = new StringBuilder(20);
                string separator = Localization.GetString("BreadCrumbSeparator", LocalSharedResourceFile);
                //we use levels+1 because the name of the current item is here, we handle that separately
                if ((bci.Count > levels + 1) && (levels>0))
                {
                    int removeNumber = bci.Count - (levels + 1);
                    for (int i = 0; i < removeNumber; i++)
                        bci.RemoveAt(0);
                }
                if (!includeCurrent)
                {
                    bci.RemoveAt(bci.Count - 1);
                }

                foreach (BreadcrumbItem bi in bci)
                {
                    sb.AppendFormat(Localization.GetString("BreadCrumbLink", LocalSharedResourceFile), bi.PageUrl, bi.PageName);
                    sb.Append(separator);
                }
                //remove the last separator
                string returnString = sb.ToString();
                int lastSeparator = returnString.LastIndexOf(separator);
                returnString = returnString.Remove(lastSeparator, separator.Length);
                return returnString;
            }
            return string.Empty;
        }

        private void loadParents(int itemId)
        {
            if (itemId > -1)
            {
                int parentId = Category.GetParentCategory(itemId, PortalId);
                if (parentId > 0)
                {
                    StringBuilder resultSet = new StringBuilder();
                    Item i = Item.GetItem(parentId, PortalId, Engage.Dnn.Publish.Util.ItemType.Category.GetId(), true);
                    string categoryUrl = Engage.Dnn.Publish.Util.Utility.GetItemLinkUrl(i);
                    bci.InsertBeginning(i.Name, categoryUrl);
                    loadParents(i.ItemId);
                }
            }
        }

        private void loadSelf(int itemId)
        {
            if (itemId > -1)
            {
                Item i = Item.GetItem(itemId, PortalId, Item.GetItemTypeId(itemId), true);
                if (i != null)
                {
                    string linkUrl = Engage.Dnn.Publish.Util.Utility.GetItemLinkUrl(i);
                    bci.Add(i.Name, linkUrl);
                }
            }

        }



        #region Optional Interfaces

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                DotNetNuke.Entities.Modules.Actions.ModuleActionCollection actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
                return actions;
            }
        }


        #endregion

    }
}

