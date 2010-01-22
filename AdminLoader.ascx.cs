//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


namespace Engage.Dnn.Publish
{

    using System;
    using System.Collections.Specialized;
    using System.Data;
    using DotNetNuke.Services.Localization;
    using Admin;
    using Util;

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
            var adminControlKeys = new StringDictionary
                                       {
                                               {"CATEGORYLIST", "categorycontrols/CategoryList.ascx"},
                                               {"CATEGORYLISTING", "categorycontrols/CategoryListing.ascx"},
                                               {"CATEGORYSORT", "categorycontrols/CategorySort.ascx"},
                                               {"CATEGORYEDIT", "categorycontrols/CategoryEdit.ascx"},
                                               {"VERSIONSLIST", "controls/ItemVersions.ascx"},
                                               {"HELP", "admin/AdminInstructions.ascx"},
                                               {"ARTICLELIST", "articlecontrols/ArticleList.ascx"},
                                               {"ARTICLEEDIT", "articlecontrols/ArticleEdit.ascx"},
                                               {"ADMINMAIN", "Admin/AdminMain.ascx"},
                                               {"ADMINTOOLS", "Admin/AdminTools.ascx"},
                                               {"COMMENTLIST", "Admin/CommentList.ascx"},
                                               {"COMMENTEDIT", "Admin/CommentEdit.ascx"},
                                               {"ITEMCREATED", "Admin/ItemCreated.ascx"},
                                               {"AMSSETTINGS", "Admin/AdminSettings.ascx"},
                                               {"DELETEITEM", "Admin/DeleteItem.ascx"},
                                               {"SYNDICATION", "Admin/Syndication.ascx"},
                                               {"DEFAULT", "Admin/AdminMain.ascx"}
                                       };

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
        private string _controlToLoad;
        #endregion

        #region " Private Methods "

        private void ReadQueryString()
        {
            StringDictionary returnDict = GetAdminControlKeys();
            string adminTypeParam = Request.Params["adminType"];

            if (Utility.HasValue(adminTypeParam))
            {
                _controlToLoad = returnDict[adminTypeParam.ToUpperInvariant()];
            }
            else
            {
                //check to see if there are any categories, if not display an instructions control
                DataTable dt = Category.GetCategories(PortalId);
                _controlToLoad = dt.Rows.Count < 1 ? "Admin/AdminInstructions.ascx" : "articlecontrols/ArticleList.ascx";
            }

            if (!IsSetup)
            {
                _controlToLoad = "Admin/AdminSettings.ascx";
            }
        }

        private void LoadControlType()
        {

            var mb = (AdminMain)LoadControl("Admin/AdminMain.ascx");
            mb.ModuleConfiguration = ModuleConfiguration;

            mb.ID = System.IO.Path.GetFileNameWithoutExtension("Admin/AdminMain.ascx");
            phAdminControls.Controls.Add(mb);

            var amb = (ModuleBase)LoadControl(_controlToLoad);
            amb.ModuleConfiguration = ModuleConfiguration;
            amb.ID = System.IO.Path.GetFileNameWithoutExtension(_controlToLoad);
            phControls.Controls.Add(amb);
            //TODO: we need to be able to restrict which controls load, it's currently possible to get to the category edit page by changing the URL
        }
        #endregion
    }
}

