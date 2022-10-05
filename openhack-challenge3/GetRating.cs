using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Openhack_Challenge3.Models;
using Microsoft.Azure.Documents.Client;

namespace openhack_challenge3
{
    public class GetRating
    {
        [FunctionName("GetRating")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetRating/{userId}/{ratingId}")] HttpRequest req,
            [CosmosDB(databaseName: "openhack-challenge3", collectionName: "ratings", ConnectionStringSetting = "CosmosDbConnectionString", Id = "{ratingId}", PartitionKey = "{userId}")] Feedback feedback,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var feedbackJson = JsonConvert.SerializeObject(feedback);
            log.LogInformation("feedbackJson = " + feedbackJson);
            return new OkObjectResult(feedbackJson);
        }
    }
}
