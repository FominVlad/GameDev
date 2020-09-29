using Reversi.Models;
using Reversi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Managers
{
    public class PlayerManagerPC : IPlayerManager
    {
        private GameService GameService { get; set; }

        public PlayerManagerPC(GameService gameService)
        {
            this.GameService = gameService;
        }

        public bool DoStep(int playerId, Chip chip)
        {
            List<Chip> availableChips = GameService.GetAvailableSteps(playerId);

            if (availableChips.Count == 0)
                return false;

            Random random = new Random();

            Chip chosenChip = availableChips[random.Next(0, availableChips.Count)];


            GameService.Board.Chips[chosenChip.PosY][chosenChip.PosX] = chosenChip;

            return true;
        }
    }
}
