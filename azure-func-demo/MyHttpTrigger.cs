using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;

namespace Company.Function
{
    public class MyHttpTrigger
    {
        private readonly ILogger<MyHttpTrigger> _logger;

        public MyHttpTrigger(ILogger<MyHttpTrigger> logger)
        {
            _logger = logger;
        }

        [Function("MyHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string response;

            if (req.Method == HttpMethods.Get)
            {

                string? name = req.Query["name"];
                int age;
                bool ageParse = int.TryParse(req.Query["age"], out age);   

                if(string.IsNullOrEmpty(name) || !ageParse){
                    response = "Please provide a valid name and age in the query string.";
                } else {
                    response = $"Hello {name}. According to the information you provided, you are {age} years old.";
                }
            }
            else if (req.Method == HttpMethods.Post)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                JsonDocument doc = JsonDocument.Parse(requestBody);
                JsonElement root = doc.RootElement;

                string? name = root.GetProperty("name").ToString();
                int? age = root.TryGetProperty("age", out JsonElement ageElement) ? ageElement.GetInt32() : (int?)null;

                if(string.IsNullOrEmpty(name) || age == 0){
                    response = "Please provide a valid name and age in the request body.";
                } else {
                    response = $"Hello {name}. According to the information you provided, you are {age} years old.";
                }
            }
            else
            {
                response = "Unsupported HTTP method.";
            }

            return new OkObjectResult(response);
        }
    }
}
