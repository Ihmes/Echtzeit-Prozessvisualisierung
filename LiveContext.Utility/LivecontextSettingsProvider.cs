using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;


namespace LiveContext.Utility
{
/*
    public class LivecontextSettingsProvider : SettingsProvider
    {
        // Define some static strings later used in our XML creation
        // XML Root node
        const string XMLROOT = "configuration";
        
        // User section node
        const string USERNODE = "userSettings";

        // Application Specific Node
        string APPNODE = Assembly.GetEntryAssembly().GetName().Name + ".Properties.Settings";

        private System.Xml.XmlDocument xmlDoc = null;

        // Override the Initialize method
        public override void Initialize(string name, NameValueCollection col)
        {
            base.Initialize(this.ApplicationName, col);
        }

        // Override the ApplicationName property, returning the solution name.  No need to set anything, we just need to
        // retrieve information, though the set method still needs to be defined.
        public override string ApplicationName
        {
            get
            {
                return (System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
            }
            set
            {
                return;
            }
        }

        public virtual string GetSettingsFilename()
        {
            string fileName = "user.config";
            if (ApplicationName.Contains("Designer"))
                fileName = "designer.user.config";
            else if (ApplicationName.Contains("Panel") || ApplicationName.Contains("Pad"))
                fileName = "pad.user.config";
            return Path.Combine(ConfigFile.Instance.dataFolder, fileName);
        }

        // Retrieve settings from the configuration file
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext sContext, SettingsPropertyCollection settingsColl)
        {
            // Create a collection of values to return
            SettingsPropertyValueCollection retValues = new SettingsPropertyValueCollection();

            // Create a temporary SettingsPropertyValue to reuse
            SettingsPropertyValue setVal;

            // Loop through the list of settings that the application has requested and add them
            // to our collection of return values.
            foreach (SettingsProperty sProp in settingsColl)
            {
                setVal = new SettingsPropertyValue(sProp);
                setVal.IsDirty = false;
                setVal.SerializedValue = GetSetting(sProp);
                retValues.Add(setVal);
            }
            return retValues;
        }

        // Save any of the applications settings that have changed (flagged as "dirty")
        public override void SetPropertyValues(SettingsContext sContext, SettingsPropertyValueCollection settingsColl)
        {
            try
            {
                // Set the values in XML
                foreach (SettingsPropertyValue spVal in settingsColl)
                {
                    SetSetting(spVal);
                }

                // Write the XML file to disk
                XMLConfig.Save(GetSettingsFilename());
            }
            catch { }
        }

        private XmlDocument XMLConfig
        {
            get
            {
                //If we dont hold an xml document, try opening one.  
                // If it doesnt exist then create a new one ready.
                if (xmlDoc == null)
                {
                    xmlDoc = new XmlDocument();

                    // If we have not loaded the config, try reading the file from disk.
                    try
                    {
                        xmlDoc.Load(GetSettingsFilename());
                    }

                    // If the file does not exist on disk, catch the exception then create the XML template for the file.
                    catch (Exception)
                    {
                        // XML Declaration
                        // <?xml version="1.0" encoding="utf-8"?>
                        XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                        xmlDoc.AppendChild(dec);

                        // Create root node and append to the document
                        // <configuration>
                        XmlElement rootNode = xmlDoc.CreateElement(XMLROOT);
                        xmlDoc.AppendChild(rootNode);
                        
                        // Create the userSettings node and append to the root node
                        // <userSettings>
                        XmlElement userNode = xmlDoc.CreateElement(USERNODE);
                        xmlDoc.DocumentElement.AppendChild(userNode);

                        // Create the Application settings node and append to the userNode above
                        // <AppName.Properties.Settings>
                        XmlElement appNode = xmlDoc.CreateElement(APPNODE);
                        userNode.AppendChild(appNode);
                    }
                }

                return xmlDoc;
            }
        }

        // Retrieve values from the configuration file, or if the setting does not exist in the file, 
        // retrieve the value from the application's default configuration
        private object GetSetting(SettingsProperty setProp)
        {
            try
            {
                // Search for the specific settings node we are looking for in the configuration file.
                // If it exists, return the InnerText or InnerXML of its first child node, depending on the setting type.

                // If the setting is serialized as a string, return the text stored in the config
                if (setProp.SerializeAs.ToString() == "String")
                {
                    var node = XMLConfig.SelectSingleNode("//setting[@name='" + setProp.Name + "']");
                    if (node != null)
                        return node.FirstChild.InnerText;
                }

                // If the setting is stored as XML, deserialize it and return the proper object.  This only supports
                // StringCollections at the moment - I will likely add other types as I use them in applications.
                else
                {
                    string settingType = setProp.PropertyType.ToString();
                    var node = XMLConfig.SelectSingleNode("//setting[@name='" + setProp.Name + "']");
                    if (node != null)
                    {
                        string xmlData = node.FirstChild.InnerXml;
                        XmlSerializer xs = new XmlSerializer(typeof(string[]));
                        string[] data = (string[])xs.Deserialize(new XmlTextReader(xmlData, XmlNodeType.Element, null));

                        switch (settingType)
                        {
                            case "System.Collections.Specialized.StringCollection":
                                StringCollection sc = new StringCollection();
                                sc.AddRange(data);
                                return sc;
                            default:
                                return "";
                        }
                    }
                }
            }
            catch (Exception) { }

            // Check to see if a default value is defined by the application.
            // If so, return that value, using the same rules for settings stored as Strings and XML as above
            if ((setProp.DefaultValue != null))
            {
                if (setProp.SerializeAs.ToString() == "String")
                {
                    return setProp.DefaultValue.ToString();
                }
                else
                {
                    string settingType = setProp.PropertyType.ToString();
                    string xmlData = setProp.DefaultValue.ToString();
                    XmlSerializer xs = new XmlSerializer(typeof(string[]));
                    string[] data = (string[])xs.Deserialize(new XmlTextReader(xmlData, XmlNodeType.Element, null));

                    switch (settingType)
                    {
                        case "System.Collections.Specialized.StringCollection":
                            StringCollection sc = new StringCollection();
                            sc.AddRange(data);
                            return sc;

                        default: return "";
                    }
                }
            }

            return "";
        }

        private void SetSetting(SettingsPropertyValue setProp)
        {
            // Define the XML path under which we want to write our settings if they do not already exist
            XmlNode SettingNode = null;

            try
            {
                // Search for the specific settings node we want to update.
                // If it exists, return its first child node, (the <value>data here</value> node)
                var node = XMLConfig.SelectSingleNode("//setting[@name='" + setProp.Name + "']");
                if (node != null)
                    SettingNode = node.FirstChild;
            }
            catch (Exception) { }

            // If we have a pointer to an actual XML node, update the value stored there
            if ((SettingNode != null))
            {
                if (setProp.Property.SerializeAs.ToString() == "String")
                {
                    SettingNode.InnerText = setProp.SerializedValue.ToString();
                }
                else
                {
                    // Write the object to the config serialized as Xml - we must remove the Xml declaration when writing
                    // the value, otherwise .Net's configuration system complains about the additional declaration.
                    SettingNode.InnerXml = setProp.SerializedValue.ToString().Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "");
                }
            }
            else
            {
                // If the value did not already exist in this settings file, create a new entry for this setting

                // Search for the application settings node (<Appname.Properties.Settings>) and store it.
                XmlNode tmpNode = XMLConfig.SelectSingleNode("//" + APPNODE);

                // Create a new settings node and assign its name as well as how it will be serialized
                XmlElement newSetting = xmlDoc.CreateElement("setting");
                
                newSetting.SetAttribute("name", setProp.Name);

                if (setProp.Property.SerializeAs.ToString() == "String")
                {
                    newSetting.SetAttribute("serializeAs", "String");
                }
                else
                {
                    newSetting.SetAttribute("serializeAs", "Xml");
                }

                // Append this node to the application settings node (<Appname.Properties.Settings>)
                tmpNode.AppendChild(newSetting);

                // Create an element under our named settings node, and assign it the value we are trying to save
                XmlElement valueElement = xmlDoc.CreateElement("value");
                if (setProp.Property.SerializeAs.ToString() == "String")
                {
                    valueElement.InnerText = setProp.SerializedValue.ToString();
                }
                else
                {
                    // Write the object to the config serialized as Xml - we must remove the Xml declaration when writing
                    // the value, otherwise .Net's configuration system complains about the additional declaration
                    valueElement.InnerXml = setProp.SerializedValue.ToString().Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "");
                }

                //Append this new element under the setting node we created above
                newSetting.AppendChild(valueElement);
            }

        }
    }
*/
}
