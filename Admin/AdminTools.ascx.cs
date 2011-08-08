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
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using DotNetNuke.Services.Localization;

    public partial class AdminTools : ModuleBase
    {
        private static IDictionary Idict;

        private string controlToLoad;

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a property")]
        public static IDictionary GetDictionary()
        {
            if (Idict == null)
            {
                FillDictionary();
            }

            return Idict;
        }

        protected override void OnInit(EventArgs e)
        {
            // this.Load += new System.EventHandler(this.Page_Load);
            this.ModuleConfiguration.ModuleTitle = Localization.GetString("Title", this.LocalResourceFile);
            this.ReadQueryString();
            this.LoadControlType();

            base.OnInit(e);
        }

        private static void FillDictionary()
        {
            var ht = new Hashtable
                {
                    {
                        "DESCRIPTIONREPLACE", "tools/DescriptionReplace.ascx"
                        }, 
                    {
                        "DASHBOARD", "tools/Dashboard.ascx"
                        }, 
                    {
                        "ITEMVIEWREPORT", "tools/ItemViewReport.ascx"
                        }, 
                    {
                        "RESETDISPLAYPAGE", "tools/ResetDisplayPage.ascx"
                        }
                };

            Idict = ht;
        }

        private void LoadControlType()
        {
            var amb = (ModuleBase)this.LoadControl(this.controlToLoad);
            amb.ModuleConfiguration = this.ModuleConfiguration;
            amb.ID = Path.GetFileNameWithoutExtension(this.controlToLoad);
            this.phAdminTools.Controls.Add(amb);
        }

        private void ReadQueryString()
        {
            IDictionary returnDict = GetDictionary();
            object o = this.Request.Params["tool"];

            this.controlToLoad = o != null ? returnDict[o.ToString().ToUpperInvariant()].ToString() : returnDict["DASHBOARD"].ToString();
        }

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
    }
}