using System;

namespace Battleships.XamFormsUI
{
    public class TileTappedEventArgs : EventArgs
    {
        public int Column { get; set; }
        public int Row { get; set; }
    }
}
