// <copyright file="CategoryPermissions.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.CategoryControls
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using DotNetNuke.Security.Roles;
    using DotNetNuke.Services.Exceptions;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Security;

    public partial class CategoryPermissions : ModuleBase
    {
        private int _categoryId;

        public int CategoryId
        {
            get { return this._categoryId; }
            set { this._categoryId = value; }
        }

        public void Save()
        {
            DataProvider dp = DataProvider.Instance();
            int permissionId = PermissionType.View.GetId();

            // should we just delete all, and then reinsert?, sounds easier
            dp.DeletePermissions(this._categoryId);

            foreach (ListItem li in this.lstSelectedItems.Items)
            {
                dp.InsertPermission(this._categoryId, Convert.ToInt32(li.Value, CultureInfo.InvariantCulture), permissionId, this.UserId);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.imgAdd.Click += this.imgAdd_Click;
            this.imgRemove.Click += this.imgRemove_Click;
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void LoadAllRoles()
        {
            var rc = new RoleController();
            ArrayList roles = rc.GetRoles();

            foreach (RoleInfo role in roles)
            {
                if (role.PortalID == this.PortalId)
                {
                    var li = new ListItem(role.RoleName, role.RoleID.ToString(CultureInfo.InvariantCulture));
                    this.lstItems.Items.Add(li);
                }
            }
        }

        private void LoadAssignedRoles()
        {
            // no need to look for new categories
            if (this._categoryId != -1)
            {
                DataTable dt = DataProvider.Instance().GetAssignedRoles(this._categoryId);
                foreach (DataRow row in dt.Rows)
                {
                    var li = new ListItem(row["RoleName"].ToString(), row["RoleId"].ToString());
                    this.lstSelectedItems.Items.Add(li);
                }
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    // get the roles assigned to this category
                    this.LoadAssignedRoles();
                    this.LoadAllRoles();

                    // 					//get the relationshipId and populate relationships
                    // 					ArrayList alItemRelationships = new ArrayList();
                    // 					alItemRelationships = ItemRelationship.GetItemRelationships(VersionInfoObject.ItemId, VersionInfoObject.ItemVersionId, CreateRelationshipTypeId);
                    // 					foreach (ItemRelationship ir in alItemRelationships)
                    // 					{
                    // 						//create a new item and add it to the selection list.
                    // 						//
                    // 						//load item
                    // 						//Item.
                    // 						string parentName = ItemType.GetItemName(ir.ParentItemId);
                    // 						ListItem li = new ListItem(ir.ParentItemId + "-" + parentName, ir.ParentItemId.ToString());
                    // 						lstSelectedItems.Items.Add(li);
                    // 					}
                    // 					if (this.enableSortOrder) {
                    // 						tdSortOrderControls.Visible = true;
                    // 					}
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        // 		public int ItemTypeId
        // 		{
        // 			get {return this.itemTypeId;}
        // 			set 
        // 			{	
        // 				this.itemTypeId = value;
        // 				UpdateAvailableRoles();
        // 			}
        // 		}

        // 		public int[] GetSelectedRoleIds()
        // 		{
        // 			ArrayList al = new ArrayList();
        // 			for (int i = 0; i < this.lstSelectedItems.Items.Count; i++)
        // 			{
        // 				ListItem li = this.lstSelectedItems.Items[i];
        // 				al.Add(Convert.ToInt32(li.Value));
        // 			}
        // 			return (int[]) al.ToArray(typeof(int));
        // 		}

        // 		public void AddToSelectedRoles(Item i)
        // 		{
        // 			ListItem li = new ListItem(i.ItemId.ToString() + "-" + i.Name, i.ItemId.ToString());
        // 			this.lstSelectedItems.Items.Add(li);
        // 		}

        // private void UpdateAvailableRoles()
        // {
        // this.lstItems.DataSource = Item.GetItems(this.itemTypeId, PortalId);
        // this.lstItems.DataTextField = "listName";
        // this.lstItems.DataValueField = "itemId";
        // this.DataBind();
        // }

        // return true if item already in the list box
        private bool RoleIdExists(string itemId)
        {
            foreach (ListItem i in this.lstSelectedItems.Items)
            {
                if (i.Value == itemId)
                {
                    return true;
                }
            }

            return false;
        }

        private void imgAdd_Click(object sender, ImageClickEventArgs e)
        {
            // check existing items, don't add again if already inserted.
            foreach (ListItem sl in this.lstItems.Items)
            {
                if (sl.Selected && !this.RoleIdExists(sl.Value))
                {
                    // get the item selected
                    var selected = new ListItem(sl.Text, sl.Value);
                    this.lstSelectedItems.Items.Add(selected);
                }
            }
        }

        private void imgRemove_Click(object sender, ImageClickEventArgs e)
        {
            var lc = new ListItemCollection();
            foreach (ListItem sl in this.lstSelectedItems.Items)
            {
                if (sl.Selected)
                {
                    lc.Add(sl);
                }
            }

            foreach (ListItem sl in lc)
            {
                this.lstSelectedItems.Items.Remove(sl);
            }
        }
    }
}