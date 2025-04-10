using RevitPlugin.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RevitPlugin.Helper.LastImageConverter
{
    public class LastImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> urls && urls.Count > 0 && urls.Last().Length>0)
            {
            
                return new BitmapImage(new Uri($"https://wsrv.nl/?url={urls.Last()}&w=400", UriKind.RelativeOrAbsolute)); // Get the last image URL
            }
            
            // Return default image from resources
            return Properties.Resources.google.ConvertBitmapToBitmapImage();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
