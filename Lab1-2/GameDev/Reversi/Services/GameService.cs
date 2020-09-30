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

            foreach (Chip chip in GetPlayerChips(playerId))
            {
                foreach (Chip availableChip in GetAvailableChips(chip))
                {
                    availableChips.Add(availableChip);
                }
            }

            return availableChips;
        }

        private IEnumerable<Chip> GetPlayerChips(int playerId)
        {
            return Board.Chips.SelectMany(chipList => chipList.Where(chip => chip?.OwnerId == playerId));
        }

        private List<Chip> GetAvailableChips(Chip chip)
        {
            List<Chip> availableChips = new List<Chip>();
            int tmpChipX, tmpChipY;
            
            foreach ((int x, int y) in GetDirectionVectors())
            {
                bool hasOpponentChip = false;

                for (int i = 1; i < Board.Size; i++)
                {
                    tmpChipX = chip.PosX + (i * x);
                    tmpChipY = chip.PosY + (i * y);

                    if (!IsInBoardIndex(tmpChipX) || !IsInBoardIndex(tmpChipY))
                        break;

                    Chip tmpChip = Board.Chips[tmpChipY][tmpChipX];

                    if (tmpChip != null && tmpChip.OwnerId != chip.OwnerId)
                    {
                        hasOpponentChip = true;
                        continue;
                    }

                    if (tmpChip == null && hasOpponentChip)
                    {
                        availableChips.Add(new Chip() { OwnerId = chip.OwnerId, PosY = tmpChipY, PosX = tmpChipX });
                        break;
                    }

                    if (tmpChip == null && !hasOpponentChip)
                    {
                        break;
                    }
                }
            }

            return availableChips;
        }

        public List<Chip> GetFlippedChips(Chip chip)
        {
            List<Chip> flippedChips = new List<Chip>();
            int tmpChipX, tmpChipY;

            foreach ((int x, int y) in GetDirectionVectors())
            {
                bool hasAllyChip = false;

                List<Chip> tmpChips = new List<Chip>();

                for (int i = 1; i < Board.Size; i++)
                {
                    tmpChipX = chip.PosX + (i * x);
                    tmpChipY = chip.PosY + (i * y);

                    if (!IsInBoardIndex(tmpChipX) || !IsInBoardIndex(tmpChipY))
                        break;

                    Chip tmpChip = Board.Chips[tmpChipY][tmpChipX];

                    if (tmpChip == null)
                        break;

                    if (tmpChip != null && tmpChip.OwnerId != chip.OwnerId)
                    {

                        tmpChips.Add(tmpChip);
                    }

                    if (tmpChip != null && tmpChip.OwnerId == chip.OwnerId)
                    {
                        hasAllyChip = true;
                        break;
                    }
                }

                if (hasAllyChip)
                {
                    foreach (Chip tmpChip in tmpChips)
                    {
                        Board.Chips[tmpChip.PosY][tmpChip.PosX].OwnerId = chip.OwnerId;
                    }

                    flippedChips.AddRange(tmpChips);
                }
            }

            // tmp

            foreach (Chip tmpChip in flippedChips)
            {
                Board.Chips[tmpChip.PosY][tmpChip.PosX].OwnerId = chip.OwnerId;
            }

            return flippedChips;
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
    }
}
