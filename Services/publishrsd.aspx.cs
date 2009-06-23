

namespace Engage.Dnn.Publish.Services
{
    using System;
    using System.Globalization;
    using System.Text;
    using DotNetNuke.Entities.Portals;
    using Util;

    public partial class Publishrsd : System.Web.UI.Page
    {
        public string EngineName;
        public string EngineUrl;
        public string HomePageUrl;
        public string ApiLink;

        public int PortalId
        {
            get
            {
                string i = this.Request.Params["portalId"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }
                return -1;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            
            this.Response.Clear();
            this.Response.ContentType = "text/xml";


            PortalSettings ps = Utility.GetPortalSettings(this.PortalId);

            object o = Request.QueryString["HomePageUrl"];
            if(o!=null)
            {
                EngineName = "EngagePublish";
                EngineUrl = "http://www.engagesoftware.com/modules/engagepublish.aspx";
                ApiLink = "http://" + ps.PortalAlias.HTTPAlias  + ModuleBase.DesktopModuleFolderName + "services/Metaweblog.ashx";
                
                HomePageUrl = o.ToString();
            }
            var responseStream = new StringBuilder(1000);
            responseStream.Append("<?xml version=\"1.0\"  encoding=\"utf-8\" ?><rsd version=\"1.0\" xmlns=\"http://archipelago.phrasewise.com/rsd\">");
            responseStream.Append("<service><engineName>");
            responseStream.Append(EngineName);
            responseStream.Append("</engineName><engineLink>");
            responseStream.Append(EngineUrl);
            responseStream.Append("</engineLink><homePageLink>");
            responseStream.Append(HomePageUrl);
            responseStream.Append("</homePageLink><apis><api name=\"MetaWeblog\" preferred=\"true\" apiLink=\"");
            responseStream.Append(ApiLink);
            responseStream.Append("\" blogID=\"0\" /></apis></service></rsd>");
            Response.Write(responseStream.ToString());
        }
    }
}
