using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

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
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

        string response;

        if (req.Method == HttpMethods.Get)
        {

            string name = req.Query["name"];
            int age;
            bool ageParse = int.TryParse(req.Query["age"], out age);   

            if(string.IsNullOrEmpty(name) || !ageParse){
                respone = "Please provide a valid name and age in the query string.";
            } else {
                respone = $"Hello {name}. According to athe information you provided, you are {age} years old.";
            }
        }
        else if (req.Method == HttpMethods.Post)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string name = data?.name;
            int age = data?.age;

            if(string.IsNullOrEmpty(name) || age == null){
                respone = "Please provide a valid name and age in the request body.";
            } else {
                respone = $"Hello {name}. According to athe information you provided, you are {age} years old.";
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
