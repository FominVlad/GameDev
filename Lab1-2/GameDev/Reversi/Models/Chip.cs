using Reversi.Models.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reversi.Models
{
    public class Chip
    {
        /// <summary>
        /// Chip owner unique id.
        /// </summary>
        public int OwnerId { get; set; }
        /// <summary>
        /// Chip position (X).
        /// </summary>
        public int PosX { get; set; }
        /// <summary>
        /// Chip position (Y).
        /// </summary>
        public int PosY { get; set; }

        public Chip(ChipDoStepDTO chipDoStepDTO, int playerId) 
        {
            this.OwnerId = playerId;
            this.PosX = chipDoStepDTO.PosX;
            this.PosY = chipDoStepDTO.PosY;
        }

        public Chip(int ownerId, int posX, int posY)
        {
            this.OwnerId = ownerId;
            this.PosX = posX;
            this.PosY = posY;
        }

        public override bool Equals(object obj)
        {
            Chip chip = obj as Chip;

            if (chip == null ||
                chip.OwnerId != this.OwnerId ||
                chip.PosX != this.PosX ||
                chip.PosY != this.PosY)
                return false;

            return true;
        }
    }
}
