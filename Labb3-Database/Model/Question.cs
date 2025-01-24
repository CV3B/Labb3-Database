using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3_Database.Model;

public class Question
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Query { get; set; }
    public string CorrectAnswer { get; set; }
    public string[] InCorrectAnswers { get; set; }

    [JsonConstructor]
    public Question()
    {
        InCorrectAnswers = new string[3];
    }

    public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2,
        string incorrectAnswer3)
    {
        Id = ObjectId.GenerateNewId();
        Query = query;
        CorrectAnswer = correctAnswer;
        InCorrectAnswers = new string[3] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
    }
}