//From https://github.com/exceptionnotfound/BattleshipModellingPractice

namespace Battleships.Model.GameObjects
{
    /// <summary>
    /// Represents a player's ship as placed on their Game Board.
    /// </summary>
    public class Ship
    {
        public Ship() { }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Hits { get; set; }
        public int OccupationType { get; set; }
        public bool IsSunk
        {
            get
            {
                return Hits >= Width;
            }
        }
    }
}