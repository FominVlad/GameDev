using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reversi.Models.DTO;
using Reversi.Services;

namespace Reversi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private BoardService BoardService { get; set; }
        private PlayerService PlayerService { get; set; }

        public GameController(PlayerService playerService, BoardService boardService)
        {
            this.PlayerService = playerService;
            this.BoardService = boardService;
        }

        /// <summary>
        /// Method to initialize game parameters.
        /// </summary>
        /// <param name="boardSize">Future board size.</param>
        /// <param name="players">Players list.</param>
        /// <returns>Game parameters.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult InitGame(int boardSize, List<PlayerCreateDTO> players)
        {
            try
            {
                return StatusCode(201, new 
                { 
                    players = PlayerService.InitPlayers(players), 
                    board = BoardService.InitBoard(boardSize, PlayerService.Players),
                    nextStepPlayerId = PlayerService.NextStepPlayerId 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        /// <summary>
        /// Method to get available steps for player.
        /// </summary>
        /// <param name="playerId">Player unique identifier.</param>
        /// <returns>Available steps list.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAvailableSteps(int playerId)
        {
            try
            {
                return StatusCode(200, BoardService.GetAvailableSteps(playerId));
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        /// <summary>
        /// Method to do step by player.
        /// </summary>
        /// <param name="playerId">Unique identifier of the player that doing step.</param>
        /// <param name="chipDoStepDTO">The chip we do a step.</param>
        /// <returns>Flipped chips list.</returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DoStep(int playerId, ChipDoStepDTO chipDoStepDTO)
        {
            try
            {
                return StatusCode(200, new
                {
                    changedChips = PlayerService.DoStep(playerId, chipDoStepDTO),
                    nextStepPlayerId = PlayerService.NextStepPlayerId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
