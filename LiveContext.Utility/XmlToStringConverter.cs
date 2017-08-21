using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace LiveContext.Utility
{
    public static class XmlToStringConverter
    {
        public static XmlDocument toXml(String value)
        {
            if (value == null) return null;
            else
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(value);
                return xmlDocument;
            }
        }

        public static String toString(XmlDocument xmlDocument)
        {
            if (xmlDocument == null) return null;
            else
            {
                var writer = new StringWriter();
                xmlDocument.Save(writer);
                return writer.ToString();
            }
        }
    
    }
}
