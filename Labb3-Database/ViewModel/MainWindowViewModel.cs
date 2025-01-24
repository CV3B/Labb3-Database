using System.Collections.ObjectModel;
using Labb3_Database.Model;
using MongoDB.Bson;

namespace Labb3_Database.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

    public PlayerViewModel PlayerViewModel { get; set; }

    public ConfigurationViewModel? ConfigurationViewModel { get; set; }

    private CommandsViewModel? _commandsViewModel;
    public CommandsViewModel CommandsViewModel
    {
        get => _commandsViewModel;
        set
        {
            _commandsViewModel = value;
            RaisePropertyChanged();
        }
    }

    private QuestionPackViewModel _activePack;
    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            _activePack = value;
            RaisePropertyChanged();
            ConfigurationViewModel?.RaisePropertyChanged();
            PlayerViewModel?.RaisePropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        Init();
    }
    
    public void Init()
    {
        using var db = new QuizContext();
        
        ObservableCollection<QuestionPackViewModel> packsFromDb = new ObservableCollection<QuestionPackViewModel>();
        foreach (var pack in db.QuestionPacks)
        {
            packsFromDb.Add(new QuestionPackViewModel(pack));
        }

        if (packsFromDb.Count <= 0)
        {
            var newPack = new QuestionPack("<Pack Name>", db.Categories.First().Name);
            
            packsFromDb.Add(new QuestionPackViewModel(newPack));
            
            db.QuestionPacks.Add(newPack);
            db.SaveChanges();
        }

        Packs = packsFromDb;
        
        ActivePack = Packs.First();
        
        CommandsViewModel = new CommandsViewModel(this);
        
        PlayerViewModel = new PlayerViewModel(this);
        
        ConfigurationViewModel = new ConfigurationViewModel(this);
        
        
    }
}