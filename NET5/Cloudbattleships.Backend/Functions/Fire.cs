using Cloudbattleships.Shared;
using Cloudbattleships.Shared.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cloudbattleships.Backend.Functions
{
    public static class Fire
    {
        [Function("Fire")]
        [SignalROutput(HubName = "cloudbattleships")]
        public static async Task<object> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Messages");
            logger.LogInformation("C# HTTP trigger function processed a request.");
            var message = await req.ReadFromJsonAsync<FireMessage>();
            var game = CosmosDbHandler.FindGame(message?.GameId, logger);
            if(game == null)
            {
                return null;
            }
            game = GameController.Fire(game, message.PlayerId, message.Row, message.Column);
            game = await CosmosDbHandler.SaveGameAsync(game, logger);
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
