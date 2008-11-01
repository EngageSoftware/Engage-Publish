//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace Engage.Dnn.Publish.Controls
{
    public partial class AdminItemSearch : PublishSettingsBase
    {
        
        #region Event Handlers
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
            if (ddlCategories.Items.Count == 0) FillDropDowns();  //if articleid was already stored the setter for SelectedArticleid hasn't been called.
        }

        private void InitializeComponent()
        {
            this.ddlCategories.SelectedIndexChanged += this.ddlCategories_SelectedIndexChanged;
        }

        private void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillArticlesDropDown();
        }

        #endregion

        public int SelectedArticleId
        {
            get
            {
                if (this.ddlArticleList.SelectedValue != null)
                {
                    return Convert.ToInt32(this.ddlArticleList.SelectedValue);
                }
                return -1;
            }
            set
            {
                if (ddlCategories.Items.Count == 0) FillDropDowns();
                Article article = Article.GetArticle(value, PortalId);
                if (article != null)
                {
                    int categoryId = article.GetParentCategoryId();
                    //find category
                    ListItem li = this.ddlCategories.Items.FindByValue(categoryId.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                        FillArticlesDropDown();
                    }
                    //find article
                    li = this.ddlArticleList.Items.FindByValue(value.ToString());
                    if (li != null)
                        li.Selected = true;
                }
            }
        }

        public void FillDropDowns()
        {
            this.ddlCategories.Items.Clear();
            ItemRelationship.DisplayCategoryHierarchy(this.ddlCategories, -1, PortalId, false);

            ListItem li = new ListItem(Localization.GetString("ChooseOne", LocalSharedResourceFile), "-1");
            this.ddlCategories.Items.Insert(0, li);
        }

        public void FillArticlesDropDown()
        {
            if (CategoryId > -1)
            {
                ddlArticleList.DataSource = Article.GetArticles(CategoryId, PortalId);
                ddlArticleList.DataBind();
            }
        }

        private int CategoryId
        {
            get
            {
                if (this.ddlCategories.SelectedIndex > 0)
                    return Convert.ToInt32(this.ddlCategories.SelectedValue);
                return -1;
            }
        }

    }
}

