using Reversi.Models;
using Reversi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Managers
{
    public class PlayerManagerHuman : IPlayerManager
    {
        private BoardService BoardService { get; set; }
        private PlayerService PlayerService { get; set; }
        public PlayerManagerHuman(BoardService boardService, PlayerService playerService)
        {
            this.BoardService = boardService;
            this.PlayerService = playerService;
        }

        public List<Chip> DoStep(int playerId, Chip chip)
        {
            List<Chip> availableChips = BoardService.GetAvailableSteps(playerId);
            List<Chip> flippedChips = new List<Chip>();

            if (availableChips.Count == 0 || !availableChips.Contains(chip))
                return flippedChips;

            BoardService.AddChipToBoard(chip);

            flippedChips = BoardService.GetFlippedChips(chip);

            BoardService.FlipChips(flippedChips, PlayerService.Players);

            flippedChips.Add(chip);

            return flippedChips;
        }
    }
}
