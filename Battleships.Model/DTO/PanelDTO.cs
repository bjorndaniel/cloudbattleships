namespace Battleships.Model.DTO
{
    public class PanelDTO
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public bool IsHit { get; set; }
        public bool IsOccupied { get; set; }
        public string Status { get; set; }
    }
}
