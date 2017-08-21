using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace System.Windows
{   
    /// <summary>
    /// needed to throw custom RoutedEventArgs
    /// </summary>
    /// <typeparam name="TArgs">your custom MyCustomEventArgs class</typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RoutedEventHandler<TArgs>(object sender, TArgs e) where TArgs : RoutedEventArgs;
}

namespace LiveContext.Utility
{
    public class BaseUtils
    {
        public static int MouseX { get { return System.Windows.Forms.Control.MousePosition.X; } }
        public static int MouseY { get { return System.Windows.Forms.Control.MousePosition.Y; } }

        public static System.Windows.Point MousePosition
        {
            get
            {
                return new System.Windows.Point(System.Windows.Forms.Control.MousePosition.X,
                                                System.Windows.Forms.Control.MousePosition.Y);
            }
        }

        /**
         * static method to sanitize a string used in e.g. an acc-id
         */
        public static string sanitizeString(string _in)
        {
            return Regex.Replace(_in, "[^A-Za-z0-9-]", "").ToLower();
        }

        public static string sanitizeStringForFilesystem(string _in)
        {
            _in = _in.Replace(" ", "_");
            return Regex.Replace(_in, "[^A-Za-z0-9-_]", "").ToLower();
        }

        public static System.Windows.Media.Color HexColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }

        public static void CopyFolder(string _sourceFolder, string _destFolder, bool _recursive)
        {
            if (!Directory.Exists(_destFolder))
                Directory.CreateDirectory(_destFolder);
            string[] files = Directory.GetFiles(_sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(_destFolder, name);
                File.Copy(file, dest, true);
            }
            if (_recursive)
            {
                string[] folders = Directory.GetDirectories(_sourceFolder);

                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Path.Combine(_destFolder, name);
                    CopyFolder(folder, dest, _recursive);
                }
            }
        }

        public static String UNIX_TIME_SECONDS
        {
            get
            {
                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                return ((int) t.TotalSeconds).ToString();
            }
        }

        public static String UNIX_TIME_MILLISECONDS
        {
            get
            {
                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                return ((long)t.TotalMilliseconds).ToString();
            }
        }

        public static DateTime ConvertToDateTimeLocal(string dateTimeString)
        {
            DateTime dateTime = new DateTime();
            try
            {
                // try to parse language independent format used since v3.5.1
                dateTime = DateTime.ParseExact(dateTimeString, ApplicationUtilities.UNIVERSAL_DATE_TIME_FORMAT, null);
            }
            catch
            {
                // handle date strings of recordings that have been created before v3.5.1
                try
                {
                    // try to parse German
                    var culture = System.Globalization.CultureInfo.CreateSpecificCulture("de");
                    dateTime = DateTime.ParseExact(dateTimeString, "dd.MM.yyyy HH:mm:ss", culture);
                }
                catch
                {
                    try
                    {
                        // try to parse English
                        var culture = System.Globalization.CultureInfo.CreateSpecificCulture("en");
                        dateTime = DateTime.ParseExact(dateTimeString, "M/d/yyyy h:mm:ss tt", culture);
                    }
                    catch { }
                }
            }
            return dateTime;
        }
        
        public static void GetScaleFactors(out double dpiX, out double dpiY)
        {
            dpiX = 1.0;
            dpiY = 1.0;
            try
            {
                System.Windows.Media.Matrix conversionMatrix = new System.Windows.Media.Matrix();
                var source = new HwndSource(new HwndSourceParameters());

                conversionMatrix = source.CompositionTarget.TransformFromDevice;
                if (conversionMatrix.M11 != 0)
                    dpiX = conversionMatrix.M11;
                if (conversionMatrix.M22 != 0)
                    dpiY = conversionMatrix.M22;
            }
            catch (Exception exc)
            {
                dpiX = 1.0;
                dpiY = 1.0;
                System.Console.Write("BaseUtils.UpdateScaleFactor(): " + exc.ToString());
            }
        }


        public static Rectangle CalculateWindowPosOnAvailableScreen(Rectangle origRect)
        {
            Rectangle screenRect = System.Windows.Forms.SystemInformation.VirtualScreen;
            Rectangle origRectInScreenCoord = origRect;
            
            System.Windows.Media.Matrix conversionMatrix = new System.Windows.Media.Matrix();
            bool matrixInitialized = false;
            try
            {
                if (System.Windows.Application.Current.MainWindow != null)
                {
                    conversionMatrix = PresentationSource.FromVisual(System.Windows.Application.Current.MainWindow).CompositionTarget.TransformFromDevice;
                    if (conversionMatrix.M11 != 0 && conversionMatrix.M22 != 0)
                        matrixInitialized = true;
                }
            }
            catch 
            {
                matrixInitialized = false;
            }

            if (matrixInitialized)
            {
                origRectInScreenCoord.X = (int)((double)origRect.X / conversionMatrix.M11);
                origRectInScreenCoord.Y = (int)((double)origRect.Y / conversionMatrix.M22);
                origRectInScreenCoord.Width = (int)((double)origRect.Width / conversionMatrix.M11);
                origRectInScreenCoord.Height = (int)((double)origRect.Height / conversionMatrix.M22);
            }

            if (screenRect.Contains(origRectInScreenCoord))
            {
                return origRect;
            }
            else
            {
                Rectangle newRect = new Rectangle(origRectInScreenCoord.Left, origRectInScreenCoord.Top, origRectInScreenCoord.Width, origRectInScreenCoord.Height);
                newRect.Width = (origRectInScreenCoord.Width < screenRect.Width) ? (int)origRectInScreenCoord.Width : (int)2 * screenRect.Width / 3;
                newRect.Height = (origRectInScreenCoord.Height < screenRect.Height) ? (int)origRectInScreenCoord.Height : (int)2 * screenRect.Height / 3;
                newRect.X = (int)(screenRect.X + (screenRect.Width - newRect.Width) / 2);
                newRect.Y = (int)(screenRect.Y + (screenRect.Height - newRect.Height) / 2);

                if (matrixInitialized)
                {
                    newRect.X = (int)(newRect.X * conversionMatrix.M11);
                    newRect.Y = (int)(newRect.Y * conversionMatrix.M22);
                    newRect.Width = (int)((double)newRect.Width * conversionMatrix.M11);
                    newRect.Height = (int)((double)newRect.Height * conversionMatrix.M22);
                }
                
                return newRect;
            }
        }

        public static Rect GetRightScreenBounds(Visual visual)
        {
           Rect returnRect = new Rect();

           if (Screen.AllScreens.Length > 0)
           {
              returnRect.X = Screen.AllScreens[0].Bounds.X;
              returnRect.Y = Screen.AllScreens[0].Bounds.Y;
              returnRect.Width = Screen.AllScreens[0].Bounds.Width;
              returnRect.Height = Screen.AllScreens[0].Bounds.Height;

              for (int i = 1; i < Screen.AllScreens.Length; ++i)
              {
                 if (Screen.AllScreens[i].Bounds.X > returnRect.X)
                 {
                    returnRect.X = Screen.AllScreens[i].Bounds.X;
                    returnRect.Y = Screen.AllScreens[i].Bounds.Y;
                    returnRect.Width = Screen.AllScreens[i].Bounds.Width;
                    returnRect.Height = Screen.AllScreens[i].Bounds.Height;
                 }
              }           
           }

           double dpiX = 1.0;
           double dpiY = 1.0;
           try
           {
              System.Windows.Media.Matrix conversionMatrix = new System.Windows.Media.Matrix();
              conversionMatrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
              if (conversionMatrix.M11 != 0)
                 dpiX = conversionMatrix.M11;
              if (conversionMatrix.M22 != 0)
                 dpiY = conversionMatrix.M22;
           }
           catch { }

           returnRect.X = returnRect.X * dpiX;
           returnRect.Y = returnRect.Y * dpiY;
           returnRect.Width = returnRect.Width * dpiX;
           returnRect.Height = returnRect.Height * dpiY;

           return returnRect;
        }

        public static Rect GetLeftScreenBounds(Visual visual)
        {
           Rect returnRect = new Rect(0, 0, 0, 0);

           if (Screen.AllScreens.Length > 0)
           {
              returnRect.X = Screen.AllScreens[0].Bounds.X;
              returnRect.Y = Screen.AllScreens[0].Bounds.Y;
              returnRect.Width = Screen.AllScreens[0].Bounds.Width;
              returnRect.Height = Screen.AllScreens[0].Bounds.Height;

              for (int i = 1; i < Screen.AllScreens.Length; ++i)
              {
                 if (Screen.AllScreens[i].Bounds.X < returnRect.X)
                 {
                    returnRect.X = Screen.AllScreens[i].Bounds.X;
                    returnRect.Y = Screen.AllScreens[i].Bounds.Y;
                    returnRect.Width = Screen.AllScreens[i].Bounds.Width;
                    returnRect.Height = Screen.AllScreens[i].Bounds.Height;
                 }
              }
           }

           double dpiX = 1.0;
           double dpiY = 1.0;
           try
           {
              System.Windows.Media.Matrix conversionMatrix = new System.Windows.Media.Matrix();
              conversionMatrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
              if (conversionMatrix.M11 != 0)
                 dpiX = conversionMatrix.M11;
              if (conversionMatrix.M22 != 0)
                 dpiY = conversionMatrix.M22;
           }
           catch { }

           returnRect.X = returnRect.X * dpiX;
           returnRect.Y = returnRect.Y * dpiY;
           returnRect.Width = returnRect.Width * dpiX;
           returnRect.Height = returnRect.Height * dpiY;

           return returnRect;
        }

        public static Rect GetVirtualScreenBounds()
        {
           Rect virtualScreenBound = new Rect(0, 0, 0, 0);

           try
           {
              virtualScreenBound.X = System.Windows.SystemParameters.VirtualScreenLeft;
              virtualScreenBound.Y = System.Windows.SystemParameters.VirtualScreenTop;
              virtualScreenBound.Width = System.Windows.SystemParameters.VirtualScreenWidth;
              virtualScreenBound.Height = System.Windows.SystemParameters.VirtualScreenHeight;
           }
           catch { 
           }

           return virtualScreenBound;
        }

        public static Rect GetWorkingAreaBounds()
        {
           Rect virtualScreenBound = new Rect(0, 0, 0, 0);

           try
           {
              virtualScreenBound.X = System.Windows.SystemParameters.WorkArea.Left;
              virtualScreenBound.Y = System.Windows.SystemParameters.WorkArea.Top;
              virtualScreenBound.Width = System.Windows.SystemParameters.WorkArea.Width;
              virtualScreenBound.Height = System.Windows.SystemParameters.WorkArea.Height;
           }
           catch
           {
           }

           return virtualScreenBound;
        }

        public static Rect GetPrimaryScreenBounds()
        {
           Rect primaryScreenBounds = new Rect(0, 0, 0, 0);

           try
           {
              primaryScreenBounds.X = 0;
              primaryScreenBounds.Y = 0;
              primaryScreenBounds.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
              primaryScreenBounds.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
           }
           catch { }

           return primaryScreenBounds;
        }
    }
}