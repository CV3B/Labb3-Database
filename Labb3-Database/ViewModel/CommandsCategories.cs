using System.Collections.ObjectModel;
using Labb3_Database.Command;
using Labb3_Database.Model;
using Labb3_Database.Views.Dialogs;

namespace Labb3_Database.ViewModel;

public partial class CommandsViewModel : ViewModelBase
{
    public CategoriesDialog CategoriesDialog { get; set; }

    public DelegateCommand ShowCategoriesCommand { get; set; }
    public DelegateCommand CreateCategoryCommand { get; set; }
    public DelegateCommand RemoveCategoryCommand { get; set; }

    private ObservableCollection<Category> _quizCategories;

    public ObservableCollection<Category> QuizCategories
    {
        get => _quizCategories;
        set
        {
            _quizCategories = value;
            RaisePropertyChanged();
        }
    }

    private string _newCategoryName;

    public string NewCategoryName
    {
        get => _newCategoryName;
        set
        {
            _newCategoryName = value;
            RaisePropertyChanged();
        }
    }

    private Category _selectedCategory;

    public Category SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            _selectedCategory = value;
            RaisePropertyChanged();
        }
    }

    public partial void CommandsCategories()
    {
        GetAllCategories();

        ShowCategoriesCommand = new DelegateCommand(ShowCategoriesButton);
        CreateCategoryCommand = new DelegateCommand(CreateNewCategoryButton);
        RemoveCategoryCommand = new DelegateCommand(RemoveCategoryButton, CanRemoveCategoryButton);
    }

    public void GetAllCategories()
    {
        using var db = new QuizContext();

        if (!db.Categories.Any())
        {
            db.Categories.Add(new Category("All"));
            db.SaveChanges();
        }
        
        QuizCategories = new ObservableCollection<Category>(db.Categories.ToList());
    }

    private void ShowCategoriesButton(object obj)
    {
        CategoriesDialog = new CategoriesDialog() { DataContext = this };
        CategoriesDialog.ShowDialog();
    }

    private void CreateNewCategoryButton(object obj)
    {
        if (string.IsNullOrWhiteSpace(NewCategoryName)) return;

        using var db = new QuizContext();

        var newCategory = new Category(NewCategoryName);

        db.Categories.Add(newCategory);

        NewCategoryName = "";
        db.SaveChanges();

        QuizCategories.Add(newCategory);

        RemoveCategoryCommand.RaiseCanExecuteChanged();
    }

    private bool CanRemoveCategoryButton(object? arg)
    {
        return QuizCategories.Count > 1;
    }

    private void RemoveCategoryButton(object obj)
    {
        using var db = new QuizContext();

        db.Categories.Remove(SelectedCategory);

        db.SaveChanges();

        QuizCategories.Remove(SelectedCategory);

        RemoveCategoryCommand.RaiseCanExecuteChanged();
    }
}