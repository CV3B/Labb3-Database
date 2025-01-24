using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Labb3_Database.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3_Database.ViewModel;

public class QuestionPackViewModel : ViewModelBase
{
    private readonly QuestionPack model;

    [BsonId]
    public ObjectId Id => model.Id;
    
    public string Name
    {
        get => model.Name;
        set
        {
            model.Name = value;
            RaisePropertyChanged();
        }
    }

    public string Category
    {
        get => model.Category;
        set
        {
            model.Category = value;
            RaisePropertyChanged();
        }
    }

    public Difficulty Difficulty
    {
        get => model.Difficulty;
        set
        {
            model.Difficulty = value;
            RaisePropertyChanged();
        }
    }

    public int TimeLimitInSeconds
    {
        get => model.TimeLimitInSeconds;
        set
        {
            model.TimeLimitInSeconds = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<Question> Questions { get; set; }

    [JsonConstructor]
    public QuestionPackViewModel()
    {
        this.model = new QuestionPack();
        Questions = new ObservableCollection<Question>(model.Questions);
    }

    public QuestionPackViewModel(QuestionPack model)
    {
        this.model = model;
        this.Questions = new ObservableCollection<Question>(model.Questions);
    }
}