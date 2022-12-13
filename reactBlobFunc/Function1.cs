using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace reactBlobFunc
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public void Run([BlobTrigger("containershebbirazfordevtp2/{name}", Connection = "conSettings")] string myBlob, string name)
        {
            // _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} ");
        }
    }
}
