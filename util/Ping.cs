//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;

    using DotNetNuke.Entities.Host;
    using DotNetNuke.Services.Exceptions;

    public static class Ping
    {
        public static void SendPing(string name, string url, string changesUrl, int portalId)
        {
            try
            {
                var listToPing = new ArrayList();
                string s = HostSettings.GetHostSetting(Utility.PublishPingServers + portalId.ToString(CultureInfo.InvariantCulture));
                if (Engage.Utility.HasValue(s))
                {
                    foreach (string sr in s.Split('\n'))
                    {
                        if (Engage.Utility.HasValue(sr))
                        {
                            listToPing.Add(sr.Replace("\r", string.Empty));
                        }
                    }
                }
                else
                {
                    listToPing.Add("http://rpc.technorati.com/rpc/ping");
                    listToPing.Add("http://rpc.pingomatic.com");
                    listToPing.Add("http://blogsearch.google.com/ping/RPC2");
                }

                for (int i = 0; i < listToPing.Count; i++)
                {
                    var request = (HttpWebRequest)WebRequest.Create(listToPing[i].ToString());
                    request.Method = "POST";
                    request.ContentType = "text/xml";

                    Stream stream = request.GetRequestStream();

                    using (var xml = new XmlTextWriter(stream, Encoding.UTF8))
                    {
                        xml.WriteStartDocument();
                        xml.WriteStartElement("methodCall");
                        if (request.ServicePoint.Address.ToString() == "http://blogsearch.google.com/ping/RPC2")
                        {
                            xml.WriteElementString("methodName", "weblogUpdates.extendedPing");
                        }
                        else
                        {
                            xml.WriteElementString("methodName", "weblogUpdates.ping");
                        }

                        xml.WriteStartElement("params");
                        xml.WriteStartElement("param");
                        xml.WriteElementString("value", name);
                        xml.WriteEndElement();

                        // changed page
                        xml.WriteStartElement("param");
                        xml.WriteElementString("value", changesUrl);
                        xml.WriteEndElement();

                        xml.WriteStartElement("param");
                        xml.WriteElementString("value", url);
                        xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.Close();
                    }

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var sr = new StreamReader(response.GetResponseStream()))
                        {
                            // string result = sr.ReadToEnd();
                            sr.ReadToEnd();
                            sr.Close();
                        }

                        response.Close();
                    }
                }
            }
            catch (Exception exc)
            {
                // do what with the exception?
                Exceptions.LogException(exc);
            }
        }
    }
}