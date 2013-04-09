// <copyright file="Breadcrumb.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Text;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class Breadcrumb : ModuleBase, IActionable
    {
        // 		protected System.Web.UI.WebControls.Label lblBreadcrumb;
        // 		protected System.Web.UI.WebControls.Label lblYouAreHere;
        private readonly BreadcrumbCollection bci = new BreadcrumbCollection();

        private bool _includeCurrent = true;

        private int _levels;

        public string IncludeCurrent
        {
            get { return this._includeCurrent.ToString(); }
            set { this._includeCurrent = Convert.ToBoolean(value); }
        }

        public string Levels
        {
            get { return this._levels.ToString(); }
            set { this._levels = Convert.ToInt32(value); }
        }

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

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private string LoadBreadcrumb()
        {
            if (this.bci.Count > 0)
            {
                var sb = new StringBuilder(20);
                string separator = Localization.GetString("BreadCrumbSeparator", this.LocalSharedResourceFile);

                // we use _levels+1 because the name of the current item is here, we handle that separately
                if ((this.bci.Count > this._levels + 1) && (this._levels > 0))
                {
                    int removeNumber = this.bci.Count - (this._levels + 1);
                    for (int i = 0; i < removeNumber; i++)
                    {
                        this.bci.RemoveAt(0);
                    }
                }

                if (!this._includeCurrent)
                {
                    this.bci.RemoveAt(this.bci.Count - 1);
                }

                foreach (BreadcrumbItem bi in this.bci)
                {
                    sb.AppendFormat(Localization.GetString("BreadCrumbLink", this.LocalSharedResourceFile), bi.PageUrl, bi.PageName);
                    sb.Append(separator);
                }

                // remove the last separator
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
                int parentId = Category.GetParentCategory(itemId, this.PortalId);

                if (parentId > 0)
                {
                    Item i = Item.GetItem(parentId, this.PortalId, ItemType.Category.GetId(), true);
                    string categoryUrl = UrlGenerator.GetItemLinkUrl(i, this.PortalSettings);
                    this.bci.InsertBeginning(i.Name, categoryUrl);
                    this.LoadParents(i.ItemId);
                }
            }
        }

        private void LoadSelf(int itemId)
        {
            if (itemId > -1)
            {
                Item i = Item.GetItem(itemId, this.PortalId, Item.GetItemTypeId(itemId), true);
                if (i != null)
                {
                    string linkUrl = UrlGenerator.GetItemLinkUrl(i, this.PortalSettings);
                    this.bci.Add(i.Name, linkUrl);
                }
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check VI for null then set information
                // if itemid<=5 then we're dealing with a toplevelitemtype, ignore for breadcrumb
                if (!this.Page.IsPostBack && this.ItemId > 5)
                {
                    var sb = new StringBuilder(100);
                    this.LoadParents(this.ItemId);
                    this.LoadSelf(this.ItemId);

                    sb.Append(this.LoadBreadcrumb());

                    this.lblBreadcrumb.Text = sb.ToString();
                    if (this.lblBreadcrumb.Text.Length == 0)
                    {
                        this.lblYouAreHere.Visible = false;
                    }
                    else
                    {
                        this.lblYouAreHere.Text = Localization.GetString("lblYouAreHere", this.LocalSharedResourceFile);
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}