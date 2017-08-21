using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Xml.Linq;
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LiveContext.Utility
{
    public class ApplicationUtilities
    {
       // static private ILog log = log4net.LogManager.GetLogger("root");

        private readonly static ResourceManager localization = new ResourceManager("LiveContext.Utility.Localization.LiveContextUtility",
                                                            Assembly.GetExecutingAssembly());

        public static string Version
        {
            get
            {
                if (string.IsNullOrEmpty(localization.GetString("Version"))) return "0.0";
                return localization.GetString("Version");
            }
        }

        public static int GetRevision()
        {
            // TODO: this is double code from LiveContext.Panel.Gui.Model.PanelViewMode.Revision
            try
            {
                /*   Which one is correct? 
                Assembly asm = Assembly.GetExecutingAssembly();
                Assembly asm = Assembly.GetEntryAssembly();
                Assembly asm = Assembly.GetExecutingAssembly();
                 * */
                Assembly asm = Assembly.GetEntryAssembly();

                Stream revisionStream = null;
                if (asm.FullName.Contains("Panel") || asm.FullName.Contains("Pad"))
                {
                    revisionStream = asm.GetManifestResourceStream("LiveContext.Panel.Gui.rversion.txt");
                    if (null == revisionStream)
                    {
                        // TODO: may be called "Pad" in future -> where exactly to find rversion.txt for Pad then?
                        revisionStream = asm.GetManifestResourceStream("LiveContext.Pad.Gui.rversion.txt");
                    }
                }
                else if (asm.FullName.Contains("Designer"))
                {
                    revisionStream = asm.GetManifestResourceStream("LiveContext.Designer.GUI.rversion.txt");
                }
                else if (asm.FullName.Contains("VersionXmlWriter"))
                {
                    FileStream fileStream = new FileStream("rversion.txt", FileMode.Open, FileAccess.Read);
                    var reader = new StreamReader(fileStream);
                    var version = int.Parse(reader.ReadToEnd());
                    return version;
                }

                if (revisionStream != null)
                {
                    var reader = new StreamReader(revisionStream);
                    var version = int.Parse(reader.ReadToEnd());
                    return version;
                }
            }
            catch
            {
                return 0;
            }

            return 0;
        }



        /***********************************************************************
         * Software Update Mechanism
         ***********************************************************************/
        #region Software Update

        // duplicate of: LiveContext.Storage.StorageBase.WORKING_PATH
        // because assambly reference not possible
        const string WORKING_PATH = "storage";

        // local program files will be copied here before overwriting them
        const string UPDATE_BACKUP_FOLDER = "_backup";
        // new/updated files will be donwloaded here
        const string UPDATE_UPDATE_FOLDER = "_update";

        // version info for local files
        const string CLIENT_VERSION_FILE = "version.xml";
        static string localClientVersionFile { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CLIENT_VERSION_FILE); } }

        // copy of version info for server based files
        const string SERVER_VERSION_FILE = "version.server.xml";
        // "version.xml" writer program
        const string VERSION_XML_WRITER = "LiveContext.VersionXmlWriter.exe";

        // check for update (according to days since last check) and download update
        // TODO: passing localization is a hack
        public static void CheckForSoftwareUpdate(string serverPath, ResourceManager localization, TimeSpan interval)
        {
            // check schedule
            // wait for given update interval (and at least one minute to avoid immediate check after restart of updated version)
            var scheduledCheck = GetLastChecked().Add(interval);
            if (scheduledCheck > DateTime.Now.ToLocalTime())
                return; // too early; skip check

          //  log.Info("SOFTWARE UPDATE: check started");

            try
            {
                // create local version checksums
                CreateVersionFile();

              //  log.Info("SOFTWARE UPDATE: search for update from: " + serverPath);

                // provide clean folder
                var clientPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UPDATE_UPDATE_FOLDER);
                if (Directory.Exists(clientPath))
                    Directory.Delete(clientPath, true);
                Directory.CreateDirectory(clientPath);

                // download file version information from server
                var webclient = new WebClient();
                var serverVersionFile = Path.Combine(clientPath, SERVER_VERSION_FILE);
                webclient.DownloadFile(Path.Combine(serverPath, CLIENT_VERSION_FILE), serverVersionFile);

                // check server revsion
                var serverVersionXml = XDocument.Load(serverVersionFile);
                var serverRevsion = 0;
                try { serverRevsion = int.Parse(serverVersionXml.Root.Element("revision").Value); }
                catch { }
                if (GetRevision() > serverRevsion)
                {
              //      log.Info("SOFTWARE UPDATE: no update available (server update outdated)");
                    // cleanup
                    if (Directory.Exists(clientPath))
                        Directory.Delete(clientPath, true);
                    return;
                }

                // read local version checksums
                var localVersions = new System.Collections.Specialized.NameValueCollection();
                var localVersionsFile = XDocument.Load(localClientVersionFile);
                foreach (XElement element in localVersionsFile.Descendants("file"))
                {
                    var name = element.Attribute("name").Value;
                    var checksum = element.Attribute("checksum").Value;
                    localVersions.Add(name, checksum);
                }

                // read and compare server version checksums
                var serverVersions = new System.Collections.Specialized.NameValueCollection();
                int overallSize = 0;
                foreach (XElement element in serverVersionXml.Descendants("file"))
                {
                    var name = element.Attribute("name").Value;
                    var size = element.Attribute("size").Value;
                    var checksum = element.Attribute("checksum").Value;
                    var localChecksum = localVersions.Get(name);

                    Console.WriteLine("file: " + name+" ("+size+" bytes)");
                    Console.WriteLine("   local# " + localChecksum);
                    Console.WriteLine("  server# " + checksum);

                    // compare with local copy
                    if (checksum != localChecksum)
                    {
                        Console.WriteLine("add");
                        serverVersions.Add(name, checksum);
                        try { overallSize += int.Parse(size); }
                        catch { }
                    }
                }

                // remember current date
                SetLastChecked(localClientVersionFile);

                if (serverVersions.Count > 0)
                {
                    //        log.Info("SOFTWARE UPDATE: files available: " + serverVersions.Count + " [" + overallSize + " bytes]");
                }
                else
                {
                    //            log.Info("SOFTWARE UPDATE: no update available");
                    // cleanup
                    if (Directory.Exists(clientPath))
                        Directory.Delete(clientPath, true);
                    return;
                }

                // TODO: ask for download

                // download updates
              //  log.Info("SOFTWARE UPDATE: download update to: " + clientPath);
                foreach (string name in serverVersions.Keys)
                {
                    Console.WriteLine("Download: " + name);
             //       log.Info("SOFTWARE UPDATE: Download: " + name);
                    var source = Path.Combine(serverPath, name);
                    var dest = Path.Combine(clientPath, name);
                    Directory.CreateDirectory(Path.GetDirectoryName(dest));
                    webclient.DownloadFile(source, dest);
                }

                // create backup of current application folder
                var backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UPDATE_BACKUP_FOLDER);
           //     log.Info("SOFTWARE UPDATE: create backup: " + backupPath);
                if (Directory.Exists(backupPath))
                    Directory.Delete(backupPath, true);
                LiveContext.Utility.BaseUtils.CopyFolder(AppDomain.CurrentDomain.BaseDirectory, backupPath, false);
                string[] folders = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Path.Combine(backupPath, name);
                    // do not copy data or temp folders
                    if (name != UPDATE_BACKUP_FOLDER && name != UPDATE_UPDATE_FOLDER && name != WORKING_PATH)
                        LiveContext.Utility.BaseUtils.CopyFolder(folder, dest, true);
                }

                // TODO: ask for restart

                // TODO: central restart method
                // restart application in temp folder
            //    log.Info("SOFTWARE UPDATE: restart application");
                Console.WriteLine("Executable: " + System.Windows.Forms.Application.ExecutablePath);
                var executable = Path.Combine(backupPath, Path.GetFileName(System.Windows.Forms.Application.ExecutablePath));
                Process.Start(executable, "-update");

                // close this instance of application
             //   log.Info("SHUTDOWN in order to aply Software Update");
                Application.Current.Shutdown(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\nUpdater failed: " + ex);
          //      log.Error("SOFTWARE UPDATE: FAILED: Exception: " + ex);
            }
        }

        // apply downloaded update after restart
        public static bool ApplySoftwareUpdate(ResourceManager localization)
        {
            try
            {
            //    log.Info("SOFTWARE UPDATE: restarted");

                // wait until all other inctances of the application are closed
                if (!WaitForClosingOtherInstances())
                {
             //       log.Error("SOFTWARE UPDATE: FAILED: LIVECONTEXT is still running");
                    // TODO: error handling
                    return false;
                }

                // started from sub folder "_backup"
                var backupPath = AppDomain.CurrentDomain.BaseDirectory;
                // "_backup" is sub folder of normal main application folder
                var basePath = Directory.GetParent(backupPath).Parent.FullName;
                // copy content of sub folder "_update", which contains the new Pad version, to the normal main application folder
                var updatePath = Path.Combine(basePath, UPDATE_UPDATE_FOLDER);
                /*
                log.Info("SOFTWARE UPDATE: applying update");
                log.Info("SOFTWARE UPDATE: update path: " + updatePath);
                log.Info("SOFTWARE UPDATE: base path: " + basePath);
                log.Info("SOFTWARE UPDATE: backup path: " + backupPath);
                */
                // copy downloaded files to application folder
                LiveContext.Utility.BaseUtils.CopyFolder(updatePath, basePath, true);

                // update version file
                // note: DO NOT use "static localClientVersionFile" here, because Pad currently runs in "_backup" sub folder!!!
                var localClientVersionFile = Path.Combine(basePath, CLIENT_VERSION_FILE);
                var localServerVersionFile = Path.Combine(basePath, SERVER_VERSION_FILE);
                File.Copy(localServerVersionFile, localClientVersionFile, true);
                File.Delete(localServerVersionFile);

                // remember current date
                // note: DO NOT use "static localClientVersionFile" here, because Pad currently runs in "_backup" sub folder!!!
                SetLastChecked(localClientVersionFile);
                SetLastUpdated(localClientVersionFile);

              //  log.Info("SOFTWARE UPDATE: update successful");

                // cleanup downloaded files
                Directory.Delete(updatePath, true);

                //  log.Info("SOFTWARE UPDATE: restart application");

                // TODO: central restart method
                // restart application in updated application folder
                var executable = Path.Combine(basePath, Path.GetFileName(System.Windows.Forms.Application.ExecutablePath));
                Process.Start(executable, "-restarted");

                // close current instance
                //   log.Info("SHUTDOWN after applying Software Update");
                Application.Current.Shutdown(0);
                return true;
            }
            catch (Exception ex)
            {
                //   log.Error("SOFTWARE UPDATE: FAILED: Exception: " + ex);

                throw new Exception(localization.GetString("SoftwareUpdate.Install.Failed")+": "+ex.Message, ex);
            }
        }

        public static bool otherInstanceRunning()
        {
            try
            {
                // wait until other instances of application have been closed
                var processName = Process.GetCurrentProcess().ProcessName;

                Process[] processByName = Process.GetProcessesByName(processName);
                foreach (var process in processByName)
                {
                    try
                    {
                        // use same session only (e.g. when running on Remote Desktop, Windows Terminal Server or Citrix environment)
                        if (process.SessionId != Process.GetCurrentProcess().SessionId)
                            continue;

                        // get other instance
                        if (process.Id == Process.GetCurrentProcess().Id)
                            continue;

                        // other Pad instance is already running in same session
                        return true;
                    }
                    catch { }
                }
            }
            catch { }
            return false;
        }

        // wait until all other inctances of the application are closed
        private static bool WaitForClosingOtherInstances()
        {
            try
            {
                //   log.Info("SOFTWARE UPDATE: wait for running instances to shutdown");

                // wait until other instances of application have been closed
                var processName = Process.GetCurrentProcess().ProcessName;
                Process[] processByName = Process.GetProcessesByName(processName);
                while (processByName.Length > 1)
                {
                    // TODO: overall timeout / ask user to close other instances
                    var waitForProcess = processByName[0];
                    if (waitForProcess.Id == Process.GetCurrentProcess().Id)
                        waitForProcess = processByName[1];
                    try
                    {
                        //     log.Info("SOFTWARE UPDATE: waiting for process: " + waitForProcess.ProcessName + " [" + waitForProcess.Id + "]");
                        waitForProcess.WaitForExit(500);
                    }
                    catch {
                        //    log.Warn("SOFTWARE UPDATE: FAILED waiting for process: " + waitForProcess.ProcessName + " [" + waitForProcess.Id + "]");
                    }
                    processByName = Process.GetProcessesByName(processName);
                }

                // special handling for restart from debug instance, which uses other name
                // but do not block here or LC Panel won't run when Visual Studio is running with LC Panel project openend
                foreach (var waitForProcess in Process.GetProcessesByName(processName + ".vshost"))
                {
                    try
                    {
                        //     log.Info("SOFTWARE UPDATE: waiting for process: " + waitForProcess.ProcessName + " [" + waitForProcess.Id + "]");
                        waitForProcess.WaitForExit(5000);
                    }
                    catch {
                        //      log.Warn("SOFTWARE UPDATE: FAILED waiting for process: " + waitForProcess.ProcessName + " [" + waitForProcess.Id + "]");
                    }
                }
                return true;
            }
            catch {}
            return false;            
        }
        #endregion

        #region last updated / last checked
        public const string UNIVERSAL_DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private static DateTime lastChecked = new DateTime(0);
        private static DateTime lastUpdated = new DateTime(0);

        private static DateTime GetLastChecked()
        {
            if (lastChecked.Ticks == 0)
            {
                try
                {
                    var localVersionsFile = XDocument.Load(localClientVersionFile);
                    var timeString = localVersionsFile.Root.Element("lastChecked").Value;
                    lastChecked = DateTime.ParseExact(timeString, UNIVERSAL_DATE_TIME_FORMAT, null);
                }
                catch { }
            }
            Console.WriteLine("last checked: " + lastChecked);
            return lastChecked;
        }

        private static void SetLastChecked(string localClientVersionFile)
        {
            try
            {
                lastChecked = DateTime.UtcNow.ToLocalTime();
                String timeString = lastChecked.ToString(UNIVERSAL_DATE_TIME_FORMAT);

                var localVersionsFile = XDocument.Load(localClientVersionFile);
                localVersionsFile.Root.SetElementValue("lastChecked", timeString);
                localVersionsFile.Save(localClientVersionFile);

                Console.WriteLine("last checked: " + lastChecked);
            }
            catch { }
        }

        private static DateTime GetLastUpdated()
        {
            if (lastUpdated.Ticks == 0)
            {
                try
                {
                    var localVersionsFile = XDocument.Load(localClientVersionFile);
                    var timeString = localVersionsFile.Root.Element("lastUpdated").Value;
                    lastUpdated = DateTime.ParseExact(timeString, UNIVERSAL_DATE_TIME_FORMAT, null);
                }
                catch { }
            }
            Console.WriteLine("last updated: " + lastUpdated);
            return lastUpdated;
        }

        private static void SetLastUpdated(string localClientVersionFile)
        {
            try
            {
                DateTime dateTime = DateTime.UtcNow.ToLocalTime();
                String timeString = dateTime.ToString(UNIVERSAL_DATE_TIME_FORMAT);

                var localVersionsFile = XDocument.Load(localClientVersionFile);
                localVersionsFile.Root.SetElementValue("lastUpdated", timeString);
                localVersionsFile.Save(localClientVersionFile);

                Console.WriteLine("last checked: " + dateTime);
            }
            catch { }
        }
        #endregion

        #region Version File

        public static void CreateVersionFile()
        {
            if (File.Exists(localClientVersionFile))
                return;

            //   log.Info("SOFTWARE UPDATE: generate locale versions file");

            // create version file
            var checksums = new XElement("files");
            CreateVersionFile(checksums, AppDomain.CurrentDomain.BaseDirectory);

            // write version file
            var document = new XDocument();
            document.Add(new XElement("ApplicationUpdateInfo"));
            document.Root.Add(new XElement("revision", GetRevision()));
            document.Root.Add(new XElement("lastChecked"));
            document.Root.Add(new XElement("lastUpdated"));
            document.Root.Add(checksums);
            document.Save(localClientVersionFile);
        }

        private static void CreateVersionFile(XElement checksums, string path)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            // handle folders
            foreach (string dir in Directory.GetDirectories(path))
            {
                // ignore data and temp folders
                if (Path.GetFullPath(dir) == Path.GetFullPath(Path.Combine(basePath, WORKING_PATH)))
                    continue;
                if (Path.GetFullPath(dir) == Path.GetFullPath(Path.Combine(basePath, UPDATE_BACKUP_FOLDER)))
                    continue;
                if (Path.GetFullPath(dir) == Path.GetFullPath(Path.Combine(basePath, UPDATE_UPDATE_FOLDER)))
                    continue;

                // recursive file pasring
                CreateVersionFile(checksums, dir);
            }

            // handle files
            foreach (string filePath in Directory.GetFiles(path))
            {
                // ignore certain files in applicton folder
                if (Path.GetDirectoryName(filePath) == Path.GetDirectoryName(basePath))
                {
                    var fileName = Path.GetFileName(filePath);

                    // ignore log files (which might be locked)
                    if (fileName.StartsWith("log."))
                        continue;
                    // ignore version files
                    if (fileName == CLIENT_VERSION_FILE)
                        continue;
                    // ignore version files
                    if (fileName == SERVER_VERSION_FILE)
                        continue;
                    // ignore db files
                    if (fileName.EndsWith(".lpx"))
                        continue;
                    // ignore configuration files
                    if (fileName.EndsWith(".ini"))
                        continue;
                    // ignore "version.xml" writer program
                    if (fileName == VERSION_XML_WRITER)
                        continue;
                }

                // translate path relative to applicton folder
                var fileRelativePath = filePath.Replace(basePath, "");

                var size = (new FileInfo(filePath)).Length;
                var checksum = GetChecksum(filePath);

                // add file to version.xml
                checksums.Add(XElement.Parse("<file name=\"" + fileRelativePath + "\" checksum=\"" + checksum + "\" size=\"" + size + "\"/>"));
            }
        }

        private static string GetChecksum(string path)
        {
            // MD5-Hash
            System.IO.FileStream file = System.IO.File.OpenRead(path);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(file);
            file.Close();
            return BitConverter.ToString(md5Hash);
        }
        #endregion
    }
}
