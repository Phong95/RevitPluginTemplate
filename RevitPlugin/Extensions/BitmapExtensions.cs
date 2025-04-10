using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RevitPlugin.Extensions
{
    public static class BitmapExtensions
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        public static BitmapSource GetBitmapSource(this Bitmap image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            //Intptr: A handle to the GDI bitmap object that this method creates.
            IntPtr hBitmap = image.GetHbitmap();
            try
            {
                //Returns a managed BitmapSource, based on the provided pointer to an unmanaged bitmap and palette information
                var bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                return bs;
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }
        public static BitmapImage ConvertBitmapToBitmapImage(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png); // Save bitmap to stream
                memory.Position = 0; // Reset stream position

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Freezes for UI thread safety

                return bitmapImage;
            }
        }
    }
}
