using Cloudbattleships.Shared;
using Cloudbattleships.Shared.Model;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Cloudbattleships.Backend
{
    public static class CosmosDbHandler
    {
        private static string _endpointUri = Environment.GetEnvironmentVariable("CosmosUri");
        private static string _primaryKey = Environment.GetEnvironmentVariable("CosmosKey");
        private static string _dbName = Environment.GetEnvironmentVariable("DbName");
        private static string _collectionName = Environment.GetEnvironmentVariable("DbCollection");
        public static async Task<Game> FindOpenGameAsync(Player player, ILogger logger)
        {
            try
            {
                using var client = new DocumentClient(new Uri(_endpointUri), _primaryKey);
                var options = new FeedOptions
                {
                    EnableCrossPartitionQuery = true
                };
                var collectionUri = UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName);
                var existingGame = client.CreateDocumentQuery<Game>(collectionUri, feedOptions: options)
                    .Where(_ => _.SpotAvailable).AsEnumerable().FirstOrDefault();
                PlayerController.PlaceShips(player);
                if (existingGame != null)
                {
                    var player1 = new Random().Next(1, 100) < 50;
                    existingGame.Player2 = player;
                    existingGame.Player1.IsMyTurn = player1;
                    existingGame.Player2.IsMyTurn = !player1;
                    await client.UpsertDocumentAsync(collectionUri, existingGame);
                }
                else
                {
                    existingGame = GameController.CreateGame(player);
                    await client.CreateDocumentAsync(collectionUri, existingGame);
                }
                return existingGame;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return null;
            }
        }

        public static async Task<Game> SaveGameAsync(Game game, ILogger logger)
        {
            try
            {
                using var client = new DocumentClient(new Uri(_endpointUri), _primaryKey);
                var options = new FeedOptions
                {
                    EnableCrossPartitionQuery = true
                };
                var collectionUri = UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName);
                await client.UpsertDocumentAsync(collectionUri, game);
              
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
            }
            return game;
        }

        public static Game? FindGame(string gameId, ILogger logger)
        {
            try
            {
                using var client = new DocumentClient(new Uri(_endpointUri), _primaryKey);
                var options = new FeedOptions
                {
                    EnableCrossPartitionQuery = true
                };
                var collectionUri = UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName);
                var existingGame = client.CreateDocumentQuery<Game>(collectionUri, feedOptions: options)
                    .Where(_ => _.Id == gameId).AsEnumerable().FirstOrDefault();
                return existingGame;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return null;
            }
        }
    }
}
