using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveContext.Utility
{
    /// <summary>
    /// Exception thrown when an asset (e.g.: Media Library item) is not found on server. 
    /// </summary>
    public class AssetUnavailableException : Exception
    {
        /// <summary>
        /// The asset's URI
        /// </summary>
        public string DocumentXmlUri { get; set; }

        public AssetUnavailableException(Exception e)
            : base(e.Message, e)
        {
        }
    }
}
