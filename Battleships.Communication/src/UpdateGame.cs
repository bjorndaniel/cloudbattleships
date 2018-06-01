using Battleships.Model.GameObjects;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Dynamic;

namespace BattleCom
{
    public static class UpdateGame
    {
        [FunctionName("updateGame")]
        [return: SignalR(ConnectionStringSetting = "AzureSignalRConnectionString", HubName = "simplechat")]
        public static SignalRMessage Run(
           [CosmosDBTrigger("Games", "GameCollection", ConnectionStringSetting = "CosmosConnection", CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            TraceWriter log)
        {
            dynamic data = new ExpandoObject();
            var message = new SignalRMessage
            {
                Target = "GameUpdated"
            };
            Game g = (dynamic)input[0];
            log.Info($"Game {g.id} with players {g.Player1.Name}, {g.Player2.Name} updates");
            data.id = g.id;
            data.game = g.ToDTO();
            message.Arguments = new object[] { data };
            return message;
        }
    }
}
