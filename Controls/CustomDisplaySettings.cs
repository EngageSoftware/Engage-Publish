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
        public const string DateSort = "Date Created";

        public const string LastUpdatedSort = "Last Updated";

        public const string MostPopularSort = "Most Popular";

        public const string StartDateSort = "Start Date Created";

        public const string TitleSort = "Title";

        private readonly IDictionary settings;

        private readonly int tabModuleId = -1;

        public CustomDisplaySettings(IDictionary settings, int tabModuleId)
        {
            this.settings = settings;
            this.tabModuleId = tabModuleId;
        }

        internal bool AllowPaging
        {
            get
            {
                object o = this.settings["AllowPaging"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "AllowPaging", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal int CategoryId
        {
            get
            {
                object o = this.settings["CategoryId"];
                return o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "CategoryId", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal string DateFormat
        {
            get
            {
                string setting = "MM.dd.yy";
                object o = this.settings["DateFormat"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    setting = o.ToString();
                }

                return setting;
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "DateFormat", value);
            }
        }

        internal bool DisplayOptionAbstract
        {
            get
            {
                object o = this.settings["DisplayOptionAbstract"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "DisplayOptionAbstract", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionAuthor
        {
            get
            {
                object o = this.settings["DisplayOptionAuthor"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "DisplayOptionAuthor", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionDate
        {
            get
            {
                object o = this.settings["DisplayOptionDate"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "DisplayOptionDate", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionReadMore
        {
            get
            {
                object o = this.settings["DisplayOptionReadMore"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "DisplayOptionReadMore", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionStats
        {
            get
            {
                object o = this.settings["DisplayOptionStats"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "DisplayOptionStats", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionThumbnail
        {
            get
            {
                object o = this.settings["DisplayOptionThumbnail"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "DisplayOptionThumbnail", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool DisplayOptionTitle
        {
            get
            {
                object o = this.settings["DisplayOptionTitle"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "DisplayOptionTitle", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool EnableRss
        {
            get
            {
                object o = this.settings["EnableRss"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "EnableRss", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        // chkRelatedItem

        internal bool GetParentFromQueryString
        {
            get
            {
                object o = this.settings["GetParentFromQueryString"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "GetParentFromQueryString", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool GetRelatedChildren
        {
            get
            {
                object o = this.settings["GetRelatedChildren"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "GetRelatedChildren", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal int ItemTypeId
        {
            get
            {
                object o = this.settings["ItemTypeId"];
                return o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "ItemTypeId", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal int MaxDisplayItems
        {
            get
            {
                object o = this.settings["MaxDisplayItems"];
                return o == null ? 10 : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "MaxDisplayItems", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool ShowParent
        {
            get
            {
                object o = this.settings["ShowParent"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "ShowParent", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal bool ShowParentDescription
        {
            get
            {
                object o = this.settings["ShowParentDescription"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "ShowParentDescription", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal string SortDirection
        {
            get
            {
                string setting = "1"; // by default sort descending
                object o = this.settings["SortDirection"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    setting = o.ToString();
                }

                return setting;
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "SortDirection", value);
            }
        }

        internal string SortOption
        {
            get
            {
                string setting = StartDateSort;
                object o = this.settings["SortOption"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    setting = o.ToString();
                }

                return setting;
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "SortOption", value);
            }
        }

        internal bool UseCustomSort
        {
            get
            {
                object o = this.settings["UseCustomSort"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.tabModuleId, "UseCustomSort", value.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}