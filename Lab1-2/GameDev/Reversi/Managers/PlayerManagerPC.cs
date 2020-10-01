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
        private BoardService BoardService { get; set; }
        private PlayerService PlayerService { get; set; }

        public PlayerManagerPC(BoardService boardService, PlayerService playerService)
        {
            this.BoardService = boardService;
            this.PlayerService = playerService;
        }

        public List<Chip> DoStep(int playerId, Chip chip)
        {
            List<Chip> availableChips = BoardService.GetAvailableSteps(playerId);
            List<Chip> flippedChips = new List<Chip>();

            if (availableChips.Count == 0)
                return flippedChips;

            Random random = new Random();

            Chip chosenChip = availableChips[random.Next(0, availableChips.Count)];

            BoardService.AddChipToBoard(chosenChip);

            flippedChips = BoardService.GetFlippedChips(chosenChip);

            BoardService.FlipChips(flippedChips, PlayerService.Players);

            flippedChips.Add(chosenChip);

            return flippedChips;
        }
    }
}
