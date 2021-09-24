using System;
using System.Globalization;
using Xamarin.Forms;

namespace Mobile.Code.Convertors
{
    public class AccessBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string LogUserId = value as string;
            if (App.LogUser.RoleName == "Admin")
            {
                return true;
            }
            else if (App.LogUser.Id.ToString() == LogUserId && App.LogUser.RoleName == "Mobile")
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //throw new NotImplementedException();
        }
    }
}
