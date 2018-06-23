
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Battleships.Communication
{
    public static class Fire
    {
        [FunctionName("fire")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Fire trigger called");
            using (var sr = new StreamReader(req.Body))
            using (var reader = new JsonTextReader(sr))
            {
                var data = new JsonSerializer().Deserialize<FireMessage>(reader);
                await CosmosHandler.Fire(data.GameId, data.PlayerId, data.Row, data.Column, log);
                return new OkObjectResult($"{{\"Message\": \"{data.PlayerId} fired at {data.Row}, {data.Column}\"}}");
            }
        }
    }
}
