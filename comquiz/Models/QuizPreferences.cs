using ReactiveUI;

namespace comquiz
{
    public class QuizPreferences : ReactiveObject
    {
        public bool RandomizeQuestionsOrder { get; set; } = false;

        public bool RandomizeAnswersOrder { get; set; } = false;


        QUIZPART _quizPart = QUIZPART.Entire;
        public QUIZPART QuizPart
        {
            get => _quizPart;
            set
            {
                QuizPartial = QUIZPARTIAL.First;
                this.RaiseAndSetIfChanged(ref _quizPart, value);
            }

        }

        QUIZPARTIAL _quizPartial = QUIZPARTIAL.First;
        public QUIZPARTIAL QuizPartial
        {
            get => _quizPartial;
            set => this.RaiseAndSetIfChanged(ref _quizPartial, value);
        }

        public QuizPreferences()
        {
            _quizPart = QUIZPART.Entire;
            _quizPartial = QUIZPARTIAL.First;
        }

    }

    public enum QUIZPART
    {
        UnsetValue,
        Entire,
        Half,
        Third,
        Sixth,
    }

    public enum QUIZPARTIAL
    {
        UnsetValue,
        First,
        Second,
        Third,
        Forth,
        Fifth,
        Sixth,
    }

}
