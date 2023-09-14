using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HotelManagement.Entity.SiteMinder
{
    public class SiteMinderHeader
    {
        public XmlDocument XmlHeader(out XmlNode xmlOTATagNode, string hotelOTA_TagName)
        {
            var soapNs = @"http://schemas.xmlsoap.org/soap/envelope/";
            var wsseNs = @"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            var pingNs = @"http://www.opentravel.org/OTA/2003/05";

            XmlDocument soapXMLDoc = new XmlDocument();

            XmlDeclaration _xmlDeclaration = soapXMLDoc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);

            XmlElement rootXmlElement = soapXMLDoc.DocumentElement;
            soapXMLDoc.InsertBefore(_xmlDeclaration, rootXmlElement);

            XmlNode root = soapXMLDoc.AppendChild(soapXMLDoc.CreateElement("SOAP-ENV", "Envelope", soapNs));
            XmlNode header = root.AppendChild(soapXMLDoc.CreateElement("SOAP-ENV", "Header", soapNs));
            XmlNode wsse_security = header.AppendChild(soapXMLDoc.CreateElement("wsse", "Security", wsseNs));
            XmlNode wsse_username_token = wsse_security.AppendChild(soapXMLDoc.CreateElement("wsse", "UsernameToken", wsseNs));
            wsse_username_token.AppendChild(soapXMLDoc.CreateElement("wsse", "Username", wsseNs)).InnerText = "TechnoSensePMS";
            wsse_username_token.AppendChild(soapXMLDoc.CreateElement("wsse", "Password", wsseNs)).InnerText = "3ziJO7HgYxXlHjBQBGCr";

            XmlNode body = root.AppendChild(soapXMLDoc.CreateElement("SOAP-ENV", "Body", soapNs));

            XmlNode HotelAvailNotifRQ = body.AppendChild(soapXMLDoc.CreateElement("", hotelOTA_TagName, ""));
            HotelAvailNotifRQ.Attributes.Append(soapXMLDoc.CreateAttribute("xmlns")).Value = pingNs;
            HotelAvailNotifRQ.Attributes.Append(soapXMLDoc.CreateAttribute("EchoToken")).Value = "TjOx5O7xhUWAoirQuhjsIg==";
            HotelAvailNotifRQ.Attributes.Append(soapXMLDoc.CreateAttribute("TimeStamp")).Value = "2005-08-01T09:30:47+08:00";
            HotelAvailNotifRQ.Attributes.Append(soapXMLDoc.CreateAttribute("Version")).Value = "1";

            XmlNode POS = HotelAvailNotifRQ.AppendChild(soapXMLDoc.CreateElement("", "POS", ""));
            XmlNode Source = POS.AppendChild(soapXMLDoc.CreateElement("", "Source", ""));
            XmlNode RequestorID = Source.AppendChild(soapXMLDoc.CreateElement("", "RequestorID", ""));
            RequestorID.Attributes.Append(soapXMLDoc.CreateAttribute("Type")).Value = "22";
            RequestorID.Attributes.Append(soapXMLDoc.CreateAttribute("ID")).Value = "TECHNOSENSEPMS";

            xmlOTATagNode = HotelAvailNotifRQ;

            return soapXMLDoc;
        }
        public Envelope ParseXmlResponse(string xmlResponse)
        {
            Envelope envelope = null;
            XmlSerializer serializer = new XmlSerializer(typeof(Envelope));

            using (StringReader reader = new StringReader(xmlResponse))
            {
                envelope = (Envelope)serializer.Deserialize(new IgnoreNamespaceXmlTextReader(reader));
            }

            return envelope;
        }

        public string postXMLData(string requestXML)
        {
            string destinationURL = "https://tpi-pmsx.preprod.siteminderlabs.com/webservices/TECHNOSENSEPMS";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);



            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.DefaultConnectionLimit = 9999;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationURL);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXML);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";

            Stream reqeustStream = request.GetRequestStream();
            reqeustStream.Write(bytes, 0, bytes.Length);
            reqeustStream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            if (httpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = httpWebResponse.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }
    }
}
