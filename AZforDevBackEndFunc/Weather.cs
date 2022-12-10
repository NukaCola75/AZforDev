using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using File = System.IO.File;

namespace AZforDevBackEndFunc
{
    public class Weather
    {
        private readonly ILogger _logger;
        public WeatherForecast[]? forecasts;

        public Weather(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Weather>();
        }

        [Function("Weather")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, ExecutionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
    
            // string jsonFilePath = Path.Combine(context.FunctionAppDirectory, "Data", "test.html");
            StreamReader weatherSample = new StreamReader(@"./sample-data/weather.json");
            forecasts = JsonSerializer.Deserialize<WeatherForecast[]>(weatherSample.ReadToEnd());

            var response = req.CreateResponse(HttpStatusCode.OK);
            // response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await response.WriteStringAsync(JsonSerializer.Serialize<WeatherForecast[]>(forecasts));

            return response;

        }

        public class WeatherForecast
        {
            public DateOnly date { get; set; }

            public int temperatureC { get; set; }

            public string? summary { get; set; }

            public int temperatureF => 32 + (int)(temperatureC / 0.5556);
        }
    }
}
