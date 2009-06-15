//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Publish.Data;
using System.Globalization;
using System.Collections.Generic;

namespace Engage.Dnn.Publish.Controls
{
	public partial class RelatedArticleLinks :  RelatedArticleLinksBase, IActionable
	{
		#region Event Handlers
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);

            BindItemData();
		}
		
		private void InitializeComponent()
		{
			this.btnShowRelatedItem.Click += new System.EventHandler(this.btnShowRelatedItem_Click);
			this.Load += new System.EventHandler(this.Page_Load);
		}

        private bool linksPopulated;
        public bool LinksPopulated
        {
            get
            {
                return linksPopulated;
            }
            set
            {
                linksPopulated = value;
            }
        }


		private void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				btnShowRelatedItem.Visible = false;
				divRelatedLinks.Visible = false;
				
				List<Article> related = new List<Article>(VersionInfoObject.GetRelatedArticles(PortalId));

                ItemVersionSetting parentRelationshipSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "ArticleSettings", "IncludeParentCategoryArticles", PortalId);
                if (parentRelationshipSetting != null && Convert.ToBoolean(parentRelationshipSetting.PropertyValue, CultureInfo.InvariantCulture))
                {
                    int parentCategoryId = Category.GetParentCategory(VersionInfoObject.ItemId, PortalId);
                    if (parentCategoryId > 0)
                    {
                        //get all articles in the same category, then removes this current article from that list.  BD
                        List<Article> categoryArticles = Category.GetCategoryArticles(parentCategoryId, PortalId);
                        categoryArticles.RemoveAll(delegate(Article a)
                            {
                                return a.ItemId == VersionInfoObject.ItemId;
                            });
                        related.AddRange(categoryArticles);
                    }
                }

				if (related.Count < 1) 
				{
					btnShowRelatedItem.Visible = false;	
					divRelatedLinks.Visible = false;
                    LinksPopulated = false;
				} 
				else
				{
                    lstItems.DataSource = related;
                    lstItems.DataBind();
                    divRelatedLinks.Visible = true;
                    LinksPopulated = true;
				}
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

		#region Optional Interfaces

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
		{
			get 
			{
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				return Actions;
			}
		}


		#endregion

		private void btnShowRelatedItem_Click(object sender, System.EventArgs e)
		{
			divRelatedLinks.Visible=true;
		
			Article[] related = VersionInfoObject.GetRelatedArticles(PortalId);
			if (related.Length == 0) 
			{
			//what to do?
			} 
			else 
			{	
				lstItems.DataSource = related;
				lstItems.DataBind();
			}
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void lstItems_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e != null)
            {
                HyperLink lnkRelatedArticle = (HyperLink)e.Item.FindControl("lnkRelatedArticle");

                ////This code makes sure that an article that is disabled does not get a link.
                if (e.Item.ItemType == ListItemType.Item)
                {
                    Article a = (Article)e.Item.DataItem;
                    int linkedItemId = a.ItemId;

                    if (Engage.Dnn.Publish.Util.Utility.IsDisabled(linkedItemId, PortalId))
                    {
                        lnkRelatedArticle.NavigateUrl = "";

                    }
                }
            }
        }

	}
}

