using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveContext.Utility
{
    /// <summary>
    /// Exception thrown when reponse from server is invalid xml
    /// 
    /// Contains details of URI request, and server renspose.
    /// </summary>
    public class InvalidXmlException : AssetUnavailableException
    {
        /// <summary>
        /// The serve response from DocumentXmlUri
        /// </summary>
        public string HttpWebResponseXml { get; set; }

        public InvalidXmlException(Exception ex)
            : base (ex)
        {
        }
    }
}
