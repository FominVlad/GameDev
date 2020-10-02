using Reversi.Models;
using Reversi.Models.DTO;
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
            Chip chipForStep = new Chip(chipDoStepDTO, playerId);

            return Players.Where(player => player.Id == playerId)
                .FirstOrDefault().PlayerManager.DoStep(playerId, chipForStep);
        }
    }
}
