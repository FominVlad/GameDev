using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameDev.Models
{
    public enum BoardColor
    {
        Green = 0,
        Blue = 1
    }

    public class Board
    {
        public int Size { get; private set; }
        public BoardColor Color { get; private set; }
        public List<Chip> Chips { get; set; }
    }
}
