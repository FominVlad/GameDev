using Reversi.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Reversi.Models.DTO
{
    public class PlayerCreateDTO
    {
        /// <summary>
        /// Player name.
        /// </summary>
        [Required(ErrorMessage = "Player name can`t be null.")]
        [StringLength(25, MinimumLength = 1, 
            ErrorMessage = "Name length can be between 1 and 25.")]
        public string Name { get; set; }

        /// <summary>
        /// Player type.
        /// </summary>
        [InEnumRange(typeof(PlayerType), 
            ErrorMessage = "Player type must be in PlayerType range.")]
        public PlayerType PlayerType { get; set; }

        /// <summary>
        /// Player chip colour.
        /// </summary>
        [InEnumRange(typeof(PlayerColour), 
            ErrorMessage = "Player colour must be in PlayerColour range.")]
        public PlayerColour PlayerColour { get; set; }
    }
}
