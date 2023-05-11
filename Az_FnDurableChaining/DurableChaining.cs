using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Az_FnDurableChaining.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Text.Json;

namespace Az_FnDurableChaining
{
    public static class DurableChaining
    {

        static Messaging messaging = new Messaging();

        [FunctionName("ChainFunction")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            Random rnd = new Random();

            var toDo = new ToDo()
            {
                Task = $"Define Durable Function : {rnd.Next(100)}"
            };

            //// Replace "hello" with the name of your Durable Activity Function.
            ///

            // Result from the First Function Call
             var result =  await context.CallActivityAsync<List<ToDo>>(nameof(AddToDo), toDo);

            // Call tyhe Second Function an pass datga of First Fucntion call to it

            await context.CallActivityAsync(nameof(AddToQueue), result);


            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        //[FunctionName(nameof(SayHello))]
        //public static string SayHello([ActivityTrigger] string name, ILogger log)
        //{
        //    log.LogInformation("Saying hello to {name}.", name);
        //    return $"Hello {name}!";
        //}

        [FunctionName(nameof(AddToDo))]
        public static List<ToDo> AddToDo([ActivityTrigger] ToDo todo)
        { 
            List<ToDo> toDos = new List<ToDo>();
            try
            {
                SqlConnection Conn = new SqlConnection("Data Source=.;Initial Catalog=Company;Integrated Security=SSPI;Encrypt=Yes;TrustServerCertificate=Yes");
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                
                // Insertuing data
                Cmd.CommandText = $"Insert into ToDo Values ('{todo.Task}')";
                Cmd.ExecuteNonQuery();

                // Read All Data
                Cmd.CommandText = "Select * from ToDo";
                SqlDataReader reader = Cmd.ExecuteReader();
                while (reader.Read()) 
                {
                    toDos.Add(new ToDo() { Id = Convert.ToInt32(reader["Id"]) , Task = reader["task"].ToString() });
                }
                reader.Close();
                Conn.Close();   

            }
            catch (System.Exception)
            {
            }

            return toDos;
        }

        [FunctionName(nameof(AddToQueue))]
        public static async Task<string> AddToQueue([ActivityTrigger]List<ToDo> todos)
        {
            await messaging.AddMessage(JsonSerializer.Serialize(todos));
            return $"Data Added To Queue";
        }




        /// <summary>
        ///  Async Http API
        ///  this will be activated when there is a HTTP Request
        ///  This is buildig the DurableClient using "IDurableOrchestrationClient" interfacae
        ///  This interface invokes the OrchestratorTrigger 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="starter"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {

            var bodyData = await new StreamReader(req.Content.ReadAsStream()).ReadToEndAsync();

            // Function input comes from the request content.
            // Execute the 
            string instanceId = await starter.StartNewAsync("ChainFunction", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}