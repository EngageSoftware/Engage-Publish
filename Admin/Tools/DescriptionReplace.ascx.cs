//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


using System;
using System.Globalization;
using DotNetNuke.Services.Localization;
using System.Data;
using Engage.Dnn.Publish.Util;


namespace Engage.Dnn.Publish.Admin.Tools
{
    public partial class DescriptionReplace : ModuleBase
    {
        //#region Event Handlers

        //override protected void OnInit(EventArgs e)
        //{
        //    Load += Page_Load;
        //    base.OnInit(e);
        //}

        //private void Page_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //    }
        //    catch (Exception exc)
        //    {
        //        Exceptions.ProcessModuleLoadException(this, exc);
        //    }
        //}

        //#endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "lb")]
        protected void lbReplace_Click(object sender, EventArgs e)
        {
            
            int articleCount = 0;
            int articleUpdate = 0;
            DataTable allArticles = Article.GetArticles(PortalId);
            foreach (DataRow dr in allArticles.Rows)
            {
                articleCount++;
                Article a = Article.GetArticle(Convert.ToInt32(dr["itemId"], CultureInfo.InvariantCulture), PortalId, true, true, true);
                if (a != null)
                {
                    //if our article is over 8k characters be sure to trim it
                    if (!Utility.HasValue(a.Description) || !Utility.HasValue(a.MetaDescription))
                    {
                        string description = DotNetNuke.Common.Utilities.HtmlUtils.StripTags(a.ArticleText, false);

                        if (!Utility.HasValue(a.MetaDescription))
                        a.MetaDescription = Utility.TrimDescription(399, description);

                        if (!Utility.HasValue(a.Description))
                            //TODO: localize the end of the description 
                            a.Description = Utility.TrimDescription(3997, description) + "...";// description + "...";
                        
                        a.UpdateDescription();
                        articleUpdate++;
                    }
                }
            }

            //Utility.ClearPublishCache(PortalId);
            //X articles updated out of Y
            lblOutput.Text = String.Format(CultureInfo.CurrentCulture, Localization.GetString("ArticleUpdate", LocalResourceFile).ToString(), articleUpdate, articleCount);

        }

       
    }
}

