//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
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
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    public partial class ItemRelationships : ModuleBase, IActionable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _allowSearch; // = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _createRelationshipTypeId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _enableDates; // = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _enableSortOrder; // = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _excludeCircularRelationships; // = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _flatView; // = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isRequired; // = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _itemTypeId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _listRelationshipTypeId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _parentItemId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _startDate;

        public bool AllowSearch
        {
            [DebuggerStepThrough]
            get { return this._allowSearch; }
            [DebuggerStepThrough]
            set { this._allowSearch = value; }
        }

        public ListSelectionMode AvailableSelectionMode
        {
            get { return this.lstItems.SelectionMode; }
            set { this.lstItems.SelectionMode = value; }
        }

        public int CreateRelationshipTypeId
        {
            [DebuggerStepThrough]
            get { return this._createRelationshipTypeId; }
            [DebuggerStepThrough]
            set { this._createRelationshipTypeId = value; }
        }

        public bool EnableDates
        {
            [DebuggerStepThrough]
            get { return this._enableDates; }
            [DebuggerStepThrough]
            set { this._enableDates = value; }
        }

        public bool EnableSortOrder
        {
            [DebuggerStepThrough]
            get { return this._enableSortOrder; }
            [DebuggerStepThrough]
            set { this._enableSortOrder = value; }
        }

        public string EndDate { [DebuggerStepThrough]
        get; [DebuggerStepThrough]
        set; }

        public bool ExcludeCircularRelationships
        {
            [DebuggerStepThrough]
            get { return this._excludeCircularRelationships; }
            [DebuggerStepThrough]
            set { this._excludeCircularRelationships = value; }
        }

        public bool FlatView
        {
            [DebuggerStepThrough]
            get { return this._flatView; }
            [DebuggerStepThrough]
            set { this._flatView = value; }
        }

        public bool IsRequired
        {
            [DebuggerStepThrough]
            get { return this._isRequired; }
            [DebuggerStepThrough]
            set { this._isRequired = value; }
        }

        public bool IsValid
        {
            get { return !this._isRequired || this.lstSelectedItems.Items.Count > 0; }
        }

        public int ItemTypeId
        {
            [DebuggerStepThrough]
            get { return this._itemTypeId; }
            set
            {
                this._itemTypeId = value;
                this.UpdateAvailableItems();
            }
        }

        public int ListRelationshipTypeId
        {
            [DebuggerStepThrough]
            get { return this._listRelationshipTypeId; }
            [DebuggerStepThrough]
            set { this._listRelationshipTypeId = value; }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), 
                            ModuleActionType.AddContent, string.Empty, string.Empty, string.Empty, false, SecurityAccessLevel.Edit, true, false
                            }
                    };
                return actions;
            }
        }

        public int ParentItemId
        {
            [DebuggerStepThrough]
            get { return this._parentItemId; }
            set
            {
                this._parentItemId = value;
                this.UpdateAvailableItems();
            }
        }

        public string StartDate
        {
            [DebuggerStepThrough]
            get { return this._startDate; }
            [DebuggerStepThrough]
            set { this._startDate = value; }
        }

        public void AddToSelectedItems(Item item)
        {
            if (item == null)
            {
                return;
            }

            var li = new ListItem(
                item.ItemId.ToString(CultureInfo.InvariantCulture) + "-" + item.Name, item.ItemId.ToString(CultureInfo.InvariantCulture));
            this.lstSelectedItems.Items.Add(li);
        }

        /// <summary>
        /// Clears the selected items from this <see cref="ItemRelationships"/>.
        /// </summary>
        public void Clear()
        {
            this.lstSelectedItems.Items.Clear();
        }

        public string GetAdditionalSetting(object setting, string itemId)
        {
            var d = (IDictionary)this.ViewState[itemId];
            if (d == null)
            {
                return string.Empty;
            }

            string returnString = Convert.ToString(d[setting], CultureInfo.InvariantCulture);
            return string.IsNullOrEmpty(returnString) ? null : returnString;
        }

        public int[] GetSelectedItemIds()
        {
            var al = new ArrayList();

            for (int i = 0; i < this.lstSelectedItems.Items.Count; i++)
            {
                ListItem li = this.lstSelectedItems.Items[i];
                al.Add(Convert.ToInt32(li.Value, CultureInfo.InvariantCulture));
            }

            return (int[])al.ToArray(typeof(int));
        }

        public void UpdateAvailableItems()
        {
            // am I looking for children or items
            if (!this._allowSearch)
            {
                if (this._flatView && this._allowSearch)
                {
                    // get all item types
                    // this._itemTypeId
                    this.lstItems.DataSource = Item.GetItems(this._itemTypeId, this.PortalId);
                    this.lstItems.DataTextField = "listName";
                    this.lstItems.DataValueField = "itemId";
                    this.DataBind();
                    ListItem li = this.lstItems.Items.FindByValue(this.ItemId.ToString(CultureInfo.InvariantCulture));
                    this.lstItems.Items.Remove(li);
                }
                else
                {
                    this.lstItems.Items.Clear();

                    var ir = new ItemRelationship
                        {
                            ParentItemId = this._parentItemId, 
                            ItemTypeId = this._itemTypeId, 
                            RelationshipTypeId = this._listRelationshipTypeId
                        };
                    ir.DisplayChildren(this.lstItems, this.PortalId, this.ExcludeCircularRelationships ? this.VersionInfoObject.ItemId : (int?)null);
                }
            }

            // look for children
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

        protected override void OnInit(EventArgs e)
        {
            this.btnItemSearch.Click += this.BtnItemSearchClick;
            this.imgAdd.Click += this.ImgAddClick;
            this.imgRemove.Click += this.ImgRemoveClick;

            if (this._enableDates)
            {
                this.lstSelectedItems.SelectedIndexChanged += this.LstSelectedItemsSelectedIndexChanged;
                this.lstSelectedItems.AutoPostBack = true;
            }

            this.imgUp.Click += this.ImgUpClick;
            this.imgDown.Click += this.ImgDownClick;
            this.btnStoreRelationshipDate.Click += this.BtnStoreRelationshipDateClick;
            this.Load += this.Page_Load;

            ClientAPI.RegisterKeyCapture(this.txtItemSearch, this.btnItemSearch, 13);
                
            // fire the search button if they hit enter while in the search box.
            base.OnInit(e);
        }

        private void BtnItemSearchClick(object sender, EventArgs e)
        {
            // Filter the relationship list by the search item
            // GetAdminKeywordSearch

            // check for 's 
            DataSet ds = DataProvider.Instance().GetAdminKeywordSearch(
                this.txtItemSearch.Text.Trim(), this._itemTypeId, ApprovalStatus.Approved.GetId(), this.PortalId);
            this.lstItems.DataSource = ds.Tables[0];
            this.lstItems.DataTextField = "listName";
            this.lstItems.DataValueField = "itemId";
            this.lstItems.DataBind();
            ListItem li = this.lstItems.Items.FindByValue(this.ItemId.ToString(CultureInfo.InvariantCulture));
            this.lstItems.Items.Remove(li);
        }

        private void BtnStoreRelationshipDateClick(object sender, EventArgs e)
        {
            // store the dates for the current item
            if (Engage.Utility.HasValue(this.txtStartDate.Text))
            {
                this.SetAdditionalSetting(
                    "startDate", 
                    Convert.ToDateTime(this.txtStartDate.Text, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture), 
                    this.lstSelectedItems.SelectedValue);
            }
            else
            {
                this.SetAdditionalSetting("startDate", DateTime.Now.Date.ToString(CultureInfo.InvariantCulture), this.lstSelectedItems.SelectedValue);
            }

            if (Engage.Utility.HasValue(this.txtEndDate.Text))
            {
                this.SetAdditionalSetting(
                    "endDate", 
                    Convert.ToDateTime(this.txtEndDate.Text, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture), 
                    this.lstSelectedItems.SelectedValue);
            }
            else
            {
                this.SetAdditionalSetting("endDate", string.Empty, this.lstSelectedItems.SelectedValue);
            }

            this.divDateControls.Visible = false;
        }

        private void ImgAddClick(object sender, ImageClickEventArgs e)
        {
            var selectedItems = new List<ListItem>();
            foreach (ListItem li in this.lstItems.Items)
            {
                if (li.Selected)
                {
                    selectedItems.Add(li);
                }
            }

            // check for single select mode
            if (this.AvailableSelectionMode == ListSelectionMode.Single && selectedItems.Count > 1)
            {
                // shouldn't ever happen, SelectMode should be Single. BD
                this.lblMessage.Text = Localization.GetString("ErrorOnlyOne", this.LocalResourceFile);
            }
            else
            {
                if (this.AvailableSelectionMode == ListSelectionMode.Single)
                {
                    // just replace the current entry if we're in Single Select mode.  BD
                    this.lstSelectedItems.Items.Clear();
                    this.lstSelectedItems.Items.AddRange(selectedItems.ToArray());
                }
                else
                {
                    // check existing items, don't add again if already inserted.
                    foreach (ListItem selectedItem in selectedItems)
                    {
                        if (!this.ItemIdExists(selectedItem.Value))
                        {
                            // add the selected item
                            this.lstSelectedItems.Items.Add(new ListItem(selectedItem.Text, selectedItem.Value));
                        }
                    }
                }
            }
        }

        private void ImgDownClick(object sender, ImageClickEventArgs e)
        {
            int index = this.lstSelectedItems.SelectedIndex;
            if (index == this.lstSelectedItems.Items.Count - 1)
            {
                return;
            }

            ListItem li = this.lstSelectedItems.SelectedItem;
            this.lstSelectedItems.Items.Remove(li);
            this.lstSelectedItems.Items.Insert(index + 1, li);
        }

        private void ImgRemoveClick(object sender, ImageClickEventArgs e)
        {
            if (this.AvailableSelectionMode == ListSelectionMode.Single)
            {
                // if it's single-selection mode, don't make them select it.  BD
                this.lstSelectedItems.Items.Clear();
            }
            else
            {
                var selectedItems = new List<ListItem>();

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

        private void ImgUpClick(object sender, ImageClickEventArgs e)
        {
            int index = this.lstSelectedItems.SelectedIndex;

            if (index == 0)
            {
                return;
            }

            ListItem li = this.lstSelectedItems.SelectedItem;
            this.lstSelectedItems.Items.Remove(li);
            this.lstSelectedItems.Items.Insert(index - 1, li);
        }

        private void LocalizeControl()
        {
            string localResourcePath = this.TemplateSourceDirectory + "/" + Localization.LocalResourceDirectory + "/";
            string itemControlResourceFile = Path.Combine(localResourcePath, "itemrelationships");
            this.btnItemSearch.Text = Localization.GetString("btnItemSearch.Text", itemControlResourceFile);
            this.lblEndDate.Text = Localization.GetString("lblEndDate.Text", itemControlResourceFile);
            this.lblStartDate.Text = Localization.GetString("lblStartDate.Text", itemControlResourceFile);
            this.btnStoreRelationshipDate.Text = Localization.GetString("btnStoreRelationshipDate.Text", itemControlResourceFile);

            this.imgAdd.AlternateText = Localization.GetString("imgAdd.AltText", itemControlResourceFile);
            this.imgRemove.AlternateText = Localization.GetString("imgRemove.AltText", itemControlResourceFile);
            this.imgUp.AlternateText = Localization.GetString("imgUp.AltText", itemControlResourceFile);
            this.imgDown.AlternateText = Localization.GetString("imgDown.AltText", itemControlResourceFile);
        }

        private void LstSelectedItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            // load the start date/end date controls	
            if (this.EnableDates)
            {
                if (Engage.Utility.HasValue(this.GetAdditionalSetting("startDate", this.lstSelectedItems.SelectedValue)))
                {
                    this.txtStartDate.Text =
                        Convert.ToDateTime(this.GetAdditionalSetting("startDate", this.lstSelectedItems.SelectedValue), CultureInfo.InvariantCulture).
                            ToString(CultureInfo.CurrentCulture);
                }

                this.txtEndDate.Text = Engage.Utility.HasValue(this.GetAdditionalSetting("endDate", this.lstSelectedItems.SelectedValue))
                                           ? Convert.ToDateTime(
                                               this.GetAdditionalSetting("endDate", this.lstSelectedItems.SelectedValue), CultureInfo.InvariantCulture)
                                                 .ToString(CultureInfo.CurrentCulture)
                                           : string.Empty;
                this.divDateControls.Visible = true;
                if (this.txtStartDate.Text.Length == 0)
                {
                    this.txtStartDate.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                }
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check VI for null then set information
                if (!this.Page.IsPostBack)
                {
                    // localize the itemrelationship controls
                    this.LocalizeControl();

                    // get the relationshipId and populate relationships
                    ArrayList alItemRelationships = ItemRelationship.GetItemRelationships(
                        this.VersionInfoObject.ItemId, this.VersionInfoObject.ItemVersionId, this.CreateRelationshipTypeId, false);
                    foreach (ItemRelationship ir in alItemRelationships)
                    {
                        string parentName = ItemType.GetItemName(ir.ParentItemId);
                        if (this._enableDates)
                        {
                            // add dates to the viewstate
                            this.SetAdditionalSetting(
                                "startDate", Utility.GetInvariantDateTime(ir.StartDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
                            this.SetAdditionalSetting(
                                "endDate", Utility.GetInvariantDateTime(ir.EndDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
                        }

                        var li = new ListItem(ir.ParentItemId + "-" + parentName, ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
                        this.lstSelectedItems.Items.Add(li);
                    }

                    if (this._enableSortOrder)
                    {
                        this.trUpImage.Visible = true;
                        this.trDownImage.Visible = true;
                    }

                    if (this._allowSearch)
                    {
                        this.pnlItemSearch.Visible = true;
                    }

                    if (this.AvailableSelectionMode == ListSelectionMode.Single)
                    {
                        this.lstSelectedItems.Rows = 1;
                        this.lstSelectedItems.CssClass += " Publish_ParentCategory";
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetAdditionalSetting(object setting, string settingValue, string itemId)
        {
            IDictionary d = (IDictionary)this.ViewState[itemId] ?? new Hashtable();
            d[setting] = settingValue;
            this.ViewState[itemId] = d;
        }
    }
}