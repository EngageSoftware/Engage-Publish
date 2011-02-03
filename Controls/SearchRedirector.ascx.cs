// <copyright file="SearchRedirector.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Web.UI;

    using DotNetNuke.Services.Exceptions;

    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// Not used, only around for old installations that have this module on the site somewhere...
    /// </summary>
    public partial class SearchRedirector : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.ItemId > -1)
                {
                    this.Response.Redirect(UrlGenerator.GetItemLinkUrl(this.ItemId, this.PortalId), true);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}