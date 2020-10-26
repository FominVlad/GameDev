using Reversi.Models;
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

        public List<Chip> GetFrontierChips(int playerId)
        {
            List<Chip> frontierChips = new List<Chip>();

            foreach (Chip chip in GetPlayerChips(playerId))
            {
                foreach ((int x, int y) in GetDirectionVectors())
                {
                    if (IsInBoardIndex(chip.PosY + y) && IsInBoardIndex(chip.PosX + x) && Board.Chips[chip.PosY + y][chip.PosX + x] == null)
                    {
                        frontierChips.Add(chip);
                        break;
                    }
                }
            }

            return frontierChips;
        }

        public List<Chip> GetStableChips(int playerId)
        {
            List<Chip> playerChips = GetPlayerChips(playerId).ToList();
            List<Chip> stableChips = new List<Chip>();

            List<List<bool>> stableMap = new List<List<bool>>();

            for (int i = 0; i < 8; i++)
            {
                stableMap.Add(new List<bool>());

                for (int j = 0; j < 8; j++)
                {
                    stableMap[i].Add(false);
                }
            }

            bool changed = true;

            while (changed)
            {
                changed = false;

                foreach (Chip chip in playerChips)
                {
                    bool result = true;

                    List<bool> directions = GetDirectionVectors().Select(((int x, int y) e) => {
                        int tmpX = chip.PosX + e.x;
                        int tmpY = chip.PosY + e.y;

                        if (IsInBoardIndex(tmpX) && IsInBoardIndex(tmpY)) return stableMap[tmpY][tmpX];
                        else return true;
                    }).ToList();

                    for (int i = 0; i < 4; i++)
                    {
                        result = result && (directions[i] || directions[4 + i]);
                    }

                    if (result && !stableMap[chip.PosY][chip.PosX])
                    {
                        changed = true;
                        stableChips.Add(chip);
                        stableMap[chip.PosY][chip.PosX] = true;
                    }
                }
            }


            //foreach (Chip chip in GetPlayerChips(playerId))
            //{
            //    bool isStable = true;
            //    List<(int, int)> directionsWithEmptyCells = new List<(int, int)>();

            //    foreach ((int x, int y) in GetDirectionVectors())
            //    {
            //        if (!isStable) break;

            //        for (int tmpX = chip.PosX + x, tmpY = chip.PosY + y; IsInBoardIndex(tmpX) && IsInBoardIndex(tmpY); tmpX += x, tmpY += y)
            //        {
            //            if (Board.Chips[tmpY][tmpX] == null)
            //            {
            //                directionsWithEmptyCells.Add((x, y));
                            
            //                if (directionsWithEmptyCells.Any(((int, int) tuple) => tuple.Item1 == -x && tuple.Item2 == -y))
            //                {
            //                    isStable = false;
            //                }

            //                break;
            //            }
            //        }
            //    }

            //    if (isStable) stableChips.Add(chip);
            //}

            return stableChips;
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
