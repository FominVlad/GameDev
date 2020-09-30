using Microsoft.Extensions.Configuration;
using Reversi.Models;
using Reversi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Reversi.Services
{
    public class GameService
    {
        public Board Board { get; private set; }
        public List<Player> Players { get; private set; }

        public GameService()
        {
            Players = new List<Player>();
        }

        public void AddPlayers(List<PlayerAddDTO> players)
        {
            if (players == null)
                throw new Exception("Players list can`t be null.");
            if (players.Count != 2)
                throw new Exception("Players list count must be 2.");

            foreach (PlayerAddDTO player in players)
            {
                if (player == null)
                    throw new Exception("Player in player list can`t be null.");

                Players.Add(new Player(player.Id, player.PlayerType, player.Name, this));
            }
        }

        

        public void CreateBoard(Board board)
        {
            if (board == null)
                throw new Exception("Board can`t be null.");

            this.Board = board;
        }

        public List<Chip> GetAvailableSteps(int playerId)
        {
            List<Chip> availableChips = new List<Chip>();

            foreach (List<Chip> chips in Board.Chips)
            {
                foreach (Chip chip in chips.Where(chip => chip != null && chip.OwnerId == playerId))
                {
                    foreach (Chip availableChip in CheckChips(chip))
                    {
                        if (Board.Chips[availableChip.PosY][availableChip.PosX] == null)
                        {
                            availableChips.Add(availableChip);
                        }
                    }
                }
            }

            return availableChips;
        }

        private List<Chip> CheckChips(Chip chip)
        {
            List<Chip> result = new List<Chip>();
            int tmpChipX, tmpChipY;
            
            foreach ((int x, int y) in GetDirectionVectors())
            {
                bool hasOpponent = false;

                for (int i = 0; i < Board.Size; i++)
                {
                    tmpChipX = chip.PosX + (i * x);
                    tmpChipY = chip.PosY + (i * y);

                    if (!IsInBoardIndex(tmpChipX) || !IsInBoardIndex(tmpChipY))
                        break;

                    Chip tmpChip = Board.Chips[tmpChipY][tmpChipX];

                    if (tmpChip != null && tmpChip.OwnerId != chip.OwnerId)
                    {
                        hasOpponent = true;
                        continue;
                    }

                    if (tmpChip == null && hasOpponent)
                    {
                        result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = tmpChipY, PosX = tmpChipX });
                        break;
                    }
                }
            }

            return result;
        }

        private bool IsInBoardIndex (int index)
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

        /*private List<Chip> CheckChips(Chip chip)
        {
            List<Chip> result = new List<Chip>();

            for (int i = 0; i < Board.Size; i++)
            {
                if (i < chip.PosX &&
                    Board.Chips[chip.PosY][i] == null && 
                    Board.Chips[chip.PosY][i + 1] != null &&
                    Board.Chips[chip.PosY][i + 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY, PosX = i });
                }

                if (i > chip.PosX &&
                    Board.Chips[chip.PosY][i] == null &&
                    Board.Chips[chip.PosY][i - 1] != null &&
                    Board.Chips[chip.PosY][i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY, PosX = i });
                }

                if (i < chip.PosY &&
                    Board.Chips[i][chip.PosX] == null &&
                    Board.Chips[i + 1][chip.PosX] != null &&
                    Board.Chips[i + 1][chip.PosX].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = i, PosX = chip.PosX });
                }

                if (i > chip.PosY &&
                    Board.Chips[i][chip.PosX] == null &&
                    Board.Chips[i - 1][chip.PosX] != null &&
                    Board.Chips[i - 1][chip.PosX].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = i, PosX = chip.PosX });
                }

                if (chip.PosY - i >= 0 &&
                    chip.PosX - i >= 0 &&
                    Board.Chips[chip.PosY - i][chip.PosX - i] == null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX - i + 1] != null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX - i + 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY - i, PosX = chip.PosX - i });
                }

                if (chip.PosY + i < Board.Size &&
                    chip.PosX + i < Board.Size &&
                    Board.Chips[chip.PosY + i][chip.PosX + i] == null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX + i - 1] != null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX + i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY + i, PosX = chip.PosX + i });
                }

                if (chip.PosY - i >= 0 &&
                    chip.PosX + i < Board.Size &&
                    Board.Chips[chip.PosY - i][chip.PosX + i] == null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX + i - 1] != null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX + i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY - i, PosX = chip.PosX - i });
                }

                if (chip.PosY + i < Board.Size &&
                    chip.PosX - i >= 0 &&
                    Board.Chips[chip.PosY + i][chip.PosX - i] == null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX - i + 1] != null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX - i + 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY + i, PosX = chip.PosX + i });
                }
            }

            return result;
        }*/

        private List<Chip> GetFlippedChipsList(Chip chip)
        {
            List<Chip> result = new List<Chip>();

            bool extremeXUp = false;
            bool extremeXDown = false;
            bool extremeYUp = false;
            bool extremeYDown = false;
            bool extremeMainDiagonalUp = false;
            bool extremeMainDiagonalDown = false;
            bool extremeSecondDiagonalUp = false;
            bool extremeSecondDiagonalDown = false;

            for (int i = 0; i < Board.Size; i++)
            {
                if (!extremeXUp &&
                    chip.PosX - i > 0 &&
                    Board.Chips[chip.PosY][chip.PosX - i] != null &&
                    Board.Chips[chip.PosY][chip.PosX - i].OwnerId == chip.OwnerId &&
                    Board.Chips[chip.PosY][chip.PosX - i - 1] != null &&
                    Board.Chips[chip.PosY][chip.PosX - i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY, PosX = i });
                }

                if (!extremeXDown &&
                    i > chip.PosX &&
                    Board.Chips[chip.PosY][i] == null &&
                    Board.Chips[chip.PosY][i - 1] != null &&
                    Board.Chips[chip.PosY][i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY, PosX = i });
                }

                if (!extremeYUp &&
                    i < chip.PosY &&
                    Board.Chips[i][chip.PosX] == null &&
                    Board.Chips[i + 1][chip.PosX] != null &&
                    Board.Chips[i + 1][chip.PosX].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = i, PosX = chip.PosX });
                }

                if (!extremeYDown &&
                    i > chip.PosY &&
                    Board.Chips[i][chip.PosX] == null &&
                    Board.Chips[i - 1][chip.PosX] != null &&
                    Board.Chips[i - 1][chip.PosX].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = i, PosX = chip.PosX });
                }

                if (!extremeMainDiagonalUp &&
                    chip.PosY - i >= 0 &&
                    chip.PosX - i >= 0 &&
                    Board.Chips[chip.PosY - i][chip.PosX - i] == null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX - i + 1] != null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX - i + 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY - i, PosX = chip.PosX - i });
                }

                if (!extremeMainDiagonalDown &&
                    chip.PosY + i < Board.Size &&
                    chip.PosX + i < Board.Size &&
                    Board.Chips[chip.PosY + i][chip.PosX + i] == null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX + i - 1] != null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX + i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY + i, PosX = chip.PosX + i });
                }

                if (!extremeSecondDiagonalUp &&
                    chip.PosY - i >= 0 &&
                    chip.PosX + i < Board.Size &&
                    Board.Chips[chip.PosY - i][chip.PosX + i] == null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX + i - 1] != null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX + i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY - i, PosX = chip.PosX - i });
                }

                if (!extremeSecondDiagonalDown &&
                    chip.PosY + i < Board.Size &&
                    chip.PosX - i >= 0 &&
                    Board.Chips[chip.PosY + i][chip.PosX - i] == null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX - i + 1] != null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX - i + 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { OwnerId = chip.OwnerId, PosY = chip.PosY + i, PosX = chip.PosX + i });
                }
            }

            return result;
        }

        public void FlipChipsBetween()
        {

        }
    }
}
