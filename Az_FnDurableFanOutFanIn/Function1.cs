using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Az_FnDurableFanOutFanIn
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // First Function that will read all files
            string[] files = await context.CallActivityAsync<string[]>(nameof(GetFiles), @"D:\home\LogFiles");

            // Use the Task object to process each record from file array seperately 
            var tasks = new Task<long>[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                // Invoke CopyFile function on separate task
                tasks[i] = context.CallActivityAsync<long>(nameof(CopyFile), files[i]);
            }

            // make sure that all tasks are executed before returning the function


             await Task.WhenAll(tasks);
            return outputs;
        }

        /// <summary>
        /// FanOut Function that will return data as Files
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <returns></returns>
        [FunctionName(nameof(GetFiles))]
        public static string[] GetFiles([ActivityTrigger] string rootDirectory)
        {
            string[] files = Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);
            return files;
        }
        [FunctionName(nameof(CopyFile))]
        public static async Task<long> CopyFile([ActivityTrigger] string filePath, ILogger logger)
        { 
            // Lets get the File Size
            long fileSizeInBytes = new FileInfo(filePath).Length;
            logger.LogInformation($"Size of file {fileSizeInBytes} ");

            // Read the File Name, the same will be used after the copy
            string targetFileName = Path.GetFileName(filePath);

            string copyLocation = $"D:\\home\\Copied";

            // Copy Contents of One file to copyLocation

            using (Stream source = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (Stream destination = new FileStream($"{copyLocation}\\{targetFileName}", FileMode.CreateNew))
            {
                if (File.Exists($"{copyLocation}\\{targetFileName}"))
                {
                    logger.LogInformation($"File {filePath} is already exist");
                }
                else
                { 
                    await source.CopyToAsync( destination );
                }
            }

                return fileSizeInBytes;
        }





        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Function1", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}