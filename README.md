
# Battleships
#### A serverless implementation of the classic Battleships game
Insipration and game logic from [ExceptionNotFound](https://exceptionnotfound.net/modeling-battleship-in-csharp-introduction-and-strategies/)

The game consists of the following projects:
## Battleships.BlazorUI
Web client for the application written using the experimental [Blazor framework](https://blazor.net)
## Battleships.Communication
A project consisting off 7 Azure functions used to communicate with the clients and the Cosmos DB
The functions are:
- InitGame - Called by a client to join a game
- Messages - Used to send messages between the clients. 
- Negotiate - called by the clients to get the Azure SignalR connection informations (stored in an application setting AzureSignalRConnectionString)
- PlayToEnd - Called by a client, will use the gamecontroller to play automatically until one client has won
- UpdateGame - Triggered by a CosmosDB update, will broadcast the updated game to all clients

Check local.settings.json for the required keys.<br/>
It has default values to use a local Cosmos DB and the functions will run on localhost:7071

## Battleships.Model
The gamelogic from [ExceptionNotFound](https://exceptionnotfound.net/modeling-battleship-in-csharp-introduction-and-strategies/)<br/>
Also contains a class used by the clients to connect to the functions called ConnectionSettings.cs

## Battleships.OouiCoreUi
A [Ooui](https://github.com/praeclarum/ooui) version of the Xamarin forms client. Using asp.net core as a server.

## Battleships.Tests
A xunit project to put tests in.

## Battleships.XamFormsUI
A [Xamarin forms](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/) project with 3 clients
- Android (not working right now due to a [signalR client issue](https://github.com/aspnet/SignalR/issues/1886))
- iOS (not working right now due to a [signalR client issue](https://github.com/aspnet/SignalR/issues/1886))
- UWP

## Links and references
- [CosmosDB emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator)
- [Azure functions bindings for Azure SignalR](https://github.com/anthonychu/AzureAdvocates.WebJobs.Extensions.SignalRService)
- [Blazor](https://blazor.net)
- [Ooui](https://github.com/praeclarum/ooui)

## The future
- Solve the shared? ui for Ooui
- Get the iOS and Android versions working
- Add an [Uno version](http://platform.uno/)
- Refactor the UI
- Add some animations
- Better error handling
