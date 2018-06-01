//From https://github.com/exceptionnotfound/BattleshipModellingPractice

using System.Collections.Generic;

namespace Battleships.Model.Boards
{
    /// <summary>
    /// Represents a collection of Panels to provide a Player with their Game Board (e.g. where their ships are placed).
    /// </summary>
    public class GameBoard
    {
        public List<Panel> Panels { get; set; } = new List<Panel>();

        public void Setup()
        {
            Panels = new List<Panel>();
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Panels.Add(new Panel(i, j));
                }
            }
        }
    }
}