// <copyright file="TagCloudOptions.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Tags
{
    using System;
    using System.Globalization;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;

    public partial class TagCloudOptions : ModuleSettingsBase
    {
        private bool PopularTagCount
        {
            get
            {
                object o = this.Settings["tcPopularTagBool"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "tcPopularTagBool", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public override void LoadSettings()
        {
            try
            {
                this.chkLimitTagCount.Checked = this.PopularTagCount;
                
                var moduleCssUrl = this.ResolveUrl("Module.css");
                PageBase.RegisterStyleSheet(this.Page, moduleCssUrl, true);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            if (this.Page.IsValid)
            {
                this.PopularTagCount = this.chkLimitTagCount.Checked;
            }
        }
    }
}