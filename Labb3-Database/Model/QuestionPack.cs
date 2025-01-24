using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3_Database.Model;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class QuestionPack
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    
    public string Category { get; set; }

    public Difficulty Difficulty { get; set; }

    public int TimeLimitInSeconds { get; set; }
    
    public List<Question> Questions { get; set; }

    [JsonConstructor]
    public QuestionPack()
    {
        Questions = new List<Question>();
    }

    public QuestionPack(string name, string category, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
    {
        Name = name;
        Category = category;
        Difficulty = difficulty;
        TimeLimitInSeconds = timeLimitInSeconds;
        Questions = new List<Question>() { new Question("New Question", " ", " ", " ", " ") };
    }
}