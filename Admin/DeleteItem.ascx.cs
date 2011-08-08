//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Admin
{
    using System;
    using System.Web.UI.WebControls;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;

    public partial class DeleteItem : ModuleBase, IActionable
    {
        protected Label lblItemCreated;

        protected Label lblItemId;

        protected Label lblItemIdValue;

        protected Label lblItemVersion;

        protected HyperLink lnkItemVersion;

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
            this.cmdDelete.Click += this.cmdDelete_Click;
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check VI for null then set information
                if (!this.Page.IsPostBack)
                {
                    // ItemId
                    ClientAPI.AddButtonConfirm(this.cmdDelete, Localization.GetString("DeleteConfirmation", this.LocalResourceFile));
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            int itemId;
            bool success = false;

            if (Int32.TryParse(this.txtItemId.Text, out itemId))
            {
                // Need to figure out if the item exists, using GetItemTypeId since an Item must have a type
                if (Item.GetItemTypeId(itemId) != -1)
                {
                    // call the delete functionality.
                    Item.DeleteItem(itemId, this.PortalId);
                    success = true;
                }
            }

            this.lblResults.Visible = true;
            this.lblResults.Text = success
                                       ? Localization.GetString("Success", this.LocalResourceFile)
                                       : Localization.GetString("Failure", this.LocalResourceFile);
        }
    }
}