using System;
using System.Globalization;
using Xamarin.Forms;

namespace Mobile.Code.Convertors
{
    public class ImageButtonInvasive : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userID = (string)value;
            if (App.IsInvasive)
            {
                return false;
            }
            else
            {
                return true;
            }



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //throw new NotImplementedException();
        }
    }
}
