using Reversi.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Reversi.Models
{
    public class Board
    {
        [Range(4, 8, ErrorMessage = "Board size must be between 4 and 8")]
        [Multiple(2, ErrorMessage = "Size must be multiple of 2.")]
        public int Size { get; set; }
        //[Required(ErrorMessage = "List of chips can`t be null.")]
        public List<List<Chip>> Chips { get; private set; }

        public Board(int size)
        {
            this.Size = size;
        }

        public Board() { }

        public void FillBoard(List<Player> players)
        {
            int boardMid = Size / 2;
            Chips = new List<List<Chip>>();

            for (int i = 0; i < Size; i++)
            {
                Chips.Add(new List<Chip>());

                for (int j = 0; j < Size; j++)
                {
                    Chips[i].Add(null);
                }
            }

            Chips[boardMid - 1][boardMid - 1] = new Chip(0, players.FirstOrDefault().Id, boardMid - 1, boardMid - 1);
            Chips[boardMid - 1][boardMid] = new Chip(0, players.LastOrDefault().Id, boardMid - 1, boardMid);
            Chips[boardMid][boardMid - 1] = new Chip(0, players.LastOrDefault().Id, boardMid, boardMid - 1);
            Chips[boardMid][boardMid] = new Chip(0, players.FirstOrDefault().Id, boardMid, boardMid);
        }
    }
}
