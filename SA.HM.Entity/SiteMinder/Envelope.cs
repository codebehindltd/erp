using HotelManagement.Entity.SiteMinder.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HotelManagement.Entity.SiteMinder
{
    [XmlRoot(ElementName = "Envelope")]
    public class Envelope
    {

        [XmlElement(ElementName = "Header")]
        public object Header { get; set; }

        [XmlElement(ElementName = "Body")]
        public Body Body { get; set; }

        [XmlAttribute(AttributeName = "SOAP-ENV")]
        public string SOAPENV { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    public class IgnoreNamespaceXmlTextReader : XmlTextReader
    {
        public IgnoreNamespaceXmlTextReader(TextReader reader) : base(reader)
        {
        }

        public override string NamespaceURI => "";
    }
}
