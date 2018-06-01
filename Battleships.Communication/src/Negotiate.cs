using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Configuration;

namespace Battleships.Communication
{
    public static class Negotiate
    {
        [FunctionName("negotiate")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Negotiate trigger called.");
            var asr = new AzureSignalR(Environment.GetEnvironmentVariable("AzureSignalRConnectionString"));
            return new OkObjectResult(JsonConvert.SerializeObject(asr.GetClientConnectionInfo("simplechat")));
        }
    }
}
