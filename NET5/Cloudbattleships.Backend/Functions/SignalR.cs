using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace Cloudbattleships.Backend.Functions
{
    public static class SignaR
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate
        (
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            [SignalRConnectionInfo(HubName = "cloudbattleships")] SignalRConnectionInfo connectionInfo
        ) => connectionInfo;
    }
}
