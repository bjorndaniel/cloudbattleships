//From https://github.com/exceptionnotfound/BattleshipModellingPractice

using Battleships.Model.Helpers;
using System.ComponentModel;

namespace Battleships.Model.Boards
{
    /// <summary>
    /// The basic class for this modelling practice.  Represents a single square on the game board.
    /// </summary>
    public class Panel
    {
        public int OccupationType { get; set; }
        public Coordinates Coordinates { get; set; }

        public Panel(int row, int column)
        {
            Coordinates = new Coordinates(row, column);
            OccupationType = (int)Helpers.OccupationType.Empty;
        }

        public Panel() { }

        public string Status
        {
            get => ((OccupationType)OccupationType).GetAttributeOfType<DescriptionAttribute>().Description;
        }

        public bool IsHit {get;set;}

        public bool IsOccupied
        {
            get
            {
                return OccupationType == (int)Helpers.OccupationType.Battleship
                    || OccupationType == (int)Helpers.OccupationType.Destroyer
                    || OccupationType == (int)Helpers.OccupationType.Cruiser
                    || OccupationType == (int)Helpers.OccupationType.Submarine
                    || OccupationType == (int)Helpers.OccupationType.Carrier;
            }
        }

        public bool IsRandomAvailable
        {
            get
            {
                return (Coordinates.Row % 2 == 0 && Coordinates.Column % 2 == 0)
                    || (Coordinates.Row % 2 == 1 && Coordinates.Column % 2 == 1);
            }
        }
    }
}