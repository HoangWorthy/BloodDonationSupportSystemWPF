using System;
using System.Globalization;
using System.Windows.Data;

namespace Blood_Donation_Support_System_WPF.Converters
{
    public class RegistrationStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isRegistered)
            {
                return isRegistered ? "Đã đăng ký" : "Đăng ký";
            }
            return "Đăng ký";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFull)
            {
                return isFull ? "#F44336" : "#4CAF50"; // Red if full, Green if available
            }
            return "#4CAF50";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 