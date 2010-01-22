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
    using System.Text;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Util;

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
            Load += Page_Load;

        }
        #endregion

        private BreadcrumbCollection bci = new BreadcrumbCollection();

        private int _levels;
        private bool _includeCurrent = true;

        public string Levels
        {
            get { return _levels.ToString(); }
            set { _levels = Convert.ToInt32(value); }
        }

        public string IncludeCurrent
        {
            get { return _includeCurrent.ToString(); }
            set { _includeCurrent = Convert.ToBoolean(value); }
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
                    var sb = new StringBuilder(100);
                    LoadParents(ItemId);
                    LoadSelf(ItemId);

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
                var sb = new StringBuilder(20);
                string separator = Localization.GetString("BreadCrumbSeparator", LocalSharedResourceFile);
                //we use _levels+1 because the name of the current item is here, we handle that separately
                if ((bci.Count > _levels + 1) && (_levels>0))
                {
                    int removeNumber = bci.Count - (_levels + 1);
                    for (int i = 0; i < removeNumber; i++)
                        bci.RemoveAt(0);
                }
                if (!_includeCurrent)
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

        private void LoadParents(int itemId)
        {
            if (itemId > -1)
            {
                int parentId = Category.GetParentCategory(itemId, PortalId);
                
                if (parentId > 0)
                {
                    Item i = Item.GetItem(parentId, PortalId, ItemType.Category.GetId(), true);
                    string categoryUrl = Utility.GetItemLinkUrl(i);
                    bci.InsertBeginning(i.Name, categoryUrl);
                    LoadParents(i.ItemId);
                }
            }
        }

        private void LoadSelf(int itemId)
        {
            if (itemId > -1)
            {
                Item i = Item.GetItem(itemId, PortalId, Item.GetItemTypeId(itemId), true);
                if (i != null)
                {
                    string linkUrl = Utility.GetItemLinkUrl(i);
                    bci.Add(i.Name, linkUrl);
                }
            }

        }



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
                                           DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent,
                                           LocalResourceFile),
                                           DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "",
                                           "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true,
                                           false
                                           }
                           };
            }
        }


        #endregion

    }
}

