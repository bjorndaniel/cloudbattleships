using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace BattleCom
{
    public static class PlayToEnd
    {
        [FunctionName("playToEnd")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Play to end trigger called");
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<GameMessage>(requestBody);
            dynamic mess = new ExpandoObject();
            mess.Message = $"Game {data.GameId} plays to end";
            await CosmosHandler.PlayToEnd(data.GameId, log);
            return new OkObjectResult(JsonConvert.SerializeObject(mess));
            
        }
    }
}
