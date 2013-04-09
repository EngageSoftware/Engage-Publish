// <copyright file="ItemVersionSetting.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.Xml.Serialization;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Services.Exceptions;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Portability;
    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// Summary description for ItemInfo.
    /// </summary>
    [XmlRoot(ElementName = "itemversionsetting", IsNullable = false)]
    public class ItemVersionSetting : TransportableElement, IEquatable<ItemVersionSetting>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _controlName = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _itemVersionId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid _itemVersionIdentifier;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _propertyName = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _propertyValue = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _settingsId = -1;

        public ItemVersionSetting()
        {
            // required for Import/Export - Reflection needs a public constructor.
        }

        public ItemVersionSetting(Setting setting)
        {
            this._propertyName = setting.PropertyName;
            this._propertyValue = setting.PropertyValue;
            this._controlName = setting.ControlName;
        }

        [XmlElement(Order = 4)]
        public string ControlName
        {
            [DebuggerStepThrough]
            get { return this._controlName; }
            [DebuggerStepThrough]
            set { this._controlName = value; }
        }

        [XmlElement(Order = 2)]
        public int ItemVersionId
        {
            [DebuggerStepThrough]
            get { return this._itemVersionId; }
            [DebuggerStepThrough]
            set { this._itemVersionId = value; }
        }

        [XmlElement(Order = 3)]
        public Guid ItemVersionIdentifier
        {
            [DebuggerStepThrough]
            get { return this._itemVersionIdentifier; }
            [DebuggerStepThrough]
            set { this._itemVersionIdentifier = value; }
        }

        [XmlElement(Order = 5)]
        public string PropertyName
        {
            [DebuggerStepThrough]
            get { return this._propertyName; }
            [DebuggerStepThrough]
            set { this._propertyName = value; }
        }

        [XmlElement(Order = 6)]
        public string PropertyValue
        {
            [DebuggerStepThrough]
            get { return this._propertyValue; }
            [DebuggerStepThrough]
            set { this._propertyValue = value; }
        }

        [XmlElement(Order = 1)]
        public int SettingsId
        {
            [DebuggerStepThrough]
            get { return this._settingsId; }
            [DebuggerStepThrough]
            set { this._settingsId = value; }
        }

        public static void AddItemVersionSetting(
            IDbTransaction trans, int itemVersionId, string controlName, string propertyName, string propertyValue)
        {
            DataProvider.Instance().AddItemVersionSetting(trans, itemVersionId, controlName, propertyName, propertyValue);
        }

        public static void AddItemVersionSetting(int itemVersionId, string controlName, string propertyName, string propertyValue)
        {
            DataProvider.Instance().AddItemVersionSetting(itemVersionId, controlName, propertyName, propertyValue);
        }

        public static ItemVersionSetting GetItemVersionSetting(int itemVersionId, string controlName, string propertyName)
        {
            IDataReader dr = DataProvider.Instance().GetItemVersionSetting(itemVersionId, controlName, propertyName);

            return (ItemVersionSetting)CBO.FillObject(dr, typeof(ItemVersionSetting));
        }

        public static ItemVersionSetting GetItemVersionSetting(int itemVersionId, string controlName, string propertyName, int portalId)
        {
            // IDataReader dr = DataProvider.Instance().GetItemVersionSetting(_itemVersionId, _controlName, _propertyName);

            // return (ItemVersionSetting)CBO.FillObject(dr, typeof(ItemVersionSetting));
            string cacheKey = Utility.CacheKeyPublishItemVersionSetting + controlName.ToString(CultureInfo.InvariantCulture) +
                              propertyName.ToString(CultureInfo.InvariantCulture) + itemVersionId.ToString(CultureInfo.InvariantCulture);
            ItemVersionSetting ivs;

            if (ModuleBase.UseCachePortal(portalId))
            {
                var o = DataCache.GetCache(cacheKey);
                if (o != null && o.ToString() != "-1")
                {
                    ivs = (ItemVersionSetting)o;
                }
                else
                {
                    ivs = GetItemVersionSetting(itemVersionId, controlName, propertyName);
                    if (ivs == null)
                    {
                        // for settings we don't have a value for in the DB we should set the cache to -1 so we don't always make requests to the DB
                        DataCache.SetCache(cacheKey, "-1", DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                        Utility.AddCacheKey(cacheKey, portalId);
                    }
                }

                if (ivs != null)
                {
                    DataCache.SetCache(cacheKey, ivs, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                ivs = GetItemVersionSetting(itemVersionId, controlName, propertyName);
            }

            return ivs;
        }

        public static int GetItemVersionSettingByIdentifier(Guid itemVersionIdentifier, int portalId)
        {
            int itemVersionId = -1;
            using (IDataReader dr = DataProvider.Instance().GetItemVersion(itemVersionIdentifier, portalId))
            {
                if (dr.Read())
                {
                    itemVersionId = (int)dr["ItemVersionId"];
                }
            }

            return itemVersionId;
        }

        public static List<ItemVersionSetting> GetItemVersionSettings(int itemVersionId, string controlName)
        {
            return CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettings(itemVersionId, controlName));
        }

        public static List<ItemVersionSetting> GetItemVersionSettings(int itemVersionId)
        {
            return CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettings(itemVersionId));
        }

        public static List<ItemVersionSetting> GetItemVersionSettings(int itemVersionId, string controlName, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishItemVersionSettings + controlName.ToString(CultureInfo.InvariantCulture) +
                              itemVersionId.ToString(CultureInfo.InvariantCulture);
            List<ItemVersionSetting> ivs;

            if (ModuleBase.UseCachePortal(portalId))
            {
                var o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    ivs = (List<ItemVersionSetting>)o;
                }
                else
                {
                    ivs = GetItemVersionSettings(itemVersionId, controlName);
                }

                if (ivs != null)
                {
                    DataCache.SetCache(cacheKey, ivs, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                ivs = GetItemVersionSettings(itemVersionId, controlName);
            }

            return ivs;
        }

        [Obsolete("This method should not be used, please use GetItemVersionSettingsByModuleId(moduleId, portalId).", true)]
        public static List<ItemVersionSetting> GetItemVersionSettingsByModuleId(int moduleId)
        {
            List<ItemVersionSetting> settings =
                CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByModuleId(moduleId));
            return settings;
        }

        public static List<ItemVersionSetting> GetItemVersionSettingsByModuleId(int moduleId, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishItemVersionSettingsByModuleId + moduleId;
            List<ItemVersionSetting> settings;
            if (ModuleBase.UseCachePortal(portalId))
            {
                var o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    settings = (List<ItemVersionSetting>)o;
                }
                else
                {
                    settings = CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByModuleId(moduleId));
                }

                if (settings != null)
                {
                    DataCache.SetCache(cacheKey, settings, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                settings = CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByModuleId(moduleId));
            }

            return settings;

            // List<ItemVersionSetting> settings = CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByModuleId(moduleId));
            // return settings;
        }

        public static List<ItemVersionSetting> GetItemVersionSettingsByPortalId(int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishItemVersionSettingsByPortalId + portalId;
            List<ItemVersionSetting> settings;
            if (ModuleBase.UseCachePortal(portalId))
            {
                var o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    settings = (List<ItemVersionSetting>)o;
                }
                else
                {
                    settings = CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByPortalId(portalId));
                }

                if (settings != null)
                {
                    DataCache.SetCache(cacheKey, settings, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                settings = CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByPortalId(portalId));
            }

            return settings;

            // List<ItemVersionSetting> settings = CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByPortalId(portalId));
            // return settings;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ItemVersionSetting);
        }

        public override int GetHashCode()
        {
            return this._itemVersionId.GetHashCode() ^ this._propertyName.GetHashCode() * 37 ^ this._controlName.GetHashCode() * 37;
        }

        public override void Import(int currentModuleId, int portalId)
        {
            // Does this exist in my db?
            int localVersionId = GetItemVersionSettingByIdentifier(this.ItemVersionIdentifier, portalId);
            if (localVersionId > 0)
            {
                // this version does not exist.
                this._itemVersionId = localVersionId;
                AddItemVersionSetting(this._itemVersionId, this._controlName, this._propertyName, this._propertyValue);
            }
            else
            {
                Exceptions.LogException(new Exception("No matching Item Version could be found to create a setting."));
            }
        }

        public void Save()
        {
            // used for adding an itemversionsetting to an existing article
            IDbConnection newConnection = DataProvider.Instance().GetConnection();
            IDbTransaction trans = newConnection.BeginTransaction();
            AddItemVersionSetting(trans, this.ItemVersionId, this.ControlName, this.PropertyName, this.PropertyValue);
            trans.Commit();
        }

        public bool Equals(ItemVersionSetting other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }

            return this._itemVersionId.Equals(other._itemVersionId) &&
                   this._propertyName.Equals(other._propertyName, StringComparison.OrdinalIgnoreCase) &&
                   this._controlName.Equals(other._controlName, StringComparison.OrdinalIgnoreCase);
        }
    }
}