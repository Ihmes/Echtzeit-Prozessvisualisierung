using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using ManagedWinapi.Accessibility;

namespace Technologies
{
    public enum AccRoles
    {
        // ACC roles from oleacc.h
        RoleSystemAlert = 8,
        RoleSystemAnimation = 54,
        RoleSystemApplication = 14,
        RoleSystemBorder = 19,
        RoleSystemButtondropdown = 56,
        RoleSystemButtondropdowngrid = 58,
        RoleSystemButtonmenu = 57,
        RoleSystemCaret = 7,
        RoleSystemCell = 29,
        RoleSystemCharacter = 32,
        RoleSystemChart = 17,
        RoleSystemCheckbutton = 44,
        RoleSystemClient = 10,
        RoleSystemClock = 61,
        RoleSystemColumn = 27,
        RoleSystemColumnheader = 25,
        RoleSystemCombobox = 46,
        RoleSystemCursor = 6,
        RoleSystemDiagram = 53,
        RoleSystemDial = 49,
        RoleSystemDialog = 18,
        RoleSystemDocument = 15,
        RoleSystemDroplist = 47,
        RoleSystemEquation = 55,
        RoleSystemGraphic = 40,
        RoleSystemGrip = 4,
        RoleSystemGrouping = 20,
        RoleSystemHelpballoon = 31,
        RoleSystemHotkeyfield = 50,
        RoleSystemIndicator = 39,
        RoleSystemLink = 30,
        RoleSystemList = 33,
        RoleSystemListitem = 34,
        RoleSystemMenubar = 2,
        RoleSystemMenuitem = 12,
        RoleSystemMenupopup = 11,
        RoleSystemOutline = 35,
        RoleSystemOutlineitem = 36,
        RoleSystemPagetab = 37,
        RoleSystemPagetablist = 60,
        RoleSystemPane = 16,
        RoleSystemProgressbar = 48,
        RoleSystemPropertypage = 38,
        RoleSystemPushbutton = 43,
        RoleSystemRadiobutton = 45,
        RoleSystemRow = 28,
        RoleSystemRowheader = 26,
        RoleSystemScrollbar = 3,
        RoleSystemSeparator = 21,
        RoleSystemSlider = 51,
        RoleSystemSound = 5,
        RoleSystemSpinbutton = 52,
        RoleSystemSplitbutton = 62,
        RoleSystemStatictext = 41,
        RoleSystemStatusbar = 23,
        RoleSystemTable = 24,
        RoleSystemText = 42,
        RoleSystemTitlebar = 1,
        RoleSystemToolbar = 22,
        RoleSystemTooltip = 13,
        RoleSystemWhitespace = 59,
        RoleSystemWindow = 9
    }

    public enum SapRoles
    {
        GuiUnknown = -1,
        GuiComponent = 0,
        GuiVComponent = 1,
        GuiVContainer = 2,
        GuiApplication = 10,
        GuiConnection = 11,
        GuiSession = 12,
        GuiFrameWindow = 20,
        GuiMainWindow = 21,
        GuiModalWindow = 22,
        GuiMessageWindow = 23,
        GuiLabel = 30,
        GuiTextField = 31,
        GuiCTextField = 32,
        GuiPasswordField = 33,
        GuiComboBox = 34,
        GuiOkCodeField = 35,
        GuiButton = 40,
        GuiRadioButton = 41,
        GuiCheckBox = 42,
        GuiStatusPane = 43,
        GuiCustomControl = 50,
        GuiContainerShell = 51,
        GuiBox = 62,
        GuiContainer = 70,
        GuiSimpleContainer = 71,
        GuiScrollContainer = 72,
        GuiListContainer = 73,
        GuiUserArea = 74,
        GuiSplitterContainer = 75,
        GuiTableControl = 80,
        GuiTableColumn = 81,
        GuiTableRow = 82,
        GuiTabStrip = 90,
        GuiTab = 91,
        GuiScrollbar = 100,
        GuiToolbar = 101,
        GuiTitlebar = 102,
        GuiStatusbar = 103,
        GuiMenu = 110,
        GuiMenubar = 111,
        GuiCollection = 120,
        GuiSessionInfo = 121,
        GuiShell = 122,
        GuiGOSShell = 123,
        GuiSplitterShell = 124,
        GuiDialogShell = 125,
        GuiDockShell = 126,
        GuiContextMenu = 127,
        GuiComponentCollection = 128,
    }

    public class TechnologyUtilities
    {
        static Dictionary<string, string> applicationNames = new Dictionary<string, string>() 
            { 
                { "iexplore",   "Internet Explorer" },
                { "firefox",    "Firefox"           },
                { "chrome",     "Google Chrome"     },
                { "explorer",   "Windows Explorer"  },
                { "WINWORD",    "Microsoft Word"    },
                { "EXCEL",      "Microsoft Excel"   },
                { "POWERPNT",   "Microsoft PowerPoint" },
                { "OUTLOOK",    "Microsoft Outlook" },
                { "ONENOTE",    "Microsoft OneNote" },
                { "WINPROJ",    "Microsoft Project" },
                { "wmplayer",   "Windows Media Player" },
                { "frontpage",  "Microsoft FrontPage" },
                { "msimn",      "Microsoft Outlook Express" },
                { "devenv",     "Microsoft Visual Studio" },
                { "saplogon",   "SAP"               },
                { "saplgpad",   "SAP"               },
                { "LiveContext.Designer.GUI",    "LIVECONTEXT Designer" },
                { "AcroRd32",   "Adobe Reader"      },
                { "java",       "Java"              },
                { "javaw",      "Java"              },
                { "soffice",    "StarOffice"        },
                { "soffice.bin","OpenOffice"        }
            };

        public static string getApplicationNameByProcessName(string processName)
        {
            if (applicationNames.ContainsKey(processName))
                return applicationNames[processName];
            return processName;
        }
        #region pattern matching for application, window, url, etc.

        public static bool checkApplicationPattern(string applicationName, string _pattern)
        {
            if (applicationName == null || _pattern == null) return false;

            var patterns = _pattern.Split(new char[] { ',', '|' });
            foreach (var pattern in patterns)
                if (string.Equals(applicationName, pattern, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }
        public static bool checkWindowPattern(string windowTitle, string windowTitleToCheck)
        {
            return checkPattern(windowTitle, windowTitleToCheck);
        }
        public static bool checkWindowPatternAndURL(string windowTitle, string windowTitleToCheck, string windowUrl, string windowUrlToCheck)
        {
            // check window title
            bool match = checkPattern(windowTitle, windowTitleToCheck);

            // check URL (if specified and supported)
            if (match && !string.IsNullOrEmpty(windowUrlToCheck))
                match = checkPattern(windowUrl, windowUrlToCheck);

            return match;
        }
        private static bool checkPattern(string _searchIn, string _pattern)
        {
            if (_searchIn == null || _pattern == null) return false;

            if (_searchIn == _pattern) return true;

            try
            {
                // try to match substring first
                if (_searchIn.ToLower().Contains(_pattern.ToLower())) return true;
                // try to match as regular expression
                Regex regex = new Regex(_pattern, RegexOptions.IgnoreCase);
                return regex.IsMatch(_searchIn);
            }
            // Return false if there is a problem with the _pattern itself
            catch { /* Argumentexception may occur because string typically is not intended as regular expression, e.g. window title with special characters */ }
            return false;
        }

        private static Regex regex = new Regex(@"(?>^(.*)(( - Mozilla Firefox.*)+|( - Windows Internet Explorer.*)+|( - Google Chrome.*)+)$)");
        public static string getWindowTitleToCheck(string application, string windowTitle)
        {
            // set title as default pattern
            string windowTitleToCheck = windowTitle;

            // remove browser specific postfix (if available)
            var match = regex.Match(windowTitle);
            if (match.Groups.Count > 2)
            {
                var matchedTitle = match.Groups[1].ToString();
                var matchedPostfix = match.Groups[2].ToString();
                var matchingBrowser = matchedPostfix.ToLower().Contains(application.ToLower()) || (application.ToLower().Equals("iexplore") && matchedPostfix.ToLower().Contains("windows internet explorer"));

                if (!string.IsNullOrEmpty(matchedTitle) && !string.IsNullOrEmpty(matchedPostfix) && matchingBrowser)
                    windowTitleToCheck = matchedTitle;
            }

            return windowTitleToCheck;
        }

        #endregion

        #region browser and web application related

        public static bool isUrlSupported(string processPatternOrName)
        {
            return TechnologyUtilities.checkApplicationPattern("iexplore", processPatternOrName)
                || TechnologyUtilities.checkApplicationPattern("firefox", processPatternOrName)
                || TechnologyUtilities.checkApplicationPattern("chrome", processPatternOrName);
        }

        public static bool isSupportingAllBrowsers(string processPatternOrName)
        {
            return TechnologyUtilities.checkApplicationPattern("iexplore", processPatternOrName)
                && TechnologyUtilities.checkApplicationPattern("firefox", processPatternOrName)
                && TechnologyUtilities.checkApplicationPattern("chrome", processPatternOrName);
        }
        public static string extendPatternSupportingAllBrowsers(string processPatternOrName)
        {
            if (!TechnologyUtilities.checkApplicationPattern("iexplore", processPatternOrName))
                processPatternOrName += ",iexplore";
            if (!TechnologyUtilities.checkApplicationPattern("firefox", processPatternOrName))
                processPatternOrName += ",firefox";
            if (!TechnologyUtilities.checkApplicationPattern("chrome", processPatternOrName))
                processPatternOrName += ",chrome";
            return processPatternOrName;
        }
        public static string removePatternSupportingAllBrowsers(string processPatternOrName)
        {
            if (processPatternOrName.StartsWith("iexplore"))
                processPatternOrName = "iexplore";
            else if (processPatternOrName.StartsWith("firefox"))
                processPatternOrName = "firefox";
            else if (processPatternOrName.StartsWith("chrome"))
                processPatternOrName = "chrome";
            return processPatternOrName;
        }

        #endregion

        #region SAP related

        public static bool isSapApplication(string processPatternOrName)
        {
            // saplogon.exe : full SAP client for unrestricted logon and possibility to edit server connection
            // saplgpad.exe : read-only SAP client to connect to pre-configured servers only
            return TechnologyUtilities.checkApplicationPattern("saplogon", processPatternOrName)
                || TechnologyUtilities.checkApplicationPattern("saplgpad", processPatternOrName);
        }

        public static string extendPatternSupportingAllSapClients(string processPatternOrName)
        {
            processPatternOrName = addProcessToPattern("saplogon", processPatternOrName);
            processPatternOrName = addProcessToPattern("saplgpad", processPatternOrName);
            return processPatternOrName;
        }

        #endregion

        #region Java related

        public static bool isJavaApplication(string processPatternOrName)
        {
            return TechnologyUtilities.checkApplicationPattern("java", processPatternOrName)
                || TechnologyUtilities.checkApplicationPattern("javaw", processPatternOrName);
        }

        public static string extendPatternSupportingAllJavaClients(string processPatternOrName)
        {
            processPatternOrName = addProcessToPattern("java", processPatternOrName);
            processPatternOrName = addProcessToPattern("javaw", processPatternOrName);
            return processPatternOrName;
        }

        #endregion

        public static string removeProcessFromPattern(string processName, string pattern)
        {
            if (TechnologyUtilities.checkApplicationPattern(processName, pattern))
            {
                var processes = pattern.Split(new char[] { ',', '|' });
                var newPattern = "";
                foreach (var process in processes)
                {
                    if (!TechnologyUtilities.checkApplicationPattern(processName, process))
                    {
                        if (newPattern.Length > 0)
                            newPattern += ",";
                        newPattern += process;
                    }
                }
                pattern = newPattern;
            }
            return pattern;
        }

        public static string addProcessToPattern(string processName, string pattern)
        {
            if (!TechnologyUtilities.checkApplicationPattern(processName, pattern))
            {
                if (string.IsNullOrEmpty(pattern))
                    pattern = processName;
                else
                    pattern += "," + processName;
            }
            return pattern;
        }

        #region debug helper

        public static void outputSaoWithChildren(SystemAccessibleObject sao)
        {
            outputSaoWithChildren(sao, "[0]");
        }
        public static void outputSaoWithChildren(SystemAccessibleObject sao, string indention)
        {
            try
            {
                if (sao == null)
                    return;

                if (!sao.Visible)
                    return;

                Console.WriteLine(indention + " SAO: " + sao + "[" + sao.RoleIndex + "] / " + sao.StateString);

                //Console.WriteLine(indention + " Name: " + sao.Name);
                //Console.WriteLine(indention + " Role: " + sao.RoleIndex + " / " + sao.RoleString);
                //Console.WriteLine(indention + " @" + sao.Location);
                if (!string.IsNullOrEmpty(sao.Value))
                    Console.WriteLine(indention + " Value: " + sao.Value);


                int nr = 0;
                foreach (var child in sao.Children)
                    outputSaoWithChildren(child, indention + "[" + nr++ + "]");
            }
            catch { }
        }

        #endregion

        // used for research only
        public static void createRolesTable()
        {
            string[] roles = { 
                                /* Abstract Roles*/ 
                                "command",      "20", "-1", "-1",
                                "composite",    "20", "-1", "-1",
                                "input",        "20", "-1", "-1",
                                "landmark",     "20", "-1", "-1",
                                "range",        "20", "-1", "51", 
                                "roletype",     "20", "-1", "-1",
                                "section",      "20", "-1", "-1",
                                "sectionhead",  "20", "-1", "-1",
                                "select",       "20", "-1", "-1",
                                "structure",    "20", "-1", "-1",
                                "widget",       "20", "-1", "-1",
                                "window",       "20", "-1", "-1",
                                /* Widget Roles */
                                "alert",        "8",  "8",  "8", 
                                "alertdialog",  "18", "18", "18", 
                                "button",       "43", "43", "43", 
                                "checkbox",     "44", "44", "44", 
                                "dialog",       "18", "18", "18", 
                                "gridcell",     "29", "29", "29", 
                                "link",         "30", "30", "30", 
                                "log",          "-1", "-1", "10", 
                                "marquee",      "54", "54", "10", 
                                "menuitem",     "12", "12", "12", 
                                "menuitemcheckbox","12","12","12", 
                                "menuitemradio","12", "12", "12", 
                                "option",       "34", "34", "34", 
                                "progressbar",  "48", "48", "48", 
                                "radio",        "45", "45", "45", 
                                "scrollbar",    "3",  "3",  "3", 
                                "slider",       "51", "51", "51", 
                                "spinbutton",   "52", "52", "48", 
                                "status",       "23", "23", "23", 
                                "tab",          "37", "37", "37", 
                                "tabpanel",     "16", "38", "38", 
                                "textbox",      "42", "42", "42", 
                                "timer",        "20", "-1", "61", 
                                "tooltip",      "13", "13", "13", 
                                "treeitem",     "36", "36", "36",
                                /* Composite Widget Roles */
                                "combobox",     "46", "46", "46", 
                                "grid",         "24", "24", "24", 
                                "listbox",      "33", "33", "33", 
                                "menu",         "11", "11", "11", 
                                "menubar",      "2",  "2",  "2", 
                                "radiogroup",   "20", "20", "20", 
                                "tablist",      "60", "60", "60", 
                                "tree",         "35", "35", "35", 
                                "treegrid",     "20", "35", "24",
                                /* Document Structure Roles */
                                "article",      "20", "15", "20", 
                                "columnheader", "25", "25", "25", 
                                "definition",   "20", "-1", "-1", 
                                "directory",    "33", "33", "33", 
                                "document",     "10", "15", "15", 
                                "group",        "20", "20", "20", 
                                "heading",      "42", "-1", "-1", 
                                "img",          "40", "40", "40", 
                                "list",         "33", "33", "33", 
                                "listitem",     "34", "34", "34",
                                "math",         "30", "55", "55", 
                                "note",         "20", "-1", "20", 
                                "presentation", "29", "29", "29", 
                                "region",       "16", "20", "20", 
                                "row",          "20", "28", "28", 
                                "rowheader",    "26", "26", "26", 
                                "separator",    "21", "21", "21", 
                                "toolbar",      "22", "22", "22",
                                /* Landmark Roles */
                                "application",  "16", "14", "20", 
                                "banner",       "20", "-1", "20", 
                                "complementary","20", "-1", "20", 
                                "contentinfo",  "20", "-1", "20", 
                                "form",         "20", "-1", "-1", 
                                "main",         "20", "-1", "20", 
                                "navigation",   "20", "-1", "20", 
                                "search",       "20", "-1", "20"
                             };

            StringBuilder output = new StringBuilder();
            output.AppendLine(@"<html>
<head>
<title>HTML Roles - ACC Roles</title>
</head>
  <body>
    <h1>HTML Roles - Accessibility Roles</h1>
    List of roles as defined by <a href=""http://www.w3.org/TR/wai-aria/roles"">http://www.w3.org/TR/wai-aria/roles</a>
    and their accessibility interpretation of the various browsers, which is used by LIVECONTEXT.<br>
    <br>
    Browser versions used for testing (but other versions will very likely return the same results):<br>
    <ul>
      <li>Internet Explorer 9 (9.0.8112.16421 64bit)</li>
      <li>Mozilla Firefox 14 (14.0.1)</li>
      <li>Google Chrome (20.0.1132.57 m)</li>
    </ul>
    Roles that are inconsistent between browsers should not be used.<br><br>

    <table border=1>
      <tr>
        <th>HTML Role</th>
        <th>ACC Role in Internet Explorer</th>
        <th>ACC Role in Firefox</th>
        <th>ACC Role in Chrome</th>
        <th></th>
      </tr>");

            for (int i = 0; i < roles.Length; )
            {
                if (i == 0)
                    output.AppendLine(@"        <tr><td colspan=4 align=center><strong>Abstract Roles (must not be used)</strong></td><tr>");
                if (i == 12 * 4)
                    output.AppendLine(@"        <tr><td colspan=4 align=center><strong>Widget Roles</strong></td><tr>");
                if (i == 37 * 4)
                    output.AppendLine(@"        <tr><td colspan=4 align=center><strong>Composite Widget Roles</strong></td><tr>");
                if (i == 46 * 4)
                    output.AppendLine(@"        <tr><td colspan=4 align=center><strong>Document Structure Roles</strong></td><tr>");

                // style=""border:1px solid; padding:5px;"" 
                output.AppendLine(@"      <tr>");
                output.AppendLine(@"        <td><div role=""" + roles[i] + @""">role=""" + roles[i++] + @"""</div></td>");

                var ie = roles[i++];
                var ff = roles[i++];
                var chrome = roles[i++];
                bool same = ie == ff && ff == chrome;

                output.AppendLine(@"        <td>" + extendRoleString(ie) + @"</td>");
                output.AppendLine(@"        <td>" + extendRoleString(ff) + @"</td>");
                output.AppendLine(@"        <td>" + extendRoleString(chrome) + @"</td>");

                output.AppendLine(@"        <td>" + (same ? "" : "inconsistent") + @"</td>");

                output.AppendLine(@"      </td></tr>");
            }

            output.AppendLine(@"</table>
  </body>
</html>");
            var file = @"C:\Users\ziewer\Desktop\RolesTest.html";
            File.WriteAllText(file, output.ToString());

        }

        private static string extendRoleString(string role)
        {
            int nr;
            try
            {
                if (int.TryParse(role, out nr) && nr != -1)
                    role += " = " + ((AccRoles)nr).ToString().Replace("RoleSystem", "");
            }
            catch { }
            return role;
        }
    }
}
