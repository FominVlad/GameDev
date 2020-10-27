using AntiReversi.App.Tree;
using Reversi.Models;
using System.Collections.Generic;
using System.Linq;

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

        public static Chip GetNextMove(Board board, List<Chip> availableChips, List<IPlayer> players)
        {
            ChipTree tree = new ChipTree(board, availableChips, players, 2);
            tree.FillTree(tree.RootChip);

            return tree.RootChip.MostProfitableChip;
        }

        public static int Evaluate(Board board, Chip chip)
        {
            int cPos = 1;
            int cMob = 1;
            int cPmob = 1;
            int cStab = 1;

            int id = chip.OwnerId;
            
            BoardService boardService = new BoardService();
            boardService.InitBoard(new Board(board));

            return cPos * Position(chip) + cMob * Mobility(boardService, id) + cPmob * 
                PotentialMobility(boardService, id) + cStab * Stability(boardService, id);
        }

        static int Position(Chip chip) {
            int x = chip.PosX > 3 ? 7 - chip.PosX : chip.PosX;
            int y = chip.PosY > 3 ? 7 - chip.PosY : chip.PosY;

            return scoreTable[x, y];
        }

        static int Mobility(BoardService boardService, int id)
        {
            return boardService.GetAvailableSteps(id).Distinct().Count();
        }

        static int PotentialMobility(BoardService boardService, int id)
        {
            return -(boardService.GetFrontierChips(id).Count());
        }

        static int Stability(BoardService boardService, int id)
        {
            return -(boardService.GetStableChips(id).Count());
        }
    }
}
