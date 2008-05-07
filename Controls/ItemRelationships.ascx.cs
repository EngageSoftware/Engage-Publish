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
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Controls
{
    public partial class ItemRelationships : ModuleBase, IActionable
    {
        #region Protected Members



        #endregion

        #region Public Properties
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _flatView;// = false;
		public bool FlatView
        {
            [DebuggerStepThrough]
            get { return _flatView; }
            [DebuggerStepThrough]
            set { _flatView = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _allowSearch;// = false;
        public bool AllowSearch
        {
            [DebuggerStepThrough]
            get { return _allowSearch; }
            [DebuggerStepThrough]
            set { _allowSearch = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _itemTypeId = -1;
        public int ItemTypeId
        {
            [DebuggerStepThrough]
            get { return _itemTypeId; }
            set
            {
                _itemTypeId = value;
                UpdateAvailableItems();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _createRelationshipTypeId;
        public int CreateRelationshipTypeId
        {
            [DebuggerStepThrough]
            get { return _createRelationshipTypeId; }
            [DebuggerStepThrough]
            set { _createRelationshipTypeId = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _listRelationshipTypeId;
        public int ListRelationshipTypeId
        {
            [DebuggerStepThrough]
            get { return _listRelationshipTypeId; }
            [DebuggerStepThrough]
            set { _listRelationshipTypeId = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _parentItemId;
        public int ParentItemId
        {
            [DebuggerStepThrough]
            get { return _parentItemId; }
            set
            {
                _parentItemId = value;
                UpdateAvailableItems();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _startDate;
        public string StartDate
        {
            [DebuggerStepThrough]
            get { return _startDate; }
            [DebuggerStepThrough]
            set { _startDate = value; }
        }

        private string _endDate;
        public string EndDate
        {
            [DebuggerStepThrough]
            get { return _endDate; }
            [DebuggerStepThrough]
            set { _endDate = value; }
        }

        public ListSelectionMode AvailableSelectionMode
        {
            get { return lstItems.SelectionMode; }
            set { lstItems.SelectionMode = value; }
        }

        public bool IsValid
        {
            get { return !_isRequired || lstSelectedItems.Items.Count > 0; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _enableSortOrder;// = false;
        public bool EnableSortOrder
        {
            [DebuggerStepThrough]
            get { return _enableSortOrder; }
            [DebuggerStepThrough]
            set { _enableSortOrder = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _enableDates;// = false;
        public bool EnableDates
        {
            [DebuggerStepThrough]
            get { return _enableDates; }
            [DebuggerStepThrough]
            set { _enableDates = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isRequired;// = false;
        public bool IsRequired
        {
            [DebuggerStepThrough]
            set { _isRequired = value; }
            [DebuggerStepThrough]
            get { return _isRequired; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _excludeCircularRelationships;// = false;
        public bool ExcludeCircularRelationships
        {
            [DebuggerStepThrough]
            get { return _excludeCircularRelationships; }
            [DebuggerStepThrough]
            set { _excludeCircularRelationships = value; }
        }
        #endregion

        #region Event Handlers
        override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{
			this.btnItemSearch.Click += this.btnItemSearch_Click;
			this.imgAdd.Click += this.imgAdd_Click;
			this.imgRemove.Click += this.imgRemove_Click;

            if (this._enableDates)
            {
                this.lstSelectedItems.SelectedIndexChanged += this.lstSelectedItems_SelectedIndexChanged;
                this.lstSelectedItems.AutoPostBack = true;
            }
			
            this.imgUp.Click += this.imgUp_Click;
			this.imgDown.Click += this.imgDown_Click;
			this.btnStoreRelationshipDate.Click += this.btnStoreRelationshipDate_Click;
			this.Load += this.Page_Load;

            ClientAPI.RegisterKeyCapture(txtItemSearch, btnItemSearch, 13); //fire the search button if they hit enter while in the search box.
		}

		private void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				//check VI for null then set information
				if (!Page.IsPostBack)
				{
					//localize the itemrelationship controls
					LocalizeControl();

					//get the relationshipId and populate relationships
					ArrayList alItemRelationships = ItemRelationship.GetItemRelationships(VersionInfoObject.ItemId, VersionInfoObject.ItemVersionId, CreateRelationshipTypeId, false);
					foreach (ItemRelationship ir in alItemRelationships)
					{
						string parentName = ItemType.GetItemName(ir.ParentItemId);										
						if (this._enableDates)
						{
							//add dates to the viewstate
                            SetAdditionalSetting("startDate", Utility.GetInvariantDateTime(ir.StartDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
                            SetAdditionalSetting("endDate",  Utility.GetInvariantDateTime(ir.EndDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
						}
                        ListItem li = new ListItem(ir.ParentItemId + "-" + parentName, ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
						lstSelectedItems.Items.Add(li);
					}

					if (this._enableSortOrder) 
					{
                        trUpImage.Visible = true;
                        trDownImage.Visible = true;
					}
					if (this._allowSearch)
					{
						pnlItemSearch.Visible = true;
					}
					if (this.AvailableSelectionMode == ListSelectionMode.Single)
					{
						lstSelectedItems.Rows = 1;
					}
				}
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private void imgAdd_Click(object sender, ImageClickEventArgs e)
		{
            List<ListItem> selectedItems = new List<ListItem>();
            foreach (ListItem li in this.lstItems.Items)
            {
                if (li.Selected)
                {
                    selectedItems.Add(li);
                }
            }

			//check for single select mode
			if (this.AvailableSelectionMode == ListSelectionMode.Single && selectedItems.Count > 1) //shouldn't ever happen, SelectMode should be Single. BD
			{
                this.lblMessage.Text = Localization.GetString("ErrorOnlyOne", LocalResourceFile);
			}
			else
			{
                if (this.AvailableSelectionMode == ListSelectionMode.Single)
                {
                    //just replace the current entry if we're in Single Select mode.  BD
                    this.lstSelectedItems.Items.Clear();
                    this.lstSelectedItems.Items.AddRange(selectedItems.ToArray());
                }
                else
                {
                    //check existing items, don't add again if already inserted.
                    foreach (ListItem selectedItem in selectedItems)
                    {
                        if (!ItemIdExists(selectedItem.Value))
                        {
                            //add the selected item
                            this.lstSelectedItems.Items.Add(new ListItem(selectedItem.Text, selectedItem.Value));
                        }
                    }
                }
			}
		}

        protected bool ItemIdExists(string itemId)
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

		private void imgRemove_Click(object sender, ImageClickEventArgs e)
		{
            if (AvailableSelectionMode == ListSelectionMode.Single)
            {
                //if it's single-selection mode, don't make them select it.  BD
                this.lstSelectedItems.Items.Clear();
            }
            else
            {
                List<ListItem> selectedItems = new List<ListItem>();

                foreach (ListItem li in this.lstSelectedItems.Items)
                {
                    if (li.Selected)
                    {
                        selectedItems.Add(li);
                    }
                }

                foreach (ListItem selectedItem in selectedItems)
                {
                    this.lstSelectedItems.Items.Remove(selectedItem);
                }
            }
		}

		private void imgUp_Click(object sender, ImageClickEventArgs e) 
		{
			int index = this.lstSelectedItems.SelectedIndex;

			if (index == 0) { return; }

			ListItem li = this.lstSelectedItems.SelectedItem;
			this.lstSelectedItems.Items.Remove(li);
			this.lstSelectedItems.Items.Insert(index - 1, li);
		}

		private void imgDown_Click(object sender, ImageClickEventArgs e) 
		{
			int index = this.lstSelectedItems.SelectedIndex;
			if (index == this.lstSelectedItems.Items.Count - 1) { return; }

			ListItem li = this.lstSelectedItems.SelectedItem;
			this.lstSelectedItems.Items.Remove(li);
			this.lstSelectedItems.Items.Insert(index + 1, li);
		}

		private void lstSelectedItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			//load the start date/end date controls	
			if (EnableDates)
			{
                if (Utility.HasValue(GetAdditionalSetting("startDate", lstSelectedItems.SelectedValue)))
                {
                    txtStartDate.Text = Convert.ToDateTime(GetAdditionalSetting("startDate", lstSelectedItems.SelectedValue), CultureInfo.InvariantCulture).ToString(CultureInfo.CurrentCulture);
                }
                if (Utility.HasValue(GetAdditionalSetting("endDate", lstSelectedItems.SelectedValue)))
                {
                    txtEndDate.Text = Convert.ToDateTime(GetAdditionalSetting("endDate", lstSelectedItems.SelectedValue), CultureInfo.InvariantCulture).ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    txtEndDate.Text = string.Empty;
                }
				divDateControls.Visible=true;
                if (txtStartDate.Text.Length == 0) txtStartDate.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture); 
			}
		}

		private void btnStoreRelationshipDate_Click(object sender, EventArgs e)
		{
			//store the dates for the current item
            if (Utility.HasValue(txtStartDate.Text))
            {
                SetAdditionalSetting("startDate", Convert.ToDateTime(txtStartDate.Text, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture), lstSelectedItems.SelectedValue);
            }
            else
            {
                SetAdditionalSetting("startDate", DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), lstSelectedItems.SelectedValue);
            }
            if (Utility.HasValue(txtEndDate.Text))
            {
                SetAdditionalSetting("endDate", Convert.ToDateTime(txtEndDate.Text, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture), lstSelectedItems.SelectedValue);
            }
            else
            {
                SetAdditionalSetting("endDate", "", lstSelectedItems.SelectedValue);
            }
			divDateControls.Visible=false;
		}

		private void btnItemSearch_Click(object sender, EventArgs e)
		{
			//Filter the relationship list by the search item
			//GetAdminKeywordSearch

            // check for 's 
			DataSet ds = DataProvider.Instance().GetAdminKeywordSearch(txtItemSearch.Text.Trim(),_itemTypeId, ApprovalStatus.Approved.GetId(), PortalId); 
			lstItems.DataSource=ds.Tables[0];
			this.lstItems.DataTextField = "listName";
			this.lstItems.DataValueField = "itemId";
			lstItems.DataBind();
            ListItem li = this.lstItems.Items.FindByValue(ItemId.ToString(CultureInfo.InvariantCulture));
            lstItems.Items.Remove(li);
		}
		#endregion

		#region Optional Interfaces

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
		{
			get 
			{
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
				return actions;
			}
		}


		#endregion

		public void AddToSelectedItems(Item item)
		{
            if (item == null)
            {
                return;
            }
            ListItem li = new ListItem(item.ItemId.ToString(CultureInfo.InvariantCulture) + "-" + item.Name, item.ItemId.ToString(CultureInfo.InvariantCulture));
			this.lstSelectedItems.Items.Add(li);
		}

		public void UpdateAvailableItems()
		{
			//am I looking for children or items
			if (!this._allowSearch)
			{
				if (this._flatView && this._allowSearch)
				{//get all item types
					//this.itemTypeId
					this.lstItems.DataSource = Item.GetItems(this._itemTypeId, PortalId);
					this.lstItems.DataTextField = "listName";
					this.lstItems.DataValueField = "itemId";
					this.DataBind();
                    ListItem li = this.lstItems.Items.FindByValue(ItemId.ToString(CultureInfo.InvariantCulture));
                    lstItems.Items.Remove(li);

				}
				else
				{
					this.lstItems.Items.Clear();

                    ItemRelationship ir = new ItemRelationship();
					ir.ParentItemId = this._parentItemId;
					ir.ItemTypeId = this._itemTypeId;
					ir.RelationshipTypeId = this._listRelationshipTypeId;
					ir.DisplayChildren(this.lstItems, PortalId, ExcludeCircularRelationships ? this.VersionInfoObject.ItemId : (int?)null);

				}
			}
			//look for children
		}

        /// <summary>
        /// Clears the selected items from this <see cref="ItemRelationships"/>.
        /// </summary>
        public void Clear()
        {
            lstSelectedItems.Items.Clear();
        }

        public string GetAdditionalSetting(object setting, string itemId)
		{
			IDictionary d = (IDictionary) ViewState[itemId];
			if (d == null)
			{
				return string.Empty;
			}
            string returnString = Convert.ToString(d[setting], CultureInfo.InvariantCulture);
            return String.IsNullOrEmpty(returnString) ? null : returnString;
		}

        public int[] GetSelectedItemIds()
        {
            ArrayList al = new ArrayList();

            for (int i = 0; i < lstSelectedItems.Items.Count; i++)
            {
                ListItem li = lstSelectedItems.Items[i];
                al.Add(Convert.ToInt32(li.Value, CultureInfo.InvariantCulture));
            }

            return (int[])al.ToArray(typeof(int));
        }

		private void SetAdditionalSetting(object setting, string settingValue, string itemId)
		{
			IDictionary d = (IDictionary) ViewState[itemId] ?? new Hashtable();
		    d[setting] = settingValue;
			ViewState[itemId] = d;
		}

		private void LocalizeControl()
		{
			string localResourcePath =  this.TemplateSourceDirectory + "/" + Localization.LocalResourceDirectory + "/";
			string ItemControlResourceFile = Path.Combine(localResourcePath, "itemrelationships");
			btnItemSearch.Text = Localization.GetString("btnItemSearch.Text", ItemControlResourceFile);
			lblEndDate.Text = Localization.GetString("lblEndDate.Text", ItemControlResourceFile);
			lblStartDate.Text = Localization.GetString("lblStartDate.Text", ItemControlResourceFile);
			btnStoreRelationshipDate.Text = Localization.GetString("btnStoreRelationshipDate.Text", ItemControlResourceFile);

            imgAdd.AlternateText = Localization.GetString("imgAdd.AltText", ItemControlResourceFile);
            imgRemove.AlternateText = Localization.GetString("imgRemove.AltText", ItemControlResourceFile);
            imgUp.AlternateText = Localization.GetString("imgUp.AltText", ItemControlResourceFile);
            imgDown.AlternateText = Localization.GetString("imgDown.AltText", ItemControlResourceFile);
		}
    }
}

