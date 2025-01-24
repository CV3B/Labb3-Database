using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Labb3_Database.Model;
using Labb3_Database.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Labb3_Database;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel) return;

        using var db = new QuizContext();

        var allCategories = db.Categories.Select(c => c.Name).ToHashSet();

        foreach (var pack in viewModel.ConfigurationViewModel?.Packs)
        {
            var packFromDb = db.QuestionPacks.Include(qp => qp.Questions).FirstOrDefault(qp => qp.Id == pack.Id);

            if (!allCategories.Contains(pack.Category))
            {
                pack.Category = "All";
            }

            if (packFromDb is not null)
            {
                packFromDb.Name = pack.Name;
                packFromDb.Category = pack.Category ?? db.Categories.First().Name;
                packFromDb.Difficulty = pack.Difficulty;
                packFromDb.TimeLimitInSeconds = pack.TimeLimitInSeconds;
                packFromDb.Questions = new List<Question>(pack.Questions.ToList());
            }
        }

        db.SaveChanges();
    }
}