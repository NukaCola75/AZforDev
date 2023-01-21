using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace AZforDevBackEndFunc
{
    public class FindDataMongo
    {
        private readonly ILogger _logger;
        private static string connectionString =
            @"mongodb://sh-storage-mongo:8mVGpTL6TPP6JfoNiJLk3S7ytOkV1FoCQndtgNG7CfpYJ40UuTg5iLRbueArFbRgJXL0e7YjMZCmACDbhueA3Q==@sh-storage-mongo.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@sh-storage-mongo@";
        MongoClientSettings settings = MongoClientSettings.FromUrl(
          new MongoUrl(connectionString)
        );

        public FindDataMongo(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FindDataMongo>();
        }

        [Function("FindDataMongo")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string? Search)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("commerce");
            var _products = db.GetCollection<Product>("products");
            var documents = _products.AsQueryable().ToList();
            string? str = Search;
            

            var document = documents.Find(X => X.Id.Contains(str)).ToJson();

            var response = req.CreateResponse(HttpStatusCode.OK);
            // response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            // var json = JsonConvert.SerializeObject(document);
            await response.WriteStringAsync(document);
            return response;
        }
    }
}
