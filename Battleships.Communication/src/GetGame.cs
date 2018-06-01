using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace BattleCom
{
    public static class GetGame
    {
        [FunctionName("getGame")]
        public static string Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var id = req.Query["id"];
            return JsonConvert.SerializeObject(CosmosHandler.GetGame(id, log));
        }
    }
}
