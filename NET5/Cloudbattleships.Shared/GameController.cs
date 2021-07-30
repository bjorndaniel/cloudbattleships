using Cloudbattleships.Shared.Model;
using System;

namespace Cloudbattleships.Shared
{
    public class GameController
    {
        public static Game CreateGame(Player player) => new Game
        {
            Id = Guid.NewGuid().ToString(),
            Player1 = player,
            StartDate = DateTimeOffset.UtcNow
        };

        public static bool IsPlayerValid(Player player)=>
            player.Id != Guid.Empty.ToString() && !string.IsNullOrWhiteSpace(player.Name);

        public static Game Fire(Game game, string playerId, int row = -1, int column = -1)
        {
            if (game.Player1.Id == playerId)
            {
                if (!game.Player1.IsMyTurn)
                {
                    Console.WriteLine($"Not {game.Player1?.Name}'s turn");
                    return game;
                }
                var coordinates = new Panel(row,column);
                if (coordinates.Column < 0 || coordinates.Row < 0)
                {
                    coordinates = PlayerController.FireShot(game.Player1);
                }
                if(coordinates == null)
                {
                    return game;
                }
                Console.WriteLine($"{game.Player1?.Name} firing at Row {coordinates.Row} and Column {coordinates.Column}");
                var result = PlayerController.ProcessShot(game.Player2!, coordinates);
                PlayerController.ProcessShotResult(game.Player1!, new Coordinates { Row = coordinates.Row, Column = coordinates.Column }, result);
            }
            else
            {
                if (!game.Player2?.IsMyTurn ?? true)
                {
                    Console.WriteLine($"Not {game.Player2?.Name}'s turn");
                    return game;
                }
                var coordinates = new Panel(row, column );
                if (coordinates.Column < 0 || coordinates.Row < 0)
                {
                    coordinates = PlayerController.FireShot(game.Player2!);
                }
                if (coordinates == null)
                {
                    return game;
                }
                Console.WriteLine($"{game.Player2?.Name} firing at Row {coordinates.Row} and Column {coordinates.Column}");
                var result = PlayerController.ProcessShot(game.Player1, coordinates);
                PlayerController.ProcessShotResult(game.Player2!, new Coordinates { Row = coordinates.Row, Column = coordinates.Column }, result);
            }
            game!.Player1!.IsMyTurn = !game.Player1.IsMyTurn;
            game!.Player2!.IsMyTurn = !game.Player2.IsMyTurn;
            if (game.Player1.HasLost)
            {
                game.GameOver = true;
                game.Winner = game.Player2.Name;
                game.EndDate = DateTimeOffset.UtcNow;
            }
            else if (game.Player2.HasLost)
            {
                game.GameOver = true;
                game.Winner = game.Player1.Name;
                game.EndDate = DateTimeOffset.UtcNow;
            }
            return game;
        }
    }
}
