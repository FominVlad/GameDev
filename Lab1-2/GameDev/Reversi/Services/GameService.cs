using Reversi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddPlayers(List<Player> players)
        {
            foreach (Player player in players)
            {
                if (player == null)
                    throw new Exception("Player in player list can`t be null.");

                Players.Add(player);
            }
        }

        public void CreateBoard(Board board)
        {
            if (board == null)
                throw new Exception("Board can`t be null.");

            this.Board = board;
        }

        public List<List<Chip>> GetAvailableSteps(int playerId)
        {
            List<List<Chip>> availableChips = new List<List<Chip>>();

            for (int i = 0; i < Board.Size; i++)
            {
                availableChips.Add(new List<Chip>());

                for (int j = 0; j < Board.Size; j++)
                {
                    availableChips[i].Add(null);
                }
            }

            foreach (List<Chip> chips in Board.Chips)
            {
                foreach (Chip chip in chips.Where(chip => chip != null && chip.OwnerId == playerId))
                {
                    

                    foreach (Chip availableChip in CheckChips(chip))
                    {
                        if (Board.Chips[availableChip.PosY][availableChip.PosX] == null && availableChips[availableChip.PosY][availableChip.PosX] == null)
                        {
                            availableChips[availableChip.PosY][availableChip.PosX] = availableChip;
                        }
                    }
                }
            }

            return availableChips;
        }

        private List<Chip> CheckChips(Chip chip)
        {
            List<Chip> result = new List<Chip>();

            for (int i = 0; i < Board.Size; i++)
            {
                if (i < chip.PosX &&
                    Board.Chips[chip.PosY][i] == null && 
                    Board.Chips[chip.PosY][i + 1] != null &&
                    Board.Chips[chip.PosY][i + 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { PosY = chip.PosY, PosX = i });
                }

                if (i > chip.PosX &&
                    Board.Chips[chip.PosY][i] == null &&
                    Board.Chips[chip.PosY][i - 1] != null &&
                    Board.Chips[chip.PosY][i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { PosY = chip.PosY, PosX = i });
                }

                if (i < chip.PosY &&
                    Board.Chips[i][chip.PosX] == null &&
                    Board.Chips[i + 1][chip.PosX] != null &&
                    Board.Chips[i + 1][chip.PosX].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { PosY = i, PosX = chip.PosX });
                }

                if (i > chip.PosY &&
                    Board.Chips[i][chip.PosX] == null &&
                    Board.Chips[i - 1][chip.PosX] != null &&
                    Board.Chips[i - 1][chip.PosX].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { PosY = i, PosX = chip.PosX });
                }

                if (chip.PosY - i >= 0 &&
                    chip.PosX - i >= 0 &&
                    Board.Chips[chip.PosY - i][chip.PosX - i] == null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX - i + 1] != null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX - i + 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { PosY = chip.PosY - i, PosX = chip.PosX - i });
                }

                if (chip.PosY + i < Board.Size &&
                    chip.PosX + i < Board.Size &&
                    Board.Chips[chip.PosY + i][chip.PosX + i] == null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX + i - 1] != null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX + i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { PosY = chip.PosY + i, PosX = chip.PosX + i });
                }

                if (chip.PosY - i >= 0 &&
                    chip.PosX + i < Board.Size &&
                    Board.Chips[chip.PosY - i][chip.PosX + i] == null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX + i - 1] != null &&
                    Board.Chips[chip.PosY - i + 1][chip.PosX + i - 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { PosY = chip.PosY - i, PosX = chip.PosX - i });
                }

                if (chip.PosY + i < Board.Size &&
                    chip.PosX - i >= 0 &&
                    Board.Chips[chip.PosY + i][chip.PosX - i] == null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX - i + 1] != null &&
                    Board.Chips[chip.PosY + i - 1][chip.PosX - i + 1].OwnerId != chip.OwnerId)
                {
                    result.Add(new Chip() { PosY = chip.PosY + i, PosX = chip.PosX + i });
                }
            }

            return result;
        }
    }
}
