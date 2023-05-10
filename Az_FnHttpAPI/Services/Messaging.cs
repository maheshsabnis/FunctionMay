using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Az_FnHttpAPI.Services
{
    public class Messaging
    {
        /// <summary>
        /// Method to add message to Queue
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddMessage(string data)
        {
            // Connect to Storage Account from the Azure Function
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("CONN=STR");

            // Create a Queeu Client

            CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();

            // Use the CloudQueue object to get the Queue Name

            CloudQueue cloudQueue = cloudQueueClient.GetQueueReference("data-queue");
            // Create Queue if not exist
            await cloudQueue.CreateIfNotExistsAsync();

            // Define a Cloud Message

            CloudQueueMessage queueMessage = new CloudQueueMessage(data);

            // Add the Message in Queue
            await cloudQueue.AddMessageAsync(queueMessage);


        }
    }
}
