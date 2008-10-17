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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security.Roles;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Security;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.CategoryControls
{
    public partial class CategoryPermissions : ModuleBase
    {
        private int categoryId;
        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.imgAdd.Click += new System.Web.UI.ImageClickEventHandler(this.imgAdd_Click);
            this.imgRemove.Click += new System.Web.UI.ImageClickEventHandler(this.imgRemove_Click);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion

        #region Event Handlers

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //get the roles assigned to this category
                    LoadAssignedRoles();
                    LoadAllRoles();

                    //					//get the relationshipId and populate relationships
                    //					ArrayList alItemRelationships = new ArrayList();
                    //
                    //					alItemRelationships = ItemRelationship.GetItemRelationships(VersionInfoObject.ItemId, VersionInfoObject.ItemVersionId, CreateRelationshipTypeId);
                    //					foreach (ItemRelationship ir in alItemRelationships)
                    //					{
                    //						//create a new item and add it to the selection list.
                    //						//
                    //
                    //						//load item
                    //						//Item.
                    //						string parentName = ItemType.GetItemName(ir.ParentItemId);
                    //												
                    //						ListItem li = new ListItem(ir.ParentItemId + "-" + parentName, ir.ParentItemId.ToString());
                    //						lstSelectedItems.Items.Add(li);
                    //					}
                    //
                    //					if (this.enableSortOrder) {
                    //						tdSortOrderControls.Visible = true;
                    //					}
                }
                else
                {
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        //		public int ItemTypeId
        //		{
        //			get {return this.itemTypeId;}
        //
        //			set 
        //			{	
        //				this.itemTypeId = value;
        //				UpdateAvailableRoles();
        //			}
        //		}


        //		public int[] GetSelectedRoleIds()
        //		{
        //			ArrayList al = new ArrayList();
        //			for (int i = 0; i < this.lstSelectedItems.Items.Count; i++)
        //			{
        //				ListItem li = this.lstSelectedItems.Items[i];
        //				al.Add(Convert.ToInt32(li.Value));
        //			}
        //
        //			return (int[]) al.ToArray(typeof(int));
        //		}

        //		public void AddToSelectedRoles(Item i)
        //		{
        //			ListItem li = new ListItem(i.ItemId.ToString() + "-" + i.Name, i.ItemId.ToString());
        //			this.lstSelectedItems.Items.Add(li);
        //		}

        //private void UpdateAvailableRoles()
        //{
        //    this.lstItems.DataSource = Item.GetItems(this.itemTypeId, PortalId);
        //    this.lstItems.DataTextField = "listName";
        //    this.lstItems.DataValueField = "itemId";
        //    this.DataBind();
        //}

        private void imgAdd_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //check existing items, don't add again if already inserted.
            foreach (ListItem sl in this.lstItems.Items)
            {
                if (sl.Selected && !RoleIdExists(sl.Value))
                {
                    //get the item selected
                    ListItem selected = new ListItem(sl.Text, sl.Value);
                    this.lstSelectedItems.Items.Add(selected);
                }
            }
        }

        //return true if item already in the list box
        private bool RoleIdExists(string itemId)
        {
            foreach (ListItem i in this.lstSelectedItems.Items)
            {
                if (i.Value == itemId) return true;
            }

            return false;
        }

        private void imgRemove_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ListItemCollection lc = new ListItemCollection();
            foreach (ListItem sl in this.lstSelectedItems.Items)
            {
                if (sl.Selected) lc.Add(sl);
            }

            foreach (ListItem sl in lc)
            {
                this.lstSelectedItems.Items.Remove(sl);
            }
        }

        public void Save()
        {
            DataProvider dp = DataProvider.Instance();
            int permissionId = PermissionType.View.GetId();

            // TODO: wrap delete and update in a transaction
            // should we just delete all, and then reinsert?, sounds easier
            dp.DeletePermissions(this.categoryId);

            foreach (ListItem li in this.lstSelectedItems.Items)
            {
                dp.InsertPermission(this.categoryId, Convert.ToInt32(li.Value, CultureInfo.InvariantCulture), permissionId, UserId);
            }
        }

        private void LoadAllRoles()
        {
            RoleController rc = new DotNetNuke.Security.Roles.RoleController();
            ArrayList roles = rc.GetRoles();

            foreach (RoleInfo role in roles)
            {
                if (role.PortalID == PortalId)
                {
                    ListItem li = new ListItem(role.RoleName, role.RoleID.ToString(CultureInfo.InvariantCulture));
                    this.lstItems.Items.Add(li);
                }
            }
        }

        private void LoadAssignedRoles()
        {
            //no need to look for new categories
            if (this.categoryId != -1)
            {
                DataTable dt = DataProvider.Instance().GetAssignedRoles(this.categoryId);
                foreach (DataRow row in dt.Rows)
                {
                    ListItem li = new ListItem(row["RoleName"].ToString(), row["RoleId"].ToString());
                    this.lstSelectedItems.Items.Add(li);
                }
            }
        }

        //		private void imgUp_Click(object sender, ImageClickEventArgs e)
        //		{
        //			int index = this.lstSelectedItems.SelectedIndex;
        //
        //			if (index == 0) { return; }
        //
        //			ListItem li = this.lstSelectedItems.SelectedItem;
        //			this.lstSelectedItems.Items.Remove(li);
        //			this.lstSelectedItems.Items.Insert(index - 1, li);
        //		}
        //
        //		private void imgDown_Click(object sender, ImageClickEventArgs e)
        //		{
        //			int index = this.lstSelectedItems.SelectedIndex;
        //			if (index == this.lstSelectedItems.Items.Count - 1) return;
        //
        //			ListItem li = this.lstSelectedItems.SelectedItem;
        //			this.lstSelectedItems.Items.Remove(li);
        //			this.lstSelectedItems.Items.Insert(index + 1, li);
        //		}

        public int CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
    }

}

