using MongoDB.Bson;

namespace Labb3_Database.Model;

public class Category
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }

    public Category(string name)
    {
        Name = name;
    }
}