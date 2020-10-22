﻿using Reversi.Models;
using Reversi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversi.Services
{
    public class BoardService
    {
        public Board Board { get; private set; }

        /// <summary>
        /// Method for initialize board.
        /// </summary>
        /// <param name="boardSize">Board size</param>
        /// <param name="players">Players list</param>
        /// <returns>Initialized board.</returns>
        public List<Chip> InitBoard(int boardSize, List<IPlayer> players, ChipDoStepDTO blackHole)
        {
            Board = new Board(boardSize, new Chip(blackHole, -1));
            Board.FillBoardInitialValues(players);

            return Board.OccupiedChips;
        }

        /// <summary>
        /// Method for initialize board.
        /// </summary>
        /// <param name="boardSize">Board size</param>
        /// <param name="players">Players list</param>
        /// <returns>Initialized board.</returns>
        public List<Chip> InitBoard(Board board)
        {
            this.Board = board;
            return Board.OccupiedChips;
        }

        /// <summary>
        /// Add chip to board.
        /// </summary>
        /// <param name="chip">Chip to add.</param>
        public void AddChipToBoard(Chip chip)
        {
            Board.Chips[chip.PosY][chip.PosX] = chip;
        }

        /// <summary>
        /// Method for flipping chips.
        /// </summary>
        /// <param name="chips">Chips to flip.</param>
        /// <param name="players">Players list.</param>
        public List<Chip> FlipChips(Chip chip, int newOwnerId)//List<IPlayer> players)
        {
            List<Chip> flippedChips = GetFlippedChips(chip);
            AddChipToBoard(chip);

            foreach (Chip flippedChip in flippedChips)
            {
                Board.Chips[flippedChip.PosY][flippedChip.PosX].OwnerId = newOwnerId;
            }

            flippedChips.Add(chip);

            return flippedChips;
        }

        /// <summary>
        /// Method for flipping chips.
        /// </summary>
        /// <param name="chips">Chips to flip.</param>
        /// <param name="players">Players list.</param>
        public Board FlipChips(Chip chip)//List<IPlayer> players)
        {
            List<Chip> flippedChips = GetFlippedChips(chip);
            flippedChips.Add(chip);

            Board newBoard = new Board(Board.Size, Board.BlackHole, Board.Chips.Select(row => 
                row.Select(c => c == null ? c : flippedChips.Exists(flippedChip => 
                    c.PosX == flippedChip.PosX && c.PosY == flippedChip.PosY) ? 
                    new Chip(chip.OwnerId, c.PosX, c.PosY) : c).ToList()).ToList());

            return newBoard;
        }

        /// <summary>
        /// Method to get available steps for player.
        /// </summary>
        /// <param name="playerId">Player unique identifier.</param>
        /// <returns>Available steps (chips) list.</returns>
        public List<Chip> GetAvailableSteps(int playerId)
        {
            if (!CheckInitBoard())
                throw new Exception("Board is not initialized.");

            List<Chip> availableChips = new List<Chip>();

            foreach (Chip chip in GetPlayerChips(playerId))
            {
                foreach (Chip availableChip in GetAvailableChips(chip))
                {
                    availableChips.Add(availableChip);
                }
            }

            return availableChips;
        }

        public bool CheckInitBoard()
        {
            if (Board == null)
                return false;

            if (Board.Chips == null)
                return false;

            return true;
        }

        /// <summary>
        /// Method to get flipped chips list (after step).
        /// </summary>
        /// <param name="chip">The chip that done step.</param>
        /// <returns>Flipped chips list.</returns>
        public List<Chip> GetFlippedChips(Chip chip)
        {
            List<Chip> flippedChips = new List<Chip>();

            foreach ((int x, int y) in GetDirectionVectors())
            {
                bool hasAllyChip = false;

                List<Chip> tmpChips = new List<Chip>();

                for (int i = 1; i < Board.Size; i++)
                {
                    int tmpChipX = chip.PosX + (i * x);
                    int tmpChipY = chip.PosY + (i * y);

                    if (!IsInBoardIndex(tmpChipX) || !IsInBoardIndex(tmpChipY))
                        break;

                    Chip tmpChip = Board.Chips[tmpChipY][tmpChipX];

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

        private IEnumerable<Chip> GetPlayerChips(int playerId)
        {
            return Board.Chips.SelectMany(chipList => 
                chipList.Where(chip => chip?.OwnerId == playerId));
        }

        private List<Chip> GetAvailableChips(Chip chip)
        {
            List<Chip> availableChips = new List<Chip>();
            
            foreach ((int x, int y) in GetDirectionVectors())
            {
                bool hasOpponentChip = false;

                for (int i = 1; i < Board.Size; i++)
                {
                    int tmpChipX = chip.PosX + (i * x);
                    int tmpChipY = chip.PosY + (i * y);

                    if (!IsInBoardIndex(tmpChipX) || !IsInBoardIndex(tmpChipY))
                        break;

                    Chip tmpChip = Board.Chips[tmpChipY][tmpChipX];

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

            availableChips.RemoveAll(chip => chip.PosX == Board.BlackHole.PosX && chip.PosY == Board.BlackHole.PosY);

            return availableChips.Distinct().ToList();
        }

        private bool IsInBoardIndex(int index)
        {
            return index < Board.Size && index >= 0;
        }

        private List<(int, int)> GetDirectionVectors()
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
