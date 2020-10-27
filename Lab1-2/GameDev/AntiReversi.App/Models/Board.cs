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
        public Chip BlackHole { get; private set; }
        public List<Chip> OccupiedChips {
            get
            {
                return Chips.SelectMany(chipList =>
                    chipList.Where(chip => chip != null)).ToList();
            }
        }

        public Board(int size, Chip blackHole)
        {
            if (size % 2 != 0)
                throw new Exception("Size must be divisible by 2 without remainder.");

            this.Size = size;
            this.BlackHole = blackHole;
            this.WinnerPlayerIdList = new List<int>();
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
            int blackPlayerId = players.Where(player => player.PlayerColour == PlayerColour.Black)
                .Select(player => player.Id)
                .FirstOrDefault();
            int whitePlayerId = players.Where(player => player.PlayerColour == PlayerColour.White)
                .Select(player => player.Id)
                .FirstOrDefault();

            Chips[boardMid - 1][boardMid - 1] = new Chip(whitePlayerId, boardMid - 1, boardMid - 1);
            Chips[boardMid][boardMid - 1] = new Chip(blackPlayerId, boardMid - 1, boardMid);
            Chips[boardMid - 1][boardMid] = new Chip(blackPlayerId, boardMid, boardMid - 1);
            Chips[boardMid][boardMid] = new Chip(whitePlayerId, boardMid, boardMid);
        }

        public Board(int size, Chip blackHole, List<List<Chip>> chips)
        {
            Chips = chips;
            this.Size = size;
            this.BlackHole = blackHole;
        }

        public Board(Board board)
        {
            this.Size = board.Size;
            this.BlackHole = new Chip(board.BlackHole);
            this.WinnerPlayerIdList = new List<int>();

            foreach (int id in board.WinnerPlayerIdList)
                WinnerPlayerIdList.Add(id);

            this.Chips = new List<List<Chip>>();

            for (int i = 0; i < board.Chips.Count; i++)
            {
                Chips.Add(new List<Chip>());

                for (int j = 0; j < board.Chips[i].Count; j++)
                {
                    Chips[i].Add(board.Chips[i][j] == null ? null : new Chip(board.Chips[i][j]));
                }
            }
        }
    }
}
