//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;

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
			btnShowRelatedItem.Click += BtnShowRelatedItemClick;
			Load += Page_Load;
		}

	    public bool LinksPopulated
	    {
	        get;
	        set;
	    }


	    private void Page_Load(object sender, EventArgs e)
	    {
	        if (e == null)
	        {
	            throw new ArgumentNullException("e");
	        }
	        try 
			{
				btnShowRelatedItem.Visible = false;
				divRelatedLinks.Visible = false;
				
				var related = new List<Article>(VersionInfoObject.GetRelatedArticles(PortalId));

                ItemVersionSetting parentRelationshipSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "ArticleSettings", "IncludeParentCategoryArticles", PortalId);
                if (parentRelationshipSetting != null && Convert.ToBoolean(parentRelationshipSetting.PropertyValue, CultureInfo.InvariantCulture))
                {
                    int parentCategoryId = Category.GetParentCategory(VersionInfoObject.ItemId, PortalId);
                    if (parentCategoryId > 0)
                    {
                        //get all articles in the same category, then removes this current article from that list.  BD
                        List<Article> categoryArticles = Category.GetCategoryArticles(parentCategoryId, PortalId);
                        categoryArticles.RemoveAll(a => a.ItemId == VersionInfoObject.ItemId);
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
				var actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				return actions;
			}
		}


		#endregion

		private void BtnShowRelatedItemClick(object sender, EventArgs e)
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
                var lnkRelatedArticle = (HyperLink)e.Item.FindControl("lnkRelatedArticle");

                ////This code makes sure that an article that is disabled does not get a link.
                if (e.Item.ItemType == ListItemType.Item)
                {
                    var a = (Article)e.Item.DataItem;
                    int linkedItemId = a.ItemId;

                    if (Util.Utility.IsDisabled(linkedItemId, PortalId))
                    {
                        lnkRelatedArticle.NavigateUrl = "";

                    }
                }
            }
        }

	}
}

