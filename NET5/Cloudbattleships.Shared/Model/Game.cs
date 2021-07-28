using Newtonsoft.Json;
using System;

namespace Cloudbattleships.Shared.Model
{
    public class Game
    {
        [JsonProperty(PropertyName = "id")]//Using Newtonsoft here since CosmosDb does not work with System.Text.Json
        public string Id { get; set; } = Guid.Empty.ToString();
        public Player Player1 { get; set; } = new Player();
        public Player? Player2 { get; set; }
        public string Winner { get; set; } = "";
        public bool GameOver { get; set; }
        public bool HasClient(string clientId) => Player1?.ClientId == clientId || Player2?.ClientId == clientId;
        public bool IsStarted => Player1 != null && Player2 != null;
        public bool SpotAvailable => Player2 == null;
    }
}
