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

namespace Engage.Dnn.Publish.Controls
{
	public partial class Breadcrumb :  ModuleBase, IActionable
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

		#region Event Handlers

		private void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				//check VI for null then set information
				if (!Page.IsPostBack)
				{
					StringBuilder sb = new StringBuilder(100);
					//sb.Append(loadParents(ItemId));
                    sb.Append(Util.Breadcrumb.ToString());
					lblBreadcrumb.Text = sb.ToString();
					if (lblBreadcrumb.Text.Length == 0) {
						lblYouAreHere.Visible = false;
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

        //private string loadParents(int categoryId)
        //{
        //    //get a list of all residential categories and their displayTabId
        //    DataSet ds = Category.GetParentCategory(categoryId, PortalId);
        //    DataTable dt = ds.Tables[0];

        //    DataView dv = dt.DefaultView;
        //    //dv.Sort = "SortOrder";
        //    StringBuilder resultSet = new StringBuilder();
        //    for (int i = 0; i < dv.Count; i++)
        //    {
        //        DataRow r = dv[i].Row;
        //        //string name = r["Name"].ToString();
        //        string categoryUrl = DotNetNuke.Common.Globals.NavigateURL(Convert.ToInt32(r["DisplayTabId"]), "", "&ControlType=categoryDisplay&itemid=" + r["ItemId"]);
        //        StringBuilder sb = new StringBuilder(20);
        //        sb.Append("&nbsp;&nbsp;<span class=\"seperator\">&gt;</span>&nbsp;<a class=\"navlink\" href=\"" + categoryUrl + "\">" + r["Name"] + "</a>\n");
        //        sb.Insert(0,loadParents(Convert.ToInt32(r["itemId"])));
        //        resultSet.Append(sb.ToString());
        //    }
        //    return resultSet;
			
        //}


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

