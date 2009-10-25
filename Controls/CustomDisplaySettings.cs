

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Collections;
    using System.Globalization;
    using DotNetNuke.Entities.Modules;

    /// <summary>
    /// This class is used as a representation for the Settings of the Custom Display control. It contains strongly 
    /// typed names for each of the required settings.  The reason it exists
    /// is so that it can be used in two contexts. The CustomDisplayOption control and CustomDisplay control. This
    /// centralizes the settings properties into one place to avoid duplicate code and errors. HK
    /// </summary>
    public class CustomDisplaySettings
    {
        public const string TitleSort = "Title";
        public const string DateSort = "Date Created";
        public const string LastUpdatedSort = "Last Updated";
        public const string StartDateSort = "Start Date Created";
        public const string MostPopularSort = "Most Popular";

        readonly IDictionary settings;
        readonly int tabModuleId = -1;

        public CustomDisplaySettings(IDictionary settings, int tabModuleId)
        {
            this.settings = settings;
            this.tabModuleId = tabModuleId;
        }

        #region Settings

        internal int ItemTypeId
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "ItemTypeId", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["ItemTypeId"];
                return (o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture));
            }
        }

        internal int CategoryId
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "CategoryId", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["CategoryId"];
                return (o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture));
            }
        }


        internal bool ShowParent
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "ShowParent", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["ShowParent"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        internal bool ShowParentDescription
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "ShowParentDescription", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["ShowParentDescription"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }



        internal bool AllowPaging
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "AllowPaging", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["AllowPaging"];
                return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        internal bool UseCustomSort
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "UseCustomSort", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["UseCustomSort"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        //chkRelatedItem

        internal bool GetParentFromQueryString
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "GetParentFromQueryString", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["GetParentFromQueryString"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        internal bool GetRelatedChildren
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "GetRelatedChildren", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["GetRelatedChildren"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }


        internal bool EnableRss
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "EnableRss", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["EnableRss"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        
        }
        
        internal int MaxDisplayItems
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "MaxDisplayItems", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["MaxDisplayItems"];
                return (o == null ? 10 : Convert.ToInt32(o, CultureInfo.InvariantCulture));
            }
        }

        internal string SortOption
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "SortOption", value);
            }
            get
            {
                string setting = StartDateSort;
                object o = settings["SortOption"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    setting = o.ToString();
                }

                return setting;
            }
        }

        internal string SortDirection
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "SortDirection", value);
            }
            get
            {
                string setting = "1";//by default sort descending
                object o = settings["SortDirection"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    setting = o.ToString();
                }

                return setting;
            }
        }

        internal bool DisplayOptionTitle
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "DisplayOptionTitle", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["DisplayOptionTitle"];
                return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionAbstract
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "DisplayOptionAbstract", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["DisplayOptionAbstract"];
                return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionThumbnail
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "DisplayOptionThumbnail", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["DisplayOptionThumbnail"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionDate
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "DisplayOptionDate", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["DisplayOptionDate"];
                return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionStats
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "DisplayOptionStats", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["DisplayOptionStats"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionReadMore
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "DisplayOptionReadMore", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["DisplayOptionReadMore"];
                return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionAuthor
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "DisplayOptionAuthor", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = settings["DisplayOptionAuthor"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }


        internal string DateFormat
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(tabModuleId, "DateFormat", value);
            }

            get
            {
                string setting = "MM.dd.yy";
                object o = settings["DateFormat"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    setting = o.ToString();
                }

                return setting;
            }
        }

        #endregion

    }
}
