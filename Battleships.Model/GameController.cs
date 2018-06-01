using System;
using Battleships.Model.Boards;
using Battleships.Model.DTO;
using Battleships.Model.GameObjects;

namespace Battleships.Model
{
    public static class GameController
    {
        public static Game CreateGame(string player1 = "", string player2 = "")
        {
            var g = new Game();
            AddPlayer(g, player1, string.Empty);
            AddPlayer(g, player2, string.Empty, false);
            return g;
        }

        public static GameDTO CreateGameDTO() =>
            CreateGame().ToDTO();
        public static void AddPlayer(Game g, string name, string clientId, bool isPlayer1 = true)
        {
            if (isPlayer1)
            {
                Console.WriteLine($"Adding player 1 with name {name}");
                g.Player1 = CreatePlayer(name, clientId);
            }
            else
            {
                Console.WriteLine($"Adding player 2 with name {name}");
                g.Player2 = CreatePlayer(name, clientId);
            }
            if (g.id == Guid.Empty.ToString())
            {
                g.id = Guid.NewGuid().ToString();
            }
        }

        public static void StartGame(Game g)
        {
            if (!g.SpotAvailable)
            {
                g.Player1.IsMyTurn = true;
            }
            else
            {
                throw new ArgumentException("Game can only be started with two players");
            }
        }

        public static void PlayRound(Game g)
        {
            //Each exchange of shots is called a Round.
            //One round = Player 1 fires a shot, then Player 2 fires a shot.
            var coordinates = PlayerController.FireShot(g.Player1);
            var result = PlayerController.ProcessShot(g.Player2, coordinates);
            PlayerController.ProcessShotResult(g.Player1, coordinates, result);

            if (!g.Player2.HasLost)//If player 2 already lost, we can't let them take another turn.
            {
                coordinates = PlayerController.FireShot(g.Player2);
                result = PlayerController.ProcessShot(g.Player1, coordinates);
                PlayerController.ProcessShotResult(g.Player2, coordinates, result);
            }
        }

        public static void Fire(Game g, string playerId, int row = -1, int column = -1)
        {
            if (g.Player1.Id == playerId)
            {
                if (!g.Player1.IsMyTurn)
                {
                    Console.WriteLine($"Not {g.Player1?.Name}'s turn");
                    return;
                }
                var coordinates = new Coordinates { Row = row, Column = column };
                if (coordinates.Column < 0 || coordinates.Row < 0)
                {
                    coordinates = PlayerController.FireShot(g.Player1);
                }
                Console.WriteLine($"{g.Player1?.Name} firing at Row {coordinates.Row} and Column {coordinates.Column}");
                var result = PlayerController.ProcessShot(g.Player2, coordinates);
                PlayerController.ProcessShotResult(g.Player1, coordinates, result);
            }
            else
            {
                if (!g.Player2.IsMyTurn)
                {
                    Console.WriteLine($"Not {g.Player2?.Name}'s turn");
                    return;
                }
                var coordinates = new Coordinates { Row = row, Column = column };
                if (coordinates.Column < 0 || coordinates.Row < 0)
                {
                    coordinates = PlayerController.FireShot(g.Player2);
                }
                Console.WriteLine($"{g.Player2?.Name} firing at Row {coordinates.Row} and Column {coordinates.Column}");
                var result = PlayerController.ProcessShot(g.Player1, coordinates);
                PlayerController.ProcessShotResult(g.Player2, coordinates, result);
            }
            g.Player1.IsMyTurn = !g.Player1.IsMyTurn;
            g.Player2.IsMyTurn = !g.Player2.IsMyTurn;
            if (g.Player1.HasLost)
            {
                g.GameOver = true;
                g.Winner = g.Player2.Name;
            }
            else if (g.Player2.HasLost)
            {
                g.GameOver = true;
                g.Winner = g.Player1.Name;
            }
        }

        public static void PlayToEnd(Game g)
        {
            while (!g.Player1.HasLost && !g.Player2.HasLost)
            {
                PlayRound(g);
            }
            g.GameOver = true;
            g.Winner = g.Player1.HasLost ? g.Player2.Name : g.Player1.Name;

        }

        private static Player CreatePlayer(string name, string clientId = "") =>
            new Player(name, clientId);
    }
}