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
    public partial class AdminItemSearch : ModuleBase
    {
        #region Protected Members

        private int _listRelationshipTypeId;
        private int _createRelationshipTypeId;
        private int _parentItemId;
        private string _startDate;
        private string _endDate;

        private int _itemTypeId = -1;
        private bool _flatView;// = false;
        private bool _isRequired;// = false;
        private bool _allowSearch;// = false;
        private bool _enableSortOrder;// = false;
        private bool _enableDates;// = false;
        #endregion

        
        #region Event Handlers
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //check VI for null then set information
                if (!Page.IsPostBack)
                {
                    ////get the relationshipId and populate relationships
                    //ArrayList alItemRelationships = ItemRelationship.GetItemRelationships(VersionInfoObject.ItemId, VersionInfoObject.ItemVersionId, CreateRelationshipTypeId, false);
                    //foreach (ItemRelationship ir in alItemRelationships)
                    //{
                    //    string parentName = ItemType.GetItemName(ir.ParentItemId);
                    //    if (this._enableDates)
                    //    {
                    //        //add dates to the viewstate
                    //        SetAdditionalSetting("startDate", Utility.GetInvariantDateTime(ir.StartDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
                    //        SetAdditionalSetting("endDate", Utility.GetInvariantDateTime(ir.EndDate), ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
                    //    }
                    //    ListItem li = new ListItem(ir.ParentItemId + "-" + parentName, ir.ParentItemId.ToString(CultureInfo.InvariantCulture));
                    //    lstSelectedItems.Items.Add(li);
                    //}

                    //if (this._allowSearch)
                    //{
                    //    pnlItemSearch.Visible = true;
                    //}
                    //if (this.AvailableSelectionMode == ListSelectionMode.Single)
                    //{
                    //    lstSelectedItems.Rows = 1;
                    //}
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
      
        #endregion

        

    }
}

