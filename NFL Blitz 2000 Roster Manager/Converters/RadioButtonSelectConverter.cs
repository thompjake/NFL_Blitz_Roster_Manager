using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace NFL_Blitz_2000_Roster_Manager.Converters
{
    public class RadioButtonSelectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
         object desiredValue, CultureInfo culture)
        {
            return Enum.Parse(
             value.GetType(),
             desiredValue.ToString(),
             true).Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object desiredValue, CultureInfo culture)
        {
            return (bool)value ? desiredValue : null;
        }
    }
}