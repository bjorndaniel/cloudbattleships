//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using Battleships.Model.Helpers;

namespace Battleships.Model.GameObjects
{
    public class Submarine : Ship
    {
        public Submarine()
        {
            Name = "Submarine";
            Width = 3;
            OccupationType = (int)Helpers.OccupationType.Submarine;
        }
    }
}