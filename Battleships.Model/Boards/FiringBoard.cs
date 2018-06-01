//From https://github.com/exceptionnotfound/BattleshipModellingPractice

using System.Collections.Generic;
using System.Linq;
using Battleships.Model.Helpers;

namespace Battleships.Model.Boards
{
     /// <summary>
    /// Represents a collection of Panels to show where the player has fired shots, and whether those shots are hits or misses.
    /// </summary>
    public class FiringBoard : GameBoard
    {
        public List<Coordinates> GetOpenRandomPanels()
        {
            return Panels.Where(x => x.OccupationType == (int)OccupationType.Empty && x.IsRandomAvailable).Select(x=>x.Coordinates).ToList();
        }

        public List<Coordinates> GetHitNeighbors()
        {
            List<Panel> panels = new List<Panel>();
            var hits = Panels.Where(x => x.OccupationType == (int)OccupationType.Hit);
            foreach(var hit in hits)
            {
                panels.AddRange(GetNeighbors(hit.Coordinates).ToList());
            }
            return panels.Distinct().Where(x => x.OccupationType == (int)OccupationType.Empty).Select(x => x.Coordinates).ToList();
        }

        public List<Panel> GetNeighbors(Coordinates coordinates)
        {
            int row = coordinates.Row;
            int column = coordinates.Column;
            List<Panel> panels = new List<Panel>();
            if (column > 1)
            {
                panels.Add(Panels.At(row, column - 1));
            }
            if (row > 1)
            {
                panels.Add(Panels.At(row - 1, column));
            }
            if (row < 10)
            {
                panels.Add(Panels.At(row + 1, column));
            }
            if (column < 10)
            {
                panels.Add(Panels.At(row, column + 1));
            }
            return panels;
        }
    }
}