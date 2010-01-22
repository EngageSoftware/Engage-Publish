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
    using Data;
    using Util;

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
			btnItemSearch.Click += BtnItemSearchClick;
			imgAdd.Click += ImgAddClick;
			imgRemove.Click += ImgRemoveClick;

            if (_enableDates)
            {
                lstSelectedItems.SelectedIndexChanged += LstSelectedItemsSelectedIndexChanged;
                lstSelectedItems.AutoPostBack = true;
            }
			
            imgUp.Click += ImgUpClick;
			imgDown.Click += ImgDownClick;
			btnStoreRelationshipDate.Click += BtnStoreRelationshipDateClick;
			Load += Page_Load;

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
						if (_enableDates)
						{
							//add dates to the viewstate
                            SetAdditionalSetting("startDate", Utility.GetInvariantDateTime(ir.StartDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
                            SetAdditionalSetting("endDate",  Utility.GetInvariantDateTime(ir.EndDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
						}
                        var li = new ListItem(ir.ParentItemId + "-" + parentName, ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
						lstSelectedItems.Items.Add(li);
					}

					if (_enableSortOrder) 
					{
                        trUpImage.Visible = true;
                        trDownImage.Visible = true;
					}
					if (_allowSearch)
					{
						pnlItemSearch.Visible = true;
					}
					if (AvailableSelectionMode == ListSelectionMode.Single)
					{
						lstSelectedItems.Rows = 1;
                        lstSelectedItems.CssClass += " Publish_ParentCategory";
					}
				}
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private void ImgAddClick(object sender, ImageClickEventArgs e)
		{
            var selectedItems = new List<ListItem>();
            foreach (ListItem li in lstItems.Items)
            {
                if (li.Selected)
                {
                    selectedItems.Add(li);
                }
            }

			//check for single select mode
			if (AvailableSelectionMode == ListSelectionMode.Single && selectedItems.Count > 1) //shouldn't ever happen, SelectMode should be Single. BD
			{
                lblMessage.Text = Localization.GetString("ErrorOnlyOne", LocalResourceFile);
			}
			else
			{
                if (AvailableSelectionMode == ListSelectionMode.Single)
                {
                    //just replace the current entry if we're in Single Select mode.  BD
                    lstSelectedItems.Items.Clear();
                    lstSelectedItems.Items.AddRange(selectedItems.ToArray());
                }
                else
                {
                    //check existing items, don't add again if already inserted.
                    foreach (ListItem selectedItem in selectedItems)
                    {
                        if (!ItemIdExists(selectedItem.Value))
                        {
                            //add the selected item
                            lstSelectedItems.Items.Add(new ListItem(selectedItem.Text, selectedItem.Value));
                        }
                    }
                }
			}
		}

        protected bool ItemIdExists(string itemId)
		{
			foreach (ListItem i in lstSelectedItems.Items)
			{
				if (i.Value == itemId)
				{
					return true;
				}
			}
			return false;
		}

		private void ImgRemoveClick(object sender, ImageClickEventArgs e)
		{
            if (AvailableSelectionMode == ListSelectionMode.Single)
            {
                //if it's single-selection mode, don't make them select it.  BD
                lstSelectedItems.Items.Clear();
            }
            else
            {
                var selectedItems = new List<ListItem>();

                foreach (ListItem li in lstSelectedItems.Items)
                {
                    if (li.Selected)
                    {
                        selectedItems.Add(li);
                    }
                }

                foreach (ListItem selectedItem in selectedItems)
                {
                    lstSelectedItems.Items.Remove(selectedItem);
                }
            }
		}

		private void ImgUpClick(object sender, ImageClickEventArgs e) 
		{
			int index = lstSelectedItems.SelectedIndex;

			if (index == 0) { return; }

			ListItem li = lstSelectedItems.SelectedItem;
			lstSelectedItems.Items.Remove(li);
			lstSelectedItems.Items.Insert(index - 1, li);
		}

		private void ImgDownClick(object sender, ImageClickEventArgs e) 
		{
			int index = lstSelectedItems.SelectedIndex;
			if (index == lstSelectedItems.Items.Count - 1) { return; }

			ListItem li = lstSelectedItems.SelectedItem;
			lstSelectedItems.Items.Remove(li);
			lstSelectedItems.Items.Insert(index + 1, li);
		}

		private void LstSelectedItemsSelectedIndexChanged(object sender, EventArgs e)
		{
			//load the start date/end date controls	
			if (EnableDates)
			{
                if (Utility.HasValue(GetAdditionalSetting("startDate", lstSelectedItems.SelectedValue)))
                {
                    txtStartDate.Text = Convert.ToDateTime(GetAdditionalSetting("startDate", lstSelectedItems.SelectedValue), CultureInfo.InvariantCulture).ToString(CultureInfo.CurrentCulture);
                }
                txtEndDate.Text = Utility.HasValue(GetAdditionalSetting("endDate", lstSelectedItems.SelectedValue)) ? Convert.ToDateTime(GetAdditionalSetting("endDate", lstSelectedItems.SelectedValue), CultureInfo.InvariantCulture).ToString(CultureInfo.CurrentCulture) : string.Empty;
				divDateControls.Visible=true;
                if (txtStartDate.Text.Length == 0) txtStartDate.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture); 
			}
		}

		private void BtnStoreRelationshipDateClick(object sender, EventArgs e)
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

		private void BtnItemSearchClick(object sender, EventArgs e)
		{
			//Filter the relationship list by the search item
			//GetAdminKeywordSearch

            // check for 's 
			DataSet ds = DataProvider.Instance().GetAdminKeywordSearch(txtItemSearch.Text.Trim(),_itemTypeId, ApprovalStatus.Approved.GetId(), PortalId); 
			lstItems.DataSource=ds.Tables[0];
			lstItems.DataTextField = "listName";
			lstItems.DataValueField = "itemId";
			lstItems.DataBind();
            ListItem li = lstItems.Items.FindByValue(ItemId.ToString(CultureInfo.InvariantCulture));
            lstItems.Items.Remove(li);
		}
		#endregion

		#region Optional Interfaces

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
		{
			get 
			{
				var actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection
				                  {
				                      {
				                          GetNextActionID(),
				                          Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent,
				                                                 LocalResourceFile),
				                          DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false,
				                          DotNetNuke.Security.SecurityAccessLevel.Edit, true, false
				                          }
				                  };
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
            var li = new ListItem(item.ItemId.ToString(CultureInfo.InvariantCulture) + "-" + item.Name, item.ItemId.ToString(CultureInfo.InvariantCulture));
			lstSelectedItems.Items.Add(li);
		}

		public void UpdateAvailableItems()
		{
			//am I looking for children or items
			if (!_allowSearch)
			{
				if (_flatView && _allowSearch)
				{//get all item types
					//this._itemTypeId
					lstItems.DataSource = Item.GetItems(_itemTypeId, PortalId);
					lstItems.DataTextField = "listName";
					lstItems.DataValueField = "itemId";
					DataBind();
                    ListItem li = lstItems.Items.FindByValue(ItemId.ToString(CultureInfo.InvariantCulture));
                    lstItems.Items.Remove(li);

				}
				else
				{
					lstItems.Items.Clear();

                    var ir = new ItemRelationship
                                 {
                                     ParentItemId = _parentItemId,
                                     ItemTypeId = _itemTypeId,
                                     RelationshipTypeId = _listRelationshipTypeId
                                 };
				    ir.DisplayChildren(lstItems, PortalId, ExcludeCircularRelationships ? VersionInfoObject.ItemId : (int?)null);

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
			var d = (IDictionary) ViewState[itemId];
			if (d == null)
			{
				return string.Empty;
			}
            string returnString = Convert.ToString(d[setting], CultureInfo.InvariantCulture);
            return String.IsNullOrEmpty(returnString) ? null : returnString;
		}

        public int[] GetSelectedItemIds()
        {
            var al = new ArrayList();

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
			string localResourcePath =  TemplateSourceDirectory + "/" + Localization.LocalResourceDirectory + "/";
			string itemControlResourceFile = Path.Combine(localResourcePath, "itemrelationships");
			btnItemSearch.Text = Localization.GetString("btnItemSearch.Text", itemControlResourceFile);
			lblEndDate.Text = Localization.GetString("lblEndDate.Text", itemControlResourceFile);
			lblStartDate.Text = Localization.GetString("lblStartDate.Text", itemControlResourceFile);
			btnStoreRelationshipDate.Text = Localization.GetString("btnStoreRelationshipDate.Text", itemControlResourceFile);

            imgAdd.AlternateText = Localization.GetString("imgAdd.AltText", itemControlResourceFile);
            imgRemove.AlternateText = Localization.GetString("imgRemove.AltText", itemControlResourceFile);
            imgUp.AlternateText = Localization.GetString("imgUp.AltText", itemControlResourceFile);
            imgDown.AlternateText = Localization.GetString("imgDown.AltText", itemControlResourceFile);
		}
    }
}

