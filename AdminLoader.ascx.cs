//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Data;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Admin;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish
{
	public partial class AdminLoader : ModuleBase
	{
    	private static StringDictionary _adminControlKeys;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a property")]
        public static StringDictionary GetAdminControlKeys()
		{
			if (_adminControlKeys == null)
			{
                FillAdminControlKeys();
			}
			return _adminControlKeys;
		}

		private static void FillAdminControlKeys()
		{	
			StringDictionary adminControlKeys = new StringDictionary();
			
			adminControlKeys.Add("CATEGORYLIST","categorycontrols/CategoryList.ascx");
			adminControlKeys.Add("CATEGORYLISTING","categorycontrols/CategoryListing.ascx");

            adminControlKeys.Add("CATEGORYSORT", "categorycontrols/CategorySort.ascx");

			adminControlKeys.Add("CATEGORYEDIT","categorycontrols/CategoryEdit.ascx");

			adminControlKeys.Add("VERSIONSLIST","controls/ItemVersions.ascx");
            adminControlKeys.Add("HELP", "admin/AdminInstructions.ascx");

   			adminControlKeys.Add("ARTICLELIST","articlecontrols/ArticleList.ascx");
			adminControlKeys.Add("ARTICLEEDIT","articlecontrols/ArticleEdit.ascx");

			adminControlKeys.Add("ADMINMAIN","Admin/AdminMain.ascx");
            adminControlKeys.Add("ADMINTOOLS", "Admin/AdminTools.ascx");

            adminControlKeys.Add("COMMENTLIST", "Admin/CommentList.ascx");
            adminControlKeys.Add("COMMENTEDIT", "Admin/CommentEdit.ascx");
			
			adminControlKeys.Add("ITEMCREATED","Admin/ItemCreated.ascx");
			adminControlKeys.Add("AMSSETTINGS","Admin/AdminSettings.ascx");
			adminControlKeys.Add("DELETEITEM","Admin/DeleteItem.ascx");
            adminControlKeys.Add("SYNDICATION", "Admin/Syndication.ascx");
			adminControlKeys.Add("DEFAULT","Admin/AdminMain.ascx");

			_adminControlKeys = adminControlKeys;			
		}

		#region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            ReadQueryString();
            LoadControlType();

            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            //this.Load += new System.EventHandler(this.Page_Load);
            ModuleConfiguration.ModuleTitle = Localization.GetString("Title", LocalResourceFile);

        }

        #endregion
	
		#region " Private Members "
		private string controlToLoad;
		#endregion

		#region " Private Methods "

		private void ReadQueryString()
		{
			StringDictionary returnDict = GetAdminControlKeys();
			string adminTypeParam = Request.Params["adminType"];

			if(Utility.HasValue(adminTypeParam))
			{
				controlToLoad = returnDict[adminTypeParam.ToUpperInvariant()];
			}
			else
			{

                //check to see if there are any categories, if not display an instructions control
                DataTable dt = Category.GetCategories(PortalId);
                if (dt.Rows.Count < 1)
                {
                    controlToLoad = "Admin/AdminInstructions.ascx";
                }
                else
                {
                    controlToLoad = "articlecontrols/ArticleList.ascx";
                }
			}
						
			if (!IsSetup)
			{
				controlToLoad = "Admin/AdminSettings.ascx";
			}
		}

		private void LoadControlType()
		{

            AdminMain mb = (AdminMain)LoadControl("Admin/AdminMain.ascx");
			mb.ModuleConfiguration = ModuleConfiguration;
            
			mb.ID = System.IO.Path.GetFileNameWithoutExtension("Admin/AdminMain.ascx");
			phAdminControls.Controls.Add(mb);

            ModuleBase amb;
            amb = (ModuleBase)LoadControl(controlToLoad);
            amb.ModuleConfiguration = ModuleConfiguration;
            amb.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
            phControls.Controls.Add(amb);
		}
		#endregion
	}
}

