using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameDev.Models
{
    public enum ChipColor
    {
        White = 0,
        Black = 1,
        Gold = 2,
        Silver = 3
    }

    public class Chip
    {
        public int PosX { get; set; }
        public int PosY { get; set; }

        public ChipColor Color { get; set; }

        public Chip(int posX, int posY, ChipColor color)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.Color = color;
        }
    }
}
