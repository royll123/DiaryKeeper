using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DiaryKeeper
{
    /**
     * DatePickerのカレンダー上の特定の日付の背景色を変更するコンバーター
     */
    class DateBGColorConverter : IValueConverter
    {
        /**
         * 日付に応じて背景色を取得する
         * @param value 日付データ
         */
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush br = Brushes.Transparent;

            if (value == null)
                return br;

            DateTime dt1;

            if (value.GetType().Equals(typeof(DateTime)))
            {
                dt1 = (DateTime)value;

            }
            else if (value.GetType().Equals(typeof(string)))
            {
                dt1 = DateTime.Parse((string)value);
            }
            else
            {
                return br;
            }

            try
            {

                var diary = Diary.checkExistFile(dt1);

                if (diary == true)
                {
                    br = new SolidColorBrush(Color.FromArgb(1, 254, 254, 254));
                    //br = Brushes.Gray;
                }

            }
            finally
            {
                // なにもしない
            }

            return br;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
