using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Az_FnHttpAPI.Services;
using Az_FnHttpAPI.Models;

namespace Az_FnHttpAPI
{
    public  class RestAPI
    {
        IServices<Department, int> _deptServ = null;

        ResponseObject<Department> _deptResponse = null;


        Messaging queueMessaging = new Messaging();

        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="serv"></param>
        public RestAPI(IServices<Department,int> serv)
        {
            _deptServ= serv;
            _deptResponse = new ResponseObject<Department>();
        }



        /// <summary>
        /// Route: The URL Expression with Route value in it 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Get")]
        public  async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Departments")] HttpRequest req,
            ILogger log)
        {
            _deptResponse = await _deptServ.GetAsync();
            return new OkObjectResult(_deptResponse);
        }

        [FunctionName("GetById")]
        public async Task<IActionResult> GetById(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Departments/{id:int}")] HttpRequest req, int id,
           ILogger log)
        {
            _deptResponse = await _deptServ.GetAsync(id);
            return new OkObjectResult(_deptResponse);
        }

        [FunctionName("Post")]
        public async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Departments")] HttpRequest req,
            ILogger log)
        {
            // 1. Read the RequestBody
            string resuesdtBody = new StreamReader(req.Body).ReadToEnd();   
            // 2. Deserialize the JSON string into the Department Object
            var dept = JsonSerializer.Deserialize<Department>(resuesdtBody);
            _deptResponse = await _deptServ.CreateAsync(dept);

            // Add Newly Added Department in Queue

            await queueMessaging.AddMessage(JsonSerializer.Serialize(dept));


            return new OkObjectResult(_deptResponse);
        }

        /// <summary>
        /// The Route has the Route-Parameter of the type integer
        /// Default datatype for Route Parameter is string 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Put")]
        public async Task<IActionResult> Put(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Departments/{id:int}")] HttpRequest req, int id,
           ILogger log)
        {

            // 1. Read the RequestBody
            string resuesdtBody = new StreamReader(req.Body).ReadToEnd();
            // 2. Deserialize the JSON string into the Department Object
            var dept = JsonSerializer.Deserialize<Department>(resuesdtBody);
            _deptResponse = await _deptServ.UpdateAsync(id,dept);
            return new OkObjectResult(_deptResponse);
        }

        [FunctionName("Delete")]
        public async Task<IActionResult> Delete(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Departments/{id:int}")] HttpRequest req, int id,
        ILogger log)
        {
            _deptResponse = await _deptServ.DeleteAsync(id);
            return new OkObjectResult(_deptResponse);
        }

    }
}
