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
        public bool HasPlayer(string id) => Player1?.Id == id || Player2?.Id == id;
        public bool IsStarted => Player1 != null && Player2 != null;
        public bool SpotAvailable => Player2 == null;
        public Player? GetPlayer(string id) => Player1.Id == id ? Player1 : Player2;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }    
    }
}
