using Reversi.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reversi.Models
{
    public class Board
    {
        [Range(4, 8, ErrorMessage = "Board size must be between 4 and 8")]
        [Multiple(2, ErrorMessage = "Size must be multiple of 2.")]
        public int Size { get; private set; }
        [Required(ErrorMessage = "List of chips can`t be null.")]
        public List<Chip> Chips { get; private set; }

        public Board(int size)
        {
            this.Size = size;
        }
    }
}
