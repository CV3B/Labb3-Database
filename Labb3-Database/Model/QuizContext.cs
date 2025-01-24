using Labb3_Database.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Labb3_Database.Model;

public class QuizContext : DbContext
{
    public DbSet<QuestionPack> QuestionPacks { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMongoDB("mongodb+srv://skrot600:iths2024@cluster0.gkp3g.mongodb.net/", "Quiz");
    }
}