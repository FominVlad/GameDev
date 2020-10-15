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
        public List<IPlayer> Players { get; private set; }
        public int? NextStepPlayerId { get; private set; }

        public PlayerService(BoardService boardService)
        {
            this.BoardService = boardService;
        }

        /// <summary>
        /// Method for initializing players.
        /// </summary>
        /// <param name="players">Players parameters for initializing.</param>
        /// <returns>Initialized players.</returns>
        public List<PlayerGetDTO> InitPlayers(List<PlayerCreateDTO> players)
        {
            if (players.Count != 2)
                throw new Exception("Players count must be 2.");

            Players = new List<IPlayer>();
            int nextId = 1;

            foreach (PlayerCreateDTO playerDTO in players)
            {
                if (playerDTO.PlayerType == PlayerType.Human)
                {
                    Players.Add(new PlayerHuman(playerDTO, nextId));
                }
                else if (playerDTO.PlayerType == PlayerType.PC)
                {
                    Players.Add(new PlayerPC(playerDTO, nextId));
                }
                nextId++;
            }

            NextStepPlayerId = Players.Where(player => 
                player.PlayerColour == PlayerColour.Black).
                Select(player => player.Id).FirstOrDefault();

            return Players.Select(player => new PlayerGetDTO(player)).ToList();
        }

        /// <summary>
        /// Method for the player do step.
        /// </summary>
        /// <param name="playerId">Player that do step.</param>
        /// <param name="chipDoStepDTO">The chip that done step.</param>
        /// <returns>Changed chips list.</returns>
        public Chip DoStep(int playerId, ChipDoStepDTO chipDoStepDTO)
        {
            if (!BoardService.CheckInitBoard())
                throw new Exception("Board is not initialized.");
            if (Players == null)
                throw new Exception("Players is not initialized.");

            if (!CheckNextStepPlayerId(playerId))
                throw new Exception("This player is not in the step queue.");

            IPlayer player = Players.Where(player => player.Id == playerId)
                .FirstOrDefault();

            List<Chip> availableChips = BoardService.GetAvailableSteps(player.Id);

            if (availableChips.Count == 0)
                return null;

            Chip chipForStep = player.PlayerType == PlayerType.Human ? 
                new Chip(chipDoStepDTO, player.Id) : 
                availableChips[new Random().Next(0, availableChips.Count)];

            if (player.PlayerType == PlayerType.Human &&
                !availableChips.Contains(chipForStep))
                throw new Exception("This step is unavailable!");

            BoardService.FlipChips(chipForStep, player.Id);
            SetNextStepPlayerId(playerId);

            return chipForStep;
        }

        private void SetNextStepPlayerId(int currStepPlayerId)
        {
            int? tmpNextStepPlayerId = Players.Where(player => player.Id != currStepPlayerId)
                .Select(player => player.Id).FirstOrDefault();

            if (BoardService.GetAvailableSteps(tmpNextStepPlayerId ?? -1).Count == 0)
            {
                tmpNextStepPlayerId = currStepPlayerId;

                if (BoardService.GetAvailableSteps(currStepPlayerId).Count == 0)
                {
                    tmpNextStepPlayerId = null;
                    SetWinner();
                }
            }

            NextStepPlayerId = tmpNextStepPlayerId;
        }

        private void SetWinner()
        {
            var chipsCount = BoardService.Board.OccupiedChips.GroupBy(chip => chip.OwnerId)
                .Select(grChips => new { playerId = grChips.Key, chipsCount = grChips.Count() });

            BoardService.Board.WinnerPlayerIdList = chipsCount
                .Where(obj => obj.chipsCount == chipsCount.Min(chipsCount => chipsCount.chipsCount))
                .Select(obj => obj.playerId).ToList();
        }

        public bool CheckNextStepPlayerId(int currStepPlayerId)
        {
            return NextStepPlayerId == currStepPlayerId;
        }
    }
}
