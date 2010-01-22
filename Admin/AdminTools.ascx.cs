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
    using System.Collections;
    using DotNetNuke.Services.Localization;

	public partial class AdminTools : ModuleBase
	{
        private static IDictionary Idict;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a property")]
        public static IDictionary GetDictionary()
        {
            if (Idict == null)
            {
                FillDictionary();
            }
            return Idict;
        }

        private static void FillDictionary()
        {
            var ht = new Hashtable
                         {
                                 {"DESCRIPTIONREPLACE", "tools/DescriptionReplace.ascx"},
                                 {"DASHBOARD", "tools/Dashboard.ascx"},
                                 {"ITEMVIEWREPORT", "tools/ItemViewReport.ascx"},
                                 {"RESETDISPLAYPAGE", "tools/ResetDisplayPage.ascx"}
                         };

            Idict = ht;
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

        #region " Private Members "
        private string controlToLoad;
        #endregion

        #region " Private Methods "

        private void ReadQueryString()
        {

            IDictionary returnDict = GetDictionary();
            object o = Request.Params["tool"];

            this.controlToLoad = o != null ? returnDict[o.ToString().ToUpperInvariant()].ToString() : returnDict["DASHBOARD"].ToString();

        }

        private void LoadControlType()
        {
            var amb = (ModuleBase)LoadControl(controlToLoad);
            amb.ModuleConfiguration = ModuleConfiguration;
            amb.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
            phAdminTools.Controls.Add(amb);
        }

        #endregion


/*
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
*/

        #endregion
    }
}

