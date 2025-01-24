using System.Collections.ObjectModel;
using Labb3_Database.Command;
using Labb3_Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Labb3_Database.ViewModel;

public class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel mainWindowViewModel;

    public DelegateCommand AddQuestionCommand { get; }
    public DelegateCommand RemoveQuestionCommand { get; }

    public QuestionPackViewModel ActivePack
    {
        get => mainWindowViewModel.ActivePack;
    }

    public ObservableCollection<QuestionPackViewModel> Packs
    {
        get => mainWindowViewModel.Packs;
    }


    public CommandsViewModel CommandsViewModel { get; }


    private Question _activeQuestion;

    public Question ActiveQuestion
    {
        get => _activeQuestion;
        set
        {
            _activeQuestion = value;
            RaisePropertyChanged();
        }
    }


    public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        AddQuestionCommand = new DelegateCommand(AddQuestionButton);
        RemoveQuestionCommand = new DelegateCommand(RemoveQuestionButton, CanRemoveQuestionButton);
    }

    private void AddQuestionButton(object obj)
    {
        var newQuestion = new Question("New Question", "", "", "", "");
        
        using var db = new QuizContext();

        var activePackFromDb = db.QuestionPacks.FirstOrDefault(qp => qp.Id == ActivePack.Id);

        activePackFromDb?.Questions.Add(newQuestion);

        db.SaveChanges();
        
        ActivePack.Questions.Add(newQuestion);
        RemoveQuestionCommand.RaiseCanExecuteChanged();
    }

    private bool CanRemoveQuestionButton(object? arg)
    {
        return ActivePack.Questions.Count > 1;
    }

    private void RemoveQuestionButton(object obj)
    {
        using var db = new QuizContext();
        
        var activePackFromDb = db.QuestionPacks.Include(qp => qp.Questions).FirstOrDefault(qp => qp.Id == ActivePack.Id);
        var questionToRemoveFromDb = activePackFromDb?.Questions.FirstOrDefault(q => q.Id == ActiveQuestion.Id);

        if (questionToRemoveFromDb != null)
        {
            activePackFromDb?.Questions.Remove(questionToRemoveFromDb);
            
            db.SaveChanges();
        }

        
        ActivePack.Questions.Remove(ActiveQuestion);
        RemoveQuestionCommand.RaiseCanExecuteChanged();
    }
}