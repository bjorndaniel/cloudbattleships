using Battleships.Model;
using Battleships.Model.DTO;
using Battleships.Model.GameObjects;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Communication
{
    public static class CosmosHandler
    {
        private static string _endpointUri = Environment.GetEnvironmentVariable("CosmosUri");
        private static string _primaryKey = Environment.GetEnvironmentVariable("CosmosKey");
        private static string _dbName = Environment.GetEnvironmentVariable("DbName");
        private static string _collectionName = Environment.GetEnvironmentVariable("DbCollection");

        public static async Task<string> FindOpenGame(string userName, string clientId, TraceWriter log)
        {
            try
            {
                using (var client = new DocumentClient(new Uri(_endpointUri), _primaryKey))
                {
                    var freeGame = client.CreateDocumentQuery<Game>(
                       UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName),
                       new FeedOptions { MaxItemCount = 1 }
                    ).Where(g => g.SpotAvailable).ToList().FirstOrDefault();
                    if (freeGame != null)
                    {
                        var d = client.CreateDocumentQuery<Document>(
                                    UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName),
                                    new FeedOptions { MaxItemCount = 1 }
                                ).Where(g => g.Id == freeGame.id).ToList().FirstOrDefault();
                        
                        Game toUpdate = (dynamic)d;
                        log?.Info($"Found a game to join with id {freeGame.id} and Player1 {freeGame.Player1.Name} adding player with cid {clientId} and starting game");
                        GameController.AddPlayer(toUpdate, userName, clientId, false);
                        GameController.StartGame(toUpdate);
                        var replaced = await client.ReplaceDocumentAsync(d.SelfLink, toUpdate);
                        return replaced.Resource.Id;
                    }
                    else
                    {
                        log?.Info($"Creating a new game for player {userName} with clientid {clientId}");
                        freeGame = GameController.CreateGame();
                        GameController.AddPlayer(freeGame, userName, clientId);
                        var d = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName), freeGame);
                        return d.Resource.Id;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return string.Empty;
            }
        }

        public static async Task<bool> PlayToEnd(string gameId, TraceWriter log)
        {
            try
            {
                using (var client = new DocumentClient(new Uri(_endpointUri), _primaryKey))
                {
                    var game = client.CreateDocumentQuery<Game>(
                       UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName),
                       new FeedOptions { MaxItemCount = 1 }
                    ).Where(g => g.id == gameId).ToList().FirstOrDefault();
                    if (game != null)
                    {
                        var d = client.CreateDocumentQuery<Document>(
                                    UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName),
                                    new FeedOptions { MaxItemCount = 1 }
                                ).Where(g => g.Id == game.id).ToList().FirstOrDefault();
                        Game toUpdate = (dynamic)d;
                        GameController.PlayToEnd(toUpdate);
                        var replaced = await client.ReplaceDocumentAsync(d.SelfLink, toUpdate);
                    }
                    else
                    {
                        log?.Info($"Could not find game with id {gameId}");
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return false;
            }

            return true;
        }

        public static GameDTO GetGame(string gameId, TraceWriter log)
        {
            try
            {
                using (var client = new DocumentClient(new Uri(_endpointUri), _primaryKey))
                {
                    var game = client.CreateDocumentQuery<Game>(
                       UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName),
                       new FeedOptions { MaxItemCount = 1 }
                    ).Where(g => g.id == gameId).ToList().FirstOrDefault();
                    if (game != null)
                    {
                        return game.ToDTO();
                    }
                    else
                    {
                        log?.Info($"Could not find game with id {gameId}");
                    }
                }
            }
            catch (Exception e)
            {
                log?.Error(e.ToString());
                return null;
            }

            return null;
        }

        public static async Task<bool> Fire(string gameId, string playerId, int row, int column, TraceWriter log)
        {
            try
            {
                using (var client = new DocumentClient(new Uri(_endpointUri), _primaryKey))
                {
                    var game = client.CreateDocumentQuery<Game>(
                       UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName),
                       new FeedOptions { MaxItemCount = 1 }
                    ).Where(g => g.id == gameId).ToList().FirstOrDefault();
                    if (game != null)
                    {
                        var d = client.CreateDocumentQuery<Document>(
                                    UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName),
                                    new FeedOptions { MaxItemCount = 1 }
                                ).Where(g => g.Id == game.id).ToList().FirstOrDefault();
                        Game toUpdate = (dynamic)d;
                        GameController.Fire(toUpdate, playerId, row, column);
                        var replaced = await client.ReplaceDocumentAsync(d.SelfLink, toUpdate);
                    }
                    else
                    {
                        log?.Info($"Could not find game with id {gameId}");
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return false;
            }

            return true;
        }
    }
}
