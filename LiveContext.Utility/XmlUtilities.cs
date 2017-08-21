using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LiveContext.Utility {

    public class XmlUtilities {
        static string DIFF_XLS_FOLDER  = "lc_diff_xls";
        static string PATH_DESIGNER    = "lc_designer\\LiveContext.Designer\\LiveContext.Designer.Gui\\Localization\\";
        static string PATH_PAD         = "lc_client\\LiveContext.Panel.Gui\\Localization\\";
        static string PATH_LOGIN       = "lc_core\\LiveContext.LoginHandling\\Localization\\";
        static string PATH_UTILITY     = "lc_core\\LiveContext.Utility\\Localization\\";
        static string PREFIX_DESIGNER  = "LivecontextDesigner";
        static string PREFIX_TELERIK   = "Telerik";
        static string PREFIX_PAD       = "Locals";
        static string PREFIX_LOGIN     = "Login";
        static string PREFIX_UTILITY   = "LiveContextUtility";

        public static List<string> GetContentIds(string _operationXml) {
            var result = new List<string>();

            var operation = XElement.Parse(_operationXml);
            foreach (XElement contentElement in operation.Descendants("content")) {
                if (contentElement.Attribute("contentId") != null) {
                    result.Add(contentElement.Attribute("contentId").Value);
                }
            }
            return result;
        }

        public static string XmlEscape(string unescaped)
        {
            XmlDocument doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerXml;
        }

        public static string XmlUnescape(string escaped)
        {
            XmlDocument doc = new XmlDocument();
            var node = doc.CreateElement("root");
            node.InnerXml = escaped;
            return node.InnerText;
        }
        
        public static void GetFlashDocumentInformation(XElement flashDocumentInfo, out int width, out int height, out string prefix)
        {
            width = 0;
            height = 0;
            prefix = "recording";

            if (flashDocumentInfo != null)
            {
                try
                {
                    IEnumerable<XElement> swfElements = flashDocumentInfo.Descendants("swf");
                    var swfNode = flashDocumentInfo.Descendants("swf").First();

                    var szFile = swfNode.Descendants("file").First().Value;
                    prefix = Path.GetFileNameWithoutExtension(szFile);
                }
                catch
                {
                }

                try
                {
                    var swfNode = flashDocumentInfo.Descendants("flashPlayer").First();

                    var szWidth = swfNode.Descendants("width").First().Value;
                    width = Convert.ToInt32(szWidth);
                    var szHeight = swfNode.Descendants("height").First().Value;
                    height = Convert.ToInt32(szHeight);
                }
                catch
                {
                }
            }
        }

        public static XElement Parse_IgnoringErrorsInFulltext(string content)
        {
            XElement xe = null;

            try
            {
                xe = XElement.Parse(content);
            }
            catch
            {
                // BUGFIX 7165: ignore special characters in fulltext which may cause errors
                int startFulltext = content.IndexOf("<fulltext>");
                int endFulltext = content.LastIndexOf("</fulltext>");
                if (startFulltext >= 0 && endFulltext >= 0 && startFulltext < endFulltext)
                    content = content.Remove(startFulltext, endFulltext - startFulltext + 11);
                xe = XElement.Parse(content);
            }

            return xe;
        }

        #region language resource helper to manage resx files

        // Create an Excel file (.xls) from  the difference .resx files (.resx.diff) from all language resources
        // The strings 'previousPath' and 'currentPath' define the given SVN repository (e.g. "C:\\sandbox_lc\\lc_dev\\trunk")
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string CreateDiffResxFilesForXls(string previousPath, string currentPath)
        {
            string resError = "";

            try
            {
                if (!previousPath.EndsWith("\\"))
                    previousPath = previousPath + "\\";
                if (!currentPath.EndsWith("\\"))
                    currentPath = currentPath + "\\";
                // Check the paths first - if wrong, a DirectoryNotFoundException is thrown.
                if (Directory.Exists(currentPath))
                {
                    if (Directory.Exists(previousPath))
                    {
                        // Create the difference .resx files (.resx.diff)
                        resError = CreateDiffResxFiles(previousPath, currentPath);

                        if (resError.Length <= 0)
                        {
                            // Move .diff files to a temporary path, assuming EN is the default language
                            string oldPath = currentPath;
                            string newPath = currentPath + DIFF_XLS_FOLDER + "\\";

                            if (Directory.Exists(newPath))
                                Directory.Delete(newPath, true);
                            Directory.CreateDirectory(newPath);

                            oldPath = currentPath + PATH_DESIGNER + PREFIX_DESIGNER + ".resx" + ".diff";
                            Directory.Move(oldPath, newPath + PREFIX_DESIGNER + ".resx");
                            oldPath = currentPath + PATH_DESIGNER + PREFIX_TELERIK + ".resx" + ".diff";
                            Directory.Move(oldPath, newPath + PREFIX_TELERIK + ".resx");
                            oldPath = currentPath + PATH_PAD + PREFIX_PAD + ".resx" + ".diff";
                            Directory.Move(oldPath, newPath + PREFIX_PAD + ".resx");
                            oldPath = currentPath + PATH_LOGIN + PREFIX_LOGIN + ".resx" + ".diff";
                            Directory.Move(oldPath, newPath + PREFIX_LOGIN + ".resx");
                            oldPath = currentPath + PATH_UTILITY + PREFIX_UTILITY + ".resx" + ".diff";
                            Directory.Move(oldPath, newPath + PREFIX_UTILITY + ".resx");
                        }

                    }
                    else
                    {
                        resError = "CreateDiffResxFilesForXls: System.IO.DirectoryNotFoundException! \nThe path " + previousPath + " can not be found.";
                    }
                }
                else
                {
                    resError = "CreateDiffResxFilesForXls: System.IO.DirectoryNotFoundException! \nThe path " + currentPath + " can not be found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateDiffResxFilesForXls: " + ex);
                resError = "CreateDiffResxFilesForXls: " + ex.ToString();
            }
            
            return resError;
        }

        // Create difference .resx files (.resx.diff) from all language resources
        // The strings 'previousPath' and 'currentPath' define the given SVN repository (e.g. "C:\\sandbox_lc\\lc_dev\\trunk")
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string CreateDiffResxFiles(string previousPath, string currentPath)
        {
            string resError = "";

            // A path check is already done in CreateDiffCsvFile()

            try
            {
                bool bDoSort = false; // sorting is for humans and textual diff tools only

                // Make diff of Designer resources
                if (resError.Length <= 0)
                    resError = CreateDiffResxFile("" + previousPath + PATH_DESIGNER + PREFIX_DESIGNER + ".resx",
                                    "" + currentPath + PATH_DESIGNER + PREFIX_DESIGNER + ".resx", bDoSort);
                if (resError.Length <= 0)
                    resError = CreateDiffResxFile("" + previousPath + PATH_DESIGNER + PREFIX_TELERIK + ".resx",
                                    "" + currentPath + PATH_DESIGNER + PREFIX_TELERIK + ".resx", bDoSort);

                // Make diff of Pad resources
                if (resError.Length <= 0)
                    resError = CreateDiffResxFile("" + previousPath + PATH_PAD + PREFIX_PAD + ".resx",
                                    "" + currentPath + PATH_PAD + PREFIX_PAD + ".resx", bDoSort);

                // Make diff of Login resources
                if (resError.Length <= 0)
                    resError = CreateDiffResxFile("" + previousPath + PATH_LOGIN + PREFIX_LOGIN + ".resx",
                                    "" + currentPath + PATH_LOGIN + PREFIX_LOGIN + ".resx", bDoSort);

                // Make diff of Utility resources
                if (resError.Length <= 0)
                    resError = CreateDiffResxFile("" + previousPath + PATH_UTILITY + PREFIX_UTILITY + ".resx",
                                    "" + currentPath + PATH_UTILITY + PREFIX_UTILITY + ".resx", bDoSort);
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateDiffResxFiles: " + ex);
                resError = "CreateDiffResxFiles: " + ex.ToString();
            }

            return resError;
        }

        // Create a difference .resx file (.resx.diff) containing the changes between previous and current .resx file
        // string 'previousResxFile' defines the .resx file with the "last known good" language resources
        // string 'currentResxFile' defines the .resx file with the modified language resources
        // string 'bDoSort' defines if the resource entries should be sorted alphabetically or not
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string CreateDiffResxFile(string previousResxFile, string currentResxFile, bool bDoSort)
        {
            string resError = "";

            // Assuming 'EN' is the default language for the difference 
            string oldEnResx = previousResxFile;
            string newEnResx = currentResxFile;

            // sorting is for humans and textual diff tools only
            if (bDoSort)
            {
                if (resError.Length <= 0)
                    resError = ResourceSort(oldEnResx);
                if (resError.Length <= 0)
                    resError = ResourceSort(newEnResx);
            }

            // create diff file with resources to be translated
            if (resError.Length <= 0)
                resError = ResourceDiff(oldEnResx, newEnResx);

            return resError;
        }

        // Merge all language resources for a given culture (e.g. 'fr-FR')
        // string 'culture' defines the culture (e.g. 'fr-FR')
        // string 'sourcePath' defines the path which contains the translated resource files (e.g. "C:\\sandbox_lc\\lc_dev\\trunk\\lc_diff_xls\\fr-FR")
        // string 'targetPath' defines the given SVN repository (e.g. "C:\\sandbox_lc\\lc_dev\\trunk")
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string MergeLanguageResourcesForCulture(string culture, string sourcePath, string targetPath)
        {
            string resError = "";

            try
            {
                // Check the paths first - if wrong, a DirectoryNotFoundException is thrown.
                if (Directory.Exists(sourcePath))
                {
                    if (Directory.Exists(targetPath))
                    {
                        // Extract 2-character language code from 'culture' (e.g. 'de', 'fr', 'it', 'es', ...)
                        // (currently used cultures: de-DE, fr-FR -- also a new one is possible)
                        string twoCharLanguageCode = culture.Substring(0, 2);

                        bool bDeleteNotExisting = false;

                        // Merge Designer resources
                        string sourceRes = sourcePath + PREFIX_DESIGNER + "." + culture + ".resx";
                        string targetRes = targetPath + PATH_DESIGNER + PREFIX_DESIGNER + "." + twoCharLanguageCode + ".resx";
                        if ((resError.Length <= 0) && (File.Exists(sourceRes)) && (File.Exists(targetRes)))
                        {
                            resError = ResourceMerge(targetRes, sourceRes, bDeleteNotExisting);
                            if (resError.Length <= 0)
                                File.Copy(sourceRes + ".merged", targetRes, true);
                        }
                        sourceRes = sourcePath + PREFIX_TELERIK + "." + culture + ".resx";
                        targetRes = targetPath + PATH_DESIGNER + PREFIX_TELERIK + "." + twoCharLanguageCode + ".resx";
                        if ((resError.Length <= 0) && (File.Exists(sourceRes)) && (File.Exists(targetRes)))
                        {
                            resError = ResourceMerge(targetRes, sourceRes, bDeleteNotExisting);
                            if (resError.Length <= 0)
                                File.Copy(sourceRes + ".merged", targetRes, true);
                        }

                        // Merge Pad resources
                        sourceRes = sourcePath + PREFIX_PAD + "." + culture + ".resx";
                        targetRes = targetPath + PATH_PAD + PREFIX_PAD + "." + twoCharLanguageCode + ".resx";
                        if ((resError.Length <= 0) && (File.Exists(sourceRes)) && (File.Exists(targetRes)))
                        {
                            resError = ResourceMerge(targetRes, sourceRes, bDeleteNotExisting);
                            if (resError.Length <= 0)
                                File.Copy(sourceRes + ".merged", targetRes, true);
                        }

                        // Merge Login resources
                        sourceRes = sourcePath + PREFIX_LOGIN + "." + culture + ".resx";
                        targetRes = targetPath + PATH_LOGIN + PREFIX_LOGIN + "." + twoCharLanguageCode + ".resx";
                        if ((resError.Length <= 0) && (File.Exists(sourceRes)) && (File.Exists(targetRes)))
                        {
                            resError = ResourceMerge(targetRes, sourceRes, bDeleteNotExisting);
                            if (resError.Length <= 0)
                                File.Copy(sourceRes + ".merged", targetRes, true);
                        }

                        // Merge Utility resources
                        sourceRes = sourcePath + PREFIX_UTILITY + "." + culture + ".resx";
                        targetRes = targetPath + PATH_UTILITY + PREFIX_UTILITY + "." + twoCharLanguageCode + ".resx";
                        if ((resError.Length <= 0) && (File.Exists(sourceRes)) && (File.Exists(targetRes)))
                        {
                            resError = ResourceMerge(targetRes, sourceRes, bDeleteNotExisting);
                            if (resError.Length <= 0)
                                File.Copy(sourceRes + ".merged", targetRes, true);
                        }
                    }
                    else
                    {
                        resError = "MergeLanguageResourcesForCulture: System.IO.DirectoryNotFoundException! \nThe path " + targetPath + " can not be found.";
                    }
                }
                else
                {
                    resError = "MergeLanguageResourcesForCulture: System.IO.DirectoryNotFoundException! \nThe path " + sourcePath + " can not be found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MergeLanguageResourcesForCulture: " + ex);
                resError = "MergeLanguageResourcesForCulture: " + ex.ToString();
            }

            return resError;
        }

        // Sort all language resources
        // string 'path' defines the given SVN repository (e.g. "C:\\sandbox_lc\\lc_dev\\trunk")
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string SortAllLanguageResources(string path)
        {
            string resError = "";
            try
            {
                if (!path.EndsWith("\\"))
                    path = path + "\\";

                // Check the path first - if wrong, a DirectoryNotFoundException is thrown.
                if (Directory.Exists(path))
                {
                    string culture = ""; // empty string means '.en'
                    if (resError.Length <= 0)
                        resError = SortLanguageResourcesforCulture(culture, path);

                    culture = ".de";
                    if (resError.Length <= 0)
                        resError = SortLanguageResourcesforCulture(culture, path);

                    culture = ".fr";
                    if (resError.Length <= 0)
                        resError = SortLanguageResourcesforCulture(culture, path);
                }
                else
                {
                    resError = "SortAllLanguageResources: System.IO.DirectoryNotFoundException! \nThe path " + path + " can not be found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SortAllLanguageResources: " + ex);
                resError = "SortAllLanguageResources: " + ex.ToString();
            }

            return resError;
        }

        // Sort language resources for a given culture
        // string 'culture' defines the 2 character language code (e.g. '.de' or '.fr'); an empty string means '.en'
        // string 'path' defines the given SVN repository (e.g. "C:\\sandbox_lc\\lc_dev\\trunk")
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string SortLanguageResourcesforCulture(string culture, string path)
        {
            string resError = "";

            try
            {
                // Sort Designer resources
                if (resError.Length <= 0)
                    resError = ResourceSort("" + path + PATH_DESIGNER + PREFIX_DESIGNER + culture + ".resx");
                if (resError.Length <= 0)
                    resError = RenameSortedResx("" + path + PATH_DESIGNER + PREFIX_DESIGNER + culture + ".resx" + ".sorted");
                if (resError.Length <= 0)
                    resError = ResourceSort("" + path + PATH_DESIGNER + PREFIX_TELERIK + culture + ".resx");
                if (resError.Length <= 0)
                    resError = RenameSortedResx("" + path + PATH_DESIGNER + PREFIX_TELERIK + culture + ".resx" + ".sorted");

                // Sort Pad resources
                if (resError.Length <= 0)
                    resError = ResourceSort("" + path + PATH_PAD + PREFIX_PAD + culture + ".resx");
                if (resError.Length <= 0)
                    resError = RenameSortedResx("" + path + PATH_PAD + PREFIX_PAD + culture + ".resx" + ".sorted");

                // Sort Login resources
                if (resError.Length <= 0)
                    resError = ResourceSort("" + path + PATH_LOGIN + PREFIX_LOGIN + culture + ".resx");
                if (resError.Length <= 0)
                    resError = RenameSortedResx("" + path + PATH_LOGIN + PREFIX_LOGIN + culture + ".resx" + ".sorted");

                // Sort Utility resources
                if (resError.Length <= 0)
                    resError = ResourceSort("" + path + PATH_UTILITY + PREFIX_UTILITY + culture + ".resx");
                if (resError.Length <= 0)
                    resError = RenameSortedResx("" + path + PATH_UTILITY + PREFIX_UTILITY + culture + ".resx" + ".sorted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("SortLanguageResourcesforCulture: " + ex);
                resError = "SortLanguageResourcesforCulture: " + ex.ToString();
            }
            return resError;
        }

        // Sort text resources and write to "<path>.sorted"
        // string 'path' defines the path and file name of the .resx file
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string ResourceSort(string path)
        {
            string resError = "";

            try
            {
                // load resource file
                var res = XDocument.Load(path);

                // read text resources
                SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
                var datas = res.Descendants("data");
                foreach (var element in datas)
                {
                    var name = element.Attribute("name").Value;
                    var value = element.Element("value").Value;
                    dict.Add(name, value);
                }

                // remove text resource
                while (res.Root.Element("data") != null)
                    res.Root.Element("data").Remove();

                // add sorted resources
                foreach (var key in dict.Keys)
                {
                    // create and add element from dictionary
                    var element = new XElement("data");
                    element.SetAttributeValue("name", key);
                    // add attribute - xml:space="preserve" - probably not required but automatically added
                    element.SetAttributeValue(XName.Get("space", "http://www.w3.org/XML/1998/namespace"), "preserve");
                    element.Add(new XElement("value", dict[key]));
                    res.Root.Add(element);
                }

                // store storted resources
                res.Save(path + ".sorted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ResourceSort: " + ex);
                resError = "ResourceSort: " + ex.ToString();
            }

            return resError;
        }

        // Rename "*.resx.sorted" back to "*.resx"
        // string 'path' defines the path and file name of the .resx file
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string RenameSortedResx(string path)
        {
            string resError = "";

            if (!path.EndsWith("resx.sorted"))
                resError = "RenameSortedResx: A file name ending with '.resx.sorted' expected. Found: " + path;

            if (resError.Length <= 0)
            {
                string newPath = path.Substring(0, (path.Length-7)); // cut off ".sorted"
                File.Delete(newPath); // delete an already existing ".resx" file (if any) before overwriting it
                Directory.Move(path, newPath);
            }

            return resError;
        }

        // Compare text resources and write to "<pathNew>.diff", which will contain any newly added or modified text resources.
        // All text resources in that diff file should be translated.
        // string 'pathOld' defines the path and file name of the previous .resx file
        // string 'pathNew' defines the path and file name of the current .resx file
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string ResourceDiff(string pathOld, string pathNew)
        {
            string resError = "";

            try
            {
                var resOld = XDocument.Load(pathOld);
                var resNew = XDocument.Load(pathNew);

                SortedDictionary<string, string> dictOld = new SortedDictionary<string, string>();
                SortedDictionary<string, string> dictNew = new SortedDictionary<string, string>();
                SortedDictionary<string, string> dictDiff = new SortedDictionary<string, string>();

                // get old values
                foreach (var element in resOld.Descendants("data"))
                {
                    var name = element.Attribute("name").Value;
                    var value = element.Element("value").Value;
                    dictOld.Add(name, value);
                }

                // get new value and diff
                foreach (var element in resNew.Descendants("data"))
                {
                    var name = element.Attribute("name").Value;
                    var value = element.Element("value").Value;
                    dictNew.Add(name, value);

                    string valueOld;
                    if (dictOld.TryGetValue(name, out valueOld))
                    {
                        if (valueOld != value)
                        {
                            Console.WriteLine("------------MODIFIED------------");
                            Console.WriteLine("name: " + name);
                            Console.WriteLine(value);
                            dictDiff.Add(name, value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("------------NEW------------");
                        Console.WriteLine("name: " + name);
                        Console.WriteLine(value);
                        dictDiff.Add(name, value);
                    }
                }
                foreach (var key in dictOld.Keys)
                {
                    if (!dictNew.ContainsKey(key))
                    {
                        Console.WriteLine("------------DELETED------------");
                        Console.WriteLine("name: " + key);
                        Console.WriteLine(dictOld[key]);
                    }
                }



                // remove text resource
                while (resNew.Root.Element("data") != null)
                    resNew.Root.Element("data").Remove();

                // add sorted resources
                foreach (var key in dictDiff.Keys)
                {
                    // create and add element from dictionary
                    var element = new XElement("data");
                    element.SetAttributeValue("name", key);
                    // add attribute - xml:space="preserve" - probably not required but automatically added
                    element.SetAttributeValue(XName.Get("space", "http://www.w3.org/XML/1998/namespace"), "preserve");
                    element.Add(new XElement("value", dictDiff[key]));
                    resNew.Root.Add(element);
                }

                // store storted resources
                resNew.Save(pathNew + ".diff");

            }
            catch (Exception ex)
            {
                Console.WriteLine("ResourceDiff: " + ex);
                resError = "ResourceDiff: " + ex.ToString();
            }

            return resError;
        }

        // merge text resources and write to "<pathNew>.merged"
        // string 'pathOld' defines the path and file name of the previous .resx file
        // string 'pathNew' defines the path and file name of the current .resx file
        // deleteNotExisting == true: text resources not available in new file will be removed
        // deleteNotExisting == false: add and overwerite text resources but do not delete
        // Return string: empty string - all OK; otherwise the string contains the error message
        public static string ResourceMerge(string pathOld, string pathNew, bool deleteNotExisting)
        {
            string resError = "";

            try
            {
                var resOld = XDocument.Load(pathOld);
                var resNew = XDocument.Load(pathNew);

                SortedDictionary<string, string> dict = new SortedDictionary<string, string>();

                // get old values
                foreach (var element in resOld.Descendants("data"))
                {
                    var name = element.Attribute("name").Value;
                    var value = element.Element("value").Value;
                    dict[name]=value;
                }

                // get new value and update dictionary
                foreach (var element in resNew.Descendants("data"))
                {
                    var name = element.Attribute("name").Value;
                    var value = element.Element("value").Value;

                    dict[name]=value;
                }

                // remove text resource
                if (deleteNotExisting)
                    while (resNew.Root.Element("data") != null)
                        resNew.Root.Element("data").Remove();

                // add sorted resources
                foreach (var key in dict.Keys)
                {
                    // create and add element from dictionary
                    var element = new XElement("data");
                    element.SetAttributeValue("name", key);
                    // add attribute - xml:space="preserve" - probably not required but automatically added
                    element.SetAttributeValue(XName.Get("space", "http://www.w3.org/XML/1998/namespace"), "preserve");
                    element.Add(new XElement("value", dict[key]));
                    resNew.Root.Add(element);
                }

                // store storted resources
                resNew.Save(pathNew + ".merged");

            }
            catch (Exception ex)
            {
                Console.WriteLine("ResourceMerge: " + ex);
                resError = "ResourceMerge: " + ex.ToString();
            }

            return resError;
        }
        #endregion
    }
}