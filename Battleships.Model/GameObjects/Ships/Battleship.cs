//From https://github.com/exceptionnotfound/BattleshipModellingPractice

namespace Battleships.Model.GameObjects
{
    public class Battleship : Ship
    {
        public Battleship()
        {
            Name = "Battleship";
            Width = 4;
            OccupationType = (int)Helpers.OccupationType.Battleship;
        }
    }
}