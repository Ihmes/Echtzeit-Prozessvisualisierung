using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace LiveContext.Utility
{
    public class HttpUtilities
    {

        public static byte[] DownloadFile(string fileUri, string userName, string passWord)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(fileUri);

            // Define the request access method.
            req.Method = "GET";

            try
            {
                // Define the request credentials according to the user's input.
                req.Credentials = new NetworkCredential(userName, passWord);

                // Issue the request.
                HttpWebResponse result = (HttpWebResponse)req.GetResponse();


                // Store the response.
                Stream sData = result.GetResponseStream();

                int chunkSize = 256 * 1024;
                byte[] buffer = new byte[chunkSize];

                var temp = Path.GetTempFileName();

                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        while (true)
                        {
                            int read = sData.Read(buffer, 0, buffer.Length);
                            if (read <= 0)
                                break;
                            ms.Write(buffer, 0, read);
                        }
                    }
                    catch
                    {
                    }

                    ms.Seek(0, SeekOrigin.Begin);
                    return ms.GetBuffer();

                }
            }
            catch (WebException e)
            {
                // Display any errors. In particular, display any protocol-related error. 
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse hresp = (HttpWebResponse)e.Response;
                    Console.WriteLine("\nAuthentication Failed, " + hresp.StatusCode);
                    Console.WriteLine("Status Code: " + (int)hresp.StatusCode);
                    Console.WriteLine("Status Description: " + hresp.StatusDescription);
                }
                Console.WriteLine("Caught Exception: " + e.Message);
                Console.WriteLine("Stack: " + e.StackTrace);

                throw e;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentXmlUri"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <exception cref="LiveContext.Utility.InvalidXmlException">Thrown if response is not valid xml</exception>
        /// <exception cref="LiveContext.Utility.AssetUnavailableException">Thrown if asset was not found on server</exception>
        /// <returns></returns>
        public static XElement DownloadDocumentInformation(string documentXmlUri, string userName, string passWord)
        {
            XElement xe = null;

            // Create the Web request object.
            try
            {
                byte[] data = DownloadFile(documentXmlUri, userName, passWord);

                string xmlData = System.Text.Encoding.UTF8.GetString(data);

                try
                {
                    xmlData = xmlData.Remove(0, 1);
                    int endPos = xmlData.IndexOf("\0");
                    if (endPos >= 0)
                        xmlData = xmlData.Remove(endPos);

                    xe = XmlUtilities.Parse_IgnoringErrorsInFulltext(xmlData);
                }
                catch (XmlException ex)
                {
                    InvalidXmlException xmlExeption = new InvalidXmlException(ex);
                    xmlExeption.DocumentXmlUri = documentXmlUri;
                    xmlExeption.HttpWebResponseXml = xmlData;
                    throw xmlExeption;
                }
            }
            catch (WebException e)
            {
                AssetUnavailableException xmlExeption = new AssetUnavailableException(e);
                xmlExeption.DocumentXmlUri = documentXmlUri;
                throw xmlExeption;
            }

            return xe;
        }

        public static string CombineUris(string baseUrl, string relativeUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(baseUrl))
                    return relativeUrl;
                if (string.IsNullOrEmpty(relativeUrl))
                    return baseUrl;

                Uri baseUri = new System.Uri(baseUrl, UriKind.Absolute);
                Uri relativeUri = new System.Uri(relativeUrl, UriKind.Relative);

                if (!baseUri.ToString().EndsWith("/"))
                {
                    string modifiedBaseUrl = baseUrl + "/";
                    baseUri = new System.Uri(modifiedBaseUrl, UriKind.Absolute);
                }

                return new Uri(baseUri, relativeUri).ToString();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return baseUrl + relativeUrl;
            }
        }

        public static bool ExtractStoredAssetInformation(string path, string userName, string passWord, bool linkIsOnServer,
                                                         out string filePrefix, out int width, out int height)
        {
            width = 0;
            height = 0;
            filePrefix = "recording";

            int lastSlash = path.LastIndexOf("/");
            string documentUri = path.Substring(0, lastSlash);

            XElement xe = null;
            if (linkIsOnServer)
            {
                // Create the Web request object.
                string documentXmlUri = Path.Combine(documentUri, Path.Combine("content", "document.xml"));
                documentXmlUri = documentXmlUri.Replace("\\", "/");
                xe = HttpUtilities.DownloadDocumentInformation(documentXmlUri, userName, passWord);
            }
            else
            {
                string documentXml = Path.Combine(documentUri, Path.Combine("content", "document.xml"));
                if ((File.Exists(documentXml)))
                {
                    var content = File.ReadAllText(documentXml);
                    xe = XmlUtilities.Parse_IgnoringErrorsInFulltext(content);
                }
            }
            XmlUtilities.GetFlashDocumentInformation(xe, out width, out height, out filePrefix);

            return true;
        }

        public static string CreateMediaUri(string serverUri, string mediaId, string assetUri)
        {
            string mediaUri;
            Uri uri = new Uri(serverUri);
            string uriString = uri.Scheme + "://" + uri.Host + ":" + uri.Port;
            mediaUri = uriString + "/lc_kbase/asset/" + mediaId + "/" + assetUri;

            return mediaUri;
        }
    }
}
