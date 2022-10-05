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

namespace openhack_challenge3
{
    public class GetRatings
    {
        [FunctionName("GetRatings")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetRatings/{userId}")] HttpRequest req,
            [CosmosDB(databaseName: "openhack-challenge3", collectionName: "ratings", ConnectionStringSetting = "CosmosDbConnectionString", PartitionKey ="{userId}")] IEnumerable<Feedback> feedbacks,
            ILogger log)
        {
            log.LogInformation("GetRatings processed a request.");
            
            var feedbackJson = JsonConvert.SerializeObject(feedbacks, Formatting.Indented);

            return new OkObjectResult(feedbackJson);
        }
    }
}

