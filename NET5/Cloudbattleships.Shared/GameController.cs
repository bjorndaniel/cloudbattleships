using Cloudbattleships.Shared.Model;
using System;

namespace Cloudbattleships.Shared
{
    public class GameController
    {
        public static Game CreateGame(Player player) => new Game
        {
            Id = Guid.NewGuid().ToString(),
            Player1 = player
        };

        public static bool IsPlayerValid(Player player)=>
            player.ClientId != Guid.Empty.ToString() && !string.IsNullOrWhiteSpace(player.Name) && player.Id != Guid.Empty.ToString();
    }
}
