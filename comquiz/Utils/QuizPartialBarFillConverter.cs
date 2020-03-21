using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace comquiz
{
    public class QuizPartialBarFillConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            bool partialBarSelected = false;

            if (values != null && values.Count == 2 && values[0] is QUIZPART && values[1] is QUIZPARTIAL)
            {
                if (Enum.IsDefined(typeof(QUIZPART), values[0]) && Enum.IsDefined(typeof(QUIZPARTIAL), values[1]))
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
                        partialBarSelected = ((int)currentPartialBar <= 3) ? true : false;

                        if (selectedQuizPartial == QUIZPARTIAL.Second) // If second half...
                        {
                            partialBarSelected = !partialBarSelected;
                        }

                    }
                    else if (selectedQuizPart == QUIZPART.Third)
                    {
                        int currentPartialBarNumber = (int)currentPartialBar;

                        if (selectedQuizPartial == QUIZPARTIAL.First && currentPartialBarNumber <= 2)
                        {
                            partialBarSelected = true;
                        }
                        else if (selectedQuizPartial == QUIZPARTIAL.Second && currentPartialBarNumber <= 4 && currentPartialBarNumber > 2)
                        {
                            partialBarSelected = true;
                        }
                        else if (selectedQuizPartial == QUIZPARTIAL.Third && currentPartialBarNumber > 4)
                        {
                            partialBarSelected = true;
                        }

                    }
                    else
                    {
                        // 1/6 quiz division

                        partialBarSelected = currentPartialBar == selectedQuizPartial ? true : false;

                    }


                }

            }

            if (partialBarSelected) return new SolidColorBrush(Color.FromRgb(7, 142, 255));
            else return new SolidColorBrush(Color.FromRgb(155, 210, 255));

        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            return null;
        }

    }
}
