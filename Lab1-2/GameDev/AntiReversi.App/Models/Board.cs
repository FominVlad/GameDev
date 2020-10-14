using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversi.Models
{
    public class Board
    {
        public int Size { get; private set; }
        public List<List<Chip>> Chips { get; private set; }
        public List<int> WinnerPlayerIdList { get; set; }
        public List<Chip> OccupiedChips {
            get
            {
                return Chips.SelectMany(chipList =>
                    chipList.Where(chip => chip != null)).ToList();
            }
        }

        public Board(int size)
        {
            if (size % 2 != 0)
                throw new Exception("Size must be divisible by 2 without remainder.");

            this.Size = size;
            WinnerPlayerIdList = new List<int>();
            FillEmptyChipList();
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

        public void FillBoardInitialValues(List<IPlayer> players)
        {
            int boardMid = Size / 2;

            Chips[boardMid - 1][boardMid - 1] = new Chip(players.FirstOrDefault().Id, boardMid - 1, boardMid - 1);
            Chips[boardMid][boardMid - 1] = new Chip(players.LastOrDefault().Id, boardMid - 1, boardMid);
            Chips[boardMid - 1][boardMid] = new Chip(players.LastOrDefault().Id, boardMid, boardMid - 1);
            Chips[boardMid][boardMid] = new Chip(players.FirstOrDefault().Id, boardMid, boardMid);
        }
    }
}
