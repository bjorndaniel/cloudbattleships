using Cloudbattleships.Shared.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Cloudbattleships.Shared.Model
{
    public class FiringBoard : GameBoard
    {
        public IEnumerable<Panel> GetOpenRandomPanels() => 
            Panels.Where(x => x.Type == OccupationType.Empty && x.IsRandomAvailable);

        public List<Panel> GetHitNeighbors()
        {
            List<Panel> panels = new List<Panel>();
            var hits = Panels.Where(x => x.Type == OccupationType.Hit);
            foreach (var hit in hits)
            {
                panels.AddRange(GetNeighbors(hit).ToList());
            }
            return panels.Distinct().Where(x => x.Type == OccupationType.Empty).ToList();
        }

        public List<Panel> GetNeighbors(Panel panel)
        {
            var row = panel.Row;
            var column = panel.Column;
            var panels = new List<Panel>();
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
