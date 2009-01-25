//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Engage.Dnn.Publish.Portability
{
    public class XmlTransporter
    {
        private XmlDocument _doc;
        private readonly int _moduleId = -1;
        private readonly int _portalId = -1;
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
                    //_version = dr["Version"].ToString();
                }
            }
        }

        #region Construct Methods

        public void BuildRootNode()
        {
            _doc = new XmlDocument();

            const string xsiNS = "http://www.w3.org/2001/XMLSchema-instance";
            XmlElement element = _doc.CreateElement("publish");
            element.SetAttribute("xmlns:xsi", xsiNS);
            element.SetAttribute("noNamespaceSchemaLocation", xsiNS, "Content.Publish.xsd");
            _doc.AppendChild(element);
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
            XmlNode publishNode = _doc.SelectSingleNode("publish");
            XmlNode categoriesNode = _doc.CreateElement("categories");

            DataTable dt = exportAll ? Category.GetCategoriesByPortalId(this._portalId) : Category.GetCategoriesByModuleId(this._moduleId);

            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    int itemVersionId = (int)row["itemVersionId"];
                    int portalId = Convert.ToInt32(row["PortalId"], CultureInfo.InvariantCulture);
                    Category c = Category.GetCategoryVersion(itemVersionId, portalId);

                    string xml = c.SerializeObjectToXml();
                    XmlDocument categoryDoc = new XmlDocument();
                    categoryDoc.LoadXml(xml);

                    //strip off namespace and schema
                    XmlNode node = categoryDoc.SelectSingleNode("category");
                    node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
                    node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

                    categoriesNode.AppendChild(_doc.ImportNode(node, true));
                }

                publishNode.AppendChild(_doc.ImportNode(categoriesNode, true));

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e);
            }

        }

        public void BuildArticles(bool exportAll)
        {
            XmlNode publishNode = _doc.SelectSingleNode("publish");
            XmlNode articlesNode = _doc.CreateElement("articles");

            DataTable dt = exportAll ? Article.GetArticlesByPortalId(this._portalId) : Article.GetArticlesByModuleId(this._moduleId);


            foreach (DataRow row in dt.Rows)
            {
                int itemVersionId = (int)row["itemVersionId"];
                int portalId = Convert.ToInt32(row["PortalId"], CultureInfo.InvariantCulture);

                Article a = Article.GetArticleVersion(itemVersionId, portalId);

                string xml = a.SerializeObjectToXml();
                XmlDocument articleDoc = new XmlDocument();
                articleDoc.LoadXml(xml);

                //strip off namespace and schema
                XmlNode node = articleDoc.SelectSingleNode("article");
                node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
                node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

                articlesNode.AppendChild(_doc.ImportNode(node, true));
            }

            publishNode.AppendChild(_doc.ImportNode(articlesNode, true));
        }

        public void BuildRelationships(bool exportAll)
        {
            XmlNode publishNode = _doc.SelectSingleNode("publish");
            XmlNode relationshipsNode = _doc.CreateElement("relationships");

            List<ItemRelationship> relationships = exportAll ? ItemRelationship.GetAllRelationshipsByPortalId(this._portalId) : ItemRelationship.GetAllRelationships(this._moduleId);

            foreach (ItemRelationship relationship  in relationships)
            {
                relationship.CorrectDates();
                string xml = relationship.SerializeObjectToXml();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                //strip off namespace and schema
                XmlNode node = doc.SelectSingleNode("relationship");
                node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
                node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

                relationshipsNode.AppendChild(_doc.ImportNode(node, true));
            }

            publishNode.AppendChild(_doc.ImportNode(relationshipsNode, true));
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
            XmlNode publishNode = _doc.SelectSingleNode("publish");
            XmlNode settingsNode = _doc.CreateElement("itemversionsettings");

            List<ItemVersionSetting> settings = exportAll ? ItemVersionSetting.GetItemVersionSettingsByPortalId(this._portalId) : ItemVersionSetting.GetItemVersionSettingsByModuleId(this._moduleId, this._portalId);

            foreach (ItemVersionSetting setting in settings)
            {
                string xml = setting.SerializeObjectToXml();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                //strip off namespace and schema
                XmlNode node = doc.SelectSingleNode("itemversionsetting");
                node.Attributes.Remove(node.Attributes["xmlns:xsd"]);
                node.Attributes.Remove(node.Attributes["xmlns:xsi"]);

                settingsNode.AppendChild(_doc.ImportNode(node, true));
            }

            publishNode.AppendChild(_doc.ImportNode(settingsNode, true));
        }
        
        #endregion

        #region Deconstruct Methods

        internal void ImportCategories(IXPathNavigable doc)
        {
            // parse categories
            XPathNavigator navigator = doc.CreateNavigator();
            XPathNavigator categoriesNode = navigator.SelectSingleNode("//publish/categories");

            foreach (XPathNavigator categoryNode in categoriesNode.Select("//category"))
            {
                // Create an instance of the XmlSerializer specifying type.
                XmlSerializer serializer = new XmlSerializer(typeof(Category));
                StringReader reader = new StringReader(categoryNode.OuterXml);

                // Use the Deserialize method to restore the object's state.
                Category c = (Category)serializer.Deserialize(reader);
                c.Import(_moduleId, _portalId);
            }
        }

        internal void ImportArticles(IXPathNavigable doc)
        {
            // parse categories
            XPathNavigator navigator = doc.CreateNavigator();
            XPathNavigator articlesNode = navigator.SelectSingleNode("//publish/articles");
            foreach (XPathNavigator articleNode in articlesNode.Select("//article"))
            {
                // Create an instance of the XmlSerializer specifying type.
                XmlSerializer serializer = new XmlSerializer(typeof(Article));
                StringReader reader = new StringReader(articleNode.OuterXml);

                // Use the Deserialize method to restore the object's state.
                Article a = (Article)serializer.Deserialize(reader);
                a.Import(_moduleId, _portalId);
            }
        }

        internal void ImportRelationships(IXPathNavigable doc)
        {

            // parse relationships
            XPathNavigator navigator = doc.CreateNavigator();
            XPathNavigator relationshipsNode = navigator.SelectSingleNode("//publish/relationships");
            foreach (XPathNavigator relationshipNode in relationshipsNode.Select("//relationship"))
            {
                // Create an instance of the XmlSerializer specifying type.
                XmlSerializer serializer = new XmlSerializer(typeof(ItemRelationship));
                StringReader reader = new StringReader(relationshipNode.OuterXml);

                // Use the Deserialize method to restore the object's state.
                ItemRelationship r = (ItemRelationship) serializer.Deserialize(reader);
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

        internal void ImportItemVersionSettings(IXPathNavigable doc)
        {
            // parse settings
            XPathNavigator navigator = doc.CreateNavigator();
            XPathNavigator settingsNode = navigator.SelectSingleNode("//publish/itemversionsettings");
            foreach (XPathNavigator settingNode in settingsNode.Select("//itemversionsetting"))
            {
                // Create an instance of the XmlSerializer specifying type.
                XmlSerializer serializer = new XmlSerializer(typeof(ItemVersionSetting));
                StringReader reader = new StringReader(settingNode.OuterXml);

                // Use the Deserialize method to restore the object's state.
                ItemVersionSetting s = (ItemVersionSetting) serializer.Deserialize(reader);
                s.Import(_moduleId, _portalId);
            }
        }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode", Justification="Need access to OuterXml property of XmlDocument")]
        public XmlDocument Document
        {
            get { return this._doc; }
        }
    }
}
