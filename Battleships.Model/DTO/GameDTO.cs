using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Model.DTO
{
    public class GameDTO
    {
        public bool GameOver { get; set; }
        public bool SpotAvailable { get; set; }
        public string id { get; set; }
        public string StatusText { get; set; }
        public bool IsStarted { get; set; }
        public PlayerDTO Player1 { get; set; }
        public PlayerDTO Player2 { get; set; }
        public PlayerDTO GetPlayerByClientId(string clientId)
        {
            if (Player1.ClientId == clientId)
            {
                return Player1;
            }
            if (Player2.ClientId == clientId)
            {
                return Player2;
            }
            return null;
        }

    }
}
