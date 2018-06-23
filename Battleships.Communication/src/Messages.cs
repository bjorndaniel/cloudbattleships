
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
            using (var sr = new StreamReader(req.Body))
            using (var reader = new JsonTextReader(sr))
            {
                var data = new JsonSerializer().Deserialize<ChatMessage>(reader);
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
}
