using Cloudbattleships.Shared.Helpers;
using Cloudbattleships.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudbattleships.Shared
{
    public static class PlayerController
    {
        public static void PlaceShips(Player p)
        {
            if (!p.Ships.Any())
            {
                p.Setup();
            }
            //Random class creation from http://stackoverflow.com/a/18267477/106356
            var rand = new Random(Guid.NewGuid().GetHashCode());
            foreach (var ship in p.Ships)
            {
                Console.WriteLine($"Placing ship {ship.Name}");
                //Select a random row/column combination, then select a random orientation.
                //If none of the proposed panels are occupied, place the ship
                //Do this for all ships
                var isOpen = true;
                while (isOpen)
                {
                    var startcolumn = rand.Next(1, 11);
                    var startrow = rand.Next(1, 11);
                    int endrow = startrow, endcolumn = startcolumn;
                    var orientation = rand.Next(1, 101) % 2; //0 for Horizontal
                    var panelNumbers = new List<int>();
                    if (orientation == 0)
                    {
                        for (int i = 1; i < ship.Width; i++)
                        {
                            endrow++;
                        }
                    }
                    else
                    {
                        for (int i = 1; i < ship.Width; i++)
                        {
                            endcolumn++;
                        }
                    }
                    //We cannot place ships beyond the boundaries of the board
                    if (endrow > 10 || endcolumn > 10)
                    {
                        isOpen = true;
                        continue;
                    }

                    //Check if specified panels are occupied
                    var affectedPanels = p.GameBoard.Panels.Range(startrow, startcolumn, endrow, endcolumn);
                    if (affectedPanels.Any(x => x.IsOccupied()))
                    {
                        isOpen = true;
                        continue;
                    }
                    foreach (var panel in affectedPanels)
                    {
                        panel.Type = ship.Type;
                        Console.WriteLine($"Adding ship to panel: {panel.Status} for ship {ship.Name}");

                    }
                    isOpen = false;
                }
            }
        }
    }
}
