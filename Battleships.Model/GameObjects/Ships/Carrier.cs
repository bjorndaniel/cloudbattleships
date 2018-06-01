//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using Battleships.Model.Helpers;

namespace Battleships.Model.GameObjects
{
    public class Carrier : Ship
    {
        public Carrier()
        {
            Name = "Aircraft Carrier";
            Width = 5;
            OccupationType = (int)Helpers.OccupationType.Carrier;
        }
    }
}