using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace Labb3_Database.Model;

public class RootObject
{
    public int response_code { get; set; }
    public List<ApiHandle> results { get; set; }
}

public class ApiHandle
{
    public string type { get; set; }
    public string difficulty { get; set; }
    public string category { get; set; }
    public string question { get; set; }
    public string correct_answer { get; set; }
    public string[] incorrect_answers { get; set; }

    public async Task<RootObject> LoadApi(int amount, int _category, Difficulty _difficulty)
    {
        var url = $"https://opentdb.com/api.php?amount={amount}&category={_category}&difficulty={_difficulty.ToString().ToLower()}&type=multiple";

        using (var httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage getResponse = await httpClient.GetAsync(url);
                getResponse.EnsureSuccessStatusCode();

                var responseJson = await getResponse.Content.ReadAsStringAsync();
                var apiContent = JsonSerializer.Deserialize<RootObject>(responseJson);

                List<ApiHandle> questions = new List<ApiHandle> { };

                if (apiContent.results != null)
                {
                    foreach (var apiResult in apiContent.results)
                    {
                        if (apiResult != null)
                        {
                            DecodeHtml(apiResult);
                            
                            questions.Add(apiResult);
                        }
                    }
                }

                return new RootObject() { response_code = apiContent.response_code, results = questions };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
    public void DecodeHtml(ApiHandle apiResult)
    {
        apiResult.question = HttpUtility.HtmlDecode(apiResult.question);
        apiResult.correct_answer = HttpUtility.HtmlDecode(apiResult.correct_answer);

        for (int i = 0; i < apiResult.incorrect_answers.Length; i++)
        {
            apiResult.incorrect_answers[i] = HttpUtility.HtmlDecode(apiResult.incorrect_answers[i]);
        }
    }
}

public class CategoryResponse
{
    public TriviaCategories[] trivia_categories { get; set; }
}

public class TriviaCategories
{
    public int id { get; set; }
    public string name { get; set; }

    public async Task<ObservableCollection<KeyValuePair<int, string>>> GetCategories()
    {
        var url = "https://opentdb.com/api_category.php";

        using (var httpClient = new HttpClient())
        {
            try
            {
                Task<HttpResponseMessage> getResponse = httpClient.GetAsync(url);

                HttpResponseMessage response = await getResponse;

                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<CategoryResponse>(responseJson);

                ObservableCollection<KeyValuePair<int, string>> importedQuestionsPair =
                    new ObservableCollection<KeyValuePair<int, string>>();

                foreach (var res in result.trivia_categories)
                {
                    importedQuestionsPair.Add(new KeyValuePair<int, string>(res.id, res.name));
                }

                return importedQuestionsPair;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}