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
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Exceptions;
    using Util;

    public partial class ItemDisplay : ModuleBase, IActionable
    {

        private string controlToLoad;

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
          
            base.OnInit(e);

            ReadItemType();
            LoadControlType();

            //add on a query string param so that we can grab ALL content, not just for this moduleId. hk
            string title = Localization.GetString(ModuleActionType.ExportModule, Localization.GlobalResourceFile);
            foreach (ModuleAction action in Actions)
            {
                if (action.Title == title)
                {
                    action.Url = action.Url + "?all=1";
                    break;
                }

            }
        }

        #endregion
        
        #region Private Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Code paths are easy to understand, test, and maintain")]
        private void ReadItemType()
        {


            //here we are looking to see if any old publish URLs are being used, if so redirect to the new URL
            object oid = Request.Params["aid"];
            if (oid != null)
            {
                //made this a 301 redirect for better SEO
                string href = ApplicationUrl + DesktopModuleFolderName + "itemlink.aspx?aid=" + oid;
                Response.Status = "301 Moved Permanently";
                Response.RedirectLocation = href;

                //Response.Redirect(ApplicationUrl + DesktopModuleFolderName + "itemlink.aspx?aid=" + oid.ToString(), false);
            }

            string displayType = string.Empty;
            if (Settings.Contains("DisplayType"))
            {
                displayType = Settings["DisplayType"].ToString();
            }

            ItemType t = TypeOfItem;

            if (t != null)
            {
                if (t.Name.Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
                {
                    this.controlToLoad = displayType == "ArticleDisplay" ? "ArticleControls/ArticleDisplay.ascx" : "ArticleControls/ArticleDisplay.ascx";
                }
                else if (t.Name.Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    if (displayType == "CustomDisplay")
                    {
                        controlToLoad = "Controls/CustomDisplay.ascx";
                    }
                    else if (displayType == "CategoryDisplay")
                    {
                        controlToLoad = "CategoryControls/CategoryDisplay.ascx";
                    }
                    else if (displayType == "CategoryFeatureDisplay")
                    {
                        controlToLoad = "CategoryControls/CategoryFeature.ascx";
                    }
                    else if (displayType == "CategorySearch")
                    {
                        controlToLoad = "CategoryControls/CategorySearch.ascx";
                    }
                    else if (displayType == "ItemListing")
                    {
                        controlToLoad = "Controls/ItemListing.ascx";
                    }
                    else if (displayType == "CategoryNLevels")
                    {
                        controlToLoad = "CategoryControls/CategoryNLevels.ascx";
                    }
                    else
                    {
                        controlToLoad = "Controls/CustomDisplay.ascx";
                    }
                }

                else if (t.Name.Equals("TOPLEVELCATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    if (displayType == "CustomDisplay")
                    {
                        controlToLoad = "Controls/CustomDisplay.ascx";
                    }
                    else if (displayType == "CategoryDisplay")
                    {
                        controlToLoad = "CategoryControls/CategoryDisplay.ascx";
                    }
                    else if (displayType == "CategoryFeatureDisplay")
                    {
                        controlToLoad = "CategoryControls/CategoryFeature.ascx";
                    }
                    else if (displayType == "CategorySearch")
                    {
                        controlToLoad = "CategoryControls/CategorySearch.ascx";
                    }
                    else if (displayType == "ItemListing")
                    {
                        controlToLoad = "Controls/ItemListing.ascx";
                    }
                    else if (displayType == "CategoryNLevels")
                    {
                        controlToLoad = "CategoryControls/CategoryNLevels.ascx";
                    }
                    else
                    {
                        controlToLoad = "CategoryControls/CategoryFeature.ascx";
                    }
                }
                else
                {
                    if (displayType == "CustomDisplay")
                    {
                        controlToLoad = "Controls/CustomDisplay.ascx";
                    }
                    else if (displayType == "ItemListing")
                    {
                        controlToLoad = "Controls/ItemListing.ascx";
                    }
                    else if (displayType == "CategorySearch")
                    {
                        controlToLoad = "CategoryControls/CategorySearch.ascx";
                    }
                    else
                    {
                        controlToLoad = "ArticleControls/ArticleDisplay.ascx";
                    }
                }
            }
            else
            {
                if (displayType == "CustomDisplay")
                {
                    controlToLoad = "Controls/CustomDisplay.ascx";
                }
                else if (displayType == "ItemListing")
                {
                    controlToLoad = "Controls/ItemListing.ascx";
                }
                else if (displayType == "CategorySearch")
                {
                    controlToLoad = "CategoryControls/CategorySearch.ascx";
                }
                else
                {
                    controlToLoad = "ArticleControls/ArticleDisplay.ascx";
                }
            }


        }

        private void LoadControlType()
        {
            try
            {
                ModuleBase mb = (ModuleBase)LoadControl(controlToLoad);
                mb.ModuleConfiguration = ModuleConfiguration;
                mb.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
                phControls.Controls.Add(mb);
                
                //Don't show the menu if we're in VIEW mode


                //Don't show the menu at the top if the control is not configured to display anything. hk
                if ((IsAdmin || IsAuthor) && mb.ItemId != -1 && IsEditable)
                {
                    //Don't show the menu if we're on a category search, itemlisting or CategoryNLevels control
                    string displayType = string.Empty;
                    if (Settings.Contains("DisplayType"))
                    {
                        displayType = Settings["DisplayType"].ToString();
                    }

                    if (displayType != "CategorySearch" && displayType != "ItemListing" && displayType != "CategoryNLevels")
                    {
                        const string adminControlToLoad = "Admin/AdminMenu.ascx";
                        ModuleBase mbl = (ModuleBase)LoadControl(adminControlToLoad);
                        mbl.ModuleConfiguration = ModuleConfiguration;
                        mbl.ID = System.IO.Path.GetFileNameWithoutExtension(adminControlToLoad);
                        phAdminControls.Controls.Add(mbl);
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString("Administration", this.LocalResourceFile), "", "", "", EditUrl(Utility.AdminContainer), false, SecurityAccessLevel.Edit, true, false);
                return actions;
            }
        }



    }
}

