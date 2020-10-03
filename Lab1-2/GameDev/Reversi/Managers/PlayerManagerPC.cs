using Reversi.Models;
using Reversi.Services;
using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Method for the player do step.
        /// </summary>
        /// <param name="playerId">Player that do step.</param>
        /// <param name="chip"></param>
        /// <returns>Changed chips list.</returns>
        public List<Chip> DoStep(int playerId, Chip chip)
        {
            List<Chip> availableChips = BoardService.GetAvailableSteps(playerId);
            List<Chip> flippedChips = new List<Chip>();

            if (availableChips.Count == 0)
                return flippedChips;

            Chip chosenChip = availableChips[new Random().Next(0, availableChips.Count)];

            BoardService.AddChipToBoard(chosenChip);

            flippedChips = BoardService.GetFlippedChips(chosenChip);

            BoardService.FlipChips(flippedChips, PlayerService.Players);

            flippedChips.Add(chosenChip);

            return flippedChips;
        }
    }
}
