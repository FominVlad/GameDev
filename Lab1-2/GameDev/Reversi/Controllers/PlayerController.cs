using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversi.Models;
using Reversi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult AddPlayers(List<Player> players)
        {
            if (players == null)
                throw new Exception("Players list can`t be null.");
            if (players.Count != 2)
                throw new Exception("Players list count must be 2.");

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
