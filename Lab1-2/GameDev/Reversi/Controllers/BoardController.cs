using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reversi.Models;
using Reversi.Services;

namespace Reversi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private GameService GameService { get; set; }

        public BoardController(GameService gameService)
        {
            this.GameService = gameService;
        }

        [HttpPost]
        public IActionResult CreateBoard(Board board)
        {
            GameService.CreateBoard(board);

            GameService.Board.FillBoard(GameService.Players);

            return Ok();
        }

        [HttpPatch]
        public IActionResult AddChip(Chip chip)
        {
            GameService.Board.Chips[chip.PosY][chip.PosX] = chip;

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAvailableSteps(int id)
        {
            return Ok(GameService.GetAvailableSteps(id));
        }

        [HttpPut]
        public IActionResult GetBoard()
        {
            return Ok(GameService.Board.Chips);
        }
    }
}
