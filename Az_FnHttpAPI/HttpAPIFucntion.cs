using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Az_FnHttpAPI
{
    public static class HttpAPIFucntion
    {
        /// <summary>
        /// IActionResult: A Contract that will be used to Send HTTP Response
        /// HttpRequest: The Object taht is used for HTTP request Processing by providing facilities to Read Header, Query, Body, etc. 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("HttpAPI")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string method = req.Method;
            string name = string.Empty;
            switch (method)
            {
                case "GET":
                    // 1. Read QueryString 
                     name = req.Query["name"];
                    break;
                case "POST":
                    // Used to Read HTTP request Body
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    dynamic data = JsonConvert.DeserializeObject(requestBody);
                    name = name ?? data?.name;
                    break;
                default:
                    break;
            }

          
           

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";
            // Status is OK 200
            return new OkObjectResult(responseMessage);
        }
    }
}
