using System;
using System.IO;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Diagnostics;
using System.Web;
/*
 * !!!!! Please do not use log4net here, it may be not initialized !!!!!
 */
//using log4net;

using AMS.Profile;

namespace LiveContext.Configuration
{
    public class ConfigFile
    {
        #region properties

        // localization
        public string cultureInfo { get; private set; }
        public string contentLanguage { get; set; }

        // server
        public string configRules { get; private set; }
        public string serverUri { get; private set; }
        public string lpxUri { get; private set; }
        public string softwareUpdateUri { get; private set; }
        public TimeSpan softwareUpdateInterval { get; private set; }
        public string dbUpdateUri { get; private set; }
        public TimeSpan dbUpdateInterval { get; private set; }
        public string contentImageMirrorDirectory { get; private set; }

        // UserInterface
        public bool showCurrentApplication { get; private set; }
        public bool showCurrentWindow { get; private set; }
        public bool showCurrentElement { get; private set; }

        public bool showUriInfo { get; private set; }
        public string themeName { get; private set; }
        public bool singleInstance { get; private set; }

        public bool welcomeMessageAfterPadLogin { get; private set; }
        public bool welcomeMessageAfterWindowsLogin { get; private set; }
        public bool welcomeMessageConfirm { get; private set; }

        public string sharepointLinkUri { get; private set; }
        public string blogLinkUri { get; private set; }

        public bool enableControlCenter { get; private set; }

        public bool showTrayStatusMessage { get; private set; }

        public bool expertMode { get; private set; }

        // search
        public bool enableSearch { get { return enableLivecontextSearch | enableWikipediaSearch | enableYoutubeSearch | enableSharepointSearch; } }
        public bool enableLivecontextSearch { get; private set; }
        public bool enableWikipediaSearch { get; private set; }
        public bool enableYoutubeSearch { get; private set; }
        public bool enableSharepointSearch { get; private set; }
        public string sharepointSearchServiceUri { get; private set; }

        // login
        public string defaultUser { get; private set; }
        public string defaultPassword { get; private set; }
        public bool useWindowsLogin { get; private set; }
        public bool showAutoLoginFailure { get; private set; }

        // feedback technologies
        public bool feedbackUsesAcc { get; private set; }
        public bool feedbackUsesUiAutomation { get; private set; }
        public bool feedbackUsesUiAutomationTree { get; private set; }
        public bool feedbackUsesSap { get; private set; }
        public bool reportAsWindowsUser { get; private set; }

        public bool feedbackEnabled
        {
            get { return feedbackUsesAcc || feedbackUsesUiAutomation || feedbackUsesUiAutomationTree || feedbackUsesSap; }
        }

        // technologies
        public bool useNameChangeEvent { get; private set; }
        public bool useNameChangeTimer { get; private set; }
        public bool supportWindows7only { get; private set; }
        public bool supportOffice2010only { get; private set; }
        public int searchDepth { get; private set; }

        // analyzer mode
        public bool analyzerMode { get; private set; }

        // storage / database
        public bool useStoragePerServer { get; private set; }
        public string dataFolder { get; private set; }

        // system
        public TimeSpan additionalGCInterval { get; private set; }

        //content
        public String defaultFontFamily { get; private set; }
        public double defaultFontSize { get; private set; }
        public int contentFadeInInterval { get; private set; }
        public int contentFadeOutInterval { get; private set; }

        // Debug
        public bool doCompleteRefresh;
        #endregion

        #region constructor and initialization
        private Ini configFile = new Ini();
        private static readonly object padlock = new object();
        private static ConfigFile instance = null;

        public static ConfigFile Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ConfigFile();
                        instance.Initialize();
                    }
                    return instance;
                }
            }
        }

        private void Initialize()
        {
            /*
             * !!!!! Please do not use log4net here, it may be not initialized !!!!!
             */ 

            // TODO: pause all timers / check if other timer is executed

            UninitializeUpdateTimer();

            // update config file from URI or server
            CopyConfigFileFromWebServer();

            // Note: the following parameters belong to Designer (D), Pad (P) or both (D/P)
            
            // localization
            cultureInfo = GetStringValue("localization", "culture", "en"); //(D/P)
            contentLanguage = GetStringValue("localization", "contentLanguage", ""); //(P)

            // server
            configRules = GetStringValue("server", "configRules", "local"); //(P)
            serverUri = GetStringValue("server", "serverUri"); //(D/P)
            lpxUri = GetStringValue("server", "lpxUri"); //(P)

            softwareUpdateUri = GetStringValue("server", "softwareUpdateUri"); //(P)
            softwareUpdateInterval = parseInterval(GetStringValue("server", "softwareUpdateInterval")); //(P)
            dbUpdateUri = GetStringValue("server", "dbUpdateUri"); //(P)
            dbUpdateInterval = parseInterval(GetStringValue("server", "dbUpdateInterval")); //(P)

            contentImageMirrorDirectory = GetStringValue("server", "contentImageMirrorDirectory"); //(P)

            // login configuration
            defaultUser = GetStringValue("login", "user"); //(P/D)
            defaultPassword = GetStringValue("login", "pass"); //(P/D)
            useWindowsLogin = GetBoolValue("login", "useWindowsLogin", false); //(P/D)
            showAutoLoginFailure = GetBoolValue("login", "showAutoLoginFailure", true); //(P)

            // user interface
            showCurrentApplication = GetBoolValue("userInterface", "showCurrentApplication", true); //(P)
            showCurrentWindow = GetBoolValue("userInterface", "showCurrentWindow", true); //(P)
            showCurrentElement = GetBoolValue("userInterface", "showCurrentElement", true); //(P)
            showUriInfo = GetBoolValue("userInterface", "showUriInfo", false); //(D/P)
            singleInstance = GetBoolValue("userInterface", "singleInstance", true); //(P)

            themeName = GetStringValue("userInterface", "theme", "default"); //(P/D)

            welcomeMessageAfterPadLogin = GetBoolValue("userInterface", "welcomeMessageAfterPadLogin", true); //(P)
            welcomeMessageAfterWindowsLogin = GetBoolValue("userInterface", "welcomeMessageAfterWindowsLogin", true); //(P)
            welcomeMessageConfirm = GetBoolValue("userInterface", "welcomeMessageConfirm", true); //(P)

            sharepointLinkUri = GetStringValue("userInterface", "sharepointLinkUri", ""); //(P)
            blogLinkUri = GetStringValue("userInterface", "blogLinkUri", ""); //(P)

            enableControlCenter = GetBoolValue("userInterface", "enableControlCenter", false); //(P)

            showTrayStatusMessage = GetBoolValue("userInterface", "showTrayStatusMessage", true); //(P)

            expertMode = GetBoolValue("userInterface", "expertMode", false); //(D)

            // search
            enableLivecontextSearch = GetBoolValue("search", "enableLivecontext", true); //(P)
            enableWikipediaSearch = GetBoolValue("search", "enableWikipedia", false); //(P)
            enableYoutubeSearch = GetBoolValue("search", "enableYoutube", false); //(P)
            enableSharepointSearch = GetBoolValue("search", "enableSharepoint", false); //(P)
            sharepointSearchServiceUri = GetStringValue("search", "sharepointSearchServiceUri", ""); //(P)
            if (string.IsNullOrEmpty(sharepointSearchServiceUri))
                enableSharepointSearch = false;

            // technologies
            useNameChangeEvent = GetBoolValue("technologies", "useNameChangeEvent", false); //(P)
            useNameChangeTimer = GetBoolValue("technologies", "useNameChangeTimer", false); //(P)
            supportWindows7only = GetBoolValue("technologies", "supportWindows7only", false); //(P)
            supportOffice2010only = GetBoolValue("technologies", "supportOffice2010only", false); //(P)

            // TODO which depth for release?
            searchDepth = GetIntValue("technologies", "searchDepth", 5); //(P)


            // feedback technologies
            feedbackUsesAcc = GetBoolValue("feedbackTechnologies", "acc", false); //(P)
            feedbackUsesUiAutomation = GetBoolValue("feedbackTechnologies", "uiAutomation", false); //(P)
            feedbackUsesUiAutomationTree = GetBoolValue("feedbackTechnologies", "uiAutomationTree", false); //(P)
            feedbackUsesSap = GetBoolValue("feedbackTechnologies", "sap", false); //(P)
            reportAsWindowsUser = GetBoolValue("feedbackTechnologies", "reportAsWindowsUser", false); //(P)

            // analyzer mode
            analyzerMode = GetBoolValue("analyzer", "active", true /*false*/); //(P)
            if (analyzerMode)
            {
                // override colliding settings
                serverUri = null;
                dbUpdateUri = null;
                softwareUpdateUri = null;
                lpxUri = null;
                useNameChangeTimer = false;
                useNameChangeEvent = false;
            }

            // storage and data folder
            useStoragePerServer = GetBoolValue("storage", "useStoragePerServer", true); //(P)

            var storageFolderMode = GetStringValue("storage", "storageFolderMode"); //(D/P)
            var storageFolder = GetStringValue("storage", "storageFolder"); //(D/P)
            if (!String.IsNullOrEmpty(storageFolder) && (String.IsNullOrEmpty(storageFolderMode) || storageFolderMode.Equals("custom")))
                dataFolder = storageFolder;
            else if (!String.IsNullOrEmpty(storageFolderMode) && storageFolderMode.Equals("application"))
                dataFolder = AppDomain.CurrentDomain.BaseDirectory;
            else // mode = "user" data path
            {
                string subFolder = "";
                try
                {
                    Assembly asm = Assembly.GetEntryAssembly();

                    if (asm.FullName.Contains("Designer"))
                        subFolder = "LivecontextDesigner";
                    else if (asm.FullName.Contains("Panel") || asm.FullName.Contains("Pad"))
                        subFolder = "LivecontextPad";
                    else
                        subFolder = "Livecontext";
                }
                catch
                {
                    subFolder = "Livecontext";
                }
                dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), subFolder);
            }
            if (!Directory.Exists(dataFolder))
                try { Directory.CreateDirectory(dataFolder); }
                catch { }

            // system
            additionalGCInterval = parseInterval(GetStringValue("system", "additionalGCInterval")); //(P);

            // content
            defaultFontFamily = GetStringValue("content", "defaultFontFamily", ""); //(D);
            defaultFontSize = GetDoubleValue("content", "defaultFontSize", 0); //(D);           
            contentFadeInInterval = GetIntValue("content", "contentFadeInInterval", 0); //(P)
            contentFadeOutInterval = GetIntValue("content", "contentFadeOutInterval", 100); //(P)

            // Debug
            doCompleteRefresh = GetBoolValue("Debug", "doCompleteRefresh", false); //(D)

            InitializeUpdateTimer();
        }
        #endregion

        private bool CopyConfigFileFromWebServer()
        {
            // TODO: move call in Application_Startup() behind initialization of log4net logger
            /*
             * !!!!! Please do not use log4net here, it may be not initialized !!!!!
             */

            bool updated = false;

            // determine configuration path (from registry, ini, or server)
            string configUri = GetConfigPath();

            if (String.IsNullOrEmpty(configUri))
                return updated;

            var ini = configFile.DefaultName;
            var update = ini + ".update";

            try
            {
                // download configuration file
                var webclient = new WebClient();
                webclient.DownloadFile(configUri, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, update));

                // check whether configuration has changed or not
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, update)))
                {
                    var newContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, update));
                    if (!string.IsNullOrEmpty(newContent))
                    {
                        string oldContent = "";
                        if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ini)))
                            oldContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ini));

                        // check for modification
                        if (newContent != oldContent)
                        {
                            // replace existing ini by updated ini
                            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, update), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ini), true);
                            updated = true;
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            finally
            {
                // remove no longer used download
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, update)))
                    File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, update));
            }

            return updated;
        }

        private string GetConfigPath()
        {
            /*
             * !!!!! Please do not use log4net here, it may be not initialized !!!!!
             */

            // do not update config in Analyzer Mode
            if (GetBoolValue("analyzer", "active", false))
                return null;

            string configUri = null;

            // check for new configuration entry in registry
            if (GetStringValue("server", "configRegistry") != null)
            {
                configUri = GetStringValue("server", "configRegistry").ToString();

                if (configUri != null)
                {
                    int valuePos = configUri.LastIndexOf("\\");
                    if (valuePos > 0)
                    {
                        string regValue = configUri.Substring(valuePos + 1);
                        string regKey = configUri.Remove(valuePos);
                        string regEntry = (string)Microsoft.Win32.Registry.GetValue(regKey, regValue, null);
                        configUri = regEntry;
                    }
                }
            }

            // check for new configuration specified by URL in Pad.ini
            if (String.IsNullOrEmpty(configUri) && GetStringValue("server", "configUri") != null)
                configUri = GetStringValue("server", "configUri");

            // check for new configuration on server (since 3.7 - BLCP-730)
            // note: this.serverUri is not initialized yet. Hence read from Pad.ini file
            var serverUri = GetStringValue("server", "serverUri");
            if (String.IsNullOrEmpty(configUri) && !String.IsNullOrEmpty(serverUri))
            {
                try
                {
                    // apply to Pad only
                    Assembly asm = Assembly.GetEntryAssembly();
                    if (asm.FullName.Contains("Panel") || asm.FullName.Contains("Pad"))
                    {
                        // get server base
                        Uri baseUri = new Uri(serverUri);
                        var serverBase = baseUri.AbsoluteUri.Replace(baseUri.AbsolutePath, "");

                        // append Pad config update path
                        configUri = serverBase + "/lc_kbase/padsettingsexport.seam";
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }

            return configUri;
        }

        #region periodical updates of configuration

        private System.Windows.Forms.Timer configUpdateTimer;

        private void InitializeUpdateTimer()
        {
            try
            {
                string configUri = GetConfigPath();
                if (!String.IsNullOrEmpty(configUri))
                {
                    TimeSpan configUpdateInterval = new TimeSpan();
                    if (GetStringValue("server", "configUpdateInterval") != null)
                        configUpdateInterval = parseInterval(GetStringValue("server", "configUpdateInterval"));

                    if (configUpdateInterval.Ticks > 0)
                    {
                        configUpdateTimer = new System.Windows.Forms.Timer();
                        configUpdateTimer.Interval = (int)configUpdateInterval.TotalMilliseconds;
                        configUpdateTimer.Tick += new EventHandler(CheckForConfigUpdate);
                        configUpdateTimer.Start();
                    }
                }
            }
            catch { }
        }

        private void UninitializeUpdateTimer()
        {
            /*
             * !!!!! Please do not use log4net here, it may be not initialized !!!!!
             */ 

            try
            {
                if (configUpdateTimer != null)
                {
                    configUpdateTimer.Stop();
                    configUpdateTimer.Dispose();
                    configUpdateTimer = null;
                }
            }
            catch { }
        }

        private void CheckForConfigUpdate(object sender, EventArgs e)
        {
            try
            {
                // check for new configuration on server
                if (CopyConfigFileFromWebServer())
                {
                    // configuration has changed -> restart application

                    // TODO: central restart method
                    // restart application
                    var executable = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(System.Windows.Forms.Application.ExecutablePath));
                    Process.Start(executable, "-restarted");

                    // close this instance of application
                    //log4net.LogManager.GetLogger("root").Info("SHUTDOWN in order to update Configuration");
                    Application.Current.Shutdown(0);
                }
            }
            catch { }
        }
        #endregion

        #region helper
        private string GetStringValue(string section, string entry)
        {
            return GetStringValue(section, entry, null);
        }

        private string GetStringValue(string section, string entry, string defaultValue)
        {
            string value = (string)configFile.GetValue(section, entry);

            // handle Windows Environment Variables in config parameters
            if (!string.IsNullOrEmpty(value))
                try { value = Environment.ExpandEnvironmentVariables(value); }
                catch { }

            if (string.IsNullOrEmpty(value))
                value = defaultValue;
            return value;
        }

        private bool GetBoolValue(string section, string entry, bool defaultValue)
        {
            bool result = defaultValue;
            try
            {
                var valueString = GetStringValue(section, entry);
                if (valueString != null)
                    result = Convert.ToBoolean(valueString);
            }
            catch { }
            return result;
        }

        private int GetIntValue(string section, string entry, int defaultValue)
        {
            int result = defaultValue;
            try
            {
                var valueString = GetStringValue(section, entry);
                if (valueString != null)
                    result = Convert.ToInt32(valueString.ToString());
            }
            catch { }
            return result;
        }

        private double GetDoubleValue(string section, string entry, double defaultValue)
        {
            double result = defaultValue;
            try
            {
                var valueString = GetStringValue(section, entry);
                if (valueString != null)
                    result = Convert.ToDouble(valueString.ToString());
            }
            catch { }
            return result;
        }

        static TimeSpan parseInterval(string softwareUpdateInterval)
        {
            TimeSpan interval = new TimeSpan();
            if (!string.IsNullOrEmpty(softwareUpdateInterval))
            {
                try
                {
                    interval = TimeSpan.Parse(softwareUpdateInterval);
                }
                catch
                {
                    try
                    {
                        if (softwareUpdateInterval.EndsWith("d"))
                        {   // days
                            int days = int.Parse(softwareUpdateInterval.Substring(0, softwareUpdateInterval.Length - 1));
                            interval = TimeSpan.FromDays(days);
                        }
                        else if (softwareUpdateInterval.EndsWith("h"))
                        {   // hours
                            int hours = int.Parse(softwareUpdateInterval.Substring(0, softwareUpdateInterval.Length - 1));
                            interval = TimeSpan.FromHours(hours);
                        }
                        else if (softwareUpdateInterval.EndsWith("m"))
                        {   // minutes
                            int minutes = int.Parse(softwareUpdateInterval.Substring(0, softwareUpdateInterval.Length - 1));
                            interval = TimeSpan.FromMinutes(minutes);
                        }
                        else if (softwareUpdateInterval.EndsWith("ms"))
                        {   // milliseconds (mainly for debug purpose or garbage collection)
                            int milliseconds = int.Parse(softwareUpdateInterval.Substring(0, softwareUpdateInterval.Length - 2));
                            interval = TimeSpan.FromMilliseconds(milliseconds);
                        }
                        else if (softwareUpdateInterval.EndsWith("s"))
                        {   // seconds (mainly for debug purpose or garbage collection)
                            int seconds = int.Parse(softwareUpdateInterval.Substring(0, softwareUpdateInterval.Length - 1));
                            interval = TimeSpan.FromSeconds(seconds);
                        }

                    }
                    catch { }
                }
            }
            return interval;
        }
        #endregion
    }
}
