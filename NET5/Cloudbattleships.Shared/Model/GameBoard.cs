using System.Collections.Generic;
using System.Linq;

namespace Cloudbattleships.Shared.Model
{
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
        public Panel? GetPanel(int row, int colum) => 
            Panels.FirstOrDefault(_ => _.Row == row && _.Column == colum);    
        
    }
}
