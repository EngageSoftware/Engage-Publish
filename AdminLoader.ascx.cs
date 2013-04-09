// <copyright file="AdminLoader.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Admin;
    using Engage.Dnn.Publish.Util;

    public partial class AdminLoader : ModuleBase
    {
        private static StringDictionary _adminControlKeys;

        private string _controlToLoad;

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a property")]
        public static StringDictionary GetAdminControlKeys()
        {
            if (_adminControlKeys == null)
            {
                FillAdminControlKeys();
            }

            return _adminControlKeys;
        }

        protected override void OnInit(EventArgs e)
        {
            this.ModuleConfiguration.ModuleTitle = Localization.GetString("Title", this.LocalResourceFile);
            this.ReadQueryString();
            this.LoadControlType();

            base.OnInit(e);
        }

        private static void FillAdminControlKeys()
        {
            var adminControlKeys = new StringDictionary
                {
                    {
                        "CATEGORYLIST", "categorycontrols/CategoryList.ascx"
                        }, 
                    {
                        "CATEGORYLISTING", "categorycontrols/CategoryListing.ascx"
                        }, 
                    {
                        "CATEGORYSORT", "categorycontrols/CategorySort.ascx"
                        }, 
                    {
                        "CATEGORYEDIT", "categorycontrols/CategoryEdit.ascx"
                        }, 
                    {
                        "VERSIONSLIST", "controls/ItemVersions.ascx"
                        }, 
                    {
                        "HELP", "admin/AdminInstructions.ascx"
                        }, 
                    {
                        "ARTICLELIST", "articlecontrols/ArticleList.ascx"
                        }, 
                    {
                        "ARTICLEEDIT", "articlecontrols/ArticleEdit.ascx"
                        }, 
                    {
                        "ADMINMAIN", "Admin/AdminMain.ascx"
                        }, 
                    {
                        "ADMINTOOLS", "Admin/AdminTools.ascx"
                        }, 
                    {
                        "COMMENTLIST", "Admin/CommentList.ascx"
                        }, 
                    {
                        "COMMENTEDIT", "Admin/CommentEdit.ascx"
                        }, 
                    {
                        "ITEMCREATED", "Admin/ItemCreated.ascx"
                        }, 
                    {
                        "AMSSETTINGS", "Admin/AdminSettings.ascx"
                        }, 
                    {
                        "DELETEITEM", "Admin/DeleteItem.ascx"
                        }, 
                    {
                        "SYNDICATION", "Admin/Syndication.ascx"
                        }, 
                    {
                        "DEFAULT", "Admin/AdminMain.ascx"
                        }
                };

            _adminControlKeys = adminControlKeys;
        }

        private void LoadControlType()
        {
            var mb = (AdminMain)this.LoadControl("Admin/AdminMain.ascx");
            mb.ModuleConfiguration = this.ModuleConfiguration;

            mb.ID = Path.GetFileNameWithoutExtension("Admin/AdminMain.ascx");
            this.phAdminControls.Controls.Add(mb);

            var amb = (ModuleBase)this.LoadControl(this._controlToLoad);
            amb.ModuleConfiguration = this.ModuleConfiguration;
            amb.ID = Path.GetFileNameWithoutExtension(this._controlToLoad);
            this.phControls.Controls.Add(amb);

            // TODO: we need to be able to restrict which controls load, it's currently possible to get to the category edit page by changing the URL
        }

        private void ReadQueryString()
        {
            StringDictionary returnDict = GetAdminControlKeys();
            string adminTypeParam = this.Request.Params["adminType"];

            if (Engage.Utility.HasValue(adminTypeParam))
            {
                this._controlToLoad = returnDict[adminTypeParam.ToUpperInvariant()];
            }
            else
            {
                // check to see if there are any categories, if not display an instructions control
                DataTable dt = Category.GetCategories(this.PortalId);
                this._controlToLoad = dt.Rows.Count < 1 ? "Admin/AdminInstructions.ascx" : "articlecontrols/ArticleList.ascx";
            }

            if (!this.IsSetup)
            {
                this._controlToLoad = "Admin/AdminSettings.ascx";
            }
        }
    }
}