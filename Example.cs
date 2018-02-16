using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

class Program
{
    class PostDTO
    {
        public int idOperation { get; set; }
        public Person Data { get; set; }
    }
    class PersonComplete
    {
        public string name { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
    }

    class Person
    {
        public string user { get; set; }
        public string pass { get; set; }
    }


    public class HttpClientSample
    {
        static List<Person> Users { get; set; }

        static HttpClient client = new HttpClient();

        static async Task<string> CreatePersonAsync(PostDTO data)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/XXXX", data);
            response.EnsureSuccessStatusCode();

            return response.StatusCode.ToString();
        }

        static async Task<PersonComplete> GetPersonAsync(string path)
        {
            PersonComplete person = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                person = await response.Content.ReadAsAsync<PersonComplete>();
            }
            return person;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://xxxx.com/v1/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("host", "XXXX");
            client.DefaultRequestHeaders.Add("referer", "XXXX");
            client.DefaultRequestHeaders.Add("origin", "XXXX");
            client.DefaultRequestHeaders.Add("Authentication", "XXXX");

            try
            {
                Users = JsonConvert.DeserializeObject<List<Person>>(File.ReadAllText(@"C:\xxxx.json"));
                for (var i = 0; i < Users.Count; i++)
                {
                    var postJson = new PostDTO
                    {
                        idOperation = 1,
                        Data = Users[i]
                    };
                    var status = await CreatePersonAsync(postJson);
                    Console.WriteLine($"Status {status} - {postJson.Data.user} - {i+1}/{Users.Count}");
                }

                var getUser = GetPersonAsync("user/500");

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
