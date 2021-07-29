namespace Cloudbattleships.Shared.Model
{
    public class Ship
    {
        public string Name { get; set; } = "";
        public int Width { get; set; }
        public int Hits { get; set; }
        public OccupationType Type { get; set; }
        public bool IsSunk => Hits >= Width;
    }

    public class Battleship : Ship
    {
        public Battleship()
        {
            Name = "Battleship";
            Width = 4;
            Type = OccupationType.Battleship;
        }
    }

    public class Carrier : Ship
    {
        public Carrier()
        {
            Name = "Aircraft Carrier";
            Width = 5;
            Type = OccupationType.Carrier;
        }
    }

    public class Cruiser : Ship
    {
        public Cruiser()
        {
            Name = "Cruiser";
            Width = 3;
            Type = OccupationType.Cruiser;
        }
    }

    public class Destroyer : Ship
    {
        public Destroyer()
        {
            Name = "Destroyer";
            Width = 2;
            Type = OccupationType.Destroyer;
        }
    }

    public class Submarine : Ship
    {
        public Submarine()
        {
            Name = "Submarine";
            Width = 3;
            Type = OccupationType.Submarine;
        }
    }
}
