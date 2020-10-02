using Reversi.Models;
using Reversi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversi.Services
{
    /// <summary>
    /// Player management service.
    /// </summary>
    public class PlayerService
    {
        private BoardService BoardService { get; set; }
        public List<Player> Players { get; private set; }

        public int NextStepPlayerId { get; private set; }

        public PlayerService(BoardService boardService)
        {
            this.BoardService = boardService;
        }

        /// <summary>
        /// Method for initializing players.
        /// </summary>
        /// <param name="players">Players parameters for initializing.</param>
        /// <returns>Initialized players.</returns>
        public List<Player> InitPlayers(List<PlayerCreateDTO> players)
        {
            Players = players.Select((playerDTO, index) => 
                new Player(playerDTO, index, BoardService, this)).ToList();

            NextStepPlayerId = Players.Where(player => 
                player.PlayerColour == PlayerColour.Black).
                Select(player => player.Id).FirstOrDefault();

            return Players;
        }

        /// <summary>
        /// Method for the player do step.
        /// </summary>
        /// <param name="playerId">Player that do step.</param>
        /// <param name="chipDoStepDTO">The chip that done step.</param>
        /// <returns>Changed chips list.</returns>
        public List<Chip> DoStep(int playerId, ChipDoStepDTO chipDoStepDTO)
        {
            if (!CheckNextStepPlayerId(playerId))
                throw new Exception("This player is not in the step queue.");

            Chip chipForStep = new Chip(chipDoStepDTO, playerId);

            List <Chip> changedChips = Players.Where(player => player.Id == playerId)
                .FirstOrDefault().PlayerManager.DoStep(playerId, chipForStep);

            if (changedChips.Count != 0)
                SetNextStepPlayerId(playerId);

            return changedChips;
        }

        private void SetNextStepPlayerId(int currStepPlayerId)
        {
            NextStepPlayerId = Players.Where(player => player.Id != currStepPlayerId)
                .Select(player => player.Id).FirstOrDefault();
        }

        public bool CheckNextStepPlayerId(int currStepPlayerId)
        {
            if (NextStepPlayerId == currStepPlayerId)
                return true;

            if (BoardService.GetAvailableSteps(NextStepPlayerId).Count == 0)
                return true;

            return false;
        }
    }
}
