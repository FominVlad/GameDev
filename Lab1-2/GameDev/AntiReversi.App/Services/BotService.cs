using Reversi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reversi.Services
{
    public class BotService
    {
        public static int[,] scoreTable = new int[4, 4] {
            { -99,  48,  -8, 6 },
            {  48,  -8, -16, 3 },
            {  -8, -16,   4, 4 },
            {   6,   3,   4, 0 }
        };

        public static Chip GetNextMove(List<Chip> availableChips)
        {
            var scores = availableChips.Select(chip => new { chip, score = Evaluate(chip) }).OrderBy(e => 100 - e.score).ToList();
            var maxScores = scores.Where(e => e.score == scores[0].score).ToList();

            Random random = new Random();
            return maxScores[random.Next(maxScores.Count)].chip;
        }

        static int Evaluate(Chip chip)
        {
            int x = chip.PosX > 3 ? 7 - chip.PosX : chip.PosX;
            int y = chip.PosY > 3 ? 7 - chip.PosY : chip.PosY;
            int score = ((x % 2) + (y % 2)) * (10 - x - y);

            return scoreTable[x, y];
        }
    }
}
