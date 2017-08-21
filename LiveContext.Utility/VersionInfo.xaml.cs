using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Resources;
using System.Reflection;
using ManagedWinapi.Hooks;


namespace LiveContext.Utility
{

    /// <summary>
    /// Interaction logic for VersionInfo.xaml
    /// </summary>
    public partial class VersionInfo : Window
    {
        private readonly static ResourceManager localization = new ResourceManager("LiveContext.Utility.Localization.LiveContextUtility",
                                                            Assembly.GetExecutingAssembly());

        private LowLevelMouseHook mouseHook;
        private bool useAsSplashScreen = false;

        public VersionInfo()
        {
            this.Focusable = false;
            this.ShowActivated = false;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            InitializeComponent();

            mouseHook = new LowLevelMouseHook();
            mouseHook.MouseIntercepted += new LowLevelMouseHook.MouseCallback(mouseHook_MouseIntercepted);
        }

        void mouseHook_MouseIntercepted(int msg, ManagedWinapi.Windows.POINT pt, int mouseData, int flags, int time, IntPtr dwExtraInfo, ref bool handled)
        {
            if (msg == 513 || msg == 516)
                Close();

            return;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
/*
            if (String.IsNullOrEmpty(UriInfoString))
                uriInfo.Visibility = Visibility.Collapsed;
            else
                uriInfo.Visibility = Visibility.Visible;

            if (useAsSplashScreen)
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(VersionInfo.SplashTimerEvent);
                RaiseEvent(newEventArgs);
            }
            else
            {
                mouseHook.StartHook();
            }
 */
        }

        public static string VersionInfoString
        {
            get
            {
                if (string.IsNullOrEmpty(localization.GetString("VersionInfo_Top"))) return "LIVECONTEXT";
                return string.Format(localization.GetString("VersionInfo_Top"), ApplicationUtilities.Version, "Rev." + ApplicationUtilities.GetRevision());
            }
        }

        public static string VersionInfoStringBase
        {
            get
            {
                if (string.IsNullOrEmpty(localization.GetString("VersionInfo_Bottom"))) return "© IMC AG | All rights reserved";
                return localization.GetString("VersionInfo_Bottom");
            }
        }
/*
        public string UriInfoString
        {

            get
            {
                if (ConfigFile.Instance.showUriInfo && !String.IsNullOrEmpty(ConfigFile.Instance.serverUri))
                    return ConfigFile.Instance.serverUri;
                else
                    return "";

            }
        }
*/
        public static RoutedEvent SplashTimerEvent;
        public event RoutedEventHandler SplashTimer
        {
            add
            {
                useAsSplashScreen = true;
                AddHandler(SplashTimerEvent, value);
            }
            remove
            {
                useAsSplashScreen = false;
                RemoveHandler(SplashTimerEvent, value);
            }
        }

        static VersionInfo()
        {
            SplashTimerEvent = EventManager.RegisterRoutedEvent("SplashTimer",
                RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VersionInfo));
        }

        protected override void OnClosed(EventArgs e)
        {
            if (mouseHook.Hooked)
                mouseHook.Unhook();
        }
    }
}
