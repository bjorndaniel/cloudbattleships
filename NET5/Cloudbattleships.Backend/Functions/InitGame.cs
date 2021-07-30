using Cloudbattleships.Shared;
using Cloudbattleships.Shared.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudbattleships.Backend.Functions
{
    public static class InitGame
    {
        [Function("InitGame")]
        [SignalROutput(HubName = "cloudbattleships")]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext
        )
        {
            var logger = executionContext.GetLogger("InitGame");
            logger.LogInformation("C# HTTP trigger function processed a request.");
            var player = await JsonSerializer.DeserializeAsync<Player>(req.Body);
            if (!GameController.IsPlayerValid(player))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            var game = await CosmosDbHandler.FindOpenGameAsync(player, logger);
            return new
            {
                Target = "gameUpdated",
                Arguments = new Game[]
                {
                    game
                }
            };
        }
    }
}
