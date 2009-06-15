//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System.Globalization;

namespace Engage.Dnn.Publish.TextHtml
{
    public partial class Settings : ModuleSettingsBase
    {   
        private ModuleSettingsBase currentSettingsBase;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.Load += ItemDisplayOptions_Load;
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }
        }

        protected void ItemDisplayOptions_Load(object sender, EventArgs e)
        {
            try
            {
                //by now ViewState has been restored so we can set the Settings control.
                LoadSettings();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        
        public override void UpdateSettings()
        {
            try
            {
                ModuleController modules = new ModuleController();
                    modules.UpdateTabModuleSetting(this.TabModuleId, "Template", txtTemplate.Text.Trim());
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void LoadSettings()
        {
            base.LoadSettings();
            try
            {
                if (Page.IsPostBack == false)
                {
                    SetOptions();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetOptions()
        {
            object o = Settings["Template"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                txtTemplate.Text = o.ToString();                
            }
        }
    }
}

