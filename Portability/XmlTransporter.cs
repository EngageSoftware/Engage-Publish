//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


namespace Engage.Dnn.Publish.Portability
{

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Xml.XPath;
    using DotNetNuke.Entities.Modules;
    using Util;
    public class XmlTransporter
    {
        private XmlDocument doc;
        private readonly int _moduleId = -1;
        private readonly int _portalId = -1;
        private readonly int _tabId = -1;
        //private string _version = string.Empty;

        public XmlTransporter(int moduleId)
        {
            this._moduleId = moduleId;

            using (IDataReader dr = Data.DataProvider.Instance().GetModuleInfo(moduleId))
            {
                if (dr.Read())
                {
                    //This might/should be CurrentPortalId????
                    _portalId = (int)dr["PortalID"];
                    _tabId = (int)dr["TabID"];
                    //_version = dr["Version"].ToString();
                }
            }
        }

        #region Construct Methods

        public void BuildRootNode()
        {
            this.doc = new XmlDocument();

            const string xsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
            XmlElement element = this.doc.CreateElement("publish");
            element.SetAttribute("xmlns:xsi", xsiNamespace);
            element.SetAttribute("noNamespaceSchemaLocation", xsiNamespace, "Content.Publish.xsd");
            this.doc.AppendChild(element);
        }

        [Obsolete("Not implemented")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not implemented")]
        public void BuildItemTypes()
        {
            throw new NotImplementedException();
        }

        [Obsolete("Not implemented")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not implemented")]
        public void BuildRelationshipTypes()
        {
            throw new NotImplementedException();
        }

        [Obsolete("Not implemented")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not implemented")]
        public void BuildApprovalStatusTypes()
        {
            throw new NotImplementedException();
        }

        public void BuildCategories(bool exportAll)
        {
            XmlNode publishNode = this.doc.SelectSingleNode("publish");
            XmlNode categoriesNode = this.doc.CreateElement("categories");
            //TODO: if we're exporting Text/HTML we at least need to pull the text/html defined category
            var mc = new ModuleController();
            ModuleInfo mi = mc.GetModule(this._moduleId, this._tabId);

            DataTable dt;

            if (mi.FriendlyName == Utility.DnnFriendlyModuleNameTextHTML)
            {
                //if we're dealing with the text/html module we need to get all categories always
                //TODO: in the future configure it so we can only get one category.
                dt = Category.GetCategoriesByPortalId(this._portalId);
            }
            else
            {
                dt = exportAll ? Category.GetCategoriesByPortalId(this._portalId) : Category.GetCategoriesByModuleId(this._moduleId);
            }
            
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    var itemVersionId = (int)row["itemVersionId"];
                    int portalId = Convert.ToInt32(row["PortalId"], CultureInfo.InvariantCulture);
                    Category c = Category.GetCategoryVersion(itemVersionId, portalId);

                    string xml = c.SerializeObjectToXml();
                    var categoryDoc = new XmlDocument();
                    categoryDoc.LoadXml(xml);

                    //strip off namespace and schema
                    XmlNode node = categoryDoc.SelectSingleNode("category");
                    node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
                    node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

                    categoriesNode.AppendChild(this.doc.ImportNode(node, true));
                }

                publishNode.AppendChild(this.doc.ImportNode(categoriesNode, true));

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e);
            }

        }

        public void BuildArticles(bool exportAll)
        {
            XmlNode publishNode = this.doc.SelectSingleNode("publish");
            XmlNode articlesNode = this.doc.CreateElement("articles");

            DataTable dt = exportAll ? Article.GetArticlesByPortalId(this._portalId) : Article.GetArticlesByModuleId(this._moduleId);


            foreach (DataRow row in dt.Rows)
            {
                var itemVersionId = (int)row["itemVersionId"];
                int portalId = Convert.ToInt32(row["PortalId"], CultureInfo.InvariantCulture);

                Article a = Article.GetArticleVersion(itemVersionId, portalId);

                string xml = a.SerializeObjectToXml();
                var articleDoc = new XmlDocument();
                articleDoc.LoadXml(xml);

                //strip off namespace and schema
                XmlNode node = articleDoc.SelectSingleNode("article");
                node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
                node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

                articlesNode.AppendChild(this.doc.ImportNode(node, true));
            }

            publishNode.AppendChild(this.doc.ImportNode(articlesNode, true));
        }

        public void BuildRelationships(bool exportAll)
        {
            XmlNode publishNode = this.doc.SelectSingleNode("publish");
            XmlNode relationshipsNode = this.doc.CreateElement("relationships");

            List<ItemRelationship> relationships = exportAll ? ItemRelationship.GetAllRelationshipsByPortalId(this._portalId) : ItemRelationship.GetAllRelationships(this._moduleId);

            foreach (ItemRelationship relationship  in relationships)
            {
                relationship.CorrectDates();
                string xml = relationship.SerializeObjectToXml();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                //strip off namespace and schema
                XmlNode node = xmlDoc.SelectSingleNode("relationship");
                node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
                node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

                relationshipsNode.AppendChild(this.doc.ImportNode(node, true));
            }

            publishNode.AppendChild(this.doc.ImportNode(relationshipsNode, true));
        }

        [Obsolete("Not implemented")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not implemented")]
        public void BuildRatings()
        {
            throw new NotImplementedException();
        }

        [Obsolete("Not implemented")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification="Not implemented")]
        public void BuildTags()
        {
            throw new NotImplementedException();
        }

        [Obsolete("Not implemented")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not implemented")]
        public void BuildComments()
        {
            throw new NotImplementedException();
        }

        public void BuildItemVersionSettings(bool exportAll)
        {
            XmlNode publishNode = this.doc.SelectSingleNode("publish");
            XmlNode settingsNode = this.doc.CreateElement("itemversionsettings");

            List<ItemVersionSetting> settings = exportAll ? ItemVersionSetting.GetItemVersionSettingsByPortalId(this._portalId) : ItemVersionSetting.GetItemVersionSettingsByModuleId(this._moduleId, this._portalId);

            foreach (ItemVersionSetting setting in settings)
            {
                string xml = setting.SerializeObjectToXml();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                //strip off namespace and schema
                XmlNode node = xmlDoc.SelectSingleNode("itemversionsetting");
                node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
                node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

                settingsNode.AppendChild(this.doc.ImportNode(node, true));
            }

            publishNode.AppendChild(this.doc.ImportNode(settingsNode, true));
        }

        public void BuildModuleSettings()
        {
            XmlNode publishNode = this.doc.SelectSingleNode("publish");
            XmlNode settingsNode = this.doc.CreateElement("modulesettings");

            //TODO: get a list of module settings, parse through the list and generate XMl
            var mc = new ModuleController();
            //we aren't using modulesettings in Publish, only tabmodulesettings
            //System.Collections.Hashtable moduleSettings = mc.GetModuleSettings(_moduleId);
            ModuleInfo mi = mc.GetModule(_moduleId, _tabId);
            System.Collections.Hashtable tabModuleSettings = mc.GetTabModuleSettings(mi.TabModuleID);

            foreach (string key in tabModuleSettings.Keys)
            {
                XmlNode settingNode = this.doc.CreateElement("tabmodulesetting");
                settingNode.Attributes.Remove(settingNode.Attributes["xmlns:xsd"]);
                settingNode.Attributes.Remove(settingNode.Attributes["xmlns:xsi"]);

                settingsNode.AppendChild(settingNode);

                XmlNode keyNode = this.doc.CreateElement("Key");
                keyNode.AppendChild(this.doc.CreateTextNode(key));
                settingNode.AppendChild(keyNode);
                XmlNode valueNode = this.doc.CreateElement("Value");
                valueNode.AppendChild(this.doc.CreateTextNode(tabModuleSettings[key].ToString()));
                settingNode.AppendChild(valueNode);
                settingsNode.AppendChild(this.doc.ImportNode(settingNode, true));                                
            }
            publishNode.AppendChild(this.doc.ImportNode(settingsNode, true));
        }

        
        #endregion

        #region Deconstruct Methods

        internal void ImportCategories(IXPathNavigable xmlDoc)
        {
            // parse categories
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XPathNavigator categoriesNode = navigator.SelectSingleNode("//publish/categories");

            foreach (XPathNavigator categoryNode in categoriesNode.Select("//category"))
            {
                // Create an instance of the XmlSerializer specifying type.
                var serializer = new XmlSerializer(typeof(Category));
                var reader = new StringReader(categoryNode.OuterXml);

                // Use the Deserialize method to restore the object's state.
                var c = (Category)serializer.Deserialize(reader);
                c.Import(_moduleId, _portalId);
            }
        }

        internal void ImportArticles(IXPathNavigable xmlDoc)
        {
            // parse categories
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XPathNavigator articlesNode = navigator.SelectSingleNode("//publish/articles");
            foreach (XPathNavigator articleNode in articlesNode.Select("//article"))
            {
                // Create an instance of the XmlSerializer specifying type.
                var serializer = new XmlSerializer(typeof(Article));
                var reader = new StringReader(articleNode.OuterXml);

                // Use the Deserialize method to restore the object's state.
                var a = (Article)serializer.Deserialize(reader);
                a.Import(_moduleId, _portalId);
            }
        }

        internal void ImportRelationships(IXPathNavigable xmlDoc)
        {

            // parse relationships
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XPathNavigator relationshipsNode = navigator.SelectSingleNode("//publish/relationships");
            foreach (XPathNavigator relationshipNode in relationshipsNode.Select("//relationship"))
            {
                // Create an instance of the XmlSerializer specifying type.
                var serializer = new XmlSerializer(typeof(ItemRelationship));
                var reader = new StringReader(relationshipNode.OuterXml);

                // Use the Deserialize method to restore the object's state.
                var r = (ItemRelationship) serializer.Deserialize(reader);
                r.Import(_moduleId, _portalId);
            }
        }

        ///// <summary>
        ///// This method verifies that all the categories that are imported have a top level category relationship
        ///// for situations where the user is importing content from another system (file not generated from Publish)
        ///// they have no way of knowing what the top level category GUIDS are nor to include the entries in the 
        ///// relationships section of the file.
        ///// </summary>
        ///// <param name="doc"></param>
        //internal void VerifyTopLevelCategoryRelationships(XPathDocument doc)
        //{
        //    // parse categories
        //    XPathNavigator navigator = doc.CreateNavigator();
        //    XPathNavigator categoriesNode = navigator.SelectSingleNode("//publish/categories");

        //    foreach (XPathNavigator categoryNode in categoriesNode.Select("//category"))
        //    {
        //        // Create an instance of the XmlSerializer specifying type.
        //        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Category));
        //        System.IO.StringReader reader = new System.IO.StringReader(categoryNode.OuterXml);

        //        // Use the Deserialize method to restore the object's state.
        //        Category c = (Category)serializer.Deserialize(reader);
        //        int authorId = c.AuthorUserId;
        //        c.CreateTopLevelCategory(authorId);
        //    }
        //}

        internal void ImportItemVersionSettings(IXPathNavigable xmlDoc)
        {
            // parse settings
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XPathNavigator settingsNode = navigator.SelectSingleNode("//publish/itemversionsettings");
            foreach (XPathNavigator settingNode in settingsNode.Select("//itemversionsetting"))
            {
                // Create an instance of the XmlSerializer specifying type.
                var serializer = new XmlSerializer(typeof(ItemVersionSetting));
                var reader = new StringReader(settingNode.OuterXml);

                // Use the Deserialize method to restore the object's state.
                var s = (ItemVersionSetting) serializer.Deserialize(reader);
                s.Import(_moduleId, _portalId);
            }
        }

        internal void ImportModuleSettings(IXPathNavigable xmlDoc)
        {
            // parse settings
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XPathNavigator settingsNode = navigator.SelectSingleNode("//publish/modulesettings");
            foreach (XPathNavigator settingNode in settingsNode.Select("//tabmodulesetting"))
            {
                var tempDoc = new XmlDocument();
                tempDoc.LoadXml(settingNode.OuterXml);


                XmlNode keyNode = tempDoc.SelectSingleNode("//tabmodulesetting/Key");

                XmlNode valueNode = tempDoc.SelectSingleNode("//tabmodulesetting/Value");
                var mc = new ModuleController();
                ModuleInfo mi = mc.GetModule(_moduleId, _tabId);
                //update the module setting.
                mc.UpdateTabModuleSetting(mi.TabModuleID, keyNode.InnerText, valueNode.InnerText);

            }
        }
        

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode", Justification="Need access to OuterXml property of XmlDocument")]
        public XmlDocument Document
        {
            get { return this.doc; }
        }
    }
}
