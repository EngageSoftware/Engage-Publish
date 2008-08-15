//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;

namespace Engage.Dnn.Publish.Admin
{
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

		private void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				//check VI for null then set information
				if (!Page.IsPostBack)
				{
					//ItemId
					ClientAPI.AddButtonConfirm(cmdDelete, Localization.GetString("DeleteConfirmation", LocalResourceFile));
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

		#region Optional Interfaces

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
		{
			get 
			{
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				Actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
				return Actions;
			}
		}

		#endregion

        private void cmdDelete_Click(object sender, System.EventArgs e)
		{
            int itemId;
            bool success = false;

            if (Int32.TryParse(txtItemId.Text, out itemId))
            {
                //Need to figure out if the item exists, using GetItemTypeId since an Item must have a type
                if (Item.GetItemTypeId(itemId) != -1)
                {
                    //call the delete functionality.
                    Item.DeleteItem(itemId);
                    success = true;
                }
            }

            lblResults.Visible = true;
            if (success)
            {
                lblResults.Text = Localization.GetString("Success", LocalResourceFile);
                
            }
            else
            {
                lblResults.Text = Localization.GetString("Failure", LocalResourceFile);
            }
		}

	}
}

