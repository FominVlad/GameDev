using Reversi.Models;
using Reversi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversi.Services
{
    public class StaticBoardService
    {
        /// <summary>
        /// Method for flipping chips.
        /// </summary>
        /// <param name="chips">Chips to flip.</param>
        /// <param name="players">Players list.</param>
        public static Board FlipChips(Board board, Chip chip)//List<IPlayer> players)
        {
            List<Chip> flippedChips = GetFlippedChips(board, chip);
            flippedChips.Add(chip);

            //Board newBoard = (Board) board.Clone();
            //newBoard.SetChips(board.Chips.Select(row => row.Select(c => flippedChips.Exists(flippedChip => c.PosX == flippedChip.PosX && c.PosY == flippedChip.PosY) ? new Chip(chip.OwnerId, c.PosX, c.PosY) : c).ToList()).ToList());

            Board newBoard = new Board(board.Size, board.BlackHole, board.Chips.Select(row => row.Select(c => c == null ? c : flippedChips.Exists(flippedChip => c.PosX == flippedChip.PosX && c.PosY == flippedChip.PosY) ? new Chip(chip.OwnerId, c.PosX, c.PosY) : c).ToList()).ToList());

            return newBoard;
        }

        /// <summary>
        /// Method to get available steps for player.
        /// </summary>
        /// <param name="playerId">Player unique identifier.</param>
        /// <returns>Available steps (chips) list.</returns>
        public static List<Chip> GetAvailableSteps(Board board, int playerId)
        {
            if (board == null)
                throw new Exception("Board is not initialized.");

            List<Chip> availableChips = new List<Chip>();

            foreach (Chip chip in GetPlayerChips(board, playerId))
            {
                foreach (Chip availableChip in GetAvailableChips(board, chip))
                {
                    availableChips.Add(availableChip);
                }
            }

            return availableChips;
        }

        /// <summary>
        /// Method to get flipped chips list (after step).
        /// </summary>
        /// <param name="chip">The chip that done step.</param>
        /// <returns>Flipped chips list.</returns>
        public static List<Chip> GetFlippedChips(Board board, Chip chip)
        {
            List<Chip> flippedChips = new List<Chip>();

            foreach ((int x, int y) in GetDirectionVectors())
            {
                bool hasAllyChip = false;

                List<Chip> tmpChips = new List<Chip>();

                for (int i = 1; i < board.Size; i++)
                {
                    int tmpChipX = chip.PosX + (i * x);
                    int tmpChipY = chip.PosY + (i * y);

                    if (!IsInBoardIndex(board, tmpChipX) || !IsInBoardIndex(board, tmpChipY))
                        break;

                    Chip tmpChip = board.Chips[tmpChipY][tmpChipX];

                    if (tmpChip == null)
                        break;

                    if (tmpChip != null && tmpChip.OwnerId != chip.OwnerId)
                        tmpChips.Add(tmpChip);

                    if (tmpChip != null && tmpChip.OwnerId == chip.OwnerId)
                    {
                        hasAllyChip = true;
                        break;
                    }
                }

                if (hasAllyChip)
                    flippedChips.AddRange(tmpChips);
            }

            return flippedChips;
        }

        private static IEnumerable<Chip> GetPlayerChips(Board board, int playerId)
        {
            return board.Chips.SelectMany(chipList =>
                chipList.Where(chip => chip?.OwnerId == playerId));
        }

        private static List<Chip> GetAvailableChips(Board board, Chip chip)
        {
            List<Chip> availableChips = new List<Chip>();

            foreach ((int x, int y) in GetDirectionVectors())
            {
                bool hasOpponentChip = false;

                for (int i = 1; i < board.Size; i++)
                {
                    int tmpChipX = chip.PosX + (i * x);
                    int tmpChipY = chip.PosY + (i * y);

                    if (!IsInBoardIndex(board, tmpChipX) || !IsInBoardIndex(board, tmpChipY))
                        break;

                    Chip tmpChip = board.Chips[tmpChipY][tmpChipX];

                    if (tmpChip != null && tmpChip.OwnerId != chip.OwnerId)
                    {
                        hasOpponentChip = true;
                        continue;
                    }

                    if ((tmpChip == null && !hasOpponentChip) ||
                        (tmpChip != null && hasOpponentChip && tmpChip.OwnerId == chip.OwnerId)
                        //||
                        //(tmpChipX == Board.BlackHole.PosX && tmpChipY == Board.BlackHole.PosY)
                        )
                        break;

                    if (tmpChip == null && hasOpponentChip)
                    {
                        availableChips.Add(new Chip(chip.OwnerId, tmpChipX, tmpChipY));
                        break;
                    }
                }
            }

            availableChips.RemoveAll(chip => chip.PosX == board.BlackHole.PosX && chip.PosY == board.BlackHole.PosY);

            return availableChips.Distinct().ToList();
        }

        private static bool IsInBoardIndex(Board board, int index)
        {
            return index < board.Size && index >= 0;
        }

        private static List<(int, int)> GetDirectionVectors()
        {
            return new List<(int, int)>()
            {
                (-1, -1),
                (0, -1),
                (1, -1),
                (1, 0),
                (1, 1),
                (0, 1),
                (-1, 1),
                (-1, 0)
            };
        }
    }
}
