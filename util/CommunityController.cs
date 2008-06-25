using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml.XPath;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Publish.Portability;
using System.Globalization;

namespace Engage.Dnn.Publish.Util
{

    /// <summary>
    /// Implements new Community functionality
    /// </summary>
    public class CommunityController 
    {

        public CommunityController()
        {

        }

        protected void AddCommunityCreditBlog(Article a)
        {
            try
            {

                CommunityCredit.Components.Blog b = new CommunityCredit.Components.Blog();
                b.Date = Convert.ToDateTime(a.StartDate.ToString(CultureInfo.InvariantCulture));
                b.Url = Utility.GetItemLinkUrl(a.ItemId, a.PortalId, -1, -1, -1, "");
                b.FeedUrl = "";
                CommunityCredit.CommunityCreditService cs = new CommunityCredit.CommunityCreditService();
                cs.AutoSubmitBlog(b);
            }
            catch(Exception exc)
            {
                
            }
        }

    }
}
