using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

using Microsoft.Win32;

namespace LiveContext.Utility
{
    public class WindowsUtilities
    {
        public static string GetUserName()
        {
            string user;
            try
            {
                user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                // eliminate domain part
                int delimiterPos = user.LastIndexOf('\\');
                if (delimiterPos >= 0)
                    user = user.Substring(delimiterPos + 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in WindowsUtilities: " + ex);
                user = "user";
            }
            return user;
        }

        public static bool isWindows7
        {
            get
            {
                try
                {
                    var osVersion = System.Environment.OSVersion;
                    var windows7 = osVersion.Platform == PlatformID.Win32NT && osVersion.Version.Major == 6 && osVersion.Version.Minor == 1;
                    return windows7;
                }
                catch { }
                return false;
            }
        }

        public static bool isWord2010
        {
            get
            {
                try
                {
                    RegistryKey applicationKey = Registry.ClassesRoot.OpenSubKey("Word.Application");
                    RegistryKey versionKey = applicationKey.OpenSubKey("CurVer");
                    var currentVersion = versionKey.GetValue("");
                    return currentVersion.ToString().EndsWith(".14");
                }
                catch { }
                return false;
            }
        }

        public static bool isPowerPoint2010
        {
            get
            {
                try
                {
                    RegistryKey applicationKey = Registry.ClassesRoot.OpenSubKey("PowerPoint.Application");
                    RegistryKey versionKey = applicationKey.OpenSubKey("CurVer");
                    var currentVersion = versionKey.GetValue("");
                    return currentVersion.ToString().EndsWith(".14");
                }
                catch { }
                return false;
            }
        }

        public static bool isOutlook2010
        {
            get
            {
                try
                {
                    RegistryKey applicationKey = Registry.ClassesRoot.OpenSubKey("Outlook.Application");
                    RegistryKey versionKey = applicationKey.OpenSubKey("CurVer");
                    var currentVersion = versionKey.GetValue("");
                    return currentVersion.ToString().EndsWith(".14");
                }
                catch { }
                return false;
            }
        }

        public static bool isExcel2010
        {
            get
            {
                try
                {
                    RegistryKey applicationKey = Registry.ClassesRoot.OpenSubKey("Excel.Application");
                    RegistryKey versionKey = applicationKey.OpenSubKey("CurVer");
                    var currentVersion = versionKey.GetValue("");
                    return currentVersion.ToString().EndsWith(".14");
                }
                catch { }
                return false;
            }
        }

        #region Window styles

        [Flags]
        public enum ExtendedWindowStyles
        {
           // ...
           WS_EX_TOOLWINDOW = 0x00000080
           // ...
        }

        public enum GetWindowLongFields
        {
           // ...
           GWL_EXSTYLE = (-20),
           // ...
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
           int error = 0;
           IntPtr result = IntPtr.Zero;
           // Win32 SetWindowLong doesn't clear error on success
           SetLastError(0);

           if (IntPtr.Size == 4)
           {
              // use SetWindowLong
              Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
              error = Marshal.GetLastWin32Error();
              result = new IntPtr(tempResult);
           }
           else
           {
              // use SetWindowLongPtr
              result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
              error = Marshal.GetLastWin32Error();
           }

           if ((result == IntPtr.Zero) && (error != 0))
           {
              throw new System.ComponentModel.Win32Exception(error);
           }

           return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
           return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);

        #endregion

        public static void HideWindowFromAltTab(Window windowToHide)
        {
           try
           {
              WindowInteropHelper wndHelper = new WindowInteropHelper(windowToHide);

              int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

              exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
              SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
           }
           catch { }
        }
    }
}
