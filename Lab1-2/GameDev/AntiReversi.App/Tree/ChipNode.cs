using Reversi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntiReversi.App.Tree
{
    public class ChipNode
    {
        public Chip Chip { get; private set; }

        public Board Board { get; private set; }

        public List<ChipNode> NextChips { get; private set; }

        public ChipNode(Board board, Chip chip)
        {
            this.Board = board;
            this.Chip = chip;
            this.NextChips = new List<ChipNode>();
        }
    }
}
