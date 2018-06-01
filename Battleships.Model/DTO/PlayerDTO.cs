using System.Collections.Generic;

namespace Battleships.Model.DTO
{
    public class PlayerDTO
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Name { get; set; }
        public List<PanelDTO> GameBoard { get; set; }
        public List<PanelDTO> FiringBoard { get; set; }
        public bool IsMyTurn { get; set; }
        public bool HasLost { get; set; }
    }
}
