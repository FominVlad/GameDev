using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversi.Models;
using Reversi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reversi.Models.DTO;

namespace GameDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private GameService GameService { get; set; }

        public PlayerController(GameService gameService)
        {
            this.GameService = gameService;
        }

        [HttpPost]
        public IActionResult AddPlayers(List<PlayerAddDTO> players)
        {
            GameService.AddPlayers(players);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetPlayers()
        {
            return Ok(GameService.Players);
        }
    }
}
