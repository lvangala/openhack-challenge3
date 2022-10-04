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
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Openhack_Challenge3
{
    public class CreateRating
    {
        private IHttpClientFactory _httpClientFactory;

        public CreateRating(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [FunctionName("CreateRating")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("CreateRating processed a request.");

            if (req.Method.ToLower().Equals("get")) return new NotFoundObjectResult("Get method is not supported");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var feedback = JsonConvert.DeserializeObject<Feedback>(requestBody);
                if (await IsValidProductId(feedback.ProductId, log) && await IsValidUserId(feedback.UserId, log))
                {
                    return new OkObjectResult(JsonConvert.SerializeObject(feedback));
                }
                return new BadRequestObjectResult("Invalid user id or product id.  Please resubmit the request with right product id and user id");
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        private async Task<bool> IsValidProductId(string productId, ILogger log)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var requestUrl = $"https://serverlessohapi.azurewebsites.net/api/GetProduct?productId={productId}";
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            log.LogInformation($"Product status code {httpResponseMessage.IsSuccessStatusCode}");
            return httpResponseMessage.IsSuccessStatusCode;
        }

        private async Task<bool> IsValidUserId(string userId, ILogger log)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var requestUrl = $"https://serverlessohapi.azurewebsites.net/api/GetUser?userId={userId}";
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            log.LogInformation($"User status code {httpResponseMessage.IsSuccessStatusCode}");
            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}
