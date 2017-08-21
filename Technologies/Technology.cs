using System;
using System.Collections.Generic;
using System.Drawing;
using ManagedWinapi.Accessibility;
using System.Text;
using System.Runtime.InteropServices;
using Accessibility;
using Point = System.Windows.Point;
using SystemInformation = System.Windows.Forms.SystemInformation;
using LiveContext.Configuration;
using LiveContext.Utility;

namespace Technologies
{
    public class Technology : ITechnology
    {
        protected SystemAccessibleObject sequentialSao;


        public override String GetNameByPosition(Point point)
        {
            string name = NO_NAME_FOUND;

            try
            {
                SystemAccessibleObject sao;
                if (sequentialSao != null) sao = sequentialSao;
                else sao = SystemAccessibleObject.FromPoint((int)point.X, (int)point.Y);
                    
                name = sao.Name;
            }
            catch { }
           
            if (String.IsNullOrEmpty(name)) name = NO_NAME_FOUND;

            // BUGFIX 6937: remove line breaks and tab from name
            name = name.Replace("\r\n", " ");
            name = name.Replace('\n', ' ');
            name = name.Replace('\r', ' ');
            name = name.Replace('\t', ' ');

            return name.Trim();
        }

        public override int GetTypeByPosition(Point point)
        {
            try
            {
                SystemAccessibleObject sao;
                if (sequentialSao != null) sao = sequentialSao;
                else sao = SystemAccessibleObject.FromPoint((int)point.X, (int)point.Y);
                       
                return sao.RoleIndex;
            }
            catch { }

            return TYPE_UNKNOWN;
        }

        public override String GetProcessNameByPosition(Point point)
        {
            string processName = ERROR_IDENTIFICATION_FAILED;
            try
            {
                SystemAccessibleObject sao;
                if (sequentialSao != null) sao = sequentialSao;
                else sao = SystemAccessibleObject.FromPoint((int)point.X, (int)point.Y);
                processName = sao.Window.Process.ProcessName.ToString();
#if DEBUG
                
#endif
            }
            catch (Exception) { }
            return processName;
        }

        public override String GetWindowNameByPosition(Point point)
        {
            string windowName = ERROR_IDENTIFICATION_FAILED;
            try
            {
                SystemAccessibleObject sao;
                if (sequentialSao != null) sao = sequentialSao;
                else sao = SystemAccessibleObject.FromPoint((int)point.X, (int)point.Y);
                IntPtr HWnd;
                windowName = getWindowTitle(sao, out HWnd);
            }
            catch (Exception) { }
            return windowName;
        }



        public static String getWindowTitle(SystemAccessibleObject sao, out IntPtr HWnd)
        {
            String name = ERROR_IDENTIFICATION_FAILED;
            HWnd = (IntPtr)0;
            try
            {
                var window = sao.Window;
                // search for the window that has no parent
                while (window.ParentSymmetric != null)
                {
                    window = window.ParentSymmetric;
                }

                name = window.Title.ToString();
                HWnd = window.HWnd;

                // TODO: maybe extract URL here, too
                //var processName = sao.Window.Process.ProcessName.ToString().ToLower();
                //string url = getUrl(window.HWnd, processName);

                // BUGFIX 6937: remove line breaks and tab from name
                name = name.Replace("\r\n", " ");
                name = name.Replace('\n', ' ');
                name = name.Replace('\r', ' ');
                name = name.Replace('\t', ' ');
            }
            catch (Exception ex)
            {
                Console.WriteLine("getWindowTitle: " + ex);
            }
            return name;
        }

       
        
    }
}