//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
    using DotNetNuke.Security;
    using Util;

    /// <summary>
    /// 
    /// </summary>
    public class PublishSettingsBase : ModuleSettingsBase
    {
        public static string DesktopModuleFolderName
        {
            get { return Utility.DesktopModuleFolderName; }
        }
    }
}