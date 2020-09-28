using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models
{
    public class Chip
    {
        /// <summary>
        /// Unique chip id.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Id can`t be less than 0.")]
        public int Id { get; private set; }
        /// <summary>
        /// Chip owner unique id.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Owner id can`t be less than 0.")]
        public int OwnerId { get; set; }
        /// <summary>
        /// Chip position (X).
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "X position can`t be less than 0.")]
        public int PosX { get; set; }
        /// <summary>
        /// Chip position (Y).
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Y position can`t be less than 0.")]
        public int PosY { get; set; }

        public Chip() { }

        public Chip(int id, int ownerId, int posX, int posY)
        {
            this.Id = id;
            this.OwnerId = ownerId;
            this.PosX = posX;
            this.PosY = posY;
        }
    }
}
