//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using Battleships.Model.DTO;
using Newtonsoft.Json;
using System;
namespace Battleships.Model.GameObjects
{
    public class Game
    {
        public Game()
        {
            id = Guid.Empty.ToString();
        }
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public string Winner { get; set; }
        public bool GameOver { get; set; }
        public bool HasClient(string clientId) =>
             Player1?.ClientId == clientId || Player2?.ClientId == clientId;
        public bool IsStarted => !string.IsNullOrEmpty(Player1?.Name ?? "")&& !string.IsNullOrEmpty(Player2?.Name ?? "");

        public string StatusText
        {
            get
            {
                if (string.IsNullOrEmpty(Player1?.Name)|| string.IsNullOrEmpty(Player2?.Name))
                {
                    return "Waiting for player";
                }
                if (GameOver)
                {
                    return $"The winner is {Winner}";
                }
                if (Player1.IsMyTurn)
                {
                    return $"{Player1.Name} firing";
                }
                if (Player2.IsMyTurn)
                {
                    return $"{Player2.Name} firing";
                }
                return string.Empty;
            }

        }
     
        public bool SpotAvailable => string.IsNullOrEmpty(Player2?.Name);

        public Player GetPlayer(string id)=> Player1.Id == id ? Player1 : Player2;

        public Player GetPlayerByName(string name)
        {
            if (Player1.Name == name)
            {
                return Player1;
            }
            if (Player2.Name == name)
            {
                return Player2;
            }
            return null;
        }

        public Player GetPlayerByClientId(string clientId)
        {
            if (Player1.ClientId == clientId)
            {
                return Player1;
            }
            if (Player2.ClientId == clientId)
            {
                return Player2;
            }
            return null;
        }

        public GameDTO ToDTO()
        {
            return new GameDTO
            {
                GameOver = GameOver,
                id = id,
                IsStarted = IsStarted,
                Player1 = Player1.ToDTO(),
                Player2 = Player2.ToDTO(),
                SpotAvailable = SpotAvailable,
                StatusText = StatusText
            };
        }
    }
}