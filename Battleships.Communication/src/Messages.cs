
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Dynamic;
using System.IO;

namespace Battleships.Communication
{
    public static class Messages
    {
        [FunctionName("messages")]
        [return: SignalR(ConnectionStringSetting = "AzureSignalRConnectionString", HubName = "simplechat")]
        public static SignalRMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("A message was received");
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject<ChatMessage>(requestBody);
            dynamic mess = new ExpandoObject();
            mess.User = data.User;
            mess.Message = data.Message;
            return new SignalRMessage
            {
                Target = "ReceiveMessage",
                Arguments = new object[] { data }
            };
        }
    }
}
