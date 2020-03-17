using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace comquiz
{
    public class SpoilAnswersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object returningValue = null;

            if (value != null && parameter != null)
            {
                bool isTheRightAnswer = (bool)value;

                if (parameter.ToString().Equals("Foreground"))
                {
                    if (isTheRightAnswer)
                    {
                        returningValue = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        returningValue = new SolidColorBrush(Colors.White);
                    }
                }
                else if (parameter.ToString().Equals("FontWeight"))
                {
                    if (isTheRightAnswer)
                    {
                        returningValue = FontWeight.Bold;
                    }
                    else
                    {
                        returningValue = FontWeight.Normal;
                    }
                }

                return returningValue;

            }

            return null;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // TODO: Too lazy to implement it
            return null;
        }
    }
}
