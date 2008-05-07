using System;
using System.Data;
using System.Globalization;
using System.Web.Services;
using System.Web.Script.Services;
using CookComputing.XmlRpc;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Host;
using Engage.Dnn.Publish.Util;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Common;
using System.Collections;

namespace Engage.Dnn.Publish.Services
{
    /// <summary>
    /// Summary description for PublishServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class PublishServices : WebService
    {
        [WebMethod][ScriptMethod]
        public string[] GetTagsCompletionList(string prefixText, int count, string contextKey)
        {
            //TODO: what are we doing for PortalID here?
            DataTable dt = Tag.GetTagsByString(prefixText, Convert.ToInt32(contextKey, CultureInfo.InvariantCulture));

            string[] returnTags = new string[dt.Rows.Count];
            foreach (DataRow dr in dt.Rows)
            {
                returnTags[0] = dr["name"].ToString();
            }

            return returnTags;
        }
    }

}
