//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;
using DotNetNuke.UI.Utilities;

namespace Engage.Dnn.Publish.Controls
{
    public partial class AdminItemSearch : PublishSettingsBase
    {
        #region Protected Members
        private int _selectedItemId;
        private int _itemTypeId = -1;
        #endregion

        
        #region Event Handlers
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
            FillDropDowns();
        }

        private void InitializeComponent()
        {
            this.cboCategories.SelectedIndexChanged += this.cboCategories_SelectedIndexChanged;
            this.ddlArticleList.SelectedIndexChanged += this.ddlArticleList_SelectedIndexChanged;
            
        }

      
        #endregion


        private int selectedItemId;

        public int SelectedItemId
        {
            get
            {
                if(txtSelectedId.Text.Trim()!=string.Empty)
                return Convert.ToInt32(txtSelectedId.Text);
                return -1;
            }
            set
            {
                selectedItemId = value;
            }
        }

        public void FillDropDowns()
        {
            cboCategories.Items.Clear();
                ItemRelationship.DisplayCategoryHierarchy(cboCategories, -1, PortalId, false);

                ListItem li = new ListItem(Localization.GetString("ChooseOne", LocalSharedResourceFile), "-1");
                this.cboCategories.Items.Insert(0, li);
            //search for itemsetting
        }

        public void FillArticlesDropDown()
        {
            if (categoryId > -1)
            {
                ddlArticleList.DataSource = Article.GetArticles(categoryId, PortalId);
                ddlArticleList.DataBind();
            }
        }

        private void cboCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillArticlesDropDown();
        }


        private void ddlArticleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSelectedId.Text = ddlArticleList.SelectedValue;
            lblSelectedItem.Text = ddlArticleList.SelectedItem.Text;
        }

        public int categoryId
        {
            get
            {
                if (cboCategories.SelectedIndex > 0)
                    return Convert.ToInt32(cboCategories.SelectedValue);
                else return -1;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillArticlesDropDown();
        }
    }
}

