////Engage: Publish - http://www.engagemodules.com
////Copyright (c) 2004-2008
////by Engage Software ( http://www.engagesoftware.com )

////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
////TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
////THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
////CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
////DEALINGS IN THE SOFTWARE.

//using System;
//using System.Collections;
//using System.Data;
//using System.Globalization;
//using System.IO;
//using System.Web.UI.WebControls;
//using DotNetNuke.Entities.Modules;
//using DotNetNuke.Services.Localization;
//using DotNetNuke.Services.Exceptions;
//using Engage.Dnn.Publish.Data;
//using Engage.Dnn.Publish.Util;
//using DotNetNuke.UI.Utilities;

//namespace Engage.Dnn.Publish.Controls
//{
//    public partial class AdminItemSearch : ModuleBase, IActionable
//    {
//        #region Protected Members

//        private int _listRelationshipTypeId;
//        private int _createRelationshipTypeId;
//        private int _parentItemId;
//        private string _startDate;
//        private string _endDate;

//        private int _itemTypeId = -1;
//        private bool _flatView;// = false;
//        private bool _isRequired;// = false;
//        private bool _allowSearch;// = false;
//        private bool _enableSortOrder;// = false;
//        private bool _enableDates;// = false;
//        #endregion

//        #region Public Properties
//        public bool FlatView
//        {
//            get
//            {
//                return _flatView;
//            }
//            set
//            {
//                _flatView = value;
//            }
//        }

//        public bool AllowSearch
//        {
//            get
//            {
//                return _allowSearch;
//            }
//            set
//            {
//                _allowSearch = value;
//            }
//        }

//        public int ItemTypeId
//        {
//            get
//            {
//                return _itemTypeId;
//            }
//            set
//            {
//                _itemTypeId = value;
//                UpdateAvailableItems();
//            }
//        }

//        public int CreateRelationshipTypeId
//        {
//            get
//            {
//                return _createRelationshipTypeId;
//            }
//            set
//            {
//                _createRelationshipTypeId = value;
//            }
//        }

//        public int ListRelationshipTypeId
//        {
//            get
//            {
//                return _listRelationshipTypeId;
//            }
//            set
//            {
//                _listRelationshipTypeId = value;
//            }
//        }

//        public int ParentItemId
//        {
//            get
//            {
//                return _parentItemId;
//            }

//            set
//            {
//                _parentItemId = value;
//                UpdateAvailableItems();
//            }
//        }

//        public string StartDate
//        {
//            get
//            {
//                return _startDate;
//            }

//            set
//            {
//                _startDate = value;
//            }
//        }

//        public string EndDate
//        {
//            get
//            {
//                return _endDate;
//            }

//            set
//            {
//                _endDate = value;
//            }
//        }

//        public ListSelectionMode AvailableSelectionMode
//        {
//            get
//            {
//                return lstItems.SelectionMode;
//            }
//            set
//            {
//                lstItems.SelectionMode = value;
//            }
//        }

//        public bool IsValid
//        {
//            get
//            {
//                return !_isRequired || lstSelectedItems.Items.Count > 0;
//            }
//        }

//        public bool EnableSortOrder
//        {
//            get
//            {
//                return _enableSortOrder;
//            }
//            set
//            {
//                _enableSortOrder = value;
//            }
//        }

//        public bool EnableDates
//        {
//            get
//            {
//                return _enableDates;
//            }
//            set
//            {
//                _enableDates = value;
//            }
//        }

//        public bool IsRequired
//        {
//            set
//            {
//                _isRequired = value;
//            }
//            get
//            {
//                return _isRequired;
//            }
//        }
//        #endregion

//        #region Event Handlers
//        override protected void OnInit(EventArgs e)
//        {
//            InitializeComponent();
//            base.OnInit(e);
//        }
		
//        private void InitializeComponent()
//        {
//            this.btnItemSearch.Click += this.btnItemSearch_Click;
//            this.Load += this.Page_Load;

//            ClientAPI.RegisterKeyCapture(txtItemSearch, btnItemSearch, 13); //fire the search button if they hit enter while in the search box.
//        }

//        private void Page_Load(object sender, EventArgs e)
//        {
//            try 
//            {
//                //check VI for null then set information
//                if (!Page.IsPostBack)
//                {
//                    //localize the itemrelationship controls
//                    LocalizeControl();

//                    //get the relationshipId and populate relationships
//                    ArrayList alItemRelationships = ItemRelationship.GetItemRelationships(VersionInfoObject.ItemId, VersionInfoObject.ItemVersionId, CreateRelationshipTypeId, false);
//                    foreach (ItemRelationship ir in alItemRelationships)
//                    {
//                        string parentName = ItemType.GetItemName(ir.ParentItemId);										
//                        if (this._enableDates)
//                        {
//                            //add dates to the viewstate
//                            SetAdditionalSetting("startDate", Utility.GetInvariantDateTime(ir.StartDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
//                            SetAdditionalSetting("endDate",  Utility.GetInvariantDateTime(ir.EndDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
//                        }
//                        ListItem li = new ListItem(ir.ParentItemId + "-" + parentName, ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
//                        lstSelectedItems.Items.Add(li);
//                    }

//                    if (this._allowSearch)
//                    {
//                        pnlItemSearch.Visible = true;
//                    }
//                    if (this.AvailableSelectionMode == ListSelectionMode.Single)
//                    {
//                        lstSelectedItems.Rows = 1;
//                    }
//                }
//            } 
//            catch (Exception exc) 
//            {
//                Exceptions.ProcessModuleLoadException(this, exc);
//            }
//        }

//        protected bool ItemIdExists(string itemId)
//        {
//            foreach (ListItem i in this.lstSelectedItems.Items)
//            {
//                if (i.Value == itemId)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }





//        private void btnItemSearch_Click(object sender, EventArgs e)
//        {
//            //Filter the relationship list by the search item
//            //GetAdminKeywordSearch

//            // check for 's 
//            DataSet ds = DataProvider.Instance().GetAdminKeywordSearch(txtItemSearch.Text.Trim(),_itemTypeId, ApprovalStatus.Approved.GetId(), PortalId); 
//            lstItems.DataSource=ds.Tables[0];
//            this.lstItems.DataTextField = "listName";
//            this.lstItems.DataValueField = "itemId";
//            lstItems.DataBind();
//            ListItem li = this.lstItems.Items.FindByValue(ItemId.ToString(CultureInfo.InvariantCulture));
//            lstItems.Items.Remove(li);
//        }
//        #endregion

//        #region Optional Interfaces

//        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
//        {
//            get 
//            {
//                DotNetNuke.Entities.Modules.Actions.ModuleActionCollection actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
//                actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
//                return actions;
//            }
//        }


//        #endregion

//        public void AddToSelectedItems(Item item)
//        {
//            if (item == null)
//            {
//                return;
//            }
//            ListItem li = new ListItem(item.ItemId.ToString(CultureInfo.InvariantCulture) + "-" + item.Name, item.ItemId.ToString(CultureInfo.InvariantCulture));
//            this.lstSelectedItems.Items.Add(li);
//        }

//        public void UpdateAvailableItems()
//        {
//            //am I looking for children or items
//            if (!this._allowSearch)
//            {
//                if (this._flatView && this._allowSearch)
//                {//get all item types
//                    //this.itemTypeId
//                    this.lstItems.DataSource = Item.GetItems(this._itemTypeId, PortalId);
//                    this.lstItems.DataTextField = "listName";
//                    this.lstItems.DataValueField = "itemId";
//                    this.DataBind();
//                    ListItem li = this.lstItems.Items.FindByValue(ItemId.ToString(CultureInfo.InvariantCulture));
//                    lstItems.Items.Remove(li);

//                }
//                else
//                {
//                    this.lstItems.Items.Clear();

//                    ItemRelationship ir = new ItemRelationship();
//                    ir.ParentItemId = this._parentItemId;
//                    ir.ItemTypeId = this._itemTypeId;
//                    ir.RelationshipTypeId = this._listRelationshipTypeId;
//                    ir.DisplayChildren(this.lstItems, PortalId);

//                }
//            }
//            //look for children
//        }

//        /// <summary>
//        /// Clears the selected items from this <see cref="ItemRelationships"/>.
//        /// </summary>
//        public void Clear()
//        {
//            lstSelectedItems.Items.Clear();
//        }

//        public string GetAdditionalSetting(object setting, string itemId)
//        {
//            IDictionary d = (IDictionary) ViewState[itemId];
//            if (d == null)
//            {
//                return string.Empty;
//            }
//            string returnString = Convert.ToString(d[setting], CultureInfo.InvariantCulture);
//            if (String.IsNullOrEmpty(returnString))
//            {
//                return null;
//            }
//            else
//            {
//                return returnString;
//            }
             
			
//        }

//        public int[] GetSelectedItemIds()
//        {
//            ArrayList al = new ArrayList();

//            for (int i = 0; i < lstSelectedItems.Items.Count; i++)
//            {
//                ListItem li = lstSelectedItems.Items[i];
//                al.Add(Convert.ToInt32(li.Value, CultureInfo.InvariantCulture));
//            }

//            return (int[])al.ToArray(typeof(int));
//        }

//        private void SetAdditionalSetting(object setting, string settingValue, string itemId)
//        {
//            IDictionary d = (IDictionary) ViewState[itemId] ?? new Hashtable();
//            d[setting] = settingValue;
//            ViewState[itemId] = d;
//        }

//        private void LocalizeControl()
//        {
//            string localResourcePath =  this.TemplateSourceDirectory + "/" + Localization.LocalResourceDirectory + "/";
//            string ItemControlResourceFile = Path.Combine(localResourcePath, "itemrelationships");
//            btnItemSearch.Text = Localization.GetString("btnItemSearch.Text", ItemControlResourceFile);
//        }
//    }
//}

