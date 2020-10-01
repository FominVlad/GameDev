using Reversi.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Reversi.Models
{
    public class Board
    {
        public int Size { get; private set; }
        public List<List<Chip>> Chips { get; private set; }

        public Board(int size)
        {
            this.Size = size;
            FillEmptyChipList();
        }

        public List<Chip> GetChipsList()
        {
            return Chips.SelectMany(chipList => chipList.Where(chip => chip != null)).ToList();
        }

        private void FillEmptyChipList()
        {
            Chips = new List<List<Chip>>();

            for (int i = 0; i < Size; i++)
            {
                Chips.Add(new List<Chip>());

                for (int j = 0; j < Size; j++)
                {
                    Chips[i].Add(null);
                }
            }
        }

        public void FillBoardInitialValues(List<Player> players)
        {
            int boardMid = Size / 2;

            Chips[boardMid - 1][boardMid - 1] = new Chip(players.FirstOrDefault().Id, boardMid - 1, boardMid - 1);
            Chips[boardMid][boardMid - 1] = new Chip(players.LastOrDefault().Id, boardMid - 1, boardMid);
            Chips[boardMid - 1][boardMid] = new Chip(players.LastOrDefault().Id, boardMid, boardMid - 1);
            Chips[boardMid][boardMid] = new Chip(players.FirstOrDefault().Id, boardMid, boardMid);
        }
    }
}
