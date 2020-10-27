using Reversi.Models;
using Reversi.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntiReversi.App.Tree
{
    public class ChipTree
    {
        /// <summary>
        /// Root chip node.
        /// </summary>
        public ChipNode RootChip { get; private set; }
        /// <summary>
        /// Thee depth.
        /// </summary>
        public int Deep { get; private set; }
        private List<IPlayer> Players { get; set; }
        private int Level { get; set; }

        public ChipTree(Board rootBoard, List<Chip> nextChips, List<IPlayer> players, int deep)
        {
            if (rootBoard == null)
                throw new Exception("Root board can`t be null.");
            if (players == null || players.Count == 0)
                throw new Exception("Players list can`t be null and must contain at least 1 cheap.");
            if (deep < 1)
                throw new Exception("Deep can`t be less than 1.");

            this.Level = 0;
            this.RootChip = new ChipNode(new Board(rootBoard), 
                new Chip() { OwnerId = nextChips.FirstOrDefault().OwnerId }, Level);
            this.Deep = deep;
            this.Players = players;
            

            if (nextChips != null || nextChips.Count != 0)
                FillChipNextList(RootChip, nextChips);
        }

        /// <summary>
        /// Method that fill tree.
        /// </summary>
        /// <param name="chipNode">Root node.</param>
        public void FillTree(ChipNode chipNode)
        {
            Level++;
            foreach (ChipNode chip in chipNode.NextChips)
            {
                FillTree(chip, Level);
                chip.SetCoefficient();
            }
            chipNode.SetCoefficient();
        }

        private void FillTree(ChipNode chipNode, int level)
        {
            if (level >= Deep)
                return;

            BoardService boardService = new BoardService();
            boardService.InitBoard(new Board(chipNode.Board));
            boardService.FlipChips(new Chip(chipNode.Chip), chipNode.Chip.OwnerId);
            List<Chip> availableChips = boardService.GetAvailableSteps(Players.
                Where(player => player.Id != chipNode.Chip.OwnerId).
                Select(player => player.Id).FirstOrDefault());

            if (availableChips.Count == 0)
                return;

            foreach (Chip chip in availableChips)
            {
                ChipNode nextChipNode = new ChipNode(new Board(boardService.Board), 
                    new Chip(chip), level);
                chipNode.NextChips.Add(nextChipNode);
                FillTree(nextChipNode, level + 1);
                nextChipNode.SetCoefficient();
            }
        }

        private void FillChipNextList(ChipNode chipNode, List<Chip> nextChips)
        {
            Level++;
            foreach (Chip chip in nextChips)
            {
                chipNode.NextChips.Add(new ChipNode(new Board(chipNode.Board), chip, Level));
            }
        }
    }
}
