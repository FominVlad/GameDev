using Reversi.Models;
using Reversi.Models.DTO;
using Reversi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiReversi.App.Tree
{
    public class ChipTree
    {
        public ChipNode RootChip { get; private set; }
        public int Deep { get; private set; }
        private PlayerService PlayerService { get; set; }

        public ChipTree(Board rootBoard, List<Chip> nextChips, PlayerService playerService, int deep)
        {
            this.RootChip = new ChipNode(new Board(rootBoard), new Chip() { OwnerId = nextChips.FirstOrDefault().OwnerId });
            this.Deep = deep;
            this.PlayerService = playerService;

            FillChipNextList(RootChip, nextChips);
            //FillTree(RootChip, 0);
            FillChipNextList(RootChip);
        }

        private void FillTree(ChipNode chipNode, int level)
        {
            if (level == Deep)
                return;

            BoardService boardService = new BoardService();
            boardService.InitBoard(new Board(chipNode.Board));
            boardService.FlipChips(chipNode.Chip, chipNode.Chip.OwnerId);

            List<Chip> availableChips = boardService.GetAvailableSteps(PlayerService.Players.Where(player => player.Id != chipNode.Chip.OwnerId).Select(player => player.Id).FirstOrDefault());

            foreach (Chip chip in availableChips)
            {
                ChipNode nextChipNode = new ChipNode(new Board(boardService.Board), chip);
                chipNode.NextChips.Add(nextChipNode);
            }

            foreach (ChipNode chip in chipNode.NextChips)
            {
                FillTree(chip, level + 1);
            }
        }

        private void FillChipNextList(ChipNode chipNode)
        {
            foreach (ChipNode chip in RootChip.NextChips)
                FillTree(chip, 1);
        }

        private void FillChipNextList(ChipNode chipNode, List<Chip> nextChips)
        {
            foreach (Chip chip in nextChips)
            {
                chipNode.NextChips.Add(new ChipNode(new Board(chipNode.Board), chip));
            }
        }
    }
}
