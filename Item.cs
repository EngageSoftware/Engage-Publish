//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.



namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Globalization;
    using System.Xml.Serialization;
    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Security.Roles;
    using DotNetNuke.Services.Mail;
    using Data;
    using Portability;
    using Util;

    /// <summary>
    /// Summary description for ItemInfo.
    /// </summary>
    public abstract class Item : TransportableElement
    {
        #region "Private Properties"

        private Guid itemIdentifier;
        private Guid itemVersionIdentifier;
        private int moduleId = -1;
        private int itemId = -1;
        private int approvedItemVersionId = -1;
        private int itemVersionId = -1;
        private int originalItemVersionId = -1;
        private string name = string.Empty;
        private string url = string.Empty;

        private bool newWindow;

        private string description = string.Empty;
        private string itemVersionDate = string.Empty;
        private string startDate = string.Empty;
        private string endDate;
        private int languageId = -1;
        private int authorUserId = -1;
        private int revisingUserId = -1;
        private int approvalStatusId = -1;
        private string approvalDate = string.Empty;
        private int approvalUserId = -1;
        private string approvalComments = string.Empty;
        private string metaKeywords = string.Empty;
        private string metaDescription = string.Empty;
        private string metaTitle = string.Empty;
        private int displayTabId = -1;
        private string lastUpdated = string.Empty;
        private int itemTypeId = -1;
        private int portalId = -1;
        private bool disabled;
        private string thumbnail = string.Empty;
        private readonly ItemRelationshipCollection relationships;
        private readonly ItemTagCollection tags;
        private readonly ItemVersionSettingCollection versionSettings;
        private string createdDate = string.Empty;

        #endregion

        #region "Public Methods"

        protected Item()
        {
            startDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            relationships = new ItemRelationshipCollection();
            versionSettings = new ItemVersionSettingCollection();
            tags = new ItemTagCollection();
        }


        [XmlIgnore]
        public ItemRelationshipCollection Relationships
        {
            get { return relationships; }
        }

        [XmlIgnore]
        public ItemTagCollection Tags
        {
            get { return tags; }
        }

        [XmlIgnore]
        public ItemVersionSettingCollection VersionSettings
        {
            get { return versionSettings; }
        }

        public abstract void Save(int revisingUserId);
        public abstract void UpdateApprovalStatus();
        public abstract string EmailApprovalBody { get; }

        public abstract string EmailApprovalSubject { get; }

        public abstract string EmailStatusChangeBody { get; }
        public abstract string EmailStatusChangeSubject { get; }


        public void UpdateDescription()
        {
            DataProvider.Instance().UpdateDescription(itemVersionId, description, metaDescription);
            Utility.ClearPublishCache(PortalId);
        }

        //public bool DisplayOnCurrentPage()
        //{
        //    //ItemVersionSetting cpSetting = ItemVersionSetting.GetItemVersionSetting(this.ItemVersionId, "ArticleSettings", "DisplayOnCurrentPage");

        //    //ItemType type = ItemType.GetFromId(this.ItemTypeId, typeof(ItemType));

        //    //ItemVersionSetting cpSetting = ItemVersionSetting.GetItemVersionSetting(this.ItemVersionId, type.Name.ToString() + "Settings", "DisplayOnCurrentPage", portalId);
        //    //if (cpSetting != null)
        //    //{
        //    //    return Convert.ToBoolean(cpSetting.PropertyValue, CultureInfo.InvariantCulture);
        //    //}
        //    //else
        //    //{
        //    //    return this.displayTabId < 0;
        //    //}

        //    ItemType type = ItemType.GetFromId(ItemTypeId, typeof(ItemType));
        //    string cacheKey = Utility.CacheKeyPublishDisplayOnCurrentPage + itemVersionId.ToString(CultureInfo.InvariantCulture);
        //    bool returnVal = false;

        //    if (ModuleBase.UseCachePortal(PortalId))
        //    {
        //        object o = DataCache.GetCache(cacheKey);
        //        if (o != null)
        //        {
        //            returnVal = (bool)o;
        //        }
        //        else
        //        {
        //            ItemVersionSetting cpSetting = ItemVersionSetting.GetItemVersionSetting(ItemVersionId, type.Name + "Settings", "DisplayOnCurrentPage", portalId);
        //            if (cpSetting != null)
        //            {
        //                returnVal = Convert.ToBoolean(cpSetting.PropertyValue, CultureInfo.InvariantCulture);
        //            }
        //            else
        //            {
        //                returnVal = displayTabId < 0;
        //            }
        //        }
        //        DataCache.SetCache(cacheKey, returnVal, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
        //        Utility.AddCacheKey(cacheKey, portalId);
        //    }
        //    return returnVal;
        //}

        public bool DisplayOnCurrentPage()
        {
            return Utility.GetValueFromCache(PortalId,
                                             Utility.CacheKeyPublishDisplayOnCurrentPage +
                                             itemVersionId.ToString(CultureInfo.InvariantCulture),
                                             delegate
                                             {
                                                 ItemType type = ItemType.GetFromId(ItemTypeId, typeof(ItemType));
                                                 ItemVersionSetting cpSetting =
                                                     ItemVersionSetting.GetItemVersionSetting(ItemVersionId,
                                                                                              type.Name + "Settings",
                                                                                              "DisplayOnCurrentPage",
                                                                                              portalId);
                                                 return cpSetting != null &&
                                                        Convert.ToBoolean(cpSetting.PropertyValue,
                                                                          CultureInfo.InvariantCulture);
                                             });
        }



        /// <summary>
        /// Determines whether this <see cref="Item"/> should be forced to always display on its assigned <see cref="DisplayTabId"/>, or whether it can display on any tab.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this <see cref="Item"/> should be forced to always display on its assigned <see cref="DisplayTabId"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool ForceDisplayOnPage()
        {
            return Utility.GetValueFromCache(PortalId, Utility.CacheKeyPublishForceDisplayOn + itemVersionId.ToString(CultureInfo.InvariantCulture),
                delegate
                {
                    ItemType type = ItemType.GetFromId(ItemTypeId, typeof(ItemType));
                    ItemVersionSetting cpSetting = ItemVersionSetting.GetItemVersionSetting(ItemVersionId, type.Name + "Settings", "ForceDisplayOnPage", portalId);
                    return cpSetting != null && Convert.ToBoolean(cpSetting.PropertyValue, CultureInfo.InvariantCulture);
                });
        }

        /// <summary>
        /// This method currently verifies that the item is assigned to a display page. Future versions
        /// will eliminate this requirement all together but for now this is needed by ItemLink.aspx when
        /// linking occurs. This could be used to test other settings to be valid before displaying.
        /// </summary>
        /// <returns></returns>
        public bool IsLinkable()
        {
            if (ForceDisplayOnPage())
            {
                return true;
            }
            bool isValid = Utility.IsPageOverrideable(portalId, displayTabId);
            return isValid;
        }

        protected void SaveInfo(IDbTransaction trans, int revisingId)
        {
            //insert new version or not
            //AuthorUserId = authorId;
            RevisingUserId = revisingId;
            if (IsNew)
            {
                if (itemIdentifier == Guid.Empty) itemIdentifier = Guid.NewGuid();
                itemId = AddItem(trans, itemTypeId, portalId, moduleId, itemIdentifier);

            }

            if (itemVersionId > 1 || ItemVersionIdentifier == Guid.Empty)
            {
                itemVersionIdentifier = Guid.NewGuid();
            }
            else
            {
                itemVersionIdentifier = ItemVersionIdentifier;
            }

            //if we aren't populating the meta description we should use the VersionInfoObject.Description                                       
            if (metaDescription.Trim() == string.Empty && description.Trim() != string.Empty)
            {
                string itemDescription = HtmlUtils.StripTags(description.Trim(), false);
                metaDescription = Utility.TrimDescription(399, itemDescription);
            }


            int ivd = AddItemVersion(trans, itemId, originalItemVersionId,
                Name, Description.Replace("<br>", "<br />"), startDate, endDate, languageId,
                authorUserId, metaKeywords, metaDescription, metaTitle,
                displayTabId, disabled, thumbnail, itemVersionIdentifier, url, newWindow, revisingUserId);
            originalItemVersionId = ItemVersionId;
            itemVersionId = ivd;
        }

        protected void SaveRelationships(IDbTransaction trans)
        {
            for (int i = 0; i < Relationships.Count; i++)
            {
                ItemRelationship ir = Relationships[i];
                ir.ChildItemId = ItemId;
                ir.ChildItemVersionId = ItemVersionId;
                if (ir.StartDate == null)
                {
                    ir.StartDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                }

                if (ir.HasSortOrderBeenSet)
                {
                    ItemRelationship.AddItemRelationship(trans, ir.ChildItemId, ir.ChildItemVersionId, ir.ParentItemId, ir.RelationshipTypeId, ir.StartDate,
                        ir.EndDate, ir.SortOrder);
                }
                else
                {
                    ItemRelationship.AddItemRelationshipWithOriginalSortOrder(trans, ir.ChildItemId, ir.ChildItemVersionId, ir.ParentItemId,
                        ir.RelationshipTypeId, ir.StartDate, ir.EndDate, OriginalItemVersionId);
                }
            }
        }

        /// <summary>
        /// if we remove a tag from a version we should decrement the TotalItems for a tag.
        /// </summary>
        /// <param name="trans"></param>
        protected void SaveTags(IDbTransaction trans)
        {
            for (int i = 0; i < Tags.Count; i++)
            {
                ItemTag it = Tags[i];

                //if this item tag relationship already existed for another versionID don't increment the count;
                //if (!ItemTag.CheckItemTag(trans, this.ItemId, it.TagId))
                //{
                //    Tag t = Tag.GetTag(it.TagId, PortalId);
                //    t.TotalItems++;
                //    t.Save(trans);
                //}

                it.ItemVersionId = ItemVersionId;
                //ad the itemtag relationship
                ItemTag.AddItemTag(trans, it.ItemVersionId, it.TagId);
            }
        }

        protected void SaveTags()
        {
            for (int i = 0; i < Tags.Count; i++)
            {
                ItemTag it = Tags[i];

                //if this item tag relationship already existed for another versionID don't increment the count;
                if (!ItemTag.CheckItemTag(ItemId, it.TagId))
                {
                    Tag t = Tag.GetTag(it.TagId, PortalId);
                    t.TotalItems++;
                    t.Save();
                }

                it.ItemVersionId = ItemVersionId;
                //ad the itemtag relationship
                ItemTag.AddItemTag(it.ItemVersionId, it.TagId);
            }
        }


        protected void SaveItemVersionSettings(IDbTransaction trans)
        {
            for (int i = 0; i < VersionSettings.Count; i++)
            {
                ItemVersionSetting ir = VersionSettings[i];

                ir.ItemVersionId = ItemVersionId;

                ItemVersionSetting.AddItemVersionSetting(trans, ir.ItemVersionId, ir.ControlName, ir.PropertyName, ir.PropertyValue);
            }
        }

        protected void SaveItemVersionSettings()
        {
            for (int i = 0; i < VersionSettings.Count; i++)
            {
                ItemVersionSetting ir = VersionSettings[i];

                ir.ItemVersionId = ItemVersionId;

                ItemVersionSetting.AddItemVersionSetting(ir.ItemVersionId, ir.ControlName, ir.PropertyName, ir.PropertyValue);
            }
        }

        //knowing that all dates come from FillObject as localized turn them back into invariantculture
        public void CorrectDates()
        {
            if (!string.IsNullOrEmpty(ApprovalDate)) ApprovalDate = Convert.ToDateTime(ApprovalDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(EndDate)) EndDate = Convert.ToDateTime(EndDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(StartDate)) StartDate = Convert.ToDateTime(StartDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(CreatedDate)) CreatedDate = Convert.ToDateTime(CreatedDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(ItemVersionDate)) ItemVersionDate = Convert.ToDateTime(ItemVersionDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(LastUpdated)) LastUpdated = Convert.ToDateTime(LastUpdated, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
        }

        public static void UpdateItem(IDbTransaction trans, int itemId, int moduleId)
        { DataProvider.Instance().UpdateItem(trans, itemId, moduleId); }

        public static void UpdateItemVersion(IDbTransaction trans, int itemId, int itemVersionId, int approvalStatusId, int userId, string approvalComments)
        { DataProvider.Instance().UpdateItemVersion(trans, itemId, itemVersionId, approvalStatusId, userId, approvalComments); }

        protected void UpdateApprovalStatus(IDbTransaction trans)
        {
            if (ApprovalStatusId == ApprovalStatus.Waiting.GetId())
            {
                if (ModuleBase.ApprovalEmailsEnabled(PortalId))
                {
                    if (!ModuleBase.IsUserAdmin(PortalId))
                    {
                        SendApprovalEmail();
                    }
                }
            }
            if (ModuleBase.ApprovalEmailsEnabled(PortalId))
            {
                SendStatusUpdateEmail();
            }
            UpdateItemVersion(trans, itemId, itemVersionId, approvalStatusId, revisingUserId, approvalComments);
        }

        private void SendApprovalEmail()
        {
            //string toAddress = string.Empty;
            int edittabid = -1;
            int editModuleId = -1;
            var objModules = new ModuleController();
            foreach (ModuleInfo mi in objModules.GetModulesByDefinition(PortalId, Utility.DnnFriendlyModuleName))
            {
                if (!mi.IsDeleted)
                {
                    var objTabs = new TabController();
                    if (!objTabs.GetTab(mi.TabID, mi.PortalID, false).IsDeleted)
                    {
                        edittabid = mi.TabID;
                        editModuleId = mi.ModuleID;
                        break;
                    }
                    continue;
                }
            }

            UserInfo ui = UserController.GetCurrentUserInfo();
            if (ui.Username != null)
            {
                var rc = new RoleController();

                ArrayList users = rc.GetUsersByRoleName(ui.PortalID, HostSettings.GetHostSetting(Utility.PublishEmailNotificationRole + PortalId));

                DotNetNuke.Entities.Portals.PortalSettings ps = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings();
                string linkUrl = Globals.NavigateURL(DisplayTabId, "", "VersionId=" + ItemVersionId.ToString(CultureInfo.InvariantCulture) + "&modid=" + moduleId);
                string linksUrl = Globals.NavigateURL(edittabid, "", "&ctl=" + Utility.AdminContainer + "&mid=" + editModuleId.ToString(CultureInfo.InvariantCulture) + "&adminType=" + "VersionsList&itemId=" + ItemId);

                //Now ask for the approriate subclass (which gets it from the correct resource file) the subject and body.
                string body = EmailApprovalBody;
                body = body.Replace("[ENGAGEITEMNAME]", name);
                body = body.Replace("[ENGAGEITEMLINK]", linkUrl);
                body = body.Replace("[ENGAGEITEMSLINK]", linksUrl);

                string subject = EmailApprovalSubject;

                foreach (UserInfo u in users)
                {
                    Mail.SendMail(ps.Email, u.Membership.Email, "", subject, body, "", "HTML", "", "", "", "");
                }
            }
        }

        private void SendStatusUpdateEmail()
        {
            //string toAddress = string.Empty;
            int edittabid = -1;
            int editModuleId = -1;
            var objModules = new ModuleController();
            foreach (ModuleInfo mi in objModules.GetModulesByDefinition(PortalId, Utility.DnnFriendlyModuleName))
            {
                if (!mi.IsDeleted && mi.TabID != -1)
                {
                    var objTabs = new TabController();
                    if (!objTabs.GetTab(mi.TabID, mi.PortalID, false).IsDeleted)
                    {
                        edittabid = mi.TabID;
                        editModuleId = mi.ModuleID;
                        break;
                    }
                    continue;
                }
            }


            UserInfo ui = UserController.GetCurrentUserInfo();
            if (ui.Username != null)
            {
                UserInfo versionAuthor = UserController.GetUser(PortalId, authorUserId, false);

                //if this is the same user, don't email them notification.
                if (versionAuthor != null && versionAuthor.Email != ui.Email)
                {
                    DotNetNuke.Entities.Portals.PortalSettings ps = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings();
                    //string linkUrl = Globals.NavigateURL(this.DisplayTabId, "", "VersionId=" + this.ItemVersionId.ToString(CultureInfo.InvariantCulture));
                    string linkUrl = Globals.NavigateURL(DisplayTabId, "", "VersionId=" + ItemVersionId.ToString(CultureInfo.InvariantCulture) + "&modid=" + ModuleId);
                    //href = Globals.NavigateURL(displayTabId, "", "VersionId=" + itemVersionId.ToString(CultureInfo.InvariantCulture) + "&modid=" + version.ModuleId.ToString());

                    string linksUrl = Globals.NavigateURL(edittabid, "", "&ctl=" + Utility.AdminContainer + "&mid=" + editModuleId.ToString(CultureInfo.InvariantCulture) + "&adminType=" + "VersionsList&itemId=" + ItemId);

                    //Now ask for the approriate subclass (which gets it from the correct resource file) the subject and body.
                    string body = EmailStatusChangeBody;
                    body = body.Replace("[ENGAGEITEMNAME]", name);
                    body = body.Replace("[ENGAGEITEMLINK]", linkUrl);
                    body = body.Replace("[ENGAGEITEMSLINK]", linksUrl);

                    body = body.Replace("[ADMINNAME]", ui.DisplayName);
                    body = body.Replace("[ENGAGEITEMCOMMENTS]", approvalComments);

                    body = body.Replace("[ENGAGESTATUS]", ApprovalStatus.GetFromId(ApprovalStatusId, typeof(ApprovalStatus)).Name);

                    string subject = EmailStatusChangeSubject;

                    Mail.SendMail(ps.Email, versionAuthor.Email, "", subject, body, "", "HTML", "", "", "", "");
                }
            }
        }
        #endregion

        #region "Public Properties"



        [XmlElement(Order = 1)]
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        [XmlElement(Order = 2)]
        public string ModuleTitle
        {
            get
            {
                string moduleTitle = string.Empty;
                using (IDataReader dr = DataProvider.Instance().GetModuleInfo(moduleId))
                {
                    if (dr.Read())
                    {
                        moduleTitle = dr["ModuleTitle"].ToString();
                    }
                }
                return moduleTitle;
            }
            set { }
        }

        [XmlIgnore]
        public int ItemTypeId
        {
            get { return itemTypeId; }
            set { itemTypeId = value; }
        }

        [XmlElement(Order = 3)]
        public int PortalId
        {
            get { return portalId; }
            set { portalId = value; }
        }

        [XmlElement(Order = 4)]
        public Guid ItemIdentifier
        {
            get { return itemIdentifier; }
            set { itemIdentifier = value; }
        }

        [XmlElement(Order = 5)]
        public Guid ItemVersionIdentifier
        {
            get { return itemVersionIdentifier; }
            set { itemVersionIdentifier = value; }
        }

        [XmlElement(Order = 6)]
        public int ApprovedItemVersionId
        {
            get { return approvedItemVersionId; }
            set { approvedItemVersionId = value; }
        }

        [XmlElement(Order = 7)]
        public string ApprovedItemVersionIdentifier
        {
            get
            {
                if (approvedItemVersionId == 0)
                {
                    return ItemVersionIdentifier.ToString();
                }
                //must resolve id to guid
                string approvedItemVersionIdentifier = string.Empty;
                using (IDataReader dr = DataProvider.Instance().GetItemVersionInfo(approvedItemVersionId))
                {
                    if (dr.Read())
                    {
                        approvedItemVersionIdentifier = dr["ItemVersionIdentifier"].ToString();
                    }
                }
                return approvedItemVersionIdentifier;
            }
            set { }
        }

        [XmlElement(Order = 8)]
        public string CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        [XmlElement(Order = 9)]
        public int ItemVersionId
        {
            get { return itemVersionId; }
            set { itemVersionId = value; }
        }

        [XmlElement(Order = 10)]
        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        [XmlElement(Order = 11)]
        public int OriginalItemVersionId
        {
            get { return originalItemVersionId; }
            set { originalItemVersionId = value; }
        }

        [XmlElement(Order = 12)]
        public string OriginalItemVersionIdentifier
        {
            get
            {
                if (originalItemVersionId <= 0)
                {
                    return ItemVersionIdentifier.ToString();
                }
                //must resolve id to guid
                string originalItemVersionIdentifier = string.Empty;
                using (IDataReader dr = DataProvider.Instance().GetItemVersionInfo(originalItemVersionId))
                {
                    if (dr.Read())
                    {
                        originalItemVersionIdentifier = dr["ItemVersionIdentifier"].ToString();
                    }
                }
                return originalItemVersionIdentifier;
            }
            set { }
        }

        [XmlElement(Order = 13)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlElement(Order = 14)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [XmlElement(Order = 15)]
        public string ItemVersionDate
        {
            get { return itemVersionDate; }
            set { itemVersionDate = value; }
        }

        [XmlElement(Order = 16)]
        public string StartDate
        {
            get { return startDate; }
            set
            {
                startDate = Utility.HasValue(value) ? value : null;
            }
        }

        [XmlElement(Order = 17)]
        public string EndDate
        {
            get { return endDate; }
            set
            {
                endDate = Utility.HasValue(value) ? value : null;
            }
        }

        [XmlElement(Order = 18)]
        public int LanguageId
        {
            get { return languageId; }
            set { languageId = value; }
        }

        //public int CurrentAuthor
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Request.IsAuthenticated)
        //        {
        //            return UserController.GetCurrentUserInfo().UserID;
        //        }

        //        return -1;
        //    }
        //}

        [XmlElement(Order = 19)]
        public int AuthorUserId
        {
            set { authorUserId = value; }
            get { return authorUserId; }
        }

        private string originalAuthor = string.Empty;
        [XmlElement(Order = 20)]
        public string Author
        {
            get
            {
                //UserController controller = new UserController();
                ////verify that the user is a user in this system. 
                //UserInfo user = controller.GetUserByUsername(portalId, originalAuthor);
                //if (user != null)
                //{
                //    return user.Username;
                //}
                //else
                //{
                //    return string.Empty;
                //}


                ItemVersionSetting auNameSetting = ItemVersionSetting.GetItemVersionSetting(ItemVersionId, "lblAuthorName", "Text", PortalId);
                if (auNameSetting != null && auNameSetting.ToString().Trim().Length > 0)
                {
                    originalAuthor = auNameSetting.PropertyValue;
                }
                else
                {
                    var uc = new UserController();
                    UserInfo ui = uc.GetUser(portalId, authorUserId);
                    if (ui != null)
                    {
                        originalAuthor = ui.DisplayName;
                    }
                }

                return originalAuthor;
            }
            set
            {
                originalAuthor = value;
            }
        }

        [XmlElement(Order = 21)]
        public int ApprovalStatusId
        {
            get { return approvalStatusId; }
            set { approvalStatusId = value; }
        }

        [XmlElement(Order = 22)]
        public string ApprovalStatusName
        {
            get
            {
                return ApprovalStatus.GetFromId(approvalStatusId, typeof(ApprovalStatus)).Name;
            }
            set { }
        }

        [XmlElement(Order = 23)]
        public string ApprovalDate
        {
            get { return approvalDate; }
            set { approvalDate = value; }
        }

        [XmlElement(Order = 24)]
        public int ApprovalUserId
        {
            get { return approvalUserId; }
            set { approvalUserId = value; }
        }

        private string originalApprovalUser = string.Empty;
        [XmlElement(Order = 25)]
        public string ApprovalUser
        {
            get
            {
                //UserController controller = new UserController();
                //UserInfo user = controller.GetUser(portalId, approvalUserId);
                //return user.Username; 
                return originalApprovalUser;
            }
            set { originalApprovalUser = value; }
        }

        [XmlElement(Order = 26)]
        public string ApprovalComments
        {
            get { return approvalComments; }
            set { approvalComments = value; }
        }

        [XmlElement(Order = 27)]
        public string MetaKeywords
        {
            get { return metaKeywords; }
            set { metaKeywords = value; }
        }

        [XmlElement(Order = 28)]
        public string MetaDescription
        {
            get { return metaDescription; }
            set { metaDescription = value; }
        }

        [XmlElement(Order = 29)]
        public string MetaTitle
        {
            get { return metaTitle; }
            set { metaTitle = value; }
        }

        [XmlElement(Order = 30)]
        public int DisplayTabId
        {
            get { return displayTabId; }
            set { displayTabId = value; }
        }

        private string displayTabName = string.Empty;
        [XmlElement(Order = 31)]
        public string DisplayTabName
        {
            get
            {
                if (displayTabName.Length == 0)
                {
                    using (IDataReader dr = DataProvider.Instance().GetPublishTabName(displayTabId, portalId))
                    {
                        if (dr.Read())
                        {
                            displayTabName = dr["TabName"].ToString();
                        }
                    }
                }
                return displayTabName;
            }
            set { displayTabName = value; }
        }

        [XmlElement(Order = 32)]
        public string LastUpdated
        {
            get { return lastUpdated; }
            set { lastUpdated = value; }
        }

        [XmlElement(Order = 33)]
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        [XmlElement(Order = 34)]
        public string Thumbnail
        {
            get { return thumbnail; }
            set { thumbnail = value; }
        }

        [XmlElement(Order = 35)]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        [XmlElement(Order = 36)]
        public bool NewWindow
        {
            get { return newWindow; }
            set { newWindow = value; }
        }

        [XmlElement(Order = 38)]
        public int RevisingUserId
        {
            set { revisingUserId = value; }
            get { return revisingUserId; }
        }

        private string originalRevisingUser = string.Empty;
        [XmlElement(Order = 37)]
        public string RevisingUser
        {
            get
            {
                //UserController controller = new UserController();
                ////verify that the user is a user in this system. 
                //UserInfo user = controller.GetUserByUsername(portalId, originalAuthor);
                //if (user != null)
                //{
                //    return user.Username;
                //}
                //else
                //{
                //    return string.Empty;
                //}
                return originalRevisingUser;
            }
            set { originalRevisingUser = value; }
        }


        [XmlIgnore]
        public string GetItemExternalUrl
        {
            get
            {
                string strUrl = string.Empty;
                switch (Globals.GetURLType(url))
                {
                    case TabType.Normal:
                        strUrl = Globals.NavigateURL(displayTabId);
                        break;
                    case TabType.Tab:
                        strUrl = Globals.NavigateURL(Convert.ToInt32(url, CultureInfo.InvariantCulture));
                        break;
                    case TabType.File:
                        strUrl = Globals.LinkClick(url, displayTabId, Null.NullInteger);
                        break;
                    case TabType.Url:
                        strUrl = url;
                        break;
                }

                return strUrl;
            }

        }

        [XmlIgnore]
        public int ViewCount
        {
            set;
            get;
        }

        [XmlIgnore]
        public int CommentCount
        {
            set;
            get;
        }


        public bool IsNew
        {
            get { return itemId == -1; }
        }

        #endregion

        public static bool DoesItemExist(string name, int authorUserId)
        {
            //try loading the item, if we get an ItemID back we know this already exists.
            if (DataProvider.Instance().FindItemId(name, authorUserId) > 0) return true;
            return false;
        }


        /// <summary>
        /// Checks to see if an item exists by a specific name, from a specific author, in a specific category.
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <param name="authorUserId">The ID of the author</param>
        /// <param name="categoryId">The ID of the category</param>
        /// <returns>true or false</returns>

        public static bool DoesItemExist(string name, int authorUserId, int categoryId)
        {
            //try loading the item, if we get an ItemID back we know this already exists.
            if (DataProvider.Instance().FindItemId(name, authorUserId, categoryId) > 0) return true;
            return false;
        }

        public static Item GetItem(int itemId, int portalId, int itemTypeId, bool isCurrent)
        {
            string cacheKey = Utility.CacheKeyPublishItem + itemId.ToString(CultureInfo.InvariantCulture);
            Item i;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    i = (Item)o;
                }
                else
                {
                    IDataReader dr = DataProvider.Instance().GetItem(itemId, portalId, isCurrent);
                    ItemType it = ItemType.GetFromId(itemTypeId, typeof(ItemType));

                    i = (Item)CBO.FillObject(dr, it.GetItemType);

                    // ReSharper disable ConditionIsAlwaysTrueOrFalse
                    if (i != null)
                    // ReSharper restore ConditionIsAlwaysTrueOrFalse
                    {
                        i.CorrectDates();
                        DataCache.SetCache(cacheKey, i, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                        Utility.AddCacheKey(cacheKey, portalId);
                    }
                }
            }
            else
            {
                IDataReader dr = DataProvider.Instance().GetItem(itemId, portalId, isCurrent);
                ItemType it = ItemType.GetFromId(itemTypeId, typeof(ItemType));

                i = (Item)CBO.FillObject(dr, it.GetItemType);
                i.CorrectDates();
            }
            return i;


            //IDataReader dr = DataProvider.Instance().GetItem(itemId, portalId, isCurrent);

            //ItemType it = ItemType.GetFromId(itemTypeId, typeof(ItemType));

            //Item a = (Item)CBO.FillObject(dr, it.GetItemType);
            //a.CorrectDates();
            //return a;
        }

        public void AddView(int userId, int tabId, string ipAddress, string userAgent, string httpReferrer, string siteUrl)
        {
            if (ModuleBase.IsViewTrackingEnabledForPortal(PortalId))
            {
                DataProvider.Instance().AddItemView(itemId, itemVersionId, userId, tabId, ipAddress, userAgent, httpReferrer, siteUrl);
            }
        }

        public static DataSet GetItemVersions(int itemId, int portalId)
        {
            return DataProvider.Instance().GetItemVersions(itemId, portalId);
        }

        public static int GetItemIdFromVersion(int itemVersionId, int portalId)
        {
            return DataProvider.Instance().GetItemIdFromVersion(itemVersionId, portalId);
        }

        public static int GetItemIdFromVersion(int itemVersionId)
        {
            return DataProvider.Instance().GetItemIdFromVersion(itemVersionId);
        }

        public static DataSet GetAllChildren(int parentItemId, int relationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetAllChildren(parentItemId, relationshipTypeId, portalId);
        }

        public static DataSet GetAllChildren(int itemTypeId, int parentItemId, int relationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetAllChildren(itemTypeId, parentItemId, relationshipTypeId, portalId);
        }

        public static DataSet GetAllChildren(int itemTypeId, int parentItemId, int relationshipTypeId, int otherRelationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetAllChildren(itemTypeId, parentItemId, relationshipTypeId, otherRelationshipTypeId, portalId);
        }

        public static IDataReader GetAllChildrenAsDataReader(int itemTypeId, int parentItemId, int relationshipTypeId, int otherRelationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetAllChildrenAsDataReader(itemTypeId, parentItemId, relationshipTypeId, otherRelationshipTypeId, portalId);
        }

        [Obsolete("This method is not used.")]
        public static DataSet GetChildren(int parentItemId, int relationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetChildren(parentItemId, relationshipTypeId, portalId);
        }

        public static int AddItem(IDbTransaction trans, int itemTypeId, int portalId, int moduleId, Guid itemIdentifier)
        {
            return DataProvider.Instance().AddItem(trans, itemTypeId, portalId, moduleId, itemIdentifier);
        }

        public static int AddItemVersion(int itemId, int originalItemVersionId, string name, string description, string startDate, string endDate, int languageId, int authorUserId, string metaKeywords, string metaDescription, string metaTitle, int displayTabId, bool disabled, string thumbnail, Guid itemVersionIdentifier, string url, bool newWindow, int revisingUserId)
        {
            return DataProvider.Instance().AddItemVersion(itemId, originalItemVersionId, name, description, startDate, endDate, languageId, authorUserId, metaKeywords, metaDescription, metaTitle, displayTabId, disabled, thumbnail, itemVersionIdentifier, url, newWindow, revisingUserId);
        }

        public static int AddItemVersion(IDbTransaction trans, int itemId, int originalItemVersionId, string name, string description, string startDate, string endDate, int languageId, int authorUserId, string metaKeywords, string metaDescription, string metaTitle, int displayTabId, bool disabled, string thumbnail, Guid itemVersionIdentifier, string url, bool newWindow, int revisingUserId)
        {
            return DataProvider.Instance().AddItemVersion(trans, itemId, originalItemVersionId, name, description, startDate, endDate, languageId, authorUserId, metaKeywords, metaDescription, metaTitle, displayTabId, disabled, thumbnail, itemVersionIdentifier, url, newWindow, revisingUserId);
        }

        public static IDataReader GetItems(int itemTypeId, int portalId)
        {
            return DataProvider.Instance().GetItems(itemTypeId, portalId);
        }

        public static DataSet GetItems(int parentItemId, int portalId, int relationshipTypeId)
        {
            return DataProvider.Instance().GetItems(parentItemId, portalId, relationshipTypeId);
        }

        public static DataSet GetItems(int parentItemId, int portalId, int relationshipTypeId, int itemTypeId)
        {
            return DataProvider.Instance().GetItems(parentItemId, portalId, relationshipTypeId, itemTypeId);
        }

        public static DataSet GetItems(int parentItemId, int portalId, int relationshipTypeId, int otherRelationshipTypeId, int itemTypeId)
        {
            return DataProvider.Instance().GetItems(parentItemId, portalId, relationshipTypeId, otherRelationshipTypeId, itemTypeId);
        }

        public static DataSet GetParentItems(int itemId, int portalId, int relationshipTypeId)
        {
            return DataProvider.Instance().GetParentItems(itemId, portalId, relationshipTypeId);
        }

        public static string GetItemType(int itemId)
        {
            return DataProvider.Instance().GetItemType(itemId);
        }

        public static string GetItemType(int itemId, int portalId)
        {
            string itemType;
            //return DataProvider.Instance().GetItemType(itemId);
            string cacheKey = Utility.CacheKeyPublishItemTypeNameItemId + itemId.ToString(CultureInfo.InvariantCulture); // +"PageId";
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                itemType = o != null ? o.ToString() : GetItemType(itemId);
                if (itemType != null)
                {
                    DataCache.SetCache(cacheKey, itemType, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                itemType = GetItemType(itemId);
            }
            return itemType;
        }


        public static int GetItemTypeId(int itemId)
        {
            return DataProvider.Instance().GetItemTypeId(itemId);
        }

        public static int GetItemTypeId(int itemId, int portalId)
        {
            int itemTypeId;
            string cacheKey = Utility.CacheKeyPublishItemTypeIntForItemId + itemId.ToString(CultureInfo.InvariantCulture); // +"PageId";
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                itemTypeId = o != null ? Convert.ToInt32(o.ToString()) : GetItemTypeId(itemId);
                if (itemTypeId != -1)
                {
                    DataCache.SetCache(cacheKey, itemTypeId, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                itemTypeId = GetItemTypeId(itemId);
            }

            return itemTypeId;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not displaying properties of this class.")]
        public static DataTable GetItemTypes()
        {
            //cached version below
            return DataProvider.Instance().GetItemTypes();
        }

        public static DataTable GetItemTypes(int portalId)
        {
            DataTable dt;
            string cacheKey = Utility.CacheKeyPublishItemTypesDT + portalId.ToString(CultureInfo.InvariantCulture); // +"PageId";
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = GetItemTypes();
                }
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = GetItemTypes();
            }
            return dt;


            //return DataProvider.Instance().GetItemTypes();
        }

        [Obsolete("This method signature should not be used, please use the signature that accepts PortalId as a parameter so that the cache is cleared properly. DeleteItem(int itemId, int portalId).", false)]
        public static void DeleteItem(int itemId)
        {
            DataProvider.Instance().DeleteItem(itemId);
        }

        public static void DeleteItem(int itemId, int portalId)
        {
            DataProvider.Instance().DeleteItem(itemId);
            Utility.ClearPublishCache(portalId);
        }

        /// <summary>
        /// Clears the view count on the item table to 0 for all items within a portal
        /// </summary>
        /// <param name="portalId">The Portal in which the items will be cleared</param>
        /// <returns></returns>
        public static void ClearItemsViewCount(int portalId)
        {
            DataProvider.Instance().ClearItemsViewCount(portalId);
        }

        /// <summary>
        /// Clears the comment count on the item table to 0 for all items within a portal
        /// </summary>
        /// <param name="portalId">The Portal in which the items will be cleared</param>
        /// <returns></returns>
        public static void ClearItemsCommentCount(int portalId)
        {
            DataProvider.Instance().ClearItemsCommentCount(portalId);
        }

        /// <summary>
        /// Runs the stored procedure to calculate the views and comment counts for all items.
        /// </summary>
        /// <returns></returns>
        public static void RunPublishStats()
        {
            DataProvider.Instance().RunPublishStats();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "The method performs a time-consuming operation. The method is perceivably slower than the time it takes to set or get a field's value.")]
        public string GetApprovalStatusTypeName()
        {
            return DataProvider.Instance().GetApprovalStatusTypeName(ApprovalStatusId);
        }

        /// <summary>
        /// Gets all articles related to this article
        /// </summary>
        /// <param name="articlePortalId">The Portal in which the related articles live</param>
        /// <returns></returns>
        public Article[] GetRelatedArticles(int articlePortalId)
        {
            ArrayList al = ItemRelationship.GetItemRelationships(ItemId, ItemVersionId, RelationshipType.ItemToRelatedArticle.GetId(), true, portalId);

            var m = new ArrayList();
            foreach (ItemRelationship ir in al)
            {
                m.Add(Article.GetArticle(ir.ParentItemId, articlePortalId));
            }
            return (Article[])m.ToArray(typeof(Article));
        }


        /// <summary>
        /// this is a single article to be displayed as a sub section of a page.
        /// </summary>
        /// <param name="articlePortalId">Portal in which the related article lives</param>
        /// <returns></returns>
        public Article GetRelatedArticle(int articlePortalId)
        {
            ArrayList al = ItemRelationship.GetItemRelationships(itemId, itemVersionId, RelationshipType.ItemToArticleLinks.GetId(), true, portalId);
            //ArrayList m = new ArrayList();
            Article a = null;
            foreach (ItemRelationship ir in al)
            {
                a = Article.GetArticle(ir.ParentItemId, articlePortalId);
            }
            return a;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Calls database")]
        public int GetParentCategoryId()
        {
            //find the parent category ID from an item
            return Category.GetParentCategory(ItemId, PortalId);
        }

        /// <summary>
        /// Loads the <see cref="ItemTag"/>s for this <see cref="Item"/>, clearing any tags already in the <see cref="Item.Tags"/> collection.
        /// </summary>
        protected void LoadTags()
        {
            Tags.Clear();
            foreach (ItemTag tag in ItemTag.GetItemTags(ItemVersionId))
            {
                Tags.Add(tag);
            }
        }

        /// <summary>
        /// Loads the <see cref="ItemRelationship"/>s for this <see cref="Item"/>, clearing any relationships already in the <see cref="Item.Relationships"/> collection.
        /// </summary>
        protected void LoadRelationships()
        {
            Relationships.Clear();
            foreach (ItemRelationship relationship in ItemRelationship.GetItemRelationships(ItemId, ItemVersionId, true))
            {
                relationship.CorrectDates();
                Relationships.Add(relationship);
            }
        }

        protected void LoadItemVersionSettings()
        {
            VersionSettings.Clear();
            foreach (ItemVersionSetting ivr in ItemVersionSetting.GetItemVersionSettings(ItemVersionId))
            {
                VersionSettings.Add(ivr);
            }
        }

        #region TransportableElement Methods

        protected virtual void ResolveIds(int currentModuleId)
        {
            //If the XML doesn't specify a start date we'll default to Today.
            if (string.IsNullOrEmpty(StartDate)) StartDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            UserInfo user = UserController.GetUserByName(portalId, Author);
            authorUserId = user != null ? user.UserID : UserController.GetCurrentUserInfo().UserID;

            //Revising user.
            user = UserController.GetUserByName(portalId, RevisingUser);
            revisingUserId = user != null ? user.UserID : UserController.GetCurrentUserInfo().UserID;

            //Approving user.
            user = UserController.GetUserByName(portalId, ApprovalUser);
            approvalUserId = user != null ? user.UserID : UserController.GetCurrentUserInfo().UserID;

            bool found = false;
            //display tab - try and resolve from name in XML file.
            using (IDataReader dr = DataProvider.Instance().GetPublishTabId(DisplayTabName, portalId))
            {
                if (dr.Read())
                {
                    found = true;
                    displayTabId = (int)dr["TabId"];
                }
            }

            if (found == false)
            {
                //The TabId couldn't be resolved using the DisplayTabName, let's try to resolve the current moduleId
                //to a TabId.
                using (IDataReader dr = DataProvider.Instance().GetModulesByModuleId(currentModuleId))
                {
                    if (dr.Read())
                    {
                        displayTabId = (int)dr["TabId"];
                    }
                    else
                    {
                        //Default to setting for module
                        string settingName = Utility.PublishDefaultDisplayPage + PortalId.ToString(CultureInfo.InvariantCulture);
                        string setting = HostSettings.GetHostSetting(settingName);
                        displayTabId = Convert.ToInt32(setting, CultureInfo.InvariantCulture);
                    }
                }
            }

            //module title
            using (IDataReader dr = DataProvider.Instance().GetPublishModuleId(ModuleTitle, portalId))
            {
                if (dr.Read())
                {
                    moduleId = (int)dr["ModuleId"];
                }
            }

            //Language ID one day.

            //Approval Status
            ApprovalStatus status = ApprovalStatus.GetFromName(ApprovalStatusName, typeof(ApprovalStatus));
            approvalStatusId = status.GetId();
        }

        #endregion
    }
}
