using System;
using System.Globalization;
using Xamarin.Forms;

namespace Mobile.Code.Convertors
{
    public class InvasiveCon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userID = (string)value;
            if (App.LogUser.RoleName == "Admin")
            {
                return true;
            }
            else if (userID == App.LogUser.Id.ToString())
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
