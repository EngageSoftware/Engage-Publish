//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.



namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.Xml.Serialization;
    using DotNetNuke.Common.Utilities;
    using Data;
    using Portability;
    using Util;

	/// <summary>
	/// Summary description for ItemInfo.
	/// </summary>
    [XmlRootAttribute(ElementName = "itemversionsetting", IsNullable = false)]
	public class ItemVersionSetting : TransportableElement, IEquatable<ItemVersionSetting>
    {
		#region "Public Properties"

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _settingsId = -1;
        [XmlElement(Order = 1)]
        public int SettingsId 
		{
            [DebuggerStepThrough]
            get { return _settingsId; }
            [DebuggerStepThrough]
            set { _settingsId = value; }
		}

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _itemVersionId = -1;
        [XmlElement(Order = 2)]
		public int ItemVersionId 
		{
            [DebuggerStepThrough]
            get { return _itemVersionId; }
            [DebuggerStepThrough]
            set { _itemVersionId = value; }
		}

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid _itemVersionIdentifier;
        [XmlElement(Order = 3)]
        public Guid ItemVersionIdentifier
        {
            [DebuggerStepThrough]
            get { return _itemVersionIdentifier; }
            [DebuggerStepThrough]
            set { _itemVersionIdentifier = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _controlName = string.Empty;
        [XmlElement(Order = 4)]
        public string ControlName 
		{
            [DebuggerStepThrough]
            get { return _controlName; }
            [DebuggerStepThrough]
            set { _controlName = value; }
		}

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _propertyName = string.Empty;
        [XmlElement(Order = 5)]
        public string PropertyName 
		{
            [DebuggerStepThrough]
            get { return _propertyName; }
            [DebuggerStepThrough]
            set { _propertyName = value; }
		}

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _propertyValue = string.Empty;
        [XmlElement(Order = 6)]
        public string PropertyValue
		{
            [DebuggerStepThrough]
            get { return _propertyValue; }
            [DebuggerStepThrough]
            set { _propertyValue = value; }
		}


		#endregion

        #region "Public Methods"

		public ItemVersionSetting()
		{
            //required for Import/Export - Reflection needs a public constructor.
		}

        public ItemVersionSetting(Setting setting)
        {
            _propertyName = setting.PropertyName;
            _propertyValue = setting.PropertyValue;
            _controlName = setting.ControlName;
        }

        public void Save()
        {//used for adding an itemversionsetting to an existing article
            IDbConnection newConnection = DataProvider.GetConnection();
            IDbTransaction trans = newConnection.BeginTransaction();
            AddItemVersionSetting(trans, ItemVersionId, ControlName, PropertyName, PropertyValue);
            trans.Commit();
        }

        public static void AddItemVersionSetting(IDbTransaction trans, int itemVersionId, string controlName, string propertyName, string propertyValue)
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
            //IDataReader dr = DataProvider.Instance().GetItemVersionSetting(_itemVersionId, _controlName, _propertyName);

            //return (ItemVersionSetting)CBO.FillObject(dr, typeof(ItemVersionSetting));

            string cacheKey = Utility.CacheKeyPublishItemVersionSetting + controlName.ToString(CultureInfo.InvariantCulture) + propertyName.ToString(CultureInfo.InvariantCulture) + itemVersionId.ToString(CultureInfo.InvariantCulture);
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
                    if(ivs==null)
                    {             
                        //for settings we don't have a value for in the DB we should set the cache to -1 so we don't always make requests to the DB
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
            string cacheKey = Utility.CacheKeyPublishItemVersionSettings + controlName.ToString(CultureInfo.InvariantCulture) + itemVersionId.ToString(CultureInfo.InvariantCulture);
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
            
            //List<ItemVersionSetting> settings = CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByPortalId(portalId));
            //return settings;
        }
        [Obsolete("This method should not be used, please use GetItemVersionSettingsByModuleId(moduleId, portalId).", true)]
        public static List<ItemVersionSetting> GetItemVersionSettingsByModuleId(int moduleId)
        {
            List<ItemVersionSetting> settings = CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByModuleId(moduleId));
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

            //List<ItemVersionSetting> settings = CBO.FillCollection<ItemVersionSetting>(DataProvider.Instance().GetItemVersionSettingsByModuleId(moduleId));
            //return settings;
        }



        public static int GetItemVersionSettingByIdentifier(Guid itemVersionIdentifier, int portalId)
        {
            int itemVersionId = -1;
            using (IDataReader dr = DataProvider.Instance().GetItemVersion(itemVersionIdentifier, portalId))
            {
                if (dr.Read())
                {
                    itemVersionId = (int) dr["ItemVersionId"];
                }
            }

            return itemVersionId;          
        }

		#endregion

        #region TransportableElement Methods

        public override void Import(int currentModuleId, int portalId)
        {
            //Does this exist in my db?
            int localVersionId = GetItemVersionSettingByIdentifier(ItemVersionIdentifier, portalId);
            if (localVersionId > 0)
            {
                //this version does not exist.
                _itemVersionId = localVersionId;
                AddItemVersionSetting(_itemVersionId, _controlName, _propertyName, _propertyValue);
            }
            else
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(new Exception("No matching Item Version could be found to create a setting."));
            }
        }

        #endregion

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

            return _itemVersionId.Equals(other._itemVersionId)
                && _propertyName.Equals(other._propertyName, StringComparison.OrdinalIgnoreCase)
                && _controlName.Equals(other._controlName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ItemVersionSetting);
        }

        public override int GetHashCode()
        {
            return _itemVersionId.GetHashCode()
                ^ _propertyName.GetHashCode() * 37
                ^ _controlName.GetHashCode() * 37;
        }
    }
}
