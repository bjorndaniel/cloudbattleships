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

        public static Panel? FireShot(Player player)
        {
            //If there are hits on the board with neighbors which don't have shots, we should fire at those first.
            var hitNeighbors = player.FiringBoard.GetHitNeighbors();
            Panel? panel;
            if (hitNeighbors.Any())
            {
                panel = SearchingShot(player);
            }
            else
            {
                panel = RandomShot(player);
            }
            Console.WriteLine(player.Name + " says: \"Firing shot at " + panel?.Row.ToString() + ", " + panel?.Column.ToString() + "\"");
            return panel;
        }

        public static Panel? RandomShot(Player p)
        {
            var availablePanels = p.FiringBoard.GetOpenRandomPanels();
            if (!availablePanels.Any())
            {
                return null;
            }
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var panelID = rand.Next(availablePanels.Count() - 1);
            while (panelID > availablePanels.Count())
            {
                panelID = rand.Next(availablePanels.Count() - 1);
            }
            return availablePanels.ElementAt(panelID);
        }

        public static Panel? SearchingShot(Player p)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var hitNeighbors = p.FiringBoard.GetHitNeighbors();
            var neighborID = rand.Next(hitNeighbors.Count);
            return hitNeighbors.ElementAt(neighborID);
        }

        public static ShotResult ProcessShot(Player p, Panel? hitPanel)
        {
            if (hitPanel == null)
            {
                Console.WriteLine(p.Name + " says: \"Miss!\"");
                return ShotResult.Miss;
            }
            var panel = p.GameBoard.Panels.At(hitPanel.Row, hitPanel.Column);
            Console.WriteLine($"Panel is occupied {panel.IsOccupied()}");
            if (!panel.IsOccupied())
            {
                Console.WriteLine(p.Name + " says: \"Miss!\"");
                return ShotResult.Miss;
            }
            var ship = p.Ships.First(x => x.Type == panel.Type);
            ship.Hits++;
            panel.IsHit = true;
            Console.WriteLine(p.Name + " says: \"Hit!\"");
            if (ship.IsSunk)
            {
                Console.WriteLine(p.Name + " says: \"You sunk my " + ship.Name + "!\"");
            }
            return ShotResult.Hit;
        }

        public static void ProcessShotResult(Player p, Coordinates coords, ShotResult result)
        {
            if (coords == null)
            {
                return;
            }
            var panel = p.FiringBoard.Panels.At(coords.Row, coords.Column);
            switch (result)
            {
                case ShotResult.Hit:
                    panel.Type = OccupationType.Hit;
                    break;

                default:
                    panel.Type = OccupationType.Miss;
                    break;
            }
        }

    }
}
