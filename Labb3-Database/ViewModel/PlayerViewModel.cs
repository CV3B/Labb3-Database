using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Labb3_Database.Command;
using Labb3_Database.Model;

namespace Labb3_Database.ViewModel;

public class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel mainWindowViewModel;

    Random rnd = new Random();

    public QuestionPackViewModel ActivePack
    {
        get => mainWindowViewModel.ActivePack;
    }

    public DelegateCommand AnswerButtonCommand { get; }
    public DelegateCommand RestartQuizCommand { get; }

    private DispatcherTimer timer;

    private int _Countdown;
    public int Countdown
    {
        get => _Countdown;
        set
        {
            _Countdown = value;
            RaisePropertyChanged();
        }
    }

    private bool _IsCompleted;
    public bool IsCompleted
    {
        get => _IsCompleted;
        set
        {
            _IsCompleted = value;
            RaisePropertyChanged();
        }
    }

    private bool _IsPlaying;
    public bool IsPlaying
    {
        get => _IsPlaying;
        set
        {
            _IsPlaying = value;
            RaisePropertyChanged();
        }
    }

    private int _AmountOfCorrectAnswers;
    public int AmountOfCorrectAnswers
    {
        get => _AmountOfCorrectAnswers;
        set
        {
            _AmountOfCorrectAnswers = value;
            RaisePropertyChanged();
        }
    }

    private string _answerOne;
    public string AnswerOne
    {
        get => _answerOne;
        set
        {
            _answerOne = value;
            RaisePropertyChanged();
        }
    }

    private string _answerTwo;
    public string AnswerTwo
    {
        get => _answerTwo;
        set
        {
            _answerTwo = value;
            RaisePropertyChanged();
        }
    }

    private string _answerThree;
    public string AnswerThree
    {
        get => _answerThree;
        set
        {
            _answerThree = value;
            RaisePropertyChanged();
        }
    }

    private string _answerFour;
    public string AnswerFour
    {
        get => _answerFour;
        set
        {
            _answerFour = value;
            RaisePropertyChanged();
        }
    }

    private Brush _answerOneBackground = Brushes.LightGray;
    public Brush AnswerOneBackground
    {
        get => _answerOneBackground;
        set
        {
            _answerOneBackground = value;
            RaisePropertyChanged();
        }
    }
    
    private Brush _answerTwoBackground = Brushes.LightGray;
    public Brush AnswerTwoBackground
    {
        get => _answerTwoBackground;
        set
        {
            _answerTwoBackground = value;
            RaisePropertyChanged();
        }
    }
    
    private Brush _answerThreeBackground = Brushes.LightGray;
    public Brush AnswerThreeBackground
    {
        get => _answerThreeBackground;
        set
        {
            _answerThreeBackground = value;
            RaisePropertyChanged();
        }
    }
    
    private Brush _answerFourBackground = Brushes.LightGray;
    public Brush AnswerFourBackground
    {
        get => _answerFourBackground;
        set
        {
            _answerFourBackground = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<Question> _randomOrderOfQuestions;
    public ObservableCollection<Question> RandomOrderOfQuestions { get; set; }

    private int _CurrentQuestionNumber;
    public int CurrentQuestionNumber
    {
        get => _CurrentQuestionNumber;
        set
        {
            _CurrentQuestionNumber = value;
            RaisePropertyChanged();
            UpdateAnswers();
        }
    }
    
    private string _currentQuestionText;

    public string CurrentQuestionText
    {
        get => _currentQuestionText;
        set
        {
            _currentQuestionText = value;
            RaisePropertyChanged();
        }
    }

    public PlayerViewModel(MainWindowViewModel mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;
        
        RandomOrderOfQuestions = new ObservableCollection<Question>(ActivePack.Questions.OrderBy(_ => rnd.Next()));

        CurrentQuestionNumber = 1;
        AmountOfCorrectAnswers = 0;
        
        IsCompleted = false;
        IsPlaying = true;
        
        Countdown = ActivePack.TimeLimitInSeconds;

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += TimerOnTick;
        timer.Start();

        AnswerButtonCommand = new DelegateCommand(AnswerButton);
        RestartQuizCommand = new DelegateCommand(RestartQuizButton);

        UpdateAnswers();
    }

    private async void AnswerButton(object obj)
    {
        var answer = obj as Button;

        if (answer?.Content.ToString() == RandomOrderOfQuestions[CurrentQuestionNumber - 1].CorrectAnswer)
        {
            SetAnswerBackground(RandomOrderOfQuestions[CurrentQuestionNumber - 1].CorrectAnswer, Brushes.ForestGreen);
            AmountOfCorrectAnswers++;
        }
        else
        {
            SetAnswerBackground(answer.Content.ToString(), Brushes.Red);
            SetAnswerBackground(RandomOrderOfQuestions[CurrentQuestionNumber - 1].CorrectAnswer, Brushes.ForestGreen);
        }

        await Task.Delay(1000);

        if (CurrentQuestionNumber >= ActivePack.Questions.Count)
        {
            IsCompleted = true;
            IsPlaying = false;

            AnswerOneBackground = Brushes.LightGray;
            AnswerTwoBackground = Brushes.LightGray;
            AnswerThreeBackground = Brushes.LightGray;
            AnswerFourBackground = Brushes.LightGray;
            return;
        }

        AnswerOneBackground = Brushes.LightGray;
        AnswerTwoBackground = Brushes.LightGray;
        AnswerThreeBackground = Brushes.LightGray;
        AnswerFourBackground = Brushes.LightGray;

        CurrentQuestionNumber++;
        UpdateAnswers();
    }

    private void SetAnswerBackground(string answer, Brush color)
    {
        if (AnswerOne == answer) AnswerOneBackground = color;
        else if (AnswerTwo == answer) AnswerTwoBackground = color;
        else if (AnswerThree == answer) AnswerThreeBackground = color;
        else if (AnswerFour == answer) AnswerFourBackground = color;
    }

    private void UpdateAnswers()
    {
        CurrentQuestionText = RandomOrderOfQuestions[CurrentQuestionNumber - 1].Query;
        var currentQuestion = RandomOrderOfQuestions[CurrentQuestionNumber - 1];
        var answers = new[]
        {
            currentQuestion.CorrectAnswer,
            currentQuestion.InCorrectAnswers[0],
            currentQuestion.InCorrectAnswers[1],
            currentQuestion.InCorrectAnswers[2],
        }.OrderBy(_ => rnd.Next()).ToList();

        AnswerOne = answers[0];
        AnswerTwo = answers[1];
        AnswerThree = answers[2];
        AnswerFour = answers[3];
    }


    private void TimerOnTick(object? sender, EventArgs e)
    {
        if (Countdown > 0) Countdown--;
        else
        {
            timer.Stop();
            IsPlaying = false;
            IsCompleted = true;
        }
    }

    public void RestartQuizButton(object obj)
    {
        RandomOrderOfQuestions = new ObservableCollection<Question>(ActivePack.Questions.OrderBy(_ => rnd.Next()));
        
        IsCompleted = false;
        IsPlaying = true;

        CurrentQuestionNumber = 1;
        AmountOfCorrectAnswers = 0;

        Countdown = ActivePack.TimeLimitInSeconds;
        timer.Start();
    }

    public void StartQuiz()
    {
        RandomOrderOfQuestions = new ObservableCollection<Question>(ActivePack.Questions.OrderBy(_ => rnd.Next()));
    }
}