using Cloudbattleships.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace Cloudbattleships.Shared.Helpers
{
    public static class PanelExtensions
    {
        public static Panel At(this List<Panel> panels, int row, int column) =>
            panels.Where(x => x.Row == row && x.Column == column).First();

        public static List<Panel> Range(this List<Panel> panels, int startRow, int startColumn, int endRow, int endColumn) =>
            panels.Where(x => x.Row >= startRow
            && x.Column >= startColumn
            && x.Row <= endRow
            && x.Column <= endColumn).ToList();
    }
}
