using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BattleCom.Test
{
    public class CosmosHandlerTests
    {
        public CosmosHandlerTests()
        {
            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);
                var variables = jObject
                    .GetValue("profiles")
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();
                variables.ForEach(_ =>
                {
                    Environment.SetEnvironmentVariable(_.Name, _.Value.ToString());
                });
            }

        }

        [Fact]
        public async Task Can_play_game_to_end()
        {
            var clientId = Guid.NewGuid().ToString();
            var id = await CosmosHandler.FindOpenGame("TestUser", clientId, null);
            var game = CosmosHandler.GetGame(id, null);
            var player = game.GetPlayerByClientId(clientId);
            Assert.Equal(100, player.GameBoard.Count);
            var clientId2 = Guid.NewGuid().ToString();
            var newId = await CosmosHandler.FindOpenGame("Player2", clientId2, null);
            Assert.Equal(id, newId);
            game = CosmosHandler.GetGame(newId, null);
            player = game.GetPlayerByClientId(clientId);
            Assert.Equal(100, player.GameBoard.Count);
            var player2 = game.GetPlayerByClientId(clientId2);
            Assert.Equal(100, player2.GameBoard.Count);
            await CosmosHandler.PlayToEnd(newId, null);
            game = CosmosHandler.GetGame(newId, null);
            Assert.True(game.GameOver);
            Assert.True(game.Player1.HasLost || game.Player2.HasLost);
        }
    }
}
