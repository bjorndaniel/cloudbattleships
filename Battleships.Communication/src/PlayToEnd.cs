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
    public static class PlayToEnd
    {
        [FunctionName("playToEnd")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Play to end trigger called");
            using (var sr = new StreamReader(req.Body))
            using (var reader = new JsonTextReader(sr))
            {
                var data = new JsonSerializer().Deserialize<GameMessage>(reader);
                await CosmosHandler.PlayToEnd(data.GameId, log);
                return new OkObjectResult($"{{\"Message\": \"Game {data.GameId} plays to end\"}}");
            }
        }
    }
}
