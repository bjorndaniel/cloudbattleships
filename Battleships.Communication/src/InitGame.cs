using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace Battleships.Communication
{
    public static class InitGame
    {
        
        [FunctionName("initGame")]
        [return: SignalR(ConnectionStringSetting = "AzureSignalRConnectionString", HubName = "simplechat")]
        public static async Task<SignalRMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("A user wants to join or create a game");
            return await GetMessage(req, log);
        }

        private static async Task<SignalRMessage> GetMessage(HttpRequest req, TraceWriter log)
        {
            using (var sr = new StreamReader(req.Body))
            using (var reader = new JsonTextReader(sr))
            {
                var data = new JsonSerializer().Deserialize<InitGameMessage>(reader);
                dynamic mess = new ExpandoObject();
                mess.User = "Server";
                mess.Message = $"A player connected to game server as {data.User}";
                await CosmosHandler.FindOpenGame(data.User, data.ClientId, log);
                return new SignalRMessage
                {
                    Target = "ReceiveMessage",
                    Arguments = new object[] { mess }
                };
            }
        }
    }

}
