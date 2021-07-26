using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Cloudbattleships.Backend.Functions
{
    public static class Negotiate
    {
        [Function("Negotiate")]
        public static HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "cloudbattleships")] string connectionInfo,
            FunctionContext executionContext
        )
        {
            var logger = executionContext.GetLogger("Negotiate");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteString(connectionInfo);

            return response;
        }
    }
}
