using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudbattleships.Shared.Model
{
    public class Player
    {
        [JsonProperty(PropertyName = "id")]//Using Newtonsoft here since CosmosDb does not work with System.Text.Json
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public GameBoard GameBoard { get; set; } = new GameBoard();
        public FiringBoard FiringBoard { get; set; } = new FiringBoard();
        public List<Ship> Ships { get; set; } = new List<Ship>();
        public bool IsMyTurn { get; set; }
        public bool HasLost => Ships.All(x => x.IsSunk);
        public void Setup()
        {
            Ships = new List<Ship>()
            {
                new Destroyer(),
                new Submarine(),
                new Cruiser(),
                new Battleship(),
                new Carrier()
            };
            GameBoard.Setup();
            FiringBoard.Setup();
        }
    }
}
