using Cloudbattleships.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudbattleships.Shared.Model
{
    public class Panel
    {
        public Panel(int row, int column)
        {
            Type = OccupationType.Empty;
            Row = row;
            Column = column;
        }
        public OccupationType Type { get; set; }
        public string Status => Type.GetAttributeOfType<DescriptionAttribute>()?.Description ?? "N/A";
        public bool IsHit { get; set; }
        public bool IsOccupied()
        {
            switch (Type)
            {
                case OccupationType.Empty:
                case OccupationType.Miss:
                case OccupationType.Hit:
                    return false;
                default:
                    return true;
            }
        }
        public bool IsRandomAvailable
        {
            get
            {
                return (Row % 2 == 0 && Column % 2 == 0)
                    || (Row % 2 == 1 && Column % 2 == 1);
            }
        }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
