//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Data;
using System.Globalization;
using DotNetNuke.Common.Utilities;
using Localize = DotNetNuke.Services.Localization.Localization;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;
using System.Xml.Serialization;

using DotNetNuke.Entities.Host;
using System.Collections;
using DotNetNuke.Entities.Portals;


namespace Engage.Dnn.Publish
{
	/// <summary>
	/// Summary description for ArticleInfo.
	/// ArticleInfo holds all of the article specific information
	/// </summary>
    [XmlRootAttribute(ElementName = "article", IsNullable = false)]
    public class Article : Item
	{
		private string articleText = "";
		private string versionNumber = "";
		private string versionDescription = "";
		private string referenceNumber = "";
        private float averageRating;

        private readonly string[] pageSeperator = new string[] { "[PAGE]" };

        public Article()
		{
			//this type is always a Category
			ItemTypeId = ItemType.Article.GetId();
		}

		#region Item method implementation

        public override void Save(int authorId)
		{
            IDbConnection newConnection = DataProvider.GetConnection();
			IDbTransaction trans = newConnection.BeginTransaction();

			try
			{
				base.SaveInfo(trans, authorId);
				base.UpdateApprovalStatus(trans);

				//update category version now
				AddArticleVersion(trans, base.ItemVersionId, base.ItemId, this.VersionNumber, this.VersionDescription, this.ArticleText, this.ReferenceNumber);

                //Save the ItemVersionSettings
                SaveItemVersionSettings(trans);

                //Save the Relationships
				SaveRelationships(trans);
				//do all category save
                string s = DotNetNuke.Entities.Host.HostSettings.GetHostSetting(Utility.PublishEnableTags + PortalId.ToString(CultureInfo.InvariantCulture));
                if (Utility.HasValue(s))
                {
                    if (Convert.ToBoolean(s, CultureInfo.InvariantCulture))
                    {
                        //Save Tags
                        SaveTags(trans);
                    }
                }
				trans.Commit();

                if (Utility.IsPingEnabledForPortal(this.PortalId))
                {
                    if (this.ApprovalStatusId == ApprovalStatus.Approved.GetId())
                    {
                        string surl = HostSettings.GetHostSetting(Utility.PublishPingChangedUrl + PortalId.ToString(CultureInfo.InvariantCulture));
                        string changedUrl = Utility.HasValue(surl) ? s.ToString() : DotNetNuke.Common.Globals.NavigateURL(this.DisplayTabId);
                        Hashtable ht = PortalSettings.GetSiteSettings(PortalId);

                        //ping
                        Ping.SendPing(ht["PortalName"].ToString(), ht["PortalAlias"].ToString(), changedUrl, PortalId);
                    }
                }

			}
			catch
			{
				trans.Rollback();
	            //Rolling back to the original version, exception thrown.
				base.ItemVersionId = base.OriginalItemVersionId;
				throw;
			}
			finally
			{
				//clean up connection stuff
                newConnection.Close();
			}

		}



        public override void UpdateApprovalStatus()
        {
            IDbConnection newConnection = DataProvider.GetConnection();
            IDbTransaction trans = newConnection.BeginTransaction();
            try
            {
                base.UpdateApprovalStatus(trans);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                //clean up connection stuff
            }
        }

        public override string EmailApprovalBody
        {
            get 
            {
                string body = Localize.GetString("EMAIL_APPROVAL_BODY", "~" + Utility.DesktopModuleFolderName + "articlecontrols/App_LocalResources/articleedit");
                return body;
            }
        }

        public override string EmailApprovalSubject
        {
            get 
            {
                string subject = Localize.GetString("EMAIL_APPROVAL_SUBJECT", "~" + Utility.DesktopModuleFolderName + "articlecontrols/App_LocalResources/articleedit");
                return subject;
            }
        }

        public override string EmailStatusChangeBody
        {
            get 
            {
                string body = Localize.GetString("EMAIL_STATUSCHANGE_BODY", "~" + Utility.DesktopModuleFolderName + "articlecontrols/App_LocalResources/articleedit");
                return body;
            }
        }

        public override string EmailStatusChangeSubject
        {
            get
            {
                string body = Localize.GetString("EMAIL_STATUSCHANGE_SUBJECT", "~" + Utility.DesktopModuleFolderName + "articlecontrols/App_LocalResources/articleedit");
                return body;
            }
        }



		#endregion

        [XmlElement(Order = 30)]
		public string ArticleText 
		{
			get {return this.articleText;}
			set {this.articleText = value;}
		}

        [XmlElement(Order = 31)]
		public string VersionNumber 
		{
			get {return this.versionNumber;}
			set {this.versionNumber = value;}
		}

        [XmlElement(Order = 32)]
		public string VersionDescription 
		{
			get {return this.versionDescription;}
			set {this.versionDescription = value;}
		}

        [XmlElement(Order = 33)]
		public string ReferenceNumber 
		{
			get {return this.referenceNumber;}
			set {this.referenceNumber = value;}
		}

                /// <summary>
        /// Gets the average rating of this article.
        /// </summary>
        /// <value>The average rating for this article.</value>
        [XmlIgnore]
        public float AverageRating
        {
            get
            {
                return averageRating;
            }
             set
            {
                //if value is NULL in database, CBO fills it with MinValue
                if (value != float.MinValue)
                {
                    averageRating = value;
                }
                else
                {
                    averageRating = 0;
                }
            }
        }

       
        public void AddRating(int rating, int? userId)
        {
            UserFeedback.Rating.AddRating(this.ItemVersionId, userId, rating, DataProvider.ModuleQualifier);
        }

        public string GetPage(int pageId)
        {
            string[] pageLocations = this.articleText.Split(pageSeperator, StringSplitOptions.None);

            if (pageLocations.Length > pageId)
            {
                return pageLocations[pageId - 1];
            }
            else return pageLocations[pageLocations.Length - 1];
        }


        public int GetNumberOfPages
        {
            get
            {
                //string[] stringSeparators = new string[] { "[PAGE]" };

                string[] pagelocations = this.articleText.Split(pageSeperator, StringSplitOptions.None);
                return pagelocations.Length;
            }
        }

		public static Article Create(int portalId)
		{
			Article a = new Article();
			a.PortalId = portalId;
			return a;
		}

		public static Article GetArticleVersion(int articleVersionId, int portalId)
		{
            Article a = (Article)CBO.FillObject(DataProvider.Instance().GetArticleVersion(articleVersionId, portalId), typeof(Article));
            if (a != null)
            {
                a.CorrectDates();
            }
            return a;
		}

		public static void AddArticleVersion(int itemVersionId, int itemId, string  versionNumber, string versionDescription, string articleText, string referenceNumber)
		{DataProvider.Instance().AddArticleVersion(itemVersionId, itemId, versionNumber, versionDescription, articleText, referenceNumber);}

		public static void AddArticleVersion(IDbTransaction trans, int itemVersionId, int itemId, string versionNumber, string versionDescription, string articleText, string referenceNumber)
		{DataProvider.Instance().AddArticleVersion(trans, itemVersionId, itemId, versionNumber, versionDescription, articleText, referenceNumber);}

		public static DataTable GetArticles(int portalId)
		{
			return DataProvider.Instance().GetArticles(portalId);
		}

        public static DataTable GetArticlesByPortalId(int portalId)
        {
            return DataProvider.Instance().GetArticlesByPortalId(portalId);
        }

        public static DataTable GetArticlesByModuleId(int moduleId)
        {
            return DataProvider.Instance().GetArticlesByModuleId(moduleId);
        }

        public static DataTable GetArticlesSearchIndexingUpdated(int portalId, int moduleDefId, int displayTabId)
		{
            return DataProvider.Instance().GetArticlesSearchIndexingUpdated(portalId, moduleDefId, displayTabId);
		}

        public static DataTable GetArticlesSearchIndexingNew(int portalId, int displayTabId)
		{
            return DataProvider.Instance().GetArticlesSearchIndexingNew(portalId, displayTabId);
		}
        
        
		public static DataTable GetArticles(int parentItemId, int portalId)
		{
			return DataProvider.Instance().GetArticles(parentItemId, portalId);
		}

        public static Article GetArticle(int itemId)
        {
            IDataReader dr = DataProvider.Instance().GetArticle(itemId);
            Article a = (Article)CBO.FillObject(dr, typeof(Article));
            if (a != null)
            {
                a.CorrectDates();
            }
            return a;
        }

		public static Article GetArticle(int itemId, int portalId)
		{
			IDataReader dr = DataProvider.Instance().GetArticle(itemId, portalId);
			Article a = (Article) CBO.FillObject(dr, typeof(Article));
            if (a != null)
            {
                a.CorrectDates();
            }
            return a;
		}

        public static int GetOldArticleId(int itemId)
        {
            return DataProvider.Instance().GetOldArticleId(itemId);
        }

        public bool DisplayReturnToList()
        {
            ItemVersionSetting rlSetting = ItemVersionSetting.GetItemVersionSetting(this.ItemVersionId, "ArticleSettings", "DisplayReturnToList");
            if (rlSetting != null)
            {
                return Convert.ToBoolean(rlSetting.PropertyValue, CultureInfo.InvariantCulture);
            }
            return false;
        }




        #region TransportableElement Methods

        /// <summary>
        /// This method is invoked by the Import mechanism and has to take this instance of a Article and resolve
        /// all the id's using the names supplied in the export. hk
        /// </summary>
        public override void Import(int currentModuleId, int portalId)
        {
            //The very first thing is that PortalID needs to be changed to the current portal where content is being
            //imported. Several methods resolving Id's is expecting the correct PortalId (current). hk
            PortalId = portalId;

            ResolveIds(currentModuleId);
        }

        protected override void ResolveIds(int currentModuleId)
        {
            base.ResolveIds(currentModuleId);

            //now the Unique Id's
            //Does this ItemVersion exist in my db?
            using (IDataReader dr = DataProvider.Instance().GetItemVersion(ItemVersionIdentifier, PortalId))
            {
                if (dr.Read())
                {
                    //this item already exists
                    //update some stuff???
                }
                else
                {   
                    //this version does not exist.
                    ItemId = -1;
                    ItemVersionId = -1;
                    ModuleId = currentModuleId;
                    Save(this.AuthorUserId);
                }
            }
        }
     
        #endregion
    }
}

