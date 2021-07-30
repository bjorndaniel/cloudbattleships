
# Battleships

#### A serverless implementation of the classic Battleships game
Insipration and game logic from [ExceptionNotFound](https://exceptionnotfound.net/modeling-battleship-in-csharp-introduction-and-strategies/)

The game consists of the following projects:
## Battleships.Blazor.UI
Web client for the application written using [Blazor WASM](https://blazor.net)
## Battleships.Backend
A project consisting off 4 Azure functions used to communicate with the clients and the Cosmos DB
The functions are:
- InitGame - Called by a client to join a game
- Messages - Used to send messages between the clients. 
- Negotiate - Called by the clients to get the Azure SignalR connection informations (stored in an application setting AzureSignalRConnectionString)
- Fire - Used by players to send a info about a shot the the server

Check local.settings.json for the required keys.<br/>
It has default values to use a local Cosmos DB and the functions will run on localhost:7071

## Battleships.Shared
The gamelogic from [ExceptionNotFound](https://exceptionnotfound.net/modeling-battleship-in-csharp-introduction-and-strategies/)<br/>
Also contains shared helper functions and the game model

## Links and references
- [CosmosDB emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator)
- [Azure functions bindings for Azure SignalR](https://github.com/anthonychu/AzureAdvocates.WebJobs.Extensions.SignalRService)
- [Blazor](https://blazor.net)
- [Ooui](https://github.com/praeclarum/ooui)

## The future
- Add Xamarin Forms versions
- Add some animations
- Better error handling
