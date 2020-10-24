using Reversi.Models;
using Reversi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace AntiReversi.App.Tree
{
    public class ChipNode
    {
        public Chip Chip { get; private set; }

        public Board Board { get; private set; }

        public List<ChipNode> NextChips { get; private set; }

        public int Coefficient { get; private set; }

        public Chip MostProfitableChip { get; private set; }
        public int Level { get; private set; }

        public ChipNode(Board board, Chip chip, int level)
        {
            this.Board = board;
            this.Chip = chip;
            this.Level = level;
            this.NextChips = new List<ChipNode>();
        }

        public void SetCoefficient()
        {
            if (NextChips.Count == 0)
            {
                Coefficient = BotService.Evaluate(Board, Chip);
            }
            else
            {
                Coefficient = Level % 2 == 0 ? NextChips.Min(chipNode => chipNode.Coefficient) : NextChips.Max(chipNode => chipNode.Coefficient);
                MostProfitableChip = NextChips.Where(chipNode => chipNode.Coefficient == Coefficient)
                    .Select(chipNode => chipNode.Chip).FirstOrDefault();
            }
        }
    }
}
