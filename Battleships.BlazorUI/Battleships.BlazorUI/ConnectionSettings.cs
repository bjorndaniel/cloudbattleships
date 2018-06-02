namespace Battleships.BlazorUI
{
    public class ConnectionSettings
    {
        public string BaseUrl { get; set; } = "http://localhost:7071";//Replace with your azure functions url when deploying to azure
        public string Fire { get; set; } = "/api/fire";//If your azure function uses a key add ?code=thegeneratedcode
        public string InitGame { get; set; } = "/api/initGame";//If your azure function uses a key add ?code=thegeneratedcode
        public string Messages { get; set; } = "/api/messages";//If your azure function uses a key add ?code=thegeneratedcode
        public string Negotiate { get; set; } = "/api/negotiate";//If your azure function uses a key add ?code=thegeneratedcode
        public string PlayToEnd { get; set; } = "/api/playtoend";//If your azure function uses a key add ?code=thegeneratedcode
    }
}
