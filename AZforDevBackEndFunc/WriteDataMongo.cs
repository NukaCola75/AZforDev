using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace AZforDevBackEndFunc
{
    public class WriteDataMongo
    {
        private readonly ILogger _logger;
        private static string connectionString =
            @"mongodb://sh-storage-mongo:8mVGpTL6TPP6JfoNiJLk3S7ytOkV1FoCQndtgNG7CfpYJ40UuTg5iLRbueArFbRgJXL0e7YjMZCmACDbhueA3Q==@sh-storage-mongo.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@sh-storage-mongo@";
        MongoClientSettings settings = MongoClientSettings.FromUrl(
          new MongoUrl(connectionString)
        );
        

        public WriteDataMongo(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WriteDataMongo>();
        }

        [Function("WriteDataMongo")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("commerce");
            var _products = db.GetCollection<Product>("products");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            Product? produit = JsonConvert.DeserializeObject<Product>(requestBody);
            
            produit.Id = Guid.NewGuid().ToString();
            _products.InsertOne(produit);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }

        
    }
    public class Product
    {
        public string? Id;
        public string? Category;
        public string? Name;
        public int Quantity;
        public bool Sale;
    }
            
}
