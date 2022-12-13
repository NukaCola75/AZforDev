using System;
using System.ComponentModel;
using System.IO;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace reactBlobFunc
{
    public class renameFile
    {
        private readonly ILogger _logger;
        private static Random random = new Random();
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=demotp;AccountKey=AUryLQFxxPNrq+yoqT/gwccij4F6CrmvekFVEguYZOjES4RXJjFIfiqPuA9iNgUpcU+38D6khQ3v+AStdUitnw==;EndpointSuffix=core.windows.net";
        private static string containerName = "containershebbirazfordevtp2"; // Choose an unique name

        // Create a BlobServiceClient object
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

        public renameFile(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<renameFile>();
        }

        [Function("renameFile")]
        public async Task Run([BlobTrigger("containershebbirazfordevtp2/{name}", Connection = "conStr")] string myBlob, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} ");
            BlobClient originBlob = blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(name);
            String newName;
            if (name.Contains("."))
            {
                // _logger.LogInformation($"Name: {name} contain .");
                var splitName = name.Split(".");
                if ((splitName[0].Length > 10) || (splitName[0].Length < 10))
                {
                    newName = RandomString(10) + "." + splitName[1];
                    await RenameFile(originBlob, name, newName);
                }
                
            } else
            {
                // _logger.LogInformation($"Name: {name} NOT contain .");
                if ((name.Length > 10) || (name.Length < 10))
                {
                    newName = RandomString(10);
                    await RenameFile(originBlob, name, newName);
                }
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> RenameFile(BlobClient originBlob, string name, string newName)
        {
            _logger.LogInformation($"C# Blob trigger function Renamed blob\n Name: {name} in {newName}");
            BlobClient destBlob = blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(newName);
            await destBlob.StartCopyFromUriAsync(originBlob.Uri);
            await originBlob.DeleteAsync();
            return true;
        }
    }
}
