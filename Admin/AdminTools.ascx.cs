//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System.Collections;
using System;
using DotNetNuke.Services.Localization;

namespace Engage.Dnn.Publish.Admin
{
	public partial class AdminTools : ModuleBase
	{
        private static IDictionary idict;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a property")]
        public static IDictionary GetDictionary()
        {
            if (idict == null)
            {
                FillDictionary();
            }
            return idict;
        }

        private static void FillDictionary()
        {
            Hashtable ht = new Hashtable();

            ht.Add("DESCRIPTIONREPLACE", "tools/DescriptionReplace.ascx");
            ht.Add("DASHBOARD", "tools/Dashboard.ascx");
            ht.Add("ITEMVIEWREPORT", "tools/ItemViewReport.ascx");
            
            idict = ht;
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

            if (o != null)
            {
                controlToLoad = returnDict[o.ToString().ToUpperInvariant()].ToString();
            }
            else
            {
                controlToLoad = returnDict["DASHBOARD"].ToString();

            }

        }

        private void LoadControlType()
        {
            ModuleBase amb = (ModuleBase)LoadControl(controlToLoad);
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

