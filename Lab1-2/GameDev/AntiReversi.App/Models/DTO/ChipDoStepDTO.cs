using System.ComponentModel.DataAnnotations;

namespace Reversi.Models.DTO
{
    public class ChipDoStepDTO
    {
        /// <summary>
        /// Chip position (X).
        /// </summary>
        [Range(0, int.MaxValue, 
            ErrorMessage = "X position can`t be less than 0.")]
        public int PosX { get; set; }

        /// <summary>
        /// Chip position (Y).
        /// </summary>
        [Range(0, int.MaxValue, 
            ErrorMessage = "Y position can`t be less than 0.")]
        public int PosY { get; set; }
    }
}
