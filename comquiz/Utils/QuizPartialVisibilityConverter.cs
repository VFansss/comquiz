using Avalonia.Data;
using Avalonia.Data.Converters;
using System;

namespace comquiz
{
    public class QuizPartialVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool visibility = false;

            if (value != null && value is QUIZPART)
            {
                QUIZPART quizPart = (QUIZPART)value;
                QUIZPARTIAL quizPartial = (QUIZPARTIAL)parameter;

                if (quizPart == QUIZPART.Entire) visibility = false;
                else if (quizPart == QUIZPART.Sixth) visibility = true;
                else
                {
                    if (quizPartial == QUIZPARTIAL.First || quizPartial == QUIZPARTIAL.Second) visibility = true;
                    else if (quizPartial == QUIZPARTIAL.Third && quizPart == QUIZPART.Third) visibility = true;
                    else visibility = false;

                }

            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : BindingOperations.DoNothing;
        }

    }
}
