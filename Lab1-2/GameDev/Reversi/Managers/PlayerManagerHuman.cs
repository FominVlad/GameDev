using Reversi.Models;
using Reversi.Services;
using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Method for the player do step.
        /// </summary>
        /// <param name="playerId">Player that do step.</param>
        /// <param name="chip">The chip that done step.</param>
        /// <returns>Changed chips list.</returns>
        public List<Chip> DoStep(int playerId, Chip chip)
        {
            if (chip == null)
                throw new Exception("Chip can`t be null!");

            List<Chip> availableChips = BoardService.GetAvailableSteps(playerId);
            List<Chip> flippedChips = new List<Chip>();

            if (availableChips.Count == 0 || 
                !availableChips.Contains(chip))
                return flippedChips;

            BoardService.AddChipToBoard(chip);

            flippedChips = BoardService.GetFlippedChips(chip);

            BoardService.FlipChips(flippedChips, PlayerService.Players);

            flippedChips.Add(chip);

            return flippedChips;
        }
    }
}
