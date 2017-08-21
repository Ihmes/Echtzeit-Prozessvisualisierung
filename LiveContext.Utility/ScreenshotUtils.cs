using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace LiveContext.Utility
{
    public class ScreenshotUtils
    {
        public static Bitmap GetScreenshotArea(Rectangle rectangle)
        {
            var bmpScreenshot = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(rectangle.X, rectangle.Y, 0, 0, rectangle.Size, CopyPixelOperation.SourceCopy);

            return bmpScreenshot;
        }

        public static Bitmap GetScreenshotArea(Rectangle rectangle, Rectangle rectangleMarker)
        {
            var bmpScreenshot = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(rectangle.X, rectangle.Y, 0, 0, rectangle.Size, CopyPixelOperation.SourceCopy);

            // gray background
            Brush brush = new SolidBrush(Color.FromArgb(30, 0x00, 0x00, 0x00));
            gfxScreenshot.ExcludeClip(rectangleMarker);
            gfxScreenshot.FillRectangle(brush, 0, 0, rectangle.Width, rectangle.Height);
            gfxScreenshot.ResetClip();

            // Marker
            Pen pen = new Pen(Color.FromArgb(255, 0x88, 0xA0, 0x63));
            pen.Width = 1;
            gfxScreenshot.DrawRectangle(pen, rectangleMarker);
            gfxScreenshot.DrawLine(pen, rectangleMarker.X, rectangleMarker.Y - 1, rectangleMarker.X + rectangleMarker.Width, rectangleMarker.Y - 1);
            gfxScreenshot.DrawLine(pen, rectangleMarker.X, rectangleMarker.Y + rectangleMarker.Height + 1, rectangleMarker.X + rectangleMarker.Width, rectangleMarker.Y + rectangleMarker.Height + 1);
            gfxScreenshot.DrawLine(pen, rectangleMarker.X - 1, rectangleMarker.Y, rectangleMarker.X - 1, rectangleMarker.Y + rectangleMarker.Height);
            gfxScreenshot.DrawLine(pen, rectangleMarker.X + rectangleMarker.Width + 1, rectangleMarker.Y, rectangleMarker.X + rectangleMarker.Width + 1, rectangleMarker.Y + rectangleMarker.Height);

            return bmpScreenshot;
        }

        public static Bitmap GetScreenshotDesktop()
        {
            // Set the bitmap object to the size of the screen
            // TODO: maybe this should be SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            return bmpScreenshot;
        }

        public static byte[] GetScreenshotAreaAsPngByteArray(Rectangle rectangle)
        {
            try
            {
                var bitmap = GetScreenshotArea(rectangle);
                return BitmapToPngByteArray(bitmap);
            }
            catch { return new byte[0]; }
        }

        // TODO: refactor to use rectangle instead of eight ints
        public static byte[] GetScreenshotAreaAsPngByteArray(Rectangle rectangle, Rectangle marker)
        {
            try
            {
                var bitmap = GetScreenshotArea(rectangle, marker);
                return BitmapToPngByteArray(bitmap);
            }
            catch { return new byte[0]; }
        }

        #region helper
        private static byte[] BitmapToPngByteArray(Bitmap bitmap)
        {
            // convert and store screenshot
            using (MemoryStream stream = new MemoryStream())
            {
                // convert to png
                bitmap.Save(stream, ImageFormat.Png);
                // reset position to read from beginning
                stream.Seek(0, SeekOrigin.Begin);
                // read png image to byte array
                byte[] imageData = ReadFully(stream, 0);
                return imageData;
            }
        }

        private static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    System.Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            System.Array.Copy(buffer, ret, read);
            return ret;
        }
        #endregion
    }
}