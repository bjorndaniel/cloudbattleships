//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using Battleships.Model.Boards;
using Battleships.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Battleships.Model.GameObjects
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GameBoard GameBoard { get; set; }
        public FiringBoard FiringBoard { get; set; }
        public List<Ship> Ships { get; set; }
        public bool HasLost => Ships.All(x => x.IsSunk);
        public bool IsMyTurn { get; set; }
        public string ConnectionId { get; set; }
        public string ClientId { get; set; }
        public Player(string name, string clientId = "")
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            ClientId = clientId;
            Ships = new List<Ship>()
            {
                new Destroyer(),
                new Submarine(),
                new Cruiser(),
                new Battleship(),
                new Carrier()
            };

            GameBoard = new GameBoard();
            FiringBoard = new FiringBoard();
            GameBoard.Setup();
            FiringBoard.Setup();
            if (!string.IsNullOrEmpty(name))
            {
                PlayerController.PlaceShips(this);
            }
        }
        public Player() { }

        public PlayerDTO ToDTO()
        {
            return new PlayerDTO
            {
                ClientId = ClientId,
                FiringBoard = FiringBoard.Panels.Select(_ => new PanelDTO
                {
                    Column = _.Coordinates.Column,
                    Row = _.Coordinates.Row,
                    IsHit = _.IsHit,
                    Status = _.Status,
                    IsOccupied = _.IsOccupied
                }).ToList(),
                GameBoard = GameBoard.Panels.Select(_ => new PanelDTO
                {
                    Column = _.Coordinates.Column,
                    Row = _.Coordinates.Row,
                    IsHit = _.IsHit,
                    Status = _.Status,
                    IsOccupied = _.IsOccupied
                }).ToList(),
                HasLost = HasLost,
                Id = Id,
                IsMyTurn = IsMyTurn, 
                Name = Name
            }; 
        }
    }
}