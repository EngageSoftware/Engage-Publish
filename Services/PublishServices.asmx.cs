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
using System.Web;

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
            //Context key is the PortalId

            DotNetNuke.Security.PortalSecurity objSecurity = new DotNetNuke.Security.PortalSecurity();

            DataTable dt = Tag.GetTagsByString(objSecurity.InputFilter(HttpUtility.UrlDecode(prefixText.ToString()), DotNetNuke.Security.PortalSecurity.FilterFlag.NoSQL), Convert.ToInt32(contextKey, CultureInfo.InvariantCulture));

            string[] returnTags = new string[dt.Rows.Count];
            foreach (DataRow dr in dt.Rows)
            {
                returnTags[0] = dr["name"].ToString();
            }

            return returnTags;
        }
    }

}
