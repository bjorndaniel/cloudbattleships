
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Dynamic;
using System.Threading.Tasks;

namespace Battleships.Communication
{
    public static class Fire
    {
        [FunctionName("fire")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Fire trigger called");
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<FireMessage>(requestBody);
            dynamic mess = new ExpandoObject();
            mess.Message = $"{data.PlayerId} fired at {data.Row}, {data.Column}";
            await CosmosHandler.Fire(data.GameId, data.PlayerId, data.Row, data.Column, log);
            return new OkObjectResult(JsonConvert.SerializeObject(mess));
        }
    }
}
