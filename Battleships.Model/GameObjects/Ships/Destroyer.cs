//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using Battleships.Model.Helpers;

namespace Battleships.Model.GameObjects
{
 public class Destroyer : Ship
    {
        public Destroyer()
        {
            Name = "Destroyer";
            Width = 2;
            OccupationType = (int)Helpers.OccupationType.Destroyer;
        }
    }
}