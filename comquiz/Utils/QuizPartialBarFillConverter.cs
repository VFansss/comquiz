using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Avalonia.Media;

namespace comquiz
{
    public class QuizPartialBarFillConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            bool partialBarSelected = false;

            if (values != null && values.Count == 2)
            {
                if (Enum.IsDefined(typeof(QUIZPART), values[0]) && Enum.IsDefined(typeof(QUIZPART), values[1]))
                {
                    QUIZPART selectedQuizPart = (QUIZPART)values[0];
                    QUIZPARTIAL selectedQuizPartial = (QUIZPARTIAL)values[1];
                    QUIZPARTIAL currentPartialBar = (QUIZPARTIAL)parameter;

                    if (selectedQuizPart == QUIZPART.Entire)
                    {
                        partialBarSelected = true;
                    }
                    else if (selectedQuizPart == QUIZPART.Half)
                    {
                        int partialBarNumber = (int)currentPartialBar;

                        partialBarSelected = ((3 - partialBarNumber) <= 0) ? true : false;

                    }
                    else if (false)
                    {

                    }
                    else
                    {
                        // 1/6 quiz division

                        partialBarSelected = currentPartialBar == selectedQuizPartial ? true : false;

                    }


                }

            }

            if (partialBarSelected) return new SolidColorBrush(Color.FromRgb(8, 90, 147));
            else return new SolidColorBrush(Color.FromRgb(130, 176, 232));

        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
