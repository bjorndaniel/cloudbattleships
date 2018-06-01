//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using System.ComponentModel;

namespace Battleships.Model.Helpers
{
    public enum OccupationType
    {
        [Description(" ")]
        Empty,
        [Description("B")]
        Battleship,
        [Description("C")]
        Cruiser,
        [Description("D")]
        Destroyer,
        [Description("S")]
        Submarine,
        [Description("A")]
        Carrier,
        [Description("X")]
        Hit,
        [Description("M")]
        Miss
    }

    public enum ShotResult
    {
        Miss,
        Hit
    }
}