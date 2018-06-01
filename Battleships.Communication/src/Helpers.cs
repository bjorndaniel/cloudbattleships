namespace Battleships.Communication
{

    public class InitGameMessage
    {
        public string User { get; set; }
        public string ClientId { get; set; }
    }

    public class GameMessage
    {
        public string GameId { get; set; }
    }
    public class ChatMessage
    {
        public string User { get; set; }
        public string Message { get; set; }
    }

    public class FireMessage
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
