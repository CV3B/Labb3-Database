using Labb3_Database.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Labb3_Database.Model;

public class QuizContext : DbContext
{
    public DbSet<QuestionPack> QuestionPacks { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMongoDB("mongodb://localhost:27017/", "CalleBjureblad");
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<QuestionPack>()
            .Property(q => q.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();
    }

}