using AntiReversi.App.Tree;
using Reversi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Reversi.Services
{
    public class BotService
    {
        /// <summary>
        /// Static position scoring table for upper-left 4x4 section
        /// </summary>
        public static int[,] scoreTable = new int[4, 4] {
            { -99,  48,  -8, 6 },
            {  48,  -8, -16, 3 },
            {  -8, -16,   4, 4 },
            {   6,   3,   4, 0 }
        };

        /// <summary>
        /// Method that provides best next move for AI based on minimax algorithm
        /// </summary>
        /// <param name="board">Current board state</param>
        /// <param name="availableChips">List of available moves to pick from</param>
        /// <param name="players">List of players to pass into minimax algorithm</param>
        /// <returns>Chip with coordinates for next move</returns>
        public static Chip GetNextMove(Board board, List<Chip> availableChips, List<IPlayer> players)
        {
            ChipTree tree = new ChipTree(board, availableChips, players, 2);
            tree.FillTree(tree.RootChip);

            return tree.RootChip.MostProfitableChip;
        }

        /// <summary>
        /// Static evaluation function that returns score for given move
        /// </summary>
        /// <param name="board">Current board state</param>
        /// <param name="chip">Chip with coordinates that indicate move position</param>
        /// <returns>Integer score (more is better)</returns>
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

        /// <summary>
        /// Method that calculates position value for given move 
        /// </summary>
        /// <param name="chip">Chip with coordinates that indicate move position</param>
        /// <returns>Integer score (more is better)</returns>
        static int Position(Chip chip) {
            int x = chip.PosX > 3 ? 7 - chip.PosX : chip.PosX;
            int y = chip.PosY > 3 ? 7 - chip.PosY : chip.PosY;

            return scoreTable[x, y];
        }

        /// <summary>
        /// Method that calculates mobility for given board and player
        /// </summary>
        /// <param name="boardService"></param>
        /// <param name="id">Player ID</param>
        /// <returns>Integer score (more is better)</returns>
        static int Mobility(BoardService boardService, int id)
        {
            return boardService.GetAvailableSteps(id).Distinct().Count();
        }

        /// <summary>
        /// Method that calculates potential mobility for given board and player
        /// </summary>
        /// <param name="boardService"></param>
        /// <param name="id">Player ID</param>
        /// <returns>Integer score (more is better)</returns>
        static int PotentialMobility(BoardService boardService, int id)
        {
            return -(boardService.GetFrontierChips(id).Count());
        }

        /// <summary>
        /// Method that calculates potential mobility for given board and player
        /// </summary>
        /// <param name="boardService"></param>
        /// <param name="id">Player ID</param>
        /// <returns>Integer score (more is better)</returns>
        static int Stability(BoardService boardService, int id)
        {
            return -(boardService.GetStableChips(id).Count());
        }
    }
}
