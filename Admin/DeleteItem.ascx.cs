//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


namespace Engage.Dnn.Publish.Admin
{
    using System;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;

	public partial class DeleteItem :  ModuleBase, IActionable
	{
		protected System.Web.UI.WebControls.Label lblItemCreated;
		protected System.Web.UI.WebControls.Label lblItemId;
		protected System.Web.UI.WebControls.Label lblItemIdValue;
		protected System.Web.UI.WebControls.Label lblItemVersion;
		protected System.Web.UI.WebControls.HyperLink lnkItemVersion;
		

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
			this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region Event Handlers

		private void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				//check VI for null then set information
				if (!Page.IsPostBack)
				{
					//ItemId
					ClientAPI.AddButtonConfirm(cmdDelete, Localization.GetString("DeleteConfirmation", LocalResourceFile));
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
			                               this.GetNextActionID(),
			                               Localization.GetString(
			                               DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent,
			                               this.LocalResourceFile),
			                               DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false
			                               , DotNetNuke.Security.SecurityAccessLevel.Edit, true, false
			                               }
			               };
			}
		}

		#endregion

        private void cmdDelete_Click(object sender, EventArgs e)
		{
            int itemId;
            bool success = false;

            if (Int32.TryParse(txtItemId.Text, out itemId))
            {
                //Need to figure out if the item exists, using GetItemTypeId since an Item must have a type
                if (Item.GetItemTypeId(itemId) != -1)
                {
                    //call the delete functionality.
                    Item.DeleteItem(itemId, PortalId);
                    success = true;
                }
            }

            lblResults.Visible = true;
            this.lblResults.Text = success ? Localization.GetString("Success", this.LocalResourceFile) : Localization.GetString("Failure", this.LocalResourceFile);
		}

	}
}

