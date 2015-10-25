using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DiaryKeeper
{
    class DateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color written_color = Brushes.Blue.Color;
            Color none_color = Brushes.Gray.Color;

            SolidColorBrush bgcolor = (SolidColorBrush)value;
            Color wbgcolor = Color.FromArgb(1, 254, 254, 254);

            if (bgcolor == null)
                return none_color;

            Color color = none_color;
            try
            {
                if (bgcolor.Color.Equals(wbgcolor))
                {
                    color = written_color;
                }


            }
            finally
            {
                // なにもしない
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
