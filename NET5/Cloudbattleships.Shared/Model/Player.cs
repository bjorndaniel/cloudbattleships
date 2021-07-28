using Newtonsoft.Json;
using System;

namespace Cloudbattleships.Shared.Model
{
    public class Player
    {
        [JsonProperty(PropertyName = "id")]//Using Newtonsoft here since CosmosDb does not work with System.Text.Json
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ClientId { get; set; } = "";
        public string Name { get; set; } = "";

    }
}
