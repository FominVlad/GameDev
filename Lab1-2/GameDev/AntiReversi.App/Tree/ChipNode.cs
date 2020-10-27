using Reversi.Models;
using Reversi.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntiReversi.App.Tree
{
    public class ChipNode
    {
        /// <summary>
        /// Chip body.
        /// </summary>
        public Chip Chip { get; private set; }
        /// <summary>
        /// A board without this chip.
        /// </summary>
        public Board Board { get; private set; }
        /// <summary>
        /// List of child chips.
        /// </summary>
        public List<ChipNode> NextChips { get; private set; }
        /// <summary>
        /// Chip profitable coefficient.
        /// </summary>
        public int Coefficient { get; private set; }
        /// <summary>
        /// The most profitable chip of childs chips.
        /// </summary>
        public Chip MostProfitableChip { get; private set; }
        /// <summary>
        /// Chip level on the tree.
        /// </summary>
        public int Level { get; private set; }

        public ChipNode(Board board, Chip chip, int level)
        {
            this.Board = board ?? throw new Exception("Board can`t be null.");
            this.Chip = chip ?? throw new Exception("Chip can`t be null.");

            if (level < 0)
                throw new Exception("Chip level can`t be less than 0.");

            this.Level = level;
            this.NextChips = new List<ChipNode>();
        }

        /// <summary>
        /// Method to set profitable coefficient to this ChipNode.
        /// </summary>
        public void SetCoefficient()
        {
            if (NextChips.Count == 0)
            {
                Coefficient = BotService.Evaluate(Board, Chip);
            }
            else
            {
                // Level that divides without modulo - bot`s chips.
                // Others - opponent`s chips.
                Coefficient = Level % 2 == 0 ? 
                    NextChips.Max(chipNode => chipNode.Coefficient) : 
                    NextChips.Min(chipNode => chipNode.Coefficient);

                MostProfitableChip = NextChips
                    .Where(chipNode => chipNode.Coefficient == Coefficient)
                    .Select(chipNode => chipNode.Chip).FirstOrDefault();
            }
        }
    }
}
