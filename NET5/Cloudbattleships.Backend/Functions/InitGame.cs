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
        public static async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "cloudbattleships")] string connectionInfo,
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
            if(game == null)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteString(JsonSerializer.Serialize(game));
            return response;
        }
    }
}
