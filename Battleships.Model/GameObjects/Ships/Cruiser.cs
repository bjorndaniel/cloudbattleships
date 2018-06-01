//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using Battleships.Model.Helpers;

namespace Battleships.Model.GameObjects
{
  public class Cruiser : Ship
    {
        public Cruiser()
        {
            Name = "Cruiser";
            Width = 3;
            OccupationType = (int)Helpers.OccupationType.Cruiser;
        }
    }
}